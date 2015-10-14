using System.Web;
using System.Web.Http.WebHost;
using System.Web.Routing;
using Jali.Serve.Definition;

namespace Jali.Serve.AspNet.Mvc
{
    public class JaliMvcRouteHandler : IRouteHandler
    {
        public JaliMvcRouteHandler(Service service)
        {
            this.Service = service;
        }

        public Service Service { get; }

        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new HttpControllerHandler(new RouteData(), new JaliHttpMessageHandler(this.Service));
        }
    }
}
