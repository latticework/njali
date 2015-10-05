using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Routing;
using Jali.Serve.Definition;

namespace Jali.Serve.AspNet.Mvc
{
    public class JaliHttpRoute : IHttpRoute
    {
        public JaliHttpRoute(Service service)
        {
            this.Handler = new JaliHttpMessageHandler(service);
        }

        public IHttpRouteData GetRouteData(string virtualPathRoot, HttpRequestMessage request)
        {
            throw new NotImplementedException();
        }

        public IHttpVirtualPathData GetVirtualPath(HttpRequestMessage request, IDictionary<string, object> values)
        {
            return null;
        }

        public string RouteTemplate => string.Empty;
        public IDictionary<string, object> Defaults => _empty;
        public IDictionary<string, object> Constraints => _empty;
        public IDictionary<string, object> DataTokens => _empty;
        public HttpMessageHandler Handler { get; }

        private readonly IDictionary<string, object>  _empty = new Dictionary<string, object>();
    }
}
