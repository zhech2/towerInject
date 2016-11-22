using System;
using System.Linq;
using Xunit;

namespace TowerInject.Tests
{
    public class ReflectionFactoryTests
    {
        [Fact]
        public void Create_WithNullConstructor_ShouldThrowArgumentNullException()
        {
            ReflectionFactory sut = createSut();

            Assert.Throws<ArgumentNullException>(() => sut.Create(null, null));
        }

        [Fact]
        public void Create_WithNullInstanceResolvers_ShouldNotThrow()
        {
            ReflectionFactory sut = createSut();

            try
            {
                sut.Create(typeof(NullLogger).GetConstructors().FirstOrDefault(), null);
            }
            catch (Exception)
            {
                Assert.True(false, "Should not throw exception");
            }
        }

        private ReflectionFactory createSut()
        {
            return new ReflectionFactory();
        }
    }
}
