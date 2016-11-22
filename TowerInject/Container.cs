using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TowerInject
{
    /// <summary>
    /// A simple Dependency Injection or IOC container.
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
            _options = options ?? throw new ArgumentNullException(nameof(options));

            if (factoryProvider == null)
            {
                throw new ArgumentNullException(nameof(factoryProvider));
            }

            _options.EnsureDefaults();
            _factory = factoryProvider.CreateFactory(this);
        }

        public void Register(Type serviceType,
            Type implementationType,
            ILifecycle lifecycle,
            RegistrationConflictBehavior behavior)
        {
            if (!serviceType.IsAssignableFrom(implementationType))
            {
                throw new InvalidOperationException($"Cannot register '{implementationType.FullName}' as '{serviceType.FullName}' because it does not implement the service type.");
            }

            lifecycle = lifecycle ?? _options.DefaultLifecycle;

            var registration = lifecycle.CreateRegistration(serviceType, implementationType);

            _registrationMap.AddOrUpdate(serviceType,
                registration,
                (type, oldRegistration) =>
                {
                    if (oldRegistration != null)
                    {
                        behavior = behavior == RegistrationConflictBehavior.Default
                            ? _options.DefaultRegistrationBehavior
                            : behavior;

                        switch (behavior)
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

        public object Resolve(Type type)
        {
            var resolver = _instanceResolverMap.GetOrAdd(type, getInstanceResolver);
            return resolver?.Resolve();
        }

        private IInstanceResolver getInstanceResolver(Type type)
        {
            return getInstanceResolver(new Stack<Type>(), type, throwOnMissing: true);
        }

        private IInstanceResolver getInstanceResolver(Stack<Type> dependencyStack, Type type, bool throwOnMissing)
        {
            IRegistration registration;
            if (!_registrationMap.TryGetValue(type, out registration))
            {
                throw new InvalidOperationException($"Could not resolve service of type '{type.FullName}'");
            }

            var constructor = _options.ConstructorSelector.SelectConstructor(registration.ImplementationType);
            var paramResolvers = getParameterResolvers(dependencyStack, registration, constructor, throwOnMissing);

            return registration?.Lifecycle?.CreateInstanceResolver(_factory, 
                registration, 
                constructor, 
                paramResolvers);
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

        IInstanceResolver IInstanceResolverProvider.GetInstanceResolver(Stack<Type> cycles, Type serviceType, bool throwOnMissing)
        {
            return getInstanceResolver(cycles, serviceType, throwOnMissing);
        }

        object IServiceProvider.GetService(Type serviceType)
        {
            return Resolve(serviceType);
        }
    }
}
