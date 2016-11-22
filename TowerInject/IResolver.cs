using System;

namespace TowerInject
{
    /// <summary>
    /// Resolves services
    /// </summary>
    public interface IResolver
    {
        /// <summary>
        /// Resolve the service based on the given type.
        /// </summary>
        /// <param name="type">The type of the service to resolve.</param>
        object Resolve(Type type);
    }
}
