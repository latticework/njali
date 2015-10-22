using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Jali.Serve.MessageConversion
{
    /// <summary>
    ///     Represents a utility that converts between an http request, http response, and a service message contract.
    /// </summary>
    public interface IMessageContractConverter
    {
        /// <summary>
        ///     Converts from an http request to a service message contract.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="conversionContext"></param>
        /// <param name="request">
        ///     The http request.
        /// </param>
        /// <param name="message">
        ///     The partially constructed request service message. The message should not be modified directly.
        /// </param>
        /// <returns>
        ///     The request service <see cref="MessageContract"/> or <see langword="null"/> if the message should 
        ///     remain unmodified.
        /// </returns>
        Task<MessageContract> FromRequest(
            IExecutionContext context, MessageConversionContext conversionContext, HttpRequestMessage request, ServiceMessage<JObject> message);

        /// <summary>
        ///     Uses a response service message contract to modify an http response.
        /// </summary>
        /// <param name="context">
        ///     The execution context.
        /// </param>
        /// <param name="conversionContext">
        ///     The message conversion context.
        /// </param>
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
        Task<bool> ToResponse(
            IExecutionContext context, 
            MessageConversionContext conversionContext,
            MessageContract contract,
            HttpRequestMessage request,
            IServiceMessage message,
            HttpResponseMessage response);
    }
}
