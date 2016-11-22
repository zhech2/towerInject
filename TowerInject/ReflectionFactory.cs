using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TowerInject
{
    public class ReflectionFactory : IFactory
    {
        public ReflectionFactory()
        {
        }

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
