 // ReSharper disable once CheckNamespace

using Jali.Serve.AspNet.Mvc;
using Jali.Serve.Definition;

namespace System.Web.Http
{
    public static class JaliHttpConfigurationExtensions
    {
        public static readonly string RouteName = "JaliHttpRoute_" + Guid.NewGuid().ToString("D");

        public static void UseJaliService(this HttpConfiguration configuration, Service service)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var route = new JaliHttpRoute(service);

            configuration.Routes.Add(RouteName, route);
        }
    }
}