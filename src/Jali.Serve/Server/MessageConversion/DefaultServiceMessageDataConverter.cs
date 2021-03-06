﻿using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Jali.Note;
using Newtonsoft.Json.Linq;

namespace Jali.Serve.Server.MessageConversion
{
    /// <summary>
    ///     A utility converts between an http request, http response, and an json object representing the service 
    ///     message data. This implementation performs no conversions.
    /// </summary>
    public class DefaultServiceMessageDataConverter : IServiceMessageDataConverter
    {
        /// <summary>
        ///     Converts from a <see cref="HttpRequestMessage"/> to a MessageContract.  This implementation performs no 
        ///     conversions.
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
        public virtual Task<JObject> FromRequest(IExecutionContext context, MessageConversionContext conversionContext, HttpRequestMessage request, ServiceMessage<JObject> message)
        {
            return Task.FromResult<JObject>(null);
        }

        /// <summary>
        ///     Uses a response <see cref="JObject"/> to modify a <see cref="HttpResponseMessage"/>.  This 
        ///     implementation performs no conversions.
        /// </summary>
        /// <param name="context">
        ///     The execution context.
        /// </param>
        /// <param name="conversionContext">
        ///     The message conversion context.
        /// </param>
        /// <param name="data">
        ///     The response service message data. Either a <see cref="JObject"/> or an <see cref="JArray"/> of 
        ///     objects.
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
        ///     A value indicating that the http response was not modified.
        /// </returns>
        public virtual Task<bool> ToResponse(IExecutionContext context, MessageConversionContext conversionContext, JToken data, IServiceMessage message, HttpRequestMessage request, HttpResponseMessage response)
        {
            return Task.FromResult(false);
        }
    }
}