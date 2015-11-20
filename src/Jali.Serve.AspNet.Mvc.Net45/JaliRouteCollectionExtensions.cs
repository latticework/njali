using System.Linq;
using System.Threading.Tasks;
using Jali;
using Jali.Serve;
using Jali.Serve.AspNet.Mvc;
using Jali.Serve.Server;

namespace System.Web.Routing
{
    public static class JaliRouteCollectionExtensions
    {
        public static readonly string RouteName = "JaliMvcRoute_" + Guid.NewGuid().ToString("D");

        public static void UseJaliService(this RouteCollection routes, 
            IExecutionContext context,
            Func<IServiceContext, Task<IService>> assignNewService,
            JaliServerOptions options)
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

            var handler = new JaliMvcRouteHandler(context, assignNewService, options);

            var route = new Route(url: "", defaults: null, constraints: null, dataTokens: null, routeHandler: handler);

            routes.Add(JaliRouteCollectionExtensions.RouteName, route);
        }
    }
}
