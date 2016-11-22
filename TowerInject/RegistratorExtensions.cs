using System;

namespace TowerInject
{
    /// <summary>
    /// Extension methods to provide easier methods for registration.  Keeps the <see cref="IRegistrator"/> interface slim.
    /// </summary>
    public static class RegistratorExtensions
    {
        /// <summary>
        /// Registers a service.
        /// </summary>
        /// <param name="registrator">The registrator.</param>
        /// <param name="serviceType">The type, base type or interface of the service to register.</param>
        /// <param name="implementationType">The implementation of the serviceType.</param>
        public static void Register(this IRegistrator registrator,
           Type serviceType,
           Type implementationType)
        {
            registrator.Register(serviceType, implementationType, null);
        }

        /// <summary>
        /// Registers a service.
        /// </summary>
        /// <param name="registrator">The registrator.</param>
        /// <param name="serviceType">The type, base type or interface of the service to register.</param>
        /// <param name="implementationType">The implementation of the serviceType.</param>
        /// <param name="lifecycle">The lifecycle used in the creating/locating the service.</param>
        public static void Register(this IRegistrator registrator,
          Type serviceType,
          Type implementationType,
          ILifecycle lifecycle)
        {
            registrator.Register(serviceType, implementationType, lifecycle, RegistrationConflictBehavior.Default);
        }

        /// <summary>
        /// Registers a service
        /// </summary>
        /// <typeparam name="TService">The type of the service to register</typeparam>
        /// <param name="registrator">The registrator.</param>
        public static void Register<TService>(this IRegistrator registrator)
        {
            registrator.Register<TService, TService>(null);
        }

        /// <summary>
        /// Registers a service
        /// </summary>
        /// <typeparam name="TService">The type of the service to register</typeparam>
        /// <param name="registrator">The registrator.</param>
        /// <param name="lifecycle">The lifecycle used in the creating/locating the service.</param>
        public static void Register<TService>(this IRegistrator registrator,
            ILifecycle lifecycle)
        {
            registrator.Register<TService, TService>(lifecycle, RegistrationConflictBehavior.Default);
        }

        /// <summary>
        /// Registers a service
        /// </summary>
        /// <typeparam name="TService">The type of the service to register</typeparam>
        /// <param name="registrator">The registrator.</param>
        /// <param name="lifecycle">The lifecycle used in the creating/locating the service.</param>
        /// <param name="behavior">The behavior to take when there is a duplicate registration.</param>
        public static void Register<TService>(this IRegistrator registrator,
            ILifecycle lifecycle,
            RegistrationConflictBehavior behavior)
        {
            registrator.Register<TService, TService>(lifecycle, behavior);
        }

        /// <summary>
        /// Registers a service.
        /// </summary>
        /// <typeparam name="TService">The type, base type or interface of the service.</typeparam>
        /// <typeparam name="TImplementation">The concrete implementation of the <typeparamref name="TService"/>.</typeparam>
        /// <param name="registrator">The registrator.</param>
        public static void Register<TService, TImplementation>(this IRegistrator registrator)
           where TImplementation : TService
        {
            registrator.Register<TService, TImplementation>(null);
        }

        /// <summary>
        /// Registers a service.
        /// </summary>
        /// <typeparam name="TService">The type, base type or interface of the service.</typeparam>
        /// <typeparam name="TImplementation">The concrete implementation of the <typeparamref name="TService"/>.</typeparam>
        /// <param name="registrator">The registrator.</param>
        /// <param name="lifecycle">The lifecycle used in the creating/locating the service.</param>
        public static void Register<TService, TImplementation>(this IRegistrator registrator,
            ILifecycle lifecycle)
            where TImplementation : TService
        {
            registrator.Register<TService, TImplementation>(lifecycle, RegistrationConflictBehavior.Default);
        }

        /// <summary>
        /// Registers a service.
        /// </summary>
        /// <typeparam name="TService">The type, base type or interface of the service.</typeparam>
        /// <typeparam name="TImplementation">The concrete implementation of the <typeparamref name="TService"/>.</typeparam>
        /// <param name="registrator">The registrator.</param>
        /// <param name="lifecycle">The lifecycle used in the creating/locating the service.</param>
        /// <param name="behavior">The behavior to take when there is a duplicate registration.</param>
        public static void Register<TService, TImplementation>(this IRegistrator registrator,
            ILifecycle lifecycle,
            RegistrationConflictBehavior behavior)
            where TImplementation : TService
        {
            registrator.Register(typeof(TService), typeof(TImplementation), lifecycle, behavior);
        }

