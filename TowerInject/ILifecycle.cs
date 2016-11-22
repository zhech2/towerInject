using System;
using System.Collections.Generic;
using System.Reflection;

namespace TowerInject
{
    public interface ILifecycle
    {
        IRegistration CreateRegistration(Type serviceType, Type implementationType);
        IInstanceResolver CreateInstanceResolver(
            IFactory factory,
            IRegistration registration,
            ConstructorInfo constructor,
            IEnumerable<IInstanceResolver> paramResolvers);
    }
}
