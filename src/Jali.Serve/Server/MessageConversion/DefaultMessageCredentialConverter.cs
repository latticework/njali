﻿using System.Net.Http;
using System.Threading.Tasks;
using Jali.Secure;
using Newtonsoft.Json.Linq;

namespace Jali.Serve.Server.MessageConversion
{
    /// <summary>
    ///     Represents a utility that converts between an http request, http response, and service message credentials. 
    ///     This implementation performs no conversions.
    /// </summary>
    public class DefaultMessageCredentialConverter : IMessageCredentialConverter
    {
        /// <summary>
        ///     Converts from an http request to service message credentials. This implementation performs no 
        /// conversions.
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
        ///     The request service <see cref="MessageCredentials"/> or <see langword="null"/> if the message should 
        ///     remain unmodified.
        /// </returns>
        public virtual async Task<MessageCredentials> FromRequest(
            IExecutionContext context, 
            MessageConversionContext conversionContext, 
            HttpRequestMessage request, 
            ServiceMessage<JObject> message)
        {
            var sidType = WellKnownClaimTypes.Sid;

            var credentials = new MessageCredentials
            {
                UserId = conversionContext.UserContext.User.Claims.GetClaimValue(sidType),
                ImpersonatorId = conversionContext.UserContext.Impersonator?.Claims.GetClaimValue(sidType),
                DeputyId = conversionContext.UserContext.Deputy?.Claims.GetClaimValue(sidType),
            };

            return await Task.FromResult(credentials);
        }

        /// <summary>
        ///     Uses response service message credentials to modify an http response. This implementation performs no 
        ///     conversions.
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
        public virtual async Task<bool> ToResponse(
            IExecutionContext context, 
            MessageConversionContext conversionContext, 
            MessageCredentials contract, 
            HttpRequestMessage request, 
            IServiceMessage message, 
            HttpResponseMessage response)
        {
            return await Task.FromResult(false);
        }
    }
}