using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Jali.Serve.MessageConversion
{
    /// <summary>
    ///     Represents a utility that converts between an http request, http response, and service message credentials. 
    ///     This implementation performs no conversions.
    /// </summary>
    public class DefaultMessageCredentialConverter : IMessageCredentialConverter
    {
        /// <summary>
        ///     Converts from an http request to service message credentials. This implementation performs no 
        /// conversions.
        /// </summary>
        /// <param name="request">
        ///     The http request.
        /// </param>
        /// <param name="message">
        ///     The partially constructed request service message. The message should not be modified directly.
        /// </param>
        /// <returns>
        ///     The request service <see cref="MessageCredentials"/> or <see langword="null"/> if the message should 
        ///     remain unmodified.
        /// </returns>
        public async Task<MessageCredentials> FromRequest(HttpRequestMessage request, ServiceMessage<JObject> message)
        {
            return await Task.FromResult<MessageCredentials>(null);
        }

        /// <summary>
        ///     Uses response service message credentials to modify an http response. This implementation performs no 
        ///     conversions.
        /// </summary>
        /// <param name="contract">
        ///     The response service message contract.
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
        public async Task<bool> ToResponse(
            MessageCredentials contract,
            HttpRequestMessage request,
            IServiceMessage message,
            HttpResponseMessage response)
        {
            return await Task.FromResult(false);
        }
    }
}