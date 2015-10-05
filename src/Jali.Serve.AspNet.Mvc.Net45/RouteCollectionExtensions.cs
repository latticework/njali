using System.Linq;
using Jali.Serve.AspNet.Mvc;
using Jali.Serve.Definition;

namespace System.Web.Routing
{
    public static class RouteCollectionExtensions
    {
        public static readonly string RouteName = "JaliMvcRoute_" + Guid.NewGuid().ToString("D");

        public static void UseJaliService(this RouteCollection routes, Service service)
        {
            if (routes == null)
            {
                throw new ArgumentNullException(nameof(routes));
            }

            var found = routes.SingleOrDefault(r => r.GetType() == typeof (JaliMvcRouteHandler)) != null;

            if (found)
            {
                throw new InvalidOperationException(
                    "Jali MVC integration already established. 'UseJaliService' may only be called once.");
            }

            var handler = new JaliMvcRouteHandler(service);

            var route = new Route(url: "", defaults: null, constraints: null, dataTokens: null, routeHandler: handler);

            routes.Add(RouteCollectionExtensions.RouteName, route);
        }
    }
}
