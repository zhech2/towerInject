using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TowerInject
{
    /// <summary>
    /// Factory that uses reflection to create new instances of services.
    /// </summary>
    public class ReflectionFactory : IFactory
    {
        /// <summary>
        /// Initializes an instance of the <see cref="ReflectionFactory"/> class. 
        /// </summary>
        public ReflectionFactory()
        {
        }

        /// <summary>
        /// Creates a factory function used to create instances of the service.
        /// </summary>
        /// <param name="constructor">The constructor used to create the service.</param>
        /// <param name="paramResolvers">The <see cref="IInstanceResolver"/>s used in creating the constructors parameters.</param>
        /// <returns></returns>
        public Func<object> Create(ConstructorInfo constructor, IEnumerable<IInstanceResolver> paramResolvers)
        {
            if (constructor == null)
            {
                throw new ArgumentNullException(nameof(constructor));
            }

            paramResolvers = paramResolvers ?? Enumerable.Empty<IInstanceResolver>();

            return () => constructor.Invoke(paramResolvers
                .Select(r => r.Resolve())
                .ToArray());
        }
    }
}
