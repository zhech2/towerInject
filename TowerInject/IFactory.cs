using System;
using System.Collections.Generic;
using System.Reflection;

namespace TowerInject
{
    /// <summary>
    /// Creates a factory function used to create the service instance based on the constructor 
    /// and the resolvers for each of the <see cref="IInstanceResolver"/>s. 
    /// </summary>
    public interface IFactory
    {
        /// <summary>
        /// Create the factory function based on the constructor and <see cref="IInstanceResolver"/>.
        /// </summary>
        /// <param name="constructor">The constructor to be used in creation of the service.</param>
        /// <param name="paramInstanceResolvers">The instance resolvers used to resolve the constructors parameters.</param>
        /// <returns></returns>
        Func<object> Create(ConstructorInfo constructor, IEnumerable<IInstanceResolver> paramInstanceResolvers);
    }
}
