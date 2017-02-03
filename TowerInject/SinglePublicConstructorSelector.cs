using System;
using System.Reflection;

namespace TowerInject
{
    /// <summary>
    /// Constructor Selector for types with one public constructor.
    /// </summary>
    /// <remarks>
    /// For types without one public constructor InvalidOperationExceptions will be thrown.
    /// </remarks>
    public class SinglePublicConstructorSelector : IConstructorSelector
    {
        /// <summary>
        /// Selects from the given type the constructor that will be used to create an instance of the Service.
        /// </summary>
        /// <param name="type">The type of the service</param>
        /// <returns>The constructorInfo representing the chosen constructor.</returns>
        public ConstructorInfo SelectConstructor(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

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
