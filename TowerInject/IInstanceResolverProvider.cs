using System;
using System.Collections.Generic;

namespace TowerInject
{
    /// <summary>
    /// Locates <see cref="InstanceResolver"/>s.
    /// </summary>
    public interface IInstanceResolverProvider
    {
        /// <summary>
        /// Get the 
        /// </summary>
        /// <param name="dependencyStack">Helps in detecting cycles in the dependencies.</param>
        /// <param name="serviceType">The type of the service used in locating the <see cref="IInstanceResolver"/>.</param>
        /// <param name="throwOnMissing">Whether to throw an exception when the locator is missing or return null.</param>
        /// <returns></returns>
        IInstanceResolver GetInstanceResolver(Stack<Type> dependencyStack, Type serviceType, bool throwOnMissing);
    }
}
