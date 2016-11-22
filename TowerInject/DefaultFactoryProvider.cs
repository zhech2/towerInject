namespace TowerInject
{
    /// <summary>
    /// Creates an instance of the Default IFactory.  The Default is the <see cref="ReflectionFactory"/>.
    /// </summary>
    public class DefaultFactoryProvider : IFactoryProvider
    {
        /// <summary>
        /// Creates the factory
        /// </summary>
        /// <param name="provider">The resolver for getting required <see cref="IInstanceResolvers"/>.</param>
        /// <returns></returns>
        public IFactory CreateFactory(IInstanceResolverProvider provider)
        {
            return new ReflectionFactory();
        }
    }
}
