namespace TowerInject
{
    /// <summary>
    /// Gets or creates instances based on the parent <see cref="Lifecycle"/>.
    /// </summary>
    public interface IInstanceResolver
    {
        /// <summary>
        /// Gets or creates an instance of a service.
        /// </summary>
        /// <returns>The service</returns>
        object Resolve();
    }
}
