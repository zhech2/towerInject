namespace TowerInject
{
    // <summary>
    /// Creates an instance of <see cref="IFactory"/>.
    /// </summary>
    public interface IFactoryProvider
    {
        /// <summary>
        /// Creates the factory
        /// </summary>
        /// <param name="provider">The resolver for getting required <see cref="IInstanceResolvers"/>.</param>
        /// <returns></returns>
        IFactory CreateFactory(IInstanceResolverProvider provider);
    }
}
