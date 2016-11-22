using System;
using System.Collections.Generic;
using System.Reflection;

namespace TowerInject
{
    public interface IFactory
    {
        Func<object> Create(ConstructorInfo constructor, IEnumerable<IInstanceResolver> paramInstanceResolvers);
    }
}
