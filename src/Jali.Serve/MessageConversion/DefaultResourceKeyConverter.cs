using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace Jali.Serve.MessageConversion
{
    // TODO: DefaultResourceKeyConverter: Needs error handling big time.
    /// <summary>
    ///     The default utility that converts between the string and JSON object representations of a Jali resource 
    ///     key.
    /// </summary>
    public class DefaultResourceKeyConverter : IResourceKeyConverter
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
        public virtual string FromResourceKey(JSchema keySchema, JObject key)
        {
            var properties = GetSchemaProperties(keySchema);

            var keyList = new List<string>(properties.Count());
            foreach (var property in properties)
            {
                ValidateSchemaPropertyType(property);

                var token = key[property.Value.Title];

                keyList.Add(token.Value<string>());
            }

            return JoinKeyString(keyList);
        }

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
        public virtual JObject ToResourceKey(JSchema keySchema, string keyString)
        {
            var properties = GetSchemaProperties(keySchema);

            var keyList = SplitKeyString(keyString);

            var jProperties = properties.Zip(keyList, (kvp, e) =>
            {
                ValidateSchemaPropertyType(kvp);

                return new JProperty(kvp.Value.Title, e);
            }).Cast<object>().ToArray();

            return new JObject(jProperties);
        }

        private static string JoinKeyString(IEnumerable<string> keyList)
        {
            // TODO: DefaultResourceKeyConverter.FromResourceKey: Add separator.
            // TODO: DefaultResourceKeyConverter.FromResourceKey: Separator escape key elements
            return string.Join("", keyList);
        }

        private static IEnumerable<string> SplitKeyString(string keyString)
        {
            // TODO: DefaultResourceKeyConverter.FromResourceKey: Add separator.
            // TODO: DefaultResourceKeyConverter.FromResourceKey: Separator escape key elements
            // return keyString.Split()
            yield return keyString;
        }

        private static void ValidateSchemaPropertyType(KeyValuePair<string, JSchema> property)
        {
            if (property.Value.Type != JSchemaType.String || property.Value.Pattern != "^[0-9]*$")
            {
                const string message =
                    "'DefaultResourceKeyConverter' currently only support numeric string resource key values.";

                throw new NotSupportedException(message);
            }
        }

        private static IOrderedEnumerable<KeyValuePair<string, JSchema>> GetSchemaProperties(JSchema keySchema)
        {
            var properties = keySchema.Properties.OrderBy(p => int.Parse(p.Key));

            if (properties.Count() != 1)
            {
                const string message =
                    "'DefaultResourceKeyConverter' currently only supports a single resource key property.";

                throw new NotSupportedException(message);
            }
            return properties;
        }
    }

}