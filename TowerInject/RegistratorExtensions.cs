using System;

namespace TowerInject
{
    public static class RegistratorExtensions
    {
        public static void Register(this IRegistrator registrator, 
           Type serviceType,
           Type implementationType)
        {
            registrator.Register(serviceType, implementationType, null);
        }

        public static void Register(this IRegistrator registrator,
          Type serviceType,
          Type implementationType,
          ILifecycle lifecycle)
        {
            registrator.Register(serviceType, implementationType, lifecycle, RegistrationConflictBehavior.Default);
        }

        public static void Register<TService>(this IRegistrator registrator)
        {
            registrator.Register<TService, TService>(null);
        }

        public static void Register<TService>(this IRegistrator registrator, 
            ILifecycle lifecycle)
        {
            registrator.Register<TService, TService>(lifecycle, RegistrationConflictBehavior.Default);
        }

        public static void Register<TService>(this IRegistrator registrator,
            ILifecycle lifecycle,
            RegistrationConflictBehavior behavior)
        {
            registrator.Register<TService, TService>(lifecycle, behavior);
        }

        public static void Register<TService, TImplementation>(this IRegistrator registrator)
           where TImplementation : TService
        {
            registrator.Register<TService, TImplementation>(null);
        }

        public static void Register<TService, TImplementation>(this IRegistrator registrator,
            ILifecycle lifecycle)
            where TImplementation : TService
        {
            registrator.Register<TService, TImplementation>(lifecycle, RegistrationConflictBehavior.Default);
        }

        public static void Register<TService, TImplementation>(this IRegistrator registrator,
            ILifecycle lifecycle,
            RegistrationConflictBehavior behavior)
            where TImplementation : TService
        {
            registrator.Register(typeof(TService), typeof(TImplementation), lifecycle, behavior);
        }

        public static void RegisterSingleton<TService, TImplementation>(this IRegistrator registrator)
            where TImplementation : TService
        {
            registrator.Register(typeof(TService), typeof(TImplementation), Lifecycle.Singleton);
        }

        public static void Register<TService, TImplementation>(this IRegistrator registrator,
            LifecycleType lifecycleType)
        {
            registrator.Register<TService, TImplementation>(lifecycleType, RegistrationConflictBehavior.Default);
        }

        public static void Register<TService, TImplementation>(this IRegistrator registrator,
            LifecycleType lifecycleType,
            RegistrationConflictBehavior behavior)
        {
            registrator.Register(typeof(TService), typeof(TImplementation), Lifecycle.GetByType(lifecycleType), behavior);
        }
    }
}
