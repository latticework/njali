using System.Web;
using System.Web.Http.WebHost;
using System.Web.Routing;
using Jali.Serve.Server;

namespace Jali.Serve.AspNet.Mvc
{
    public class JaliMvcRouteHandler : IRouteHandler
    {
        public JaliMvcRouteHandler(IExecutionContext context, IService service, JaliServerOptions options)
        {
            this.Handler = new JaliHttpMessageHandler(context, service, options);
        }

        public JaliHttpMessageHandler Handler { get; }

        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new HttpControllerHandler(new RouteData(), this.Handler);
        }
    }
}
