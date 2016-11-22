namespace TowerInject
{
    public static class Lifecycle
    {
        public static readonly ILifecycle Singleton = new SingletonLifecycle();
        public static readonly ILifecycle Transient = new TransientLifecycle();

        public static ILifecycle GetByType(LifecycleType lifecycleType)
        {
            switch (lifecycleType)
            {
                case LifecycleType.Singleton:
                    return Lifecycle.Singleton;
                case LifecycleType.Transient:
                    return Lifecycle.Transient;
                default:
                case LifecycleType.Default:
                    return null;
            }
        }
    }
}
