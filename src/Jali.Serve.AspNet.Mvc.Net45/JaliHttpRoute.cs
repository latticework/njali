using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http.Routing;
using Jali.Core;
using Jali.Note;
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
            var baseUrl = ((JaliHttpMessageHandler) this.Handler).Service.Definition.Url;

            var rootUrl = (baseUrl.IsAbsoluteUri)
                ? baseUrl.GetComponents(UriComponents.Path, UriFormat.Unescaped)
                : baseUrl.ToString();

            var uri = request.RequestUri;

            // TODO: JaliHttpRoute.GetRoutData: Consider scheme check in JaliParse, itself.
            if (!(uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
            {
                return null;
            }

            var result = uri.JaliParse(rootUrl);

            // TODO: JaliHttpRoute.GetRoutData: Improve error handling.
            //if (result.Messages.GetSeverity() == MessageSeverity.Critical)
            //{
            //    throw new NotificationMessageException(result.Messages);
            //}

            //if (result.Messages.GetSeverity() == MessageSeverity.Error)
            if (result.Messages.HasErrors())
            {
                // TODO: JaliHttpRoute.GetRoutData: Debug log errors.
                return null;
            }

            return new HttpRouteData(this);
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
