using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Dispatcher;

namespace TowerInject.WebApi
{
    public static class ContainerExtensions
    {
        public static void RegisterWebApiControllers(this IContainer container, HttpConfiguration configuration)
        {
            var assembliesResolver = configuration.Services.GetAssembliesResolver();
            if (assembliesResolver == null)
            {
                throw new InvalidOperationException("Could not find AssembliesResolver");
            }

            var typeResolver = configuration.Services.GetHttpControllerTypeResolver();

            var apiControllerType = typeof(ApiController);

            var assemblies = assembliesResolver.GetAssemblies();
            var cachedAssembliesResolver = new AssembliesResolver(assemblies.ToList());

            var apiControllerTypes = typeResolver.GetControllerTypes(cachedAssembliesResolver).Select(t => t);

            foreach (var type in apiControllerTypes)
            {
                container.Register(type, type);
            }
        }

        private class AssembliesResolver : IAssembliesResolver
        {
            private readonly ICollection<Assembly> _assemblies;

            public AssembliesResolver(ICollection<Assembly> assemblies)
            {
                _assemblies = assemblies;
            }

            public ICollection<Assembly> GetAssemblies()
            {
                return _assemblies;
            }
        }
    }
}
