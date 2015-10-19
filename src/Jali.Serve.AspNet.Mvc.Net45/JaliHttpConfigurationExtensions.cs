 // ReSharper disable once CheckNamespace

using Jali.Serve;
using Jali.Serve.AspNet.Mvc;
using Jali.Serve.Server;

namespace System.Web.Http
{
    public static class JaliHttpConfigurationExtensions
    {
        public static readonly string RouteName = "JaliHttpRoute_" + Guid.NewGuid().ToString("D");

        public static void UseJaliService(
            this HttpConfiguration configuration, IService service, JaliServerOptions options)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var route = new JaliHttpRoute(service, options);

            configuration.Routes.Add(RouteName, route);
        }
    }
}