using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TowerInject
{
    public sealed class TransientLifecycle : ILifecycle
    {
        public IRegistration CreateRegistration(Type serviceType, Type implementationType)
        {
            return new TransientRegistration(serviceType, implementationType, this);   
        }

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
                _factoryFunction = factory.Create(constructor, paramResolvers);
            }

            public object Resolve()
            {
                return _factoryFunction?.Invoke();
            }
        }

        private class TransientRegistration : IRegistration
        {
            public Type ServiceType { get; }
            public Type ImplementationType { get; }
            public ILifecycle Lifecycle { get; }

            public TransientRegistration(Type serviceType, Type implementationType, ILifecycle lifecycle)
            {
                ServiceType = serviceType;
                ImplementationType = implementationType;
                Lifecycle = lifecycle;
            }
        }
    }
}
