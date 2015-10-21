﻿using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Jali.Serve.MessageConversion
{
    /// <summary>
    ///     A utility that converts between an http request, http response, and a service message contract. This 
    ///     implementation performs no conversions.
    /// </summary>
    public class DefaultMessageContractConverter : IMessageContractConverter
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
        public Task<MessageContract> FromRequest(HttpRequestMessage request, ServiceMessage<JObject> message)
        {
            return Task.FromResult<MessageContract>(null);
        }

        /// <summary>
        ///     Uses a response service message contract to modify an http response. This implementation performs no 
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
        ///     A value indicating that the http response was not modified.
        /// </returns>
        public Task<bool> ToResponse(MessageContract contract, HttpRequestMessage request, IServiceMessage message,
            HttpResponseMessage response)
        {
            return Task.FromResult(false);
        }
    }
}