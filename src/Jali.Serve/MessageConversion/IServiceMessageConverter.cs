using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Jali.Serve.MessageConversion
{
    /// <summary>
    ///     Represents a utility converts between <see cref="HttpRequestMessage"/>, <see cref="HttpResponseMessage"/>,
    ///     and <see cref="ServiceMessage{JObject}"/>.
    /// </summary>
    public interface IServiceMessageConverter
    {
        /// <summary>
        ///     Converts from a <see cref="HttpRequestMessage"/> to a <see cref="ServiceMessage{JObject}"/>.
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
        /// <returns>
        ///     The request service message.
        /// </returns>
        Task<ServiceMessage<JObject>> FromRequest(
            IExecutionContext context, MessageConversionContext conversionContext, HttpRequestMessage request);

        /// <summary>
        ///     Converts from an <see cref="IServiceMessage"/> to an <see cref="HttpResponseMessage"/>.
        /// </summary>
        /// <param name="context">
        ///     The execution context.
        /// </param>
        /// <param name="conversionContext">
        ///     The message conversion context.
        /// </param>
        /// <param name="message">
        ///     The response service message.
        /// </param>
        /// <param name="request">
        ///     The initial http request.
        /// </param>
        /// <returns>
        ///     The http response.
        /// </returns>
        Task<HttpResponseMessage> ToResponse(
            IExecutionContext context, 
            MessageConversionContext conversionContext, 
            IServiceMessage message, 
            HttpRequestMessage request);
    }
}