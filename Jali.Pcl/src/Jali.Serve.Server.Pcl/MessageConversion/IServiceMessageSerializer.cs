using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Jali.Serve.Server.MessageConversion
{
    /// <summary>
    ///     Converts a service message to and from a JSON string.
    /// </summary>
    public interface IServiceMessageSerializer
    {
        /// <summary>
        ///     Converts an <see cref="IServiceMessage"/> to a JSON object.
        /// </summary>
        /// <param name="message">
        ///     The service message object.
        /// </param>
        /// <returns>
        ///     The new service message JSON object.
        /// </returns>
        Task<JObject> FromServiceMessage(IServiceMessage message);

        /// <summary>
        ///     Converts a message JSON object to a <see cref="ServiceMessage{JObject}"/>.
        /// </summary>
        /// <param name="json">
        ///     The service message JSON object.
        /// </param>
        /// <returns>
        ///     The new service message object.
        /// </returns>
        Task<ServiceMessage<JObject>> ToServiceMessage(JObject json);

        /// <summary>
        ///     Serializes the message JSON object to a JSON string.
        /// </summary>
        /// <param name="json">
        ///     The message JSON object.
        /// </param>
        /// <returns>
        ///     The new message JSON string.
        /// </returns>
        Task<string> Serialize(JObject json);
    }
}