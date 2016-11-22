using System;
using System.Reflection;

namespace TowerInject
{
    public class SinglePublicConstructorSelector : IConstructorSelector
    {
        public ConstructorInfo SelectConstructor(Type type)
        {
            var constructors = type.GetConstructors();

            if (constructors.Length == 0)
            {
                throw new InvalidOperationException($"Type '{type.FullName}' must have a single public constructor.  A custom IConstructorSelector can also be configured.");
            }
            else if (constructors.Length > 1)
            {
                throw new InvalidOperationException($"Type '{type.FullName}' must have a single public constructor.  A custom IConstructorSelector can also be configured.");
            }
            else
            {
                return constructors[0];
            }
        }
    }
}
