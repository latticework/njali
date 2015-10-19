using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Jali.Notification;
using Newtonsoft.Json.Linq;

namespace Jali.Serve.MessageConversion
{
    /// <summary>
    ///     Represents a utility converts between an http request, an http response, and a sequence of Jali notification 
    ///     messages.
    /// </summary>
    public interface INotificationMessageConverter
    {
        /// <summary>
        ///     Converts from an http request to a sequence of Jali notification messages.
        /// </summary>
        /// <param name="request">
        ///     The http request.
        /// </param>
        /// <param name="message">
        ///     The partially constructed request service message. The message should not be modified directly.
        /// </param>
        /// <returns>
        ///     The request service message <see cref="NotificationMessage"/> list or <see langword="null"/> if 
        ///     the message should remain unmodified.
        /// </returns>
        Task<IEnumerable<NotificationMessage>> FromRequest(HttpRequestMessage request, ServiceMessage<JObject> message);

        /// <summary>
        ///     Uses a response service message notification message sequence to modify an http response.
        /// </summary>
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
        ///     A value indicating whether the converter modified the http response.
        /// </returns>
        Task<bool> ToResponse(
            IEnumerable<NotificationMessage> messages,
            HttpRequestMessage request,
            IServiceMessage message,
            HttpResponseMessage response);
    }
}