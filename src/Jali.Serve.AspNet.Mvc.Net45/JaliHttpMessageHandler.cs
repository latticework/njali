using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Jali.Serve.Server;

namespace Jali.Serve.AspNet.Mvc
{
    public class JaliHttpMessageHandler : HttpMessageHandler
    {
        public JaliHttpMessageHandler(
            IExecutionContext context, 
            Func<IServiceContext, Task<IService>> assignNewService, 
            JaliServerOptions options)
        {
            this.Server = new JaliServer(assignNewService, options);

            // TODO: JaliHttpMessageHandler.ctor: this should be handled lazily on first request.
            this.Server.Initialize(context).Wait();
            this.Service = this.Server.Service;

            this.Server.Run(context, CancellationToken.None).Wait();
        }

        public JaliServer Server { get; }

        public IService Service { get; }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return await this.Server.Send(request, cancellationToken);
        }
    }
}
