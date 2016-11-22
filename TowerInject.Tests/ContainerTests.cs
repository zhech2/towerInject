using System;
using System.Reflection;
using Xunit;

namespace TowerInject.Tests
{
    public class ContainerTests
    {
        [Fact]
        public void Constructor_WithNullContainerOptions_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new Container(null));
        }

        [Fact]
        public void Constructor_WithNullFactoryProvider_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new Container(new ContainerOptions(), null));
        }

        [Fact]
        public void Resolve_WhenRegisteringService_ShouldResolveInstance()
        {
            Container sut = createSut();
            sut.Register<IEmailService, NullEmailService>();

            var emailService = sut.Resolve<IEmailService>();

            Assert.IsAssignableFrom(typeof(NullEmailService), emailService);
        }

        [Fact]
        public void Register_WhenRegisteringServiceType_ThatIsNotAssignableToImplementationType_ShouldThrowInvalidOperationException()
        {
            Container sut = createSut();

            Assert.Throws<InvalidOperationException>(() =>
                sut.Register(typeof(IEmailService), typeof(NullLogger)));
        }

        [Fact]
        public void Register_WhenRegisteringWithNullLifecycle_ShouldUseDefaultLifecycle()
        {
            Container sut = createSut();
            sut.Register<IEmailService, NullEmailService>();

            var emailService1 = sut.Resolve<IEmailService>();
            var emailService2 = sut.Resolve<IEmailService>();

            Assert.NotSame(emailService1, emailService2);
        }

        [Fact]
        public void Dispose_WhenDisposingContainerDisposableSingletons_ShouldBeDisposed()
        {
            Container sut = createSut();
            sut.RegisterSingleton<IEmailService, DisposableEmailService>();

            var emailService = (DisposableEmailService)sut.Resolve<IEmailService>();

            sut.Dispose();

            Assert.True(emailService.IsDisposed);
        }

        [Fact]
        public void Resolve_WhenResolvingSingletonWithTwoContainers_EachContainer_ShouldHaveUniqueSingletonInstances()
        {
            Container sut1 = new Container();
            sut1.Register<IEmailService, NullEmailService>(LifecycleType.Singleton);

            Container sut2 = new Container();
            sut2.Register<IEmailService, NullEmailService>(LifecycleType.Singleton);

            Assert.NotSame(sut1.Resolve<IEmailService>(), sut2.Resolve<IEmailService>());
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

            var emailService1 = sut.Resolve<IEmailService>();
            var emailService2 = sut.Resolve<IEmailService>();

            Assert.Same(emailService1, emailService2);
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
        public void Register_WhenRegisteringSameServiceTwice_ShouldThrowInvalidOperationException()
        {
            Container sut = createSut();
            sut.Register<IEmailService, NullEmailService>(Lifecycle.Transient);

            Assert.Throws<InvalidOperationException>(() =>
                sut.Register<IEmailService, NullEmailService>(Lifecycle.Transient, RegistrationConflictBehavior.Throw));
        }

        [Fact]
        public void Register_WithConflictRegistrationBehavior_Default_ShouldUseDefaultRegistrationBehaviorFromOptions()
        {
            Container sut = createSut();
            sut.Register<IEmailService, NullEmailService>();


            Assert.Throws<InvalidOperationException>(() =>
                sut.Register<IEmailService, NullEmailService>(LifecycleType.Transient, RegistrationConflictBehavior.Default));
        }

        [Fact]
        public void Register_WhenRegisteringWithRegistrationConflictBehavior_Keep_ShouldResolveInitialRegistration()
        {
            Container sut = createSut();
            sut.RegisterSingleton<IEmailService, NullEmailService>(RegistrationConflictBehavior.Keep);
            sut.RegisterSingleton<IEmailService, DisposableEmailService>(RegistrationConflictBehavior.Keep);

            var emailService = sut.Resolve<IEmailService>();

            Assert.IsType<NullEmailService>(emailService);
        }

        [Fact]
        public void Register_WhenRegisteringWithRegistrationConflictBehavior_Replace_ShouldResolveLastRegistration()
        {
            Container sut = createSut();
            sut.RegisterSingleton<IEmailService, NullEmailService>();
            sut.RegisterSingleton<IEmailService, DisposableEmailService>(RegistrationConflictBehavior.Replace);

            var emailService = sut.Resolve<IEmailService>();

            Assert.IsType<DisposableEmailService>(emailService);
        }

        [Fact]
        public void Register_WhenRegisteringService_WithNullServiceType_ShouldThrowArgumentNullException()
        {
            Container sut = createSut();

            Assert.Throws<ArgumentNullException>(() => sut.Register(null, typeof(NullEmailService)));
        }

        [Fact]
        public void Register_WhenRegisteringService_WithNullImplementationType_ShouldThrowArgumentNullException()
        {
            Container sut = createSut();

            Assert.Throws<ArgumentNullException>(() => sut.Register(typeof(IEmailService), null));
        }

        [Fact]
        public void Resolve_WhenResolvingService_WithServiceTypeEqualToNull_ShouldThrowArgumentNullException()
        {
            Container sut = createSut();

            Assert.Throws<ArgumentNullException>(() => sut.Resolve(null));
        }

        [Fact]
        public void Resolve_WhenCreatingInstancesWithCircularDependencies_ShouldThrowInvalidOperationException()
        {
            Container sut = createSut();
            sut.Register<A>();
            sut.Register<B>();

            Assert.Throws<InvalidOperationException>(() =>
                sut.Resolve<A>());
        }

        [Fact]
        public void Resolve_WithTypeNotRegistered_ShouldThrowInvalidOperationException()
        {
            Container sut = createSut();

            Assert.Throws<InvalidOperationException>(() =>
                sut.Resolve<IEmailService>());
        }

        [Fact]
        public void Resolve_WithConstructorSelector_ThatReturnsNull_ShouldThrowArgumentNullException()
        {
            var sut = new Container(new ContainerOptions
            {
                ConstructorSelector = new NullConstructorSelector(),
            });

            sut.Register<IEmailService, NullEmailService>();

            Assert.Throws<ArgumentNullException>(() => sut.Resolve<IEmailService>());
        }

        /// <summary>
        /// Using the 'mother' pattern to create the SUT - Software under test.
        /// If the creation of the container requires different combinations then
        /// a builder pattern would be better.
        /// </summary>
        /// <returns></returns>
        private static Container createSut()
        {
            return new Container();
        }

        private class NullConstructorSelector : IConstructorSelector
        {
            public ConstructorInfo SelectConstructor(Type type)
            {
                return null;
            }
        }
    }
}
