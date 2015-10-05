using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
            throw new NotImplementedException();
        }
    }
}
