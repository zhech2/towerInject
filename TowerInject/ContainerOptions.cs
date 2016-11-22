namespace TowerInject
{
    /// <summary>
    /// Options for customizing the behavior of the <see cref="Container"/>. 
    /// </summary>
    public class ContainerOptions
    {
        public ILifecycle DefaultLifecycle;
        public RegistrationConflictBehavior DefaultRegistrationConflictBehavior;
        public IConstructorSelector ConstructorSelector;

        /// <summary>
        /// Initializes an instance of the <see cref="ContainerOptions"/> class. 
        /// </summary>
        public ContainerOptions()
        {
            DefaultRegistrationConflictBehavior = RegistrationConflictBehavior.Throw;
            EnsureDefaults();
        }

        /// <summary>
        /// Ensures that there is a default for each option.
        /// </summary>
        public void EnsureDefaults()
        {
            if (DefaultLifecycle == null)
            {
                DefaultLifecycle = Lifecycle.Transient;
            }

            if (ConstructorSelector == null)
            {
                ConstructorSelector = new SinglePublicConstructorSelector();
            }
        }
    }
}
