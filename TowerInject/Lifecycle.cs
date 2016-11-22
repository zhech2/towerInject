namespace TowerInject
{
    /// <summary>
    /// The Default Lifecycles
    /// </summary>
    public static class Lifecycle
    {
        /// <summary>
        /// Register a service as a Singleton service
        /// </summary>
        public static readonly ILifecycle Singleton = new SingletonLifecycle();

        /// <summary>
        /// Register a service as a Transient service
        /// </summary>
        public static readonly ILifecycle Transient = new TransientLifecycle();

        /// <summary>
        /// Convert the LifecycleType to a <see cref="ILifecycle"/>
        /// </summary>
        /// <param name="lifecycleType">The type of the lifecycle to get</param>
        /// <returns>The <see cref="ILifecycle"/> instance </returns>
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
