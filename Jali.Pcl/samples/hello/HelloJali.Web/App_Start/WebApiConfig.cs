using System;
using System.Linq;
using System.Security.Policy;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using Jali;
using Jali.Core;
using Jali.Secure;
using Jali.Serve.Samples.HelloServices;
using Jali.Serve.Server;

namespace HelloJali.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            var context = new AspNetExecutionContext();

            var absolutePath = VirtualPathUtility.ToAbsolute("~");
            var baseUri = new Uri(absolutePath, UriKind.Relative);

            var options = new JaliServerOptions
            {
                CorsOptions = new CorsOptions
                {
                    SupportsCors = true,
                    AllowAllOrigins = true,
                    SupportsCredentials = true,
                },
            };

            config.UseJaliService(context, async ctx => await Task.FromResult(new HelloService(ctx, baseUri)), options);

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
