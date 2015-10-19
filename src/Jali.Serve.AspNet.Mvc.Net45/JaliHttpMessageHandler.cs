using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Jali.Serve.Server;

namespace Jali.Serve.AspNet.Mvc
{
    public class JaliHttpMessageHandler : HttpMessageHandler
    {
        public JaliHttpMessageHandler(IService service, JaliServerOptions options)
        {
            this.Service = service;
            this.Server = new JaliServer(this.Service, options);

            // TODO: JaliHttpMessageHandler.ctor: this should be handled lazily on first request.
            this.Server.Run(CancellationToken.None).Wait();
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
