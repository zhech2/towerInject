using System;

namespace TowerInject
{
    /// <summary>
    /// The registration information requried to create a service.
    /// </summary>
    public interface IRegistration
    {
        /// <summary>
        /// The type, base type or interface of the service.
        /// </summary>
        Type ServiceType { get; }

        /// <summary>
        /// The type that implements the service type.
        /// </summary>
        Type ImplementationType { get; }

        /// <summary>
        /// The lifecycle used in creating the object.
        /// </summary>
        ILifecycle Lifecycle { get; }
    }
}
