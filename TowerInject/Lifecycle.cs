namespace TowerInject
{
    public static class Lifecycle
    {
        public static readonly ILifecycle Singleton = new SingletonLifecycle();
        public static readonly ILifecycle Transient = new TransientLifecycle();

        public static ILifecycle GetByType(LifecycleType lifecycleType)
        {
            ILifecycle lifecycle;
            switch (lifecycleType)
            {
                case LifecycleType.Singleton:
                    lifecycle = Lifecycle.Singleton;
                    break;
                case LifecycleType.Transient:
                    lifecycle = Lifecycle.Transient;
                    break;
                default:
                    lifecycle = null;
                    break;
            }

            return lifecycle;
        }
    }
}
