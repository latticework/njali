using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Jali.Serve.Server.MessageConversion
{
    /// <summary>
    ///     Represents a utility that converts between an http request, http response, and a service message tenant. 
    ///     This implementation performs no conversions.
    /// </summary>
    public class DefaultServiceMessageTenantConverter : IServiceMessageTenantConverter
    {
        /// <summary>
        ///     Converts from an http request to a service message tenant. This implementation performs no conversions.
        /// </summary>
        /// <param name="request">
        ///     The http request.
        /// </param>
        /// <param name="message">
        ///     The partially constructed request service message. The message should not be modified directly.
        /// </param>
        /// <returns>
        ///     <see langword="null"/> so the message remains unmodified.
        /// </returns>
        public Task<TenantIdentity> FromRequest(HttpRequestMessage request, ServiceMessage<JObject> message)
        {
            return Task.FromResult<TenantIdentity>(null);
        }

        /// <summary>
        ///     Uses a response service message tenant to modify an http response. This implementation performs no 
        ///     conversions.
        /// </summary>
        /// <param name="tenant">
        ///     The response service message tenant.
        /// </param>
        /// <param name="request">
        ///     The initial http request.
        /// </param>
        /// <param name="message">
        ///     The response service message.
        /// </param>
        /// <param name="response">
        ///     The partial constructed http response.
        /// </param>
        /// <returns>
        ///     A value indicating that the http response was not modified.
        /// </returns>
        public Task<bool> ToResponse(TenantIdentity tenant, HttpRequestMessage request, IServiceMessage message, HttpResponseMessage response)
        {
            return Task.FromResult(false);
        }
    }
}