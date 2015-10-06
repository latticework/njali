using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Jali.Core;
using Jali.Serve.AspNet.Mvc;

namespace Jali.Serve
{
    public static class JaliMvcServiceMessageExtensions
    {
        public static HttpResponseMessage AsResponse(this ServiceMessage message, HttpRequestMessage request)
        {
            var builder = new JaliHttpMessageResponseBuilder(message, request);

            builder.Build();

            return builder.Response;
        }
    }
}
