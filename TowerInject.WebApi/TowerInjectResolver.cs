using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;

namespace TowerInject.WebApi
{
    public class TowerInjectResolver : IDependencyResolver
    {
        private readonly IContainer _container;
        private readonly IServiceProvider _serviceProvider;

        public TowerInjectResolver(IContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            _container = container;
            _serviceProvider = (IServiceProvider)_container;
        }

        public IDependencyScope BeginScope()
        {
            return new DependencyScope(_serviceProvider);
        }

        public void Dispose()
        {
            _container.Dispose();
        }

        public object GetService(Type serviceType)
        {
            return _serviceProvider.GetService(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return Enumerable.Empty<object>();
        }

        private class DependencyScope : IDependencyScope
        {
            private readonly IServiceProvider _serviceProvider;

            public DependencyScope(IServiceProvider serviceProvider)
            {
                _serviceProvider = serviceProvider;
            }

            public void Dispose()
            {
                // TODO: need to implement IScope for container
            }

            public object GetService(Type serviceType)
            {
                return _serviceProvider.GetService(serviceType);
            }

            public IEnumerable<object> GetServices(Type serviceType)
            {
                return Enumerable.Empty<object>();
            }
        }
    }
}
