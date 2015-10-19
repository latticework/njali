using System.Web;
using System.Web.Http.WebHost;
using System.Web.Routing;
using Jali.Serve.Server;

namespace Jali.Serve.AspNet.Mvc
{
    public class JaliMvcRouteHandler : IRouteHandler
    {
        public JaliMvcRouteHandler(IService service, JaliServerOptions options)
        {
            this.Service = service;
            this.Options = options;
        }

        public JaliServerOptions Options { get; }
        public IService Service { get; }

        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new HttpControllerHandler(new RouteData(), new JaliHttpMessageHandler(this.Service, this.Options));
        }
    }
}
