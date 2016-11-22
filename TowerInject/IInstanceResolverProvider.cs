using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerInject
{
    public interface IInstanceResolverProvider
    {
        IInstanceResolver GetInstanceResolver(Stack<Type> cycles, Type serviceType, bool throwOnMissing);
    }
}
