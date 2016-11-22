using System;

namespace TowerInject
{
    /// <summary>
    /// Registers a service
    /// </summary>
    public interface IRegistrator
    {
        /// <summary>
        /// Register a service with the given implementation
        /// </summary>
        /// <param name="serviceType">The type, base type or interface of the service.</param>
        /// <param name="implementationType">The implementation of the service type.</param>
        /// <param name="lifecycle">The lifecycle used to create the service.</param>
        /// <param name="conflictBehavior">The behavior used when the given serviceType has already been registered.</param>
        void Register(Type serviceType,
            Type implementationType,
            ILifecycle lifecycle,
            RegistrationConflictBehavior conflictBehavior);
    }
}
