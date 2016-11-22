namespace TowerInject
{
    /// <summary>
    /// The default LifecycleTypes.
    /// </summary>
    public enum LifecycleType
    {
        /// <summary>
        /// Uses the ContainerOptions.DefaultLifecycle
        /// </summary>
        Default,

        /// <summary>
        /// Transient Lifecycle, creates a new instance every time requested.
        /// </summary>
        Transient,

        /// <summary>
        /// Singleton Lifecycle, creates and then returns the same instance every time requested.
        /// </summary>
        Singleton
    }
}
