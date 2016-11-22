using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;

namespace TowerInject.Tests
{
    public class TransientLifecycleTests
    {
        [Fact]
        public void CreateRegistration_WhenRegistering_WithNullServiceType_ShouldThrowArgumentNullException()
        {
            TransientLifecycle sut = createSut();

            Assert.Throws<ArgumentNullException>(() => sut.CreateRegistration(null, typeof(NullEmailService)));
        }

        [Fact]
        public void CreateRegistration_WhenRegistering_WithNullImplementationType_ShouldThrowArgumentNullException()
        {
            TransientLifecycle sut = createSut();

            Assert.Throws<ArgumentNullException>(() => sut.CreateRegistration(typeof(IEmailService), null));
        }

        [Fact]
        public void CreateInstanceResolver_WithNullFactory_ShouldThrowArgumentNullException()
        {
            TransientLifecycle sut = createSut();
            IRegistration registration = createRegistration(sut);
            var constructor = getConstructor();

            Assert.Throws<ArgumentNullException>(() =>
                sut.CreateInstanceResolver(null, registration, constructor, Enumerable.Empty<IInstanceResolver>()));
        }

        [Fact]
        public void CreateInstanceResolver_WithNullConstructor_ShouldThrowArgumentNullException()
        {
            TransientLifecycle sut = createSut();
            IRegistration registration = createRegistration(sut);

            Assert.Throws<ArgumentNullException>(() =>
                sut.CreateInstanceResolver(createFactory(), registration, null, Enumerable.Empty<IInstanceResolver>()));
        }

        [Fact]
        public void CreateInstanceResolver_WithFactoryThatReturnsNull_ShouldThrowInvalidOperationException()
        {
            TransientLifecycle sut = createSut();
            IRegistration registration = createRegistration(sut);
            var constructor = getConstructor();

            Assert.Throws<InvalidOperationException>(() =>
                sut.CreateInstanceResolver(new NullFactory(), registration, constructor, Enumerable.Empty<IInstanceResolver>()));
        }

        [Fact]
        public void CreateInstanceResolver_WithNullParamInstanceResolvers_ShouldReturnValidInstanceResolver()
        {
            TransientLifecycle sut = createSut();
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

        private static IRegistration createRegistration(TransientLifecycle sut)
        {
            return sut.CreateRegistration(typeof(IEmailService), typeof(NullEmailService));
        }

        private TransientLifecycle createSut()
        {
            return new TransientLifecycle();
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
