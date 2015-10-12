using System.Net.Http;
using Jali.Serve.AspNet.Mvc;

namespace Jali.Serve
{
    public static class JaliMvcServiceMessageExtensions
    {
        public static HttpResponseMessage AsResponse(this IServiceMessage message, HttpRequestMessage request)
        {
            var builder = new JaliHttpMessageResponseBuilder(message, request);

            builder.Build();

            return builder.Response;
        }
    }
}
