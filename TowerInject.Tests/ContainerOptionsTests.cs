using Xunit;

namespace TowerInject.Tests
{
    public class ContainerOptionsTests
    {
        [Fact]
        public void EnsureDefaults_WithNullDefaultLifecycle_ShouldDefaultToTransientLifecycle()
        {
            ContainerOptions sut = createSut();

            sut.DefaultLifecycle = null;
            sut.EnsureDefaults();

            Assert.IsType<TransientLifecycle>(sut.DefaultLifecycle);
        }

        [Fact]
        public void EnsureDefaults_WithNullConstructorSelector()
        {
            ContainerOptions sut = createSut();

            sut.ConstructorSelector = null;
            sut.EnsureDefaults();

            Assert.IsType<SinglePublicConstructorSelector>(sut.ConstructorSelector);
        }

        private static ContainerOptions createSut()
        {
            return new ContainerOptions();
        }
    }
}
