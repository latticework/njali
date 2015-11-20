using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
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

            var claims = WindowsIdentity.GetCurrent()?.Claims.Select(c => new Jali.Secure.Claim(c.Type, c.Value));

            if (claims == null)
            {
                throw new InternalErrorException("The AppPool identity is not assigned. Cannot create Jali Service");
            }

            var identity = new SecurityIdentity(claims);
            var context = new DefaultExecutionContext(identity);

            config.UseJaliService(context, async ctx => await Task.FromResult(new HelloService(ctx)),  null);

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
