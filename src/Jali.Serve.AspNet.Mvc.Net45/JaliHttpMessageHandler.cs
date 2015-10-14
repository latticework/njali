using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Jali.Serve.Server;

namespace Jali.Serve.AspNet.Mvc
{
    public class JaliHttpMessageHandler : HttpMessageHandler
    {
        public JaliHttpMessageHandler(IService service)
        {
            this.Service = service;
            this.Server = new JaliServer(this.Service);
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
