using System.Net.Http;
using Jali.Secure;
using Jali.Serve.Server.MessageConversion;

namespace Jali.Serve.Server
{
    /// <summary>
    ///     Provides initialization options for a Jali server.
    /// </summary>
    public class JaliServerOptions
    {
        /// <summary>
        ///     Represents a utility that provides a security context based on the current http request.
        /// </summary>
        public IAuthenticator Authenticator { get; set; }

        /// <summary>
        ///     Represents a utility that provides application claims based on the current http request and the current 
        ///     message security context.
        /// </summary>
        public IAuthorizer Authorizer { get; set; }

        /// <summary>
        ///     Represents a utility converts between <see cref="HttpRequestMessage"/>, 
        ///     <see cref="HttpResponseMessage"/>, and <see cref="ServiceMessage{JObject}"/>.
        /// </summary>
        public IServiceMessageConverter MessageConverter { get; set; }

        /// <summary>
        ///     Represents a utility that converts between the string and JSON object representations of a Jali resource 
        ///     key.
        /// </summary>
        public IResourceKeyConverter KeyConverter { get; set; }
    }    
}