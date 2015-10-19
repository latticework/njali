using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace Jali.Serve.MessageConversion
{
    /// <summary>
    ///     Represents a utility that converts between the string and JSON object representations of a Jali resource 
    ///     key.
    /// </summary>
    public interface IResourceKeyConverter
    {
        /// <summary>
        ///     Converts from a JSON object representation of a resource key to a string representation.
        /// </summary>
        /// <param name="keySchema">
        ///     The resource key schema.
        /// </param>
        /// <param name="key">
        ///     The JSON object representation.
        /// </param>
        /// <returns>
        ///     The new string representation.
        /// </returns>
        string FromResourceKey(JSchema keySchema, JObject key);

        /// <summary>
        ///     Converts from a string representation of a resource key to a JSON object representation.
        /// </summary>
        /// <param name="keySchema">
        ///     The resource key schema.
        /// </param>
        /// <param name="keyString">
        ///     The string representation.
        /// </param>
        /// <returns>
        ///     The new JSON object representation.
        /// </returns>
        JObject ToResourceKey(JSchema keySchema, string keyString);
    }

}