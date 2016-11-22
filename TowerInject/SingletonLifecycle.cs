using System;
using System.Collections.Generic;
using System.Reflection;

namespace TowerInject
{
    /// <summary>
    /// Creates Singleton services.  Each time one is requested the same instance
    /// will be returned.  Scoped to the Container.
    /// </summary>
    public sealed class SingletonLifecycle : ILifecycle
    {
        /// <summary>
        /// Create a registration for based on this lifecycle for the service and implementation types.
        /// </summary>
        /// <param name="serviceType">The type, base type or interface of the service.</param>
        /// <param name="implementationType">The implementation of <paramref name="serviceType"/>.</param>
        /// <returns></returns>
        public IRegistration CreateRegistration(Type serviceType, Type implementationType)
        {
            return new SingletonRegistration(serviceType, implementationType, this);
        }

        /// <summary>
        /// Creates an instance resolver based on the given parameters.
        /// </summary>
        /// <param name="factory">The factory used to create an instance of the service when needed.</param>
        /// <param name="registration">The registration information for a service.</param>
        /// <param name="constructor">The constructor used to create the service</param>
        /// <param name="paramResolvers">The <see cref="IInstanceResolver"/>s used to resolve each of the constructors parameters.</param>
        /// <returns></returns>
        public IInstanceResolver CreateInstanceResolver(
            IFactory factory,
            IRegistration registration,
            ConstructorInfo constructor,
            IEnumerable<IInstanceResolver> paramResolvers)
        {
            return new SingletonInstanceResolver(factory, constructor, paramResolvers);
        }

        private class SingletonInstanceResolver : IInstanceResolver, IDisposable
        {
            private readonly Lazy<object> _lazyInstance;

            public SingletonInstanceResolver(IFactory factory,
                ConstructorInfo constructor,
                IEnumerable<IInstanceResolver> paramResolvers)
            {
                if (constructor == null)
                {
                    throw new ArgumentNullException(nameof(constructor));
                }

                if (factory == null)
                {
                    throw new ArgumentNullException(nameof(factory));
                }

                var factoryFunction = factory.Create(constructor, paramResolvers);
                if (factoryFunction == null)
                {
                    throw new InvalidOperationException();
                }

                _lazyInstance = new Lazy<object>(factoryFunction, isThreadSafe: true);
            }

            public void Dispose()
            {
                var disposable = _lazyInstance.Value as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }

            public object Resolve()
            {
                return _lazyInstance.Value;
            }
        }

        private class SingletonRegistration : IRegistration
        {
            public Type ServiceType { get; }
            public Type ImplementationType { get; }
            public ILifecycle Lifecycle { get; }

            public SingletonRegistration(Type serviceType, Type implementationType, ILifecycle lifecycle)
            {
                if (serviceType == null)
                {
                    throw new ArgumentNullException(nameof(serviceType));
                }
                if (implementationType == null)
                {
                    throw new ArgumentNullException(nameof(implementationType));
                }

                ServiceType = serviceType;
                ImplementationType = implementationType;
                Lifecycle = lifecycle;
            }
        }
    }
}
