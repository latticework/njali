
using System;
using System.Net.Http;

namespace Jali.Secure
{
    /// <summary>
    ///     Represents the result of the <see cref="IAuthenticator.Authenticate"/> method.
    /// </summary>
    public class AuthenticationResult
    {
        /// <summary>
        ///     Initializes a new <see cref="AuthenticationResult"/> with the user security context.
        /// </summary>
        /// <param name="userContext">
        ///     The security context of the user.
        /// </param>
        public AuthenticationResult(ISecurityContext userContext)
        {
            if (userContext == null) throw new ArgumentNullException(nameof(userContext));

            this.User = userContext;
            this.Response = null;
        }

        /// <summary>
        ///     Initializes a new <see cref="AuthenticationResult"/> with the user security context.
        /// </summary>
        /// <param name="response">
        ///     A response used to negotiate the appropriate credentials for authentication.
        /// </param>
        public AuthenticationResult(HttpResponseMessage response)
        {
            if (response == null) throw new ArgumentNullException(nameof(response));

            this.User = null;
            this.Response = response;
        }

        /// <summary>
        ///     Gets the user context or <c>null</c> if the http response is returned.
        /// </summary>
        public ISecurityContext User { get; }

        /// <summary>
        ///     Gets the http response or <c>null</c> if the user context is returned.
        /// </summary>
        public HttpResponseMessage Response { get; }
    }
}