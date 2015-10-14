using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
//using System.Web.Hosting;
using Jali.Serve.Definition;

namespace Jali.Serve.AspNet.Mvc
{
    public class JaliHttpMessageHandler : HttpMessageHandler
    {
        public JaliHttpMessageHandler(Service service)
        {
            this.Service = service;
        }

        public Service Service { get; }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {

            var requestMessage = request.AsServiceMessage();



            var responseMessage = (IServiceMessage) null; // TODO: Magic happens here.

            return Task.FromResult(responseMessage.AsResponse(request));
            //HostingEnvironment.QueueBackgroundWorkItem();
            throw new NotImplementedException();
        }

        //private BufferBlock
    }
}
