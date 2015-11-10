using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Jali.Core;
using Jali.Serve;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Jali.Secure
{
    /// <summary>
    ///     A utility that provides a security context based on the current http request. This implementation supplies 
    ///     an empty identity.
    /// </summary>
    public class DefaultAuthenticator : IAuthenticator
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultAuthenticator"/> class.
        /// </summary>
        /// <param name="key">
        ///     A 256 byte encryption key.
        /// </param>
        public DefaultAuthenticator(byte[] key)
        {
            _key = key;
        }

        // TODO: IAuthenticator.AuthenticateToken: Add serviceContext parameter.
        ///// <param name="serviceContext">
        /////     The Jali Service context.
        ///// </param>
        /// <summary>
        ///     Provides a security context based on the current http request. Ths implementation supplies an empty 
        ///     identity.
        /// </summary>
        /// <param name="context">
        ///     The authenticator's execution context.
        /// </param>
        /// <param name="httpRequest">
        ///     The http request.
        /// </param>
        /// <returns>
        ///     The new security context or an http response.
        /// </returns>
        public async Task<AuthenticationResult> AuthenticateToken(
            IExecutionContext context, HttpRequestMessage httpRequest)
        {
            var token = httpRequest.GetSecurityToken();

            if (token == null)
            {
                var response = new HttpResponseMessage()
                {
                    RequestMessage = httpRequest,
                    StatusCode = HttpStatusCode.Moved,
                    ReasonPhrase = nameof(HttpStatusCode.Moved)
                };

                // TODO: DefaultAuthenticator.Authenticate: Does the redirect only work for GET/DELETE HTTP methods?
                var json = new JObject(new JProperty("redirectUrl", httpRequest.RequestUri));
                var jsonString = json.ToString(Formatting.None);

                var baseUrl = new Uri(httpRequest.RequestUri.GetBaseUrl());
                response.Headers.Location = new Uri(baseUrl, $"resources/User?json={jsonString}");

                return new AuthenticationResult(response);
            }

            ISecurityContext user = AuthenticationOperations.Decode(token, this._key);

            return await this.AuthenticateTokenCore(context, httpRequest, user);
        }


        /// <summary>
        ///     Authenticates the user from the request service message.
        /// </summary>
        /// <param name="context">
        ///     The authenticator's execution context.
        /// </param>
        /// <param name="httpRequest">
        ///     The http request.
        /// </param>
        /// <param name="request">
        ///     The request service message.
        /// </param>
        /// <returns>
        ///     A Task resulting in an <see cref="AuthenticationResult"/>.
        /// </returns>
        public async Task<AuthenticationResult> AuthenticateUser(
            IExecutionContext context, HttpRequestMessage httpRequest, IServiceMessage<JObject> request)
        {
            return await AuthenticateUserCore(context, httpRequest, request);
        }

        /// <summary>
        ///     Encodes a user context to a JSON Web Token (JWT).
        /// </summary>
        /// <param name="user">
        ///     The security context to encode.
        /// </param>
        /// <returns>
        ///     The JWT token.
        /// </returns>
        public async Task<string> EncodeUser(ISecurityContext user)
        {
            await Task.FromResult(true);

            return AuthenticationOperations.Encode(user, this._key);
        }

        /// <summary>
        ///     Provides a security context based on the current http request. Ths implementation supplies an empty 
        ///     identity.
        /// </summary>
        /// <param name="context">
        ///     The authenticator's execution context.
        /// </param>
        /// <param name="httpRequest">
        ///     The http request.
        /// </param>
        /// <param name="user">
        ///     The decrypted user.
        /// </param>
        /// <returns>
        ///     The new security context. This implementation returns the passed in user.
        /// </returns>
        protected virtual async Task<AuthenticationResult> AuthenticateTokenCore(
            IExecutionContext context, HttpRequestMessage httpRequest, ISecurityContext user)
        {
            await Task.FromResult(true);

            return new AuthenticationResult(user);
        }

        /// <summary>
        ///     Authenticates the user from the request.
        /// </summary>
        /// <param name="context">
        ///     The authenticator's execution context.
        /// </param>
        /// <param name="httpRequest">
        ///     The http request.
        /// </param>
        /// <param name="request">
        ///     The request service message.
        /// </param>
        /// <returns>
        ///     A Task resulting in an <see cref="AuthenticationResult"/>.
        /// </returns>
        protected virtual async Task<AuthenticationResult> AuthenticateUserCore(
            IExecutionContext context, HttpRequestMessage httpRequest, IServiceMessage<JObject> request)
        {
            await Task.FromResult(true);

            return new AuthenticationResult(new SecurityContext());
        }

        private readonly byte[] _key;
    }
}