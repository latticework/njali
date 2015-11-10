using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Jali.Secure;
using Newtonsoft.Json.Linq;

namespace Jali.Serve.Server.MessageConversion
{
    /// <summary>
    ///     Represents a utility that converts between an http request, http response, and a service message tenant. 
    ///     This implementation performs no conversions.
    /// </summary>
    public class DefaultServiceMessageTenantConverter : IServiceMessageTenantConverter
    {
        /// <summary>
        ///     Converts from an http request to a service message tenant. This implementation performs no conversions.
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
        ///     <see langword="null"/> so the message remains unmodified.
        /// </returns>
        public virtual async Task<TenantIdentity> FromRequest(IExecutionContext context, MessageConversionContext conversionContext, HttpRequestMessage request, ServiceMessage<JObject> message)
        {
            await Task.FromResult(true);
            var user = conversionContext.UserContext.User;

            if (!user.Authenticated) { return await Task.FromResult<TenantIdentity>(null); }


            var tid = user.Claims.FirstOrDefault(c => c.Type == JaliClaimTypes.TenantId)?.Value;
            var toid = user.Claims.FirstOrDefault(c => c.Type == JaliClaimTypes.TenantOrgId)?.Value;

            var identity = new TenantIdentity { TenantId = tid, TenantOrgId = toid, };

            return identity;
        }

        /// <summary>
        ///     Uses a response service message tenant to modify an http response. This implementation performs no 
        ///     conversions.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="conversionContext"></param>
        /// <param name="tenant">
        ///     The response service message tenant.
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
        public virtual Task<bool> ToResponse(IExecutionContext context, MessageConversionContext conversionContext, TenantIdentity tenant, HttpRequestMessage request, IServiceMessage message, HttpResponseMessage response)
        {
            return Task.FromResult(false);
        }
    }
}