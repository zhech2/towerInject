namespace TowerInject
{
    public class ContainerOptions
    {
        public ILifecycle DefaultLifecycle = Lifecycle.Transient;
        public RegistrationConflictBehavior DefaultRegistrationBehavior = RegistrationConflictBehavior.Throw;
        public IConstructorSelector ConstructorSelector = new SinglePublicConstructorSelector();

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
