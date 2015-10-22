using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Jali.Serve.MessageConversion
{
    /// <summary>
    ///     Represents a utility converts between an http request, http response, and an json object representing the 
    ///     service message data.
    /// </summary>
    public interface IServiceMessageDataConverter
    {
        /// <summary>
        ///     Converts from a <see cref="HttpRequestMessage"/> to a MessageContract.
        /// </summary>
        /// <param name="context">
        ///     The execution context.
        /// </param>
        /// <param name="conversionContext">
        ///     The message conversion context.
        /// </param>
        /// <param name="request">
        ///     The http request.
        /// </param>
        /// <param name="message">
        ///     The partially constructed request service message. The message should not be modified directly.
        /// </param>
        /// <returns>
        ///     The request <see cref="JObject"/> representing the service message data or <see langword="null"/> if 
        ///     the message should remain unmodified.
        /// </returns>
        Task<JObject> FromRequest(
            IExecutionContext context, MessageConversionContext conversionContext, HttpRequestMessage request, ServiceMessage<JObject> message);

        /// <summary>
        ///     Uses a response <see cref="JObject"/> to modify a <see cref="HttpResponseMessage"/>.
        /// </summary>
        /// <param name="context">
        ///     The execution context.
        /// </param>
        /// <param name="conversionContext">
        ///     The message conversion context.
        /// </param>
        /// <param name="data">
        ///     The response service message data.
        /// </param>
        /// <param name="message">
        ///     The response service message.
        /// </param>
        /// <param name="request">
        ///     The initial http request.
        /// </param>
        /// <param name="response">
        ///     The partial constructed http response.
        /// </param>
        /// <returns>
        ///     A value indicating whether the converter modified the http response.
        /// </returns>
        Task<bool> ToResponse(
            IExecutionContext context, 
            MessageConversionContext conversionContext,
            JObject data,
            IServiceMessage message,
            HttpRequestMessage request,
            HttpResponseMessage response);
    }
}