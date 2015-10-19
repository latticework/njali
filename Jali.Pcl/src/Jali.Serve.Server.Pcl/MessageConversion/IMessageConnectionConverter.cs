using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Jali.Serve.Server.MessageConversion
{
    /// <summary>
    ///     Represents a utility that converts between an http request, http response, and a service message connection.
    /// </summary>
    public interface IMessageConnectionConverter
    {
        /// <summary>
        ///     Converts from an http request to a service message contract.
        /// </summary>
        /// <param name="request">
        ///     The http request.
        /// </param>
        /// <param name="message">
        ///     The partially constructed request service message. The message should not be modified directly.
        /// </param>
        /// <returns>
        ///     The request service <see cref="MessageConnection"/> or <see langword="null"/> if the message should 
        ///     remain unmodified.
        /// </returns>
        Task<MessageConnection> FromRequest(HttpRequestMessage request, ServiceMessage<JObject> message);

        /// <summary>
        ///     Uses a response service message connection to modify an http response.
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
        ///     A value indicating whether the converter modified the http response.
        /// </returns>
        Task<bool> ToResponse(
            MessageConnection connection,
            HttpRequestMessage request,
            IServiceMessage message,
            HttpResponseMessage response);
    }
}