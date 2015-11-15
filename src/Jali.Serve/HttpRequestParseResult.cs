using System;
using System.Collections.Generic;
using System.Net.Http;
using Jali.Note;

namespace Jali.Serve
{
    /// <summary>
    ///     Represents the result of a <see cref="JaliHttpRequestMessageExtensions.JaliParse"/>.
    /// </summary>
    public class HttpRequestParseResult
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="HttpRequestParseResult"/> class.
        /// </summary>
        /// <param name="method">
        ///     The http method
        /// </param>
        /// <param name="resourceName">
        ///     The Jali resource name.
        /// </param>
        /// <param name="payload">
        ///     The JSON payload.
        /// </param>
        /// <param name="resourceKey">
        ///     The resource key string.
        /// </param>
        /// <param name="routineName">
        ///     The routine name.
        /// </param>
        /// <param name="messageAction">
        ///     The routine message action.
        /// </param>
        /// <param name="messages">
        ///     Notification messages containing no errors.
        /// </param>
        public HttpRequestParseResult(
            string method,
            string resourceName,
            string payload,
            string resourceKey,
            string routineName,
            string messageAction,
            IEnumerable<INotificationMessage> messages)
        {
            this.Method = method;
            this.ResourceName = resourceName;
            this.JsonPayload = payload;
            this.ResourceKey = resourceKey;
            this.RoutineName = routineName;
            this.MessageAction = messageAction;

            this.Messages = NotificationMessageCollection.FromEnumerable(messages);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="HttpRequestParseResult"/> class with parse errors.
        /// </summary>
        /// <param name="errors">
        ///     Notification messages containing errors.
        /// </param>
        public HttpRequestParseResult(IEnumerable<INotificationMessage> errors)
        {
            if (errors == null) throw new ArgumentNullException(nameof(errors));

            var messages = NotificationMessageCollection.FromEnumerable(errors);

            if (!messages.HasErrors())
            {
                throw new ArgumentException(
                    $"'{nameof(HttpRequestParseResult)}' error initialization must include error messages. Yours does not.");
            }

            this.Messages = messages;
        }

        /// <summary>
        ///     Gets the name of the resource.
        /// </summary>
        public string ResourceName { get; }

        /// <summary>
        ///     Gets the http method.
        /// </summary>
        public string Method { get; }

        /// <summary>
        ///     Gets the unparsed resource key string or <c>null</c> if no resource key specified.
        /// </summary>
        public string ResourceKey { get; }

        /// <summary>
        ///     Gets the routine name or <c>null</c> if no routine specified.
        /// </summary>
        public string RoutineName { get; }

        /// <summary>
        ///     Gets the routine message action or <c>null</c> if no routine specified.
        /// </summary>
        public string MessageAction { get; }

        /// <summary>
        ///     Gets the unparsed json payload or <c>null</c> if no payload included.
        /// </summary>
        public string JsonPayload { get; }

        /// <summary>
        ///     Gets the Jali notification messages.
        /// </summary>
        public NotificationMessageCollection Messages { get; }

        /// <summary>
        ///     Returns whether the parse operation succeeded.
        /// </summary>
        public bool Succeeded => this.ResourceName != null;
    }

}