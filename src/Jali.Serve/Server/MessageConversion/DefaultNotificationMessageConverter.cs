using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Jali.Note;
using Newtonsoft.Json.Linq;

namespace Jali.Serve.Server.MessageConversion
{
    /// <summary>
    ///     A utility converts between an http request, an http response, and a sequence of Jali notification messages. 
    ///     This implementation performs no conversions.
    /// </summary>
    public class DefaultNotificationMessageConverter : INotificationMessageConverter
    {
        /// <summary>
        ///     Converts from an http request to a sequence of Jali notification messages. This implementation performs 
        ///     no conversions.
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
        ///     <see langword="null"/> so the message remains unmodified.
        /// </returns>
        public virtual Task<IEnumerable<INotificationMessage>> FromRequest(IExecutionContext context, MessageConversionContext conversionContext, HttpRequestMessage request, ServiceMessage<JObject> message)
        {
            return Task.FromResult<IEnumerable<INotificationMessage>>(null);
        }

        /// <summary>
        ///     Uses a response service message notification message sequence to modify an http response. This 
        ///     implementation performs no conversions.
        /// </summary>
        /// <param name="context">
        ///     The execution context.
        /// </param>
        /// <param name="conversionContext">
        ///     The message conversion context.
        /// </param>
        /// <param name="messages">
        ///     The response service message <see cref="NotificationMessage"/> sequence.
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
        public virtual Task<bool> ToResponse(IExecutionContext context, MessageConversionContext conversionContext, IEnumerable<INotificationMessage> messages, HttpRequestMessage request, IServiceMessage message, HttpResponseMessage response)
        {
            return Task.FromResult(false);
        }
    }
}