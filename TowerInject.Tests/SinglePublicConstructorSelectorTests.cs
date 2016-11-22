using System;
using Xunit;

namespace TowerInject.Tests
{
    public class SinglePublicConstructorSelectorTests
    {
        [Fact]
        public void SelectConstructor_WithNullType_ShouldThrowArgumentNullException()
        {
            SinglePublicConstructorSelector sut = createSut();

            Assert.Throws<ArgumentNullException>(() => sut.SelectConstructor(null));
        }

        [Fact]
        public void SelectConstructor_TypeWithNoPublicConstructors_ShouldThrowInvalidOperationException()
        {
            SinglePublicConstructorSelector sut = createSut();

            Assert.Throws<InvalidOperationException>(() => sut.SelectConstructor(typeof(ClassNoPublicCtors)));
        }

        [Fact]
        public void SelectConstructor_TypeWithNoMultipleConstructors_ShouldThrowInvalidOperationException()
        {
            SinglePublicConstructorSelector sut = createSut();

            Assert.Throws<InvalidOperationException>(() => sut.SelectConstructor(typeof(ClassMultipleConstructors)));
        }

        private static SinglePublicConstructorSelector createSut()
        {
            return new SinglePublicConstructorSelector();
        }       

        private class ClassMultipleConstructors
        {
            public ClassMultipleConstructors()
            {

            }

            public ClassMultipleConstructors(int value)
            {

            }
        }
    }
}
