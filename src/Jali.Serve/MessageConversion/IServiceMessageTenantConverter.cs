using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Jali.Serve.MessageConversion
{
    /// <summary>
    ///     Represents a utility that converts between an http request, http response, and a service message tenant.
    /// </summary>
    public interface IServiceMessageTenantConverter
    {
        /// <summary>
        ///     Converts from an http request to a service message tenant.
        /// </summary>
        /// <param name="request">
        ///     The http request.
        /// </param>
        /// <param name="message">
        ///     The partially constructed request service message. The message should not be modified directly.
        /// </param>
        /// <returns>
        ///     The request service message <see cref="TenantIdentity"/> or <see langword="null"/> if the message 
        ///     should remain unmodified.
        /// </returns>
        Task<TenantIdentity> FromRequest(HttpRequestMessage request, ServiceMessage<JObject> message);

        /// <summary>
        ///     Uses a response service message tenant to modify an http response.
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
        ///     A value indicating whether the converter modified the http response.
        /// </returns>
        Task<bool> ToResponse(
            TenantIdentity tenant,
            HttpRequestMessage request,
            IServiceMessage message,
            HttpResponseMessage response);
    }
}