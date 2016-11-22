namespace TowerInject
{
    public interface IFactoryProvider
    {
        IFactory CreateFactory(IContainer container);
    }
}
