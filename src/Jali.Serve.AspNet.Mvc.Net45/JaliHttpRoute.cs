using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http.Routing;
using Jali.Serve.Server;

namespace Jali.Serve.AspNet.Mvc
{
    public class JaliHttpRoute : IHttpRoute
    {
        public JaliHttpRoute(
            IExecutionContext context, Func<IServiceContext, Task<IService>> assignNewService, JaliServerOptions options)
        {
            this.Handler = new JaliHttpMessageHandler(context, assignNewService, options);
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


            if (string.Equals(components, "/", StringComparison.OrdinalIgnoreCase) && request.Method.Method == "GET")
            {
                return routeData;
            }

            if (Regex.IsMatch(components, 
@"^resources/(?<resourceName>[_a-zA-Z][_a-zA-Z0-9]*)(/(?<resourceKey>[_a-zA-Z0-9]+))?$"))
            {
                return routeData;
            }

            if (Regex.IsMatch(components,
@"^resources/(?<resourceName>[_a-zA-Z][_a-zA-Z0-9]*)(/(?<resourceKey>[_a-zA-Z0-9]+))?/routines/(?<routineName>[_a-zA-Z][_a-zA-Z0-9]*)$"))
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
