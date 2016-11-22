using System;

namespace TowerInject
{
    public interface IRegistrator
    {
        void Register(Type serviceType, 
            Type implementationType,
            ILifecycle lifecycle, 
            RegistrationConflictBehavior behavior);
    }
}
