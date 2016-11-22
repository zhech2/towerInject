using System;
using System.Collections.Generic;
using System.Reflection;

namespace TowerInject
{
    /// <summary>
    /// Controls the lifecycle of services, ex: Singleton or Transient.
    /// </summary>
    public interface ILifecycle
    {
        /// <summary>
        /// Create a registration for based on the lifecycle for the service and implementation types.
        /// </summary>
        /// <param name="serviceType">The type, base type or interface of the service.</param>
        /// <param name="implementationType">The implementation of <paramref name="serviceType"/>.</param>
        /// <returns></returns>
        IRegistration CreateRegistration(Type serviceType, Type implementationType);

        /// <summary>
        /// Creates an instance resolver based on the given information.
        /// </summary>
        /// <param name="factory">The factory used to create an instance of the service when needed.</param>
        /// <param name="registration">The registration information for a service.</param>
        /// <param name="constructor">The constructor used to create the service</param>
        /// <param name="paramResolvers">The <see cref="IInstanceResolver"/>s used to resolve each of the constructors parameters.</param>
        /// <returns></returns>
        IInstanceResolver CreateInstanceResolver(
            IFactory factory,
            IRegistration registration,
            ConstructorInfo constructor,
            IEnumerable<IInstanceResolver> paramResolvers);
    }
}
