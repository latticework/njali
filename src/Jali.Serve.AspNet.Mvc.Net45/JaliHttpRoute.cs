using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http.Routing;
using System.Web.Routing;
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
            var uri = request.RequestUri;

            if (!(uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
            {
                return null;
            }

            var components = uri.GetComponents(UriComponents.Path, UriFormat.Unescaped);

            var routeData = new HttpRouteData(this);


            if (string.Equals(components, "/", StringComparison.InvariantCultureIgnoreCase))
            {
                return routeData;
            }

            if (Regex.IsMatch(components, @"^resources/[_a-zA-Z][_a-zA-Z0-9]*(/[_a-zA-Z0-9]+)?$"))
            {
                return routeData;
            }

            if (Regex.IsMatch(components, @"^resources/[_a-zA-Z][_a-zA-Z0-9]*(/[_a-zA-Z0-9]+)?/routines/[_a-zA-Z][_a-zA-Z0-9]*$"))
            {
                return routeData;
            }

            return null;
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
