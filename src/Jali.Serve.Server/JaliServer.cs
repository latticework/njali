using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Jali.Core;
using Jali.Secure;
using Jali.Serve.Server.MessageConversion;
using Jali.Serve.Server.ServiceDescription;
using Jali.Serve.Server.User;

namespace Jali.Serve.Server
{
    /// <summary>
    ///     The Jali Service host.
    /// </summary>
    public sealed class JaliServer : AsyncInitializedBase
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="JaliServer"/> class.
        /// </summary>
        /// <param name="assignNewService">
        ///     Assignes a new instance of a service to the server during <see cref="AsyncInitializedBase.Initialize"/>.
        /// </param>
        /// <param name="options">
        ///     Initialization options.
        /// </param>
        public JaliServer(
            Func<IServiceContext, Task<IService>> assignNewService,  JaliServerOptions options = null)
        {
            if (assignNewService == null) throw new ArgumentNullException(nameof(assignNewService));


            this.Running = false;

            var overrideOptions = options ?? new JaliServerOptions();

            this.Options = new JaliServerOptions
            {
                Authenticator = overrideOptions.Authenticator ?? new DefaultAuthenticator(GenerateRandom(32)),
                Authorizer = overrideOptions.Authorizer ?? new DefaultAuthorizor(),
                MessageConverter = overrideOptions.MessageConverter ?? new CompositeServiceMessageConverter(),
                KeyConverter = overrideOptions.KeyConverter ?? new DefaultResourceKeyConverter(),
                CorsOptions = overrideOptions.CorsOptions ?? new CorsOptions(),
            };

            this._jaliServiceManager = new ServiceManager(this, async ctx =>
            {
                await Task.FromResult(true);
                return new JaliService(this.Options, ctx);
            });

            this._serviceManager = new ServiceManager(this, assignNewService);
        }

        /// <summary>
        ///     The execution context of the Jali server.
        /// </summary>
        public IExecutionContext ExecutionContext { get; private set; }

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
        public IService Service { get; private set; }

        /// <summary>
        ///     Starts the host runtime.
        /// </summary>
        /// <param name="context">
        ///     The execution context.
        /// </param>
        /// <param name="ct">
        ///     A cancellation token.
        /// </param>
        /// <returns>
        ///     A <see cref="Task"/>.
        /// </returns>
        public async Task Run(IExecutionContext context, CancellationToken ct)
        {
            await EnsureInitialized();

            if (this.Running)
            {
                throw  new InvalidOperationException("The Jali Server is already running.");
            }

            await this._jaliServiceManager.Run(context);

            await this._serviceManager.Run(context);

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

            var baseUrl = this.Service.Definition.Url.ToString();

            var parseResult = await request.JaliParse(baseUrl);

            if (!parseResult.Succeeded)
            {
                throw DomainErrorException.CreateException(parseResult.Messages);
            }

            // Redirect to ServiceSecription resource.
            if (parseResult.ResourceName == string.Empty)
            {
                var redirectResponse = new HttpResponseMessage
                {
                    RequestMessage = request,
                    StatusCode = HttpStatusCode.Moved,
                    ReasonPhrase = $"{nameof(HttpStatusCode.Moved)}",
                };

                // TODO: JaliServer.Send: Handle servicedescription url better.
                redirectResponse.Headers.Location = new Uri(
                    request.RequestUri, $"resources/{ServiceDescriptionResource.Name}");

                return redirectResponse;
            }

            // TODO: JaliServer.Send: Make list of Jali Service resources.
            if (parseResult.ResourceName == ServiceDescriptionResource.Name ||
                parseResult.ResourceName == UserResource.Name)
            {

                var jaliResponse = await this._jaliServiceManager.SendMethod(
                    this.ExecutionContext, parseResult, request);

                return jaliResponse;

                // TODO: JaliServer.Send: Handle ServiceDescription HTML result.
                // TODO: JaliServer.Send: Determine correct default request service message content.
                // TODO: JaliServer.Send: Determine better way to pass typed ServiceMessages.
                //var typedResult = (ServiceMessage<GetServiceDescriptionResponse>)result;
                //return new HttpResponseMessage
                //{
                //    RequestMessage = request,
                //    StatusCode = HttpStatusCode.OK,
                //    ReasonPhrase = "OK",
                //    Content = new StringContent(typedResult.Data.Html, Encoding.UTF8, "text/html"),
                //};

            }
            var response = await this._serviceManager.SendMethod(this.ExecutionContext, parseResult, request);

            return response;
        }


        private readonly ServiceManager _jaliServiceManager;
        private readonly ServiceManager _serviceManager;

        // From https://github.com/dotnet/corefx/blob/master/src/System.Security.Cryptography.Algorithms/src/Internal/Cryptography/Helpers.cs
        private static byte[] GenerateRandom(int count)
        {
            // Use this for non portable implementations:
            // https://github.com/dotnet/corefx/blob/c02d33b18398199f6acc17d375dab154e9a1df66/src/System.Security.Cryptography.Algorithms/src/System/Security/Cryptography/RandomNumberGenerator.cs
            var bytes = new byte[count];
            new Random().NextBytes(bytes);

            return bytes;
        }

        /// <summary>
        ///     Provides the initialization process of the instance.
        /// </summary>
        /// <param name="context">
        ///     The execution context.
        /// </param>
        /// <returns>
        ///     A <see cref="Task"/> representing the instance initialization process.
        /// </returns>
        protected override async Task InitializeCore(IExecutionContext context)
        {
            // TODO: JaliServer.ctor: Verify that it's ok to set the server's execution context as a property even though it will cross threads.
            this.ExecutionContext = context;

            await this._jaliServiceManager.Initialize(context);
            await this._serviceManager.Initialize(context);
            this.Service = this._serviceManager.Service;
        }
    }
}