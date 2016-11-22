using System;
using System.Reflection;

namespace TowerInject
{
    /// <summary>
    /// Selects the constructor from the type that should be used in creating of the Service.
    /// </summary>
    public interface IConstructorSelector
    {
        /// <summary>
        /// Selects from the given type the constructor that will be used to create an instance of the Service.
        /// </summary>
        /// <param name="type">The type of the service</param>
        /// <returns>The constructorInfo representing the chosen constructor.</returns>
        ConstructorInfo SelectConstructor(Type type);
    }
}
