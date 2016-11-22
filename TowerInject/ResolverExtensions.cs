namespace TowerInject
{
    /// <summary>
    /// Helpers fo resolving services.
    /// </summary>
    public static class ResolverExtensions
    {
        /// <summary>
        /// Resolves the service for the given <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of service to resolve.</typeparam>
        /// <param name="resolver">The resolver.</param>
        /// <returns></returns>
        public static T Resolve<T>(this IResolver resolver)
        {
            return (T)resolver.Resolve(typeof(T));
        }
    }
}
