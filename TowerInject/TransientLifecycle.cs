using System;
using System.Collections.Generic;
using System.Reflection;

namespace TowerInject
{
    /// <summary>
    /// Creates transient services.  Services are created new each time requested.
    /// </summary>
    public sealed class TransientLifecycle : ILifecycle
    {
        /// <summary>
        /// Create a registration for based on this lifecycle for the service and implementation types.
        /// </summary>
        /// <param name="serviceType">The type, base type or interface of the service.</param>
        /// <param name="implementationType">The implementation of <paramref name="serviceType"/>.</param>
        /// <returns></returns>
        public IRegistration CreateRegistration(Type serviceType, Type implementationType)
        {
            return new TransientRegistration(serviceType, implementationType, this);
        }

        /// <summary>
        /// Creates an instance resolver based on the given information.
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
            return new TransientInstanceResolver(factory, constructor, paramResolvers);
        }

        private class TransientInstanceResolver : IInstanceResolver
        {
            private readonly Func<object> _factoryFunction;

            public TransientInstanceResolver(
                IFactory factory,
                ConstructorInfo constructor,
                IEnumerable<IInstanceResolver> paramResolvers)
            {
                if (factory == null)
                {
                    throw new ArgumentNullException(nameof(factory));
                }

                if (constructor == null)
                {
                    throw new ArgumentNullException(nameof(constructor));
                }

                _factoryFunction = factory.Create(constructor, paramResolvers);
                if (_factoryFunction == null)
                {
                    throw new InvalidOperationException("Factory '{factory.GetType().FullName}' must not return null");
                }
            }

            public object Resolve()
            {
                return _factoryFunction.Invoke();
            }
        }

        private class TransientRegistration : IRegistration
        {
            public Type ServiceType { get; }
            public Type ImplementationType { get; }
            public ILifecycle Lifecycle { get; }

            public TransientRegistration(Type serviceType, Type implementationType, ILifecycle lifecycle)
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
