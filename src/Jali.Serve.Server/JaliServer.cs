using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Jali.Serve.Server.ServiceDescription;

namespace Jali.Serve.Server
{
    using JaliSchemaReference = Definition.SchemaReference;

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

            var jaliServiceDefinition = JaliService.GetDefinition();
            var jaliService = new JaliService(jaliServiceDefinition);
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
                var getServiceDescriptionRequest = new ServiceMessage<GetServiceDescriptionRequest>
                {
                    
                };

                var result = await this._jaliServiceManager.SendMethod(
                    "servicedescription", "GET", getServiceDescriptionRequest);
            }


            //request.RequestUri
            return await Task.FromResult((HttpResponseMessage) null);
        }

        //public Task<ServiceMessage> GetServiceDescription(
        //    ServiceMessage getServiceMessageRequest)
        //{

        //    var response = getServiceMessageRequest.CreateFromMessage(null, null);

        //    return Task.FromResult(response);
        //}

        private ServiceManager _jaliServiceManager;
        private ServiceManager _serviceManager;
    }
}