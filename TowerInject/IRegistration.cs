using System;

namespace TowerInject
{
    public interface IRegistration
    {
        Type ServiceType { get; }
        Type ImplementationType { get; }
        ILifecycle Lifecycle { get; }
    }
}
