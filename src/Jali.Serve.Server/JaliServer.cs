using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Jali.Core;
using Jali.Serve.Server.ServiceDescription;

namespace Jali.Serve.Server
{
    public sealed class JaliServer
    {
        public JaliServer(IService service)
        {
            this.Running = false;
            this.Service = service;
        }

        public bool Running { get; private set; }

        public IService Service { get; }

        public async Task Run(CancellationToken ct)
        {
            if (this.Running)
            {
                throw  new InvalidOperationException("The 'JaliService' is already running.");
            }

            var jaliService = new JaliService();
            this._jaliServiceManager = new ServiceManager(jaliService);

            await this._jaliServiceManager.Run();

            this._serviceManager = new ServiceManager(this.Service);
            await this._serviceManager.Run();

            this.Running = true;

            await Task.FromResult(true);
        }

        // TODO: JaliServer: This may come in handy: http://www.strathweb.com/2015/01/migrating-asp-net-web-api-mvc-6-exploring-web-api-compatibility-shim/
        // How to get HttpRequest/Response in a PCL package: http://blogs.msdn.com/b/bclteam/archive/2013/02/18/portable-httpclient-for-net-framework-and-windows-phone.aspx
        public async Task<HttpResponseMessage> Send(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var uri = request.RequestUri;

            var components = uri.GetComponents(UriComponents.Path, UriFormat.Unescaped);

            if (string.Equals(components, "/", StringComparison.OrdinalIgnoreCase))
            {
                var getServiceDescriptionRequest = new ServiceMessage<GetServiceDescriptionRequest>();

                var result = await this._jaliServiceManager.SendMethod(
                    ServiceDescriptionResource.Name, "GET", getServiceDescriptionRequest);

                return result.AsResponse(request);
            }

            var resourceMatch = Regex.Match(
                components,
                @"^resources/(?<resourceName>[_a-zA-Z][_a-zA-Z0-9]*)(/(?<resourceKey>[_a-zA-Z0-9]+))?$");

            if (resourceMatch.Success)
            {
                var resourceName = resourceMatch.Groups["resourceName"].Value;
                var resourceKey = resourceMatch.Groups["resourceKey"].Value;

                var requestMessage = request.AsServiceMessage();

                var result = await this._serviceManager.SendMethod(resourceName, request.Method.Method, requestMessage);

                return result.AsResponse(request);
            }


            var routeMatch = Regex.Match(
                components,
                @"^resources/(?<resourceName>[_a-zA-Z][_a-zA-Z0-9]*)(/(?<resourceKey>[_a-zA-Z0-9]+))?/routines/(?<routineName>[_a-zA-Z][_a-zA-Z0-9]*)$");

            if (routeMatch.Success)
            {
                throw new NotImplementedException("Jali Server has not yet implemented direct route message support.");
            }

            var message = $"Jali Server cannot handle route HTTP {request.Method.Method} {request.RequestUri}. The request should not have been forwarded to it.";
            throw new InternalErrorException(message);
        }


        private ServiceManager _jaliServiceManager;
        private ServiceManager _serviceManager;
    }
}