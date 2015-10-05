using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
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
            throw new NotImplementedException();
        }
    }
}
