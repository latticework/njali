using System.Web;
using System.Web.Http.WebHost;
using System.Web.Routing;

namespace Jali.Serve.AspNet.Mvc
{
    public class JaliMvcRouteHandler : IRouteHandler
    {
        public JaliMvcRouteHandler(IService service)
        {
            this.Service = service;
        }

        public IService Service { get; }

        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new HttpControllerHandler(new RouteData(), new JaliHttpMessageHandler(this.Service));
        }
    }
}
