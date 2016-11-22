using System;

namespace TowerInject
{
    /// <summary>
    /// Represents the DependencyInjection or IoC container.
    /// </summary>
    public interface IContainer : IResolver, IRegistrator, IDisposable
    {

    }
}
