namespace TowerInject
{
    public static class ResolverExtensions
    {
        public static T Resolve<T>(this IResolver resolver)
        {
            return (T)resolver.Resolve(typeof(T));
        }
    }
}
