 // ReSharper disable once CheckNamespace

using System.Threading.Tasks;
using Jali;
using Jali.Serve;
using Jali.Serve.AspNet.Mvc;
using Jali.Serve.Server;

namespace System.Web.Http
{
    public static class JaliHttpConfigurationExtensions
    {
        public static readonly string RouteName = "JaliHttpRoute_" + Guid.NewGuid().ToString("D");

        public static void UseJaliService(
            this HttpConfiguration configuration, 
            IExecutionContext context, 
            Func<IServiceContext, Task<IService>> assignNewService, 
            JaliServerOptions options)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var route = new JaliHttpRoute(context, assignNewService, options);

            configuration.Routes.Add(RouteName, route);
        }
    }
}