using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Jali.Core;
using Jali.Serve.MessageConversion;
using Jali.Serve.Server.ServiceDescription;
using Newtonsoft.Json.Linq;

namespace Jali.Serve.Server
{
    /// <summary>
    ///     The Jali Service host.
    /// </summary>
    public sealed class JaliServer
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="JaliServer"/> class.
        /// </summary>
        /// <param name="service">
        ///     The Jali service being hosted.
        /// </param>
        /// <param name="options">
        ///     Initialization options.
        /// </param>
        public JaliServer(IService service, JaliServerOptions options)
        {
            this.Running = false;
            this.Service = service;

            var overrideOptions = options ?? new JaliServerOptions();

            this.Options = new JaliServerOptions
            {
                MessageConverter = overrideOptions.MessageConverter ?? new CompositeServiceMessageConverter(),
                KeyConverter = overrideOptions.KeyConverter ?? new DefaultResourceKeyConverter(),
            };
        }

        /// <summary>
        ///     Initialization options.
        /// </summary>
        public JaliServerOptions Options { get; }

        /// <summary>
        ///     Gets a value indicating whether the server is running.
        /// </summary>
        public bool Running { get; private set; }

        /// <summary>
        ///     The Jali service being hosted.
        /// </summary>
        public IService Service { get; }

        /// <summary>
        ///     Starts the host runtime.
        /// </summary>
        /// <param name="ct">
        ///     A cancellation token.
        /// </param>
        /// <returns>
        ///     A <see cref="Task"/>.
        /// </returns>
        public async Task Run(CancellationToken ct)
        {
            if (this.Running)
            {
                throw  new InvalidOperationException("The Jali Server is already running.");
            }

            var jaliService = new JaliService();
            this._jaliServiceManager = new ServiceManager(this, jaliService);

            await this._jaliServiceManager.Run();

            this._serviceManager = new ServiceManager(this, this.Service);
            await this._serviceManager.Run();

            this.Running = true;

            await Task.FromResult(true);
        }

        // TODO: JaliServer: This may come in handy: http://www.strathweb.com/2015/01/migrating-asp-net-web-api-mvc-6-exploring-web-api-compatibility-shim/
        // How to get HttpRequest/Response in a PCL package: http://blogs.msdn.com/b/bclteam/archive/2013/02/18/portable-httpclient-for-net-framework-and-windows-phone.aspx
        /// <summary>
        ///     Sends the specified Jali service message http request to the hosted service.
        /// </summary>
        /// <param name="request">
        ///     An http request configured to send Jali service messages.
        /// </param>
        /// <param name="cancellationToken">
        ///     Permits cancellation of the request.
        /// </param>
        /// <returns>
        ///     The http response from the hosted service.
        /// </returns>
        public async Task<HttpResponseMessage> Send(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var uri = request.RequestUri;

            var components = uri.GetComponents(UriComponents.Path, UriFormat.Unescaped);

            if (components == string.Empty)
            {
                // TODO: JaliServer.Send: Determine correct default request service message content.
                // TODO: JaliServer.Send: Determine better way to pass typed ServiceMessages.
                var getServiceDescriptionRequest = new ServiceMessage<JObject>
                {
                    Connection = new MessageConnection { },
                    Credentials = new MessageCredentials { },
                    Data = JObject.FromObject(new GetServiceDescriptionRequest
                    {
                        Service = this.Service.Definition,
                    }),
                    Contract = new MessageContract { },
                    Identity = new MessageIdentity { },
                    Tenant = new TenantIdentity { },
                };


                var result = await this._jaliServiceManager.SendMethod(
                    ServiceDescriptionResource.Name, "GET", getServiceDescriptionRequest);

                var typedResult = (ServiceMessage<GetServiceDescriptionResponse>) result;

                return new HttpResponseMessage
                {
                    RequestMessage = request,
                    StatusCode = HttpStatusCode.OK,
                    ReasonPhrase = "OK",
                    Content = new StringContent(typedResult.Data.Html, Encoding.UTF8, "text/html"),
                };
            }

            var resourceMatch = Regex.Match(
                components,
                @"^resources/(?<resourceName>[_a-zA-Z][_a-zA-Z0-9]*)(/(?<resourceKey>[_a-zA-Z0-9]+))?$");

            if (resourceMatch.Success)
            {
                var resourceName = resourceMatch.Groups["resourceName"].Value;
                var resourceKey = resourceMatch.Groups["resourceKey"].Value;

                var requestMessage = await this.Options.MessageConverter.FromRequest(request);

                var result = await this._serviceManager.SendMethod(
                    resourceName, request.Method.Method, requestMessage, resourceKey);

                return result.AsResponse(request);
            }


            var routeMatch = Regex.Match(
                components,
                @"^resources/(?<resourceName>[_a-zA-Z][_a-zA-Z0-9]*)(/(?<resourceKey>[_a-zA-Z0-9]+))?/routines/(?<routineName>[_a-zA-Z][_a-zA-Z0-9]*)$");

            if (routeMatch.Success)
            {
                throw new NotImplementedException("Jali Server has not yet implemented direct route message support.");
            }

            // TODO: JaliServer.Send: Review this error internal error message.
            var message = $"Jali Server cannot handle route HTTP {request.Method.Method} {request.RequestUri}. The request should not have been forwarded to it.";
            throw new InternalErrorException(message);
        }


        private ServiceManager _jaliServiceManager;
        private ServiceManager _serviceManager;
    }
}