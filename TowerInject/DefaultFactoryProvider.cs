namespace TowerInject
{
    public class DefaultFactoryProvider : IFactoryProvider
    {
        public IFactory CreateFactory(IContainer container)
        {
            return new ReflectionFactory();
        }
    }
}
