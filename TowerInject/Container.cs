using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TowerInject
{
    /// <summary>
    /// A simple Dependency Injection or IoC container.
    /// </summary>
    public sealed class Container : IContainer,
        IServiceProvider,
        IInstanceResolverProvider
    {
        private readonly ConcurrentDictionary<Type, IRegistration> _registrationMap =
            new ConcurrentDictionary<Type, IRegistration>();

        private readonly ConcurrentDictionary<Type, IInstanceResolver> _instanceResolverMap =
            new ConcurrentDictionary<Type, IInstanceResolver>();

        private readonly IFactory _factory;
        private readonly ContainerOptions _options;

        /// <summary>
        /// Initializes an instance of the <see cref="Container"/> class.
        /// </summary>
        public Container()
            : this(new ContainerOptions(), new DefaultFactoryProvider())
        {

        }

        /// <summary>
        /// Initializes an instance of the <see cref="Container"/> class.
        /// </summary>
        /// <param name="containerOptions">The options used to customize the container.  Any options set to null will use the defaults.</param>
        public Container(ContainerOptions containerOptions)
            : this(containerOptions, new DefaultFactoryProvider())
        {
        }

        /// <summary>
        /// Initializes an instance of the <see cref="Container"/> class.
        /// </summary>
        public Container(ContainerOptions options, IFactoryProvider factoryProvider)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (factoryProvider == null)
            {
                throw new ArgumentNullException(nameof(factoryProvider));
            }

            _options = options;
            _options.EnsureDefaults();
            _factory = factoryProvider.CreateFactory(this);
        }

        /// <summary>
        /// Registers type <paramref name="implementationType"/> to be created when 
        /// <paramref name="serviceType" /> is resolved.
        /// </summary>
        /// <param name="serviceType">The basetype or interface to register.</param>
        /// <param name="implementationType">The type that will be created or returned when resolving the service type.</param>
        /// <param name="lifecycle">The lifecycle object used to create the <see cref="IInstanceResolver"/>s and <see cref="InstanceResolver"/>s.</param>
        /// <param name="conflictBehavior">The behavior to use when there is another type already registered for the given service type.</param>
        /// <remarks>
        /// See <seealso cref="RegistratorExtensions"/> for additional ways to register types.
        /// </remarks>
        public void Register(Type serviceType,
            Type implementationType,
            ILifecycle lifecycle,
            RegistrationConflictBehavior conflictBehavior)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }
            if (implementationType == null)
            {
                throw new ArgumentNullException(nameof(implementationType));
            }
            if (!serviceType.IsAssignableFrom(implementationType))
            {
                throw new InvalidOperationException($"Cannot register '{implementationType.FullName}' as '{serviceType.FullName}' because it does not implement the service type.");
            }
            if (implementationType.IsAbstract || implementationType.IsInterface)
            {
                throw new InvalidOperationException($"Cannot create an instance of type '{implementationType.FullName}'.");
            }

            lifecycle = lifecycle ?? _options.DefaultLifecycle;

            var registration = lifecycle.CreateRegistration(serviceType, implementationType);

            _registrationMap.AddOrUpdate(serviceType,
                registration,
                (type, oldRegistration) =>
                {
                    if (oldRegistration != null)
                    {
                        conflictBehavior = conflictBehavior == RegistrationConflictBehavior.Default
                            ? _options.DefaultRegistrationConflictBehavior
                            : conflictBehavior;

                        switch (conflictBehavior)
                        {
                            case RegistrationConflictBehavior.Keep:
                                return oldRegistration;
                            case RegistrationConflictBehavior.Replace:
                                return registration;
                            default:
                            case RegistrationConflictBehavior.Throw:
                                throw new InvalidOperationException($"Service '{type}' has already been registered");
                        }
                    }

                    return registration;
                });
        }

        /// <summary>
        /// Gets or creates the instance registered for the given type.
        /// </summary>
        /// <param name="type">The type, base type or interface used to get or create the registered implementation.</param>
        /// <returns></returns>
        public object Resolve(Type type)
        {
            var resolver = _instanceResolverMap.GetOrAdd(type, getInstanceResolver);

            return resolver?.Resolve();
        }

        private object resolveWithoutThrow(Type type)
        {
            var resolver = _instanceResolverMap.GetOrAdd(type, t => getInstanceResolver(new Stack<Type>(), t, throwOnMissing: false));

            return resolver?.Resolve();
        }

        private IInstanceResolver getInstanceResolver(Type type)
        {
            return getInstanceResolver(new Stack<Type>(), type, throwOnMissing: true);
        }

        private IInstanceResolver getInstanceResolver(Stack<Type> dependencyStack, Type type, bool throwOnMissing)
        {
            IRegistration registration;
            if (!_registrationMap.TryGetValue(type, out registration) && throwOnMissing)
            {
                throw new InvalidOperationException($"Could not resolve service of type '{type.FullName}'");
            }

            if (registration == null)
            {
                return null;
            }
            else
            {
                var constructor = _options.ConstructorSelector.SelectConstructor(registration.ImplementationType);
                var paramResolvers = getParameterResolvers(dependencyStack, registration, constructor, throwOnMissing);

                return registration.Lifecycle?.CreateInstanceResolver(_factory,
                    registration,
                    constructor,
                    paramResolvers);
            }
        }

        private IEnumerable<IInstanceResolver> getParameterResolvers(Stack<Type> dependencyStack,
            IRegistration registration,
            ConstructorInfo constructor,
            bool throwOnMissing)
        {
            if (constructor == null)
            {
                return Enumerable.Empty<IInstanceResolver>();
            }

            var serviceType = registration.ServiceType;
            if (dependencyStack.Contains(serviceType))
            {
                throw new InvalidOperationException($"A circular dependency was detected for '{serviceType.FullName}'");
            }

            try
            {
                dependencyStack.Push(serviceType);

                var instanceResolvers = constructor.GetParameters()
                    .Select(p => ((IInstanceResolverProvider)this).GetInstanceResolver(dependencyStack, p.ParameterType, throwOnMissing: true))
                    .ToArray();

                return instanceResolvers;
            }
            finally
            {
                dependencyStack.Pop();
            }
        }

        IInstanceResolver IInstanceResolverProvider.GetInstanceResolver(Stack<Type> dependencyStack, Type serviceType, bool throwOnMissing)
        {
            return getInstanceResolver(dependencyStack, serviceType, throwOnMissing);
        }

        object IServiceProvider.GetService(Type serviceType)
        {
            return resolveWithoutThrow(serviceType);
        }

        /// <summary>
        /// Disposes of all types marked as IDisposable based on the behavior of the associated Lifecycle.
        /// </summary>
        public void Dispose()
        {
            foreach (var resolverPair in _instanceResolverMap)
            {
                var disposable = resolverPair.Value as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }

            _instanceResolverMap.Clear();
            _registrationMap.Clear();
        }
    }
}
