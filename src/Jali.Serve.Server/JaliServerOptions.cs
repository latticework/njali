using System.Net.Http;
using Jali.Serve.MessageConversion;

namespace Jali.Serve.Server
{
    /// <summary>
    ///     Provides initialization options for <see cref="JaliServer"/>.
    /// </summary>
    public class JaliServerOptions
    {
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