using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Jali.Serve.MessageConversion
{
    /// <summary>
    ///     Converts a service message to and from a JSON string.
    /// </summary>
    public class DefaultServiceMessageSerializer : IServiceMessageSerializer
    {
        /// <summary>
        ///     Initializes a new <see cref="DefaultServiceMessageSerializer"/> with the specified options.
        /// </summary>
        /// <param name="options">
        ///     Serialization options or <see langword="null"/> for default settings.
        /// </param>
        public DefaultServiceMessageSerializer(DefaultServiceMessageSerializerOptions options = null)
        {
            this._serializer = new Lazy<JsonSerializer>(() => JsonSerializer.Create(this.Options.SerializerSettings));

            var overrideOptions = options ?? new DefaultServiceMessageSerializerOptions();
            this.Options = new DefaultServiceMessageSerializerOptions
            {
                SerializerSettings = overrideOptions.SerializerSettings ?? _default.Value.SerializerSettings,
            };
        }

        /// <summary>
        ///     Gets the serializer's configuration options.
        /// </summary>
        public DefaultServiceMessageSerializerOptions Options { get; }

        /// <summary>
        ///     Converts a JSON string to a <see cref="ServiceMessage{JObject}"/>.
        /// </summary>
        /// <param name="context">
        ///     The execution context.
        /// </param>
        /// <param name="conversionContext">
        ///     The message conversion context.
        /// </param>
        /// <param name="message">
        ///     The service message object.
        /// </param>
        /// <returns>
        ///     The new service message JSON string.
        /// </returns>
        public virtual async Task<JObject> FromServiceMessage(IExecutionContext context, MessageConversionContext conversionContext, IServiceMessage message)
        {
            var json = JObject.FromObject(message);

            return await Task.FromResult(json);
        }

        /// <summary>
        ///     Converts a <see cref="ServiceMessage{JObject}"/> to a JSON object.
        /// </summary>
        /// <param name="context">
        ///     The execution context.
        /// </param>
        /// <param name="conversionContext">
        ///     The message conversion context.
        /// </param>
        /// <param name="json">
        ///     The service message JSON object.
        /// </param>
        /// <returns>
        ///     The new service message object.
        /// </returns>
        /// <remarks>
        ///     This implementation uses the default <see cref="ServiceMessage{TData}"/> serializer.
        /// </remarks>
        public virtual async Task<ServiceMessage<JObject>> ToServiceMessage(
            IExecutionContext context, MessageConversionContext conversionContext, JObject json)
        {
            var message = this._serializer.Value.Deserialize<ServiceMessage<JObject>>(new JTokenReader(json));

            return await Task.FromResult(message);
        }

        /// <summary>
        ///     Serializes the message JSON object to a JSON string.
        /// </summary>
        /// <param name="context">
        ///     The execution context.
        /// </param>
        /// <param name="conversionContext">
        ///     The message conversion context.
        /// </param>
        /// <param name="json">
        ///     The message JSON object.
        /// </param>
        /// <returns>
        ///     The new message JSON string.
        /// </returns>
        public virtual Task<string> Serialize(IExecutionContext context, MessageConversionContext conversionContext, JObject json)
        {
            var jsonString = json.ToString(this._serializer.Value.Formatting);

            return Task.FromResult(jsonString);
        }


        static DefaultServiceMessageSerializer()
        {
            _default = new Lazy<DefaultServiceMessageSerializerOptions>(() =>
                new DefaultServiceMessageSerializerOptions());
        }

        private static readonly Lazy<DefaultServiceMessageSerializerOptions> _default;

        private readonly Lazy<JsonSerializer> _serializer;
    }
}