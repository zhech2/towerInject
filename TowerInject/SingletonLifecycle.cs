using System;
using System.Collections.Generic;
using System.Reflection;

namespace TowerInject
{
    public sealed class SingletonLifecycle : ILifecycle
    {
        public IRegistration CreateRegistration(Type serviceType, Type implementationType)
        {
            return new SingletonRegistration(serviceType, implementationType, this);
        }

        public IInstanceResolver CreateInstanceResolver(
            IFactory factory, 
            IRegistration registration,
            ConstructorInfo constructor,
            IEnumerable<IInstanceResolver> paramResolvers)
        {
            return new SingletonInstanceResolver(factory, constructor, paramResolvers);
        }

        private class SingletonInstanceResolver : IInstanceResolver
        {
            private readonly Lazy<object> _lazyInstance;

            public SingletonInstanceResolver(IFactory factory, 
                ConstructorInfo constructor,
                IEnumerable<IInstanceResolver> paramResolvers)
            {
                if (factory == null)
                {
                    throw new ArgumentNullException(nameof(factory));
                }

                var factoryFunction = factory.Create(constructor, paramResolvers);
                _lazyInstance = new Lazy<object>(factoryFunction, isThreadSafe: true);
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
                ServiceType = serviceType;
                ImplementationType = implementationType;
                Lifecycle = lifecycle;
            }
        }
    }
}
