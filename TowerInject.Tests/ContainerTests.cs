using System;
using Xunit;

namespace TowerInject.Tests
{
    public class ContainerTests
    {
        [Fact]
        public void Resolve_WhenRegisteringService_ShouldResolve()
        {
            Container sut = createSut();       
                
            sut.Register<IEmailService, NullEmailService>();

            var emailService = sut.Resolve<IEmailService>();

            Assert.IsAssignableFrom(typeof(NullEmailService), emailService);
        }

        [Fact]
        public void Resolve_WhenRegisteringServicesWithDependencies_ShouldResolveDependencies()
        {
            Container container = createSut();

            container.Register<ILogger, NullLogger>();
            container.Register<ICalculator, NullCalculator>();
            container.Register<IEmailService, NullEmailService>();
            container.Register<UsersController>();

            var controller = container.Resolve<UsersController>();

            Assert.NotNull(controller.EmailService);
            Assert.NotNull(controller.Calculator);
            Assert.NotNull(controller.Calculator.Logger);
        }

        [Fact]
        public void Resolve_WhenResolvingSingletonServiceTwice_InstancesShouldBeTheSame()
        {
            Container sut = createSut();

            sut.RegisterSingleton<IEmailService, NullEmailService>();

            var first = sut.Resolve<IEmailService>();
            var second = sut.Resolve<IEmailService>();

            Assert.Same(first, second);
        }

        [Fact]
        public void Resolve_WhenResolvingTransientServiceTwice_InstancesShouldNotBeTheSame()
        {
            Container sut = createSut();

            sut.Register<IEmailService, NullEmailService>(Lifecycle.Transient);

            var first = sut.Resolve<IEmailService>();
            var second = sut.Resolve<IEmailService>();

            Assert.NotSame(first, second);
        }

        [Fact]
        public void Register_WhenRegisteringTwice_ShouldThrow()
        {
            Container sut = createSut();

            sut.Register<IEmailService, NullEmailService>();

            Assert.Throws<InvalidOperationException>(() =>
                sut.Register<IEmailService, NullEmailService>());
        }

        [Fact]
        public void Resolve_WhenCreatingInstancesWithCircularDependencies_ShouldThrow()
        {
            Container sut = createSut();

            sut.Register<A>();
            sut.Register<B>();

            Assert.Throws<InvalidOperationException>(() =>
                sut.Resolve<A>());
        }

        private static Container createSut()
        {
            return new Container();
        }
    }
}
