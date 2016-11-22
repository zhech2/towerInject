namespace TowerInject
{
    /// <summary>
    /// When a service is registered more than once this enumeration is used to determine
    /// what action should be taken.
    /// </summary>
    public enum RegistrationConflictBehavior
    {
        /// <summary>
        /// The Default Behavior - defined by the <see cref="ContainerOptions"/>.DefaultRegistrationBehavior 
        /// </summary>
        Default,

        /// <summary>
        /// Replace the existing service registration.
        /// </summary>
        Replace,

        /// <summary>
        /// Keep the existing service registration.
        /// </summary>
        Keep,

        /// <summary>
        /// Throw an exception about the duplicate registration.
        /// </summary>
        Throw,
    }
}
