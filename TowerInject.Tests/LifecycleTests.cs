using Xunit;

namespace TowerInject.Tests
{
    public class LifecycleTests
    {
        [Fact]
        public void GetByType_WithLifecycleType_Singleton_ShouldReturnSingletonLifecycle()
        {
            var singleton = Lifecycle.GetByType(LifecycleType.Singleton);

            Assert.IsType<SingletonLifecycle>(singleton);
        }

        [Fact]
        public void GetByType_WithLifecycleType_Transient_ShouldReturnTransientLifecycle()
        {
            var singleton = Lifecycle.GetByType(LifecycleType.Transient);

            Assert.IsType<TransientLifecycle>(singleton);
        }

        [Fact]
        public void GetByType_WithLifecycleType_Default_ShouldReturnNull()
        {
            var lifecycle = Lifecycle.GetByType(LifecycleType.Default);

            Assert.Null(lifecycle);
        }
    }
}
