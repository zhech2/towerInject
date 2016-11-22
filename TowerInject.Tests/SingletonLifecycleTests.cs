using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;

namespace TowerInject.Tests
{
    public class SingletonLifecycleTests
    {
        [Fact]
        public void CreateRegistration_WhenRegistering_WithNullServiceType_ShouldThrowArgumentNullException()
        {
            SingletonLifecycle sut = createSut();

            Assert.Throws<ArgumentNullException>(() => sut.CreateRegistration(null, typeof(NullEmailService)));
        }

        [Fact]
        public void CreateRegistration_WhenRegistering_WithNullImplementationType_ShouldThrowArgumentNullException()
        {
            SingletonLifecycle sut = createSut();

            Assert.Throws<ArgumentNullException>(() => sut.CreateRegistration(typeof(IEmailService), null));
        }

        [Fact]
        public void CreateInstanceResolver_WithNullFactory_ShouldThrowArgumentNullException()
        {
            SingletonLifecycle sut = createSut();
            IRegistration registration = createRegistration(sut);
            var constructor = getConstructor();

            Assert.Throws<ArgumentNullException>(() =>
                sut.CreateInstanceResolver(null, registration, constructor, Enumerable.Empty<IInstanceResolver>()));
        }

        [Fact]
        public void CreateInstanceResolver_WithNullConstructor_ShouldThrowArgumentNullException()
        {
            SingletonLifecycle sut = createSut();
            IRegistration registration = createRegistration(sut);

            Assert.Throws<ArgumentNullException>(() =>
                sut.CreateInstanceResolver(createFactory(), registration, null, Enumerable.Empty<IInstanceResolver>()));
        }

        [Fact]
        public void CreateInstanceResolver_WithFactoryThatReturnsNull_ShouldThrowInvalidOperationException()
        {
            SingletonLifecycle sut = createSut();
            IRegistration registration = createRegistration(sut);
            var constructor = getConstructor();

            Assert.Throws<InvalidOperationException>(() =>
                sut.CreateInstanceResolver(new NullFactory(), registration, constructor, Enumerable.Empty<IInstanceResolver>()));
        }

        [Fact]
        public void CreateInstanceResolver_WithNullParamInstanceResolvers_ShouldReturnValidInstanceResolver()
        {
            SingletonLifecycle sut = createSut();
            IRegistration registration = createRegistration(sut);
            var constructor = getConstructor();

            Assert.NotNull(sut.CreateInstanceResolver(createFactory(), registration, constructor, null));
        }

        private static ReflectionFactory createFactory()
        {
            return new ReflectionFactory();
        }

        private static System.Reflection.ConstructorInfo getConstructor()
        {
            return typeof(NullEmailService).GetConstructors().FirstOrDefault();
        }

        private static IRegistration createRegistration(SingletonLifecycle sut)
        {
            return sut.CreateRegistration(typeof(IEmailService), typeof(NullEmailService));
        }

        private SingletonLifecycle createSut()
        {
            return new SingletonLifecycle();
        }

        private class NullFactory : IFactory
        {
            public Func<object> Create(ConstructorInfo constructor, IEnumerable<IInstanceResolver> paramInstanceResolvers)
            {
                return null;
            }
        }
    }
}