        /// <summary>
        /// Registers a service.
        /// </summary>
        /// <typeparam name="TService">The type, base type or interface of the service.</typeparam>
        /// <typeparam name="TImplementation">The concrete implementation of the <typeparamref name="TService"/>.</typeparam>
        /// <param name="registrator">The registrator.</param>
        public static void RegisterSingleton<TService, TImplementation>(this IRegistrator registrator)
            where TImplementation : TService
        {
            registrator.Register(typeof(TService), typeof(TImplementation), Lifecycle.Singleton);
        }

        /// <summary>
        /// Registers a service.
        /// </summary>
        /// <typeparam name="TService">The type, base type or interface of the service.</typeparam>
        /// <typeparam name="TImplementation">The concrete implementation of the <typeparamref name="TService"/>.</typeparam>
        /// <param name="registrator">The registrator.</param>
        /// <param name="behavior">The behavior to take when there is a duplicate registration.</param>
        public static void RegisterSingleton<TService, TImplementation>(this IRegistrator registrator, RegistrationConflictBehavior behavior)
            where TImplementation : TService
        {
            registrator.Register(typeof(TService), typeof(TImplementation), Lifecycle.Singleton, behavior);
        }

        /// <summary>
        /// Registers a service.
        /// </summary>
        /// <typeparam name="TService">The type, base type or interface of the service.</typeparam>
        /// <typeparam name="TImplementation">The concrete implementation of the <typeparamref name="TService"/>.</typeparam>
        /// <param name="registrator">The registrator.</param>
        public static void RegisterTransient<TService, TImplementation>(this IRegistrator registrator)
          where TImplementation : TService
        {
            registrator.Register(typeof(TService), typeof(TImplementation), Lifecycle.Transient);
        }

        /// <summary>
        /// Registers a service.
        /// </summary>
        /// <typeparam name="TService">The type, base type or interface of the service.</typeparam>
        /// <typeparam name="TImplementation">The concrete implementation of the <typeparamref name="TService"/>.</typeparam>
        /// <param name="registrator">The registrator.</param>
        /// <param name="behavior">The behavior to take when there is a duplicate registration.</param>
        public static void RegisterTransient<TService, TImplementation>(this IRegistrator registrator, RegistrationConflictBehavior behavior)
            where TImplementation : TService
        {
            registrator.Register(typeof(TService), typeof(TImplementation), Lifecycle.Transient, behavior);
        }

        /// <summary>
        /// Registers a service.
        /// </summary>
        /// <typeparam name="TService">The type, base type or interface of the service.</typeparam>
        /// <typeparam name="TImplementation">The concrete implementation of the <typeparamref name="TService"/>.</typeparam>
        /// <param name="registrator">The registrator.</param>
        /// <param name="lifecycle">The lifecycle used in the creating/locating the service.</param>
        public static void Register<TService, TImplementation>(this IRegistrator registrator,
            LifecycleType lifecycleType)
        {
            registrator.Register<TService, TImplementation>(lifecycleType, RegistrationConflictBehavior.Default);
        }

        /// <summary>
        /// Registers a service.
        /// </summary>
        /// <typeparam name="TService">The type, base type or interface of the service.</typeparam>
        /// <typeparam name="TImplementation">The concrete implementation of the <typeparamref name="TService"/>.</typeparam>
        /// <param name="registrator">The registrator.</param>
        /// <param name="lifecycle">The lifecycle used in the creating/locating the service.</param>
        /// <param name="behavior">The behavior to take when there is a duplicate registration.</param>
        public static void Register<TService, TImplementation>(this IRegistrator registrator,
            LifecycleType lifecycleType,
            RegistrationConflictBehavior behavior)
        {
            registrator.Register(typeof(TService), typeof(TImplementation), Lifecycle.GetByType(lifecycleType), behavior);
        }
    }
}
