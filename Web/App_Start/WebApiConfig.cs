using System.Web.Http;
using TowerInject;
using TowerInject.WebApi;
using Web.Services;

namespace Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            Container container = createCompositionRootContainer(config);

            var resolver = new TowerInjectResolver(container);

            config.DependencyResolver = resolver;

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }

        private static Container createCompositionRootContainer(HttpConfiguration config)
        {
            var container = new Container();
            container.RegisterWebApiControllers(config);
            container.RegisterSingleton<IBookingService, BookingService>();
            container.RegisterSingleton<ILogger, Logger>();
            container.Register<IEmailService, EmailService>();

            return container;
        }
    }
}
