using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Jali.Serve.Server.MessageConversion
{
    /// <summary>
    ///     A utility that converts between an http request, http response, and a service message connection. This 
    ///     implementation performs no conversions.
    /// </summary>
    public class DefaultMessageConnectionConverter : IMessageConnectionConverter
    {
        /// <summary>
        ///     Converts from an http request to a service message contract. This implementation performs no 
        ///     conversions.
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
        public Task<MessageConnection> FromRequest(HttpRequestMessage request, ServiceMessage<JObject> message)
        {
            return Task.FromResult<MessageConnection>(null);
        }

        /// <summary>
        ///     Uses a response service message connection to modify an http response. This implementation performs no 
        ///     conversions.
        /// </summary>
        /// <param name="connection">
        ///     The response service message connection.
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
        ///     A value indicating tha the http response was not modified.
        /// </returns>
        public Task<bool> ToResponse(
            MessageConnection connection,
            HttpRequestMessage request,
            IServiceMessage message,
            HttpResponseMessage response)
        {
            return Task.FromResult(false);
        }
    }
}