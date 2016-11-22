using System;
using System.Reflection;

namespace TowerInject
{
    public interface IConstructorSelector
    {
        ConstructorInfo SelectConstructor(Type type);
    }
}
