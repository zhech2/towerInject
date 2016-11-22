using System;
using Xunit;

namespace TowerInject.Tests
{
    public class RegisterExtensionsTests
    {
        [Fact]
        public void Register_WithImplementationTypeOnly_WithLifecycle_ShouldNotThrow()
        {
            try
            {
                var container = new Container();
                container.Register<NullEmailService>(Lifecycle.Singleton);
            }
            catch (Exception)
            {
                Assert.True(false, "Should not throw");
            }
        }

        [Fact]
        public void Register_ImplementationTypeOnly_WithLifecycle_AndRegistrationConflictBehavior_ShouldNotThrow()
        {
            try
            {
                var container = new Container();
                container.Register<NullEmailService>(Lifecycle.Singleton, RegistrationConflictBehavior.Default);
            }
            catch (Exception)
            {
                Assert.True(false, "Should not throw");
            }
        }

        [Fact]
        public void RegisterTransient_ShouldReturnInstance()
        {
            var container = new Container();
            container.RegisterTransient<IEmailService, NullEmailService>();

            Assert.NotNull(container.Resolve<IEmailService>());
        }

        [Fact]
        public void RegisterTransient_WhenRegisteringSameServiceTwice_WithRegistrationConflictBehavior_Throw_ShouldThrowInvalidOperationException()
        {
            var container = new Container();
            container.RegisterTransient<IEmailService, NullEmailService>();

            Assert.Throws<InvalidOperationException>(() =>
                container.RegisterTransient<IEmailService, NullEmailService>(RegistrationConflictBehavior.Throw));
        }

        // The rest of the tests are covered elsewhere
    }
}
