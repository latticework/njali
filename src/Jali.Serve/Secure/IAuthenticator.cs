using System.Net.Http;
using System.Threading.Tasks;
using Jali.Serve;
using Newtonsoft.Json.Linq;

namespace Jali.Secure
{
    /// <summary>
    ///     Represents a utility that provides a security context based on the current http request.
    /// </summary>
    public interface IAuthenticator
    {
        /// <summary>
        ///     Provides a security context based on the current request.
        /// </summary>
        /// <param name="context">
        ///     The authenticator's execution context.
        /// </param>
        /// <param name="httpRequest">
        ///     The http request.
        /// </param>
        /// <param name="request">
        ///     The request service message or an http response.
        /// </param>
        /// <returns>
        ///     The new security context.
        /// </returns>
        Task<AuthenticationResult> AuthenticateToken(
            IExecutionContext context, HttpRequestMessage httpRequest);

        /// <summary>
        ///     Provides a security context based on the current request.
        /// </summary>
        /// <param name="context">
        ///     The authenticator's execution context.
        /// </param>
        /// <param name="httpRequest">
        ///     The http request.
        /// </param>
        /// <param name="request">
        ///     The request service message or an http response.
        /// </param>
        /// <returns>
        ///     The new security context.
        /// </returns>
        Task<AuthenticationResult> AuthenticateUser(
            IExecutionContext context, HttpRequestMessage httpRequest, IServiceMessage<JObject> request);

        /// <summary>
        ///     Encodes a user context to a JSON Web Token (JWT).
        /// </summary>
        /// <param name="user">
        ///     The security context to encode.
        /// </param>
        /// <returns>
        ///     The JWT token.
        /// </returns>
        Task<string> EncodeUser(ISecurityContext user);
    }
}