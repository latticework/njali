using System;
using System.Globalization;
using System.Runtime.Serialization.Formatters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Jali.Serve.Server.MessageConversion
{
    /// <summary>
    ///     Provides configuration options for a <see cref="DefaultServiceMessageSerializer"/> instance.
    /// </summary>
    public class DefaultServiceMessageSerializerOptions
    {
        /// <summary>
        ///     Intializes a new <see cref="DefaultServiceMessageSerializerOptions"/> instance with default values.
        /// </summary>
        public DefaultServiceMessageSerializerOptions()
        {
            // TODO: DefaultServiceMessageSerializerOptions.ctor: Document all JsonSerializer settings in a table.
            this.SerializerSettings = new JsonSerializerSettings()
            {
                ConstructorHandling = ConstructorHandling.Default,
                Culture = CultureInfo.InvariantCulture,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateParseHandling = DateParseHandling.DateTimeOffset,
                DateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind,
                DefaultValueHandling = DefaultValueHandling.Include,
                FloatFormatHandling = FloatFormatHandling.DefaultValue,
                FloatParseHandling = FloatParseHandling.Decimal,
                Formatting = Formatting.None,
                MaxDepth = 10,
                MetadataPropertyHandling = MetadataPropertyHandling.Default,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                NullValueHandling = NullValueHandling.Include,
                ObjectCreationHandling = ObjectCreationHandling.Auto,
                PreserveReferencesHandling = PreserveReferencesHandling.All,
                ReferenceLoopHandling = ReferenceLoopHandling.Error,
                StringEscapeHandling = StringEscapeHandling.Default,
                TypeNameAssemblyFormat = FormatterAssemblyStyle.Full,
                TypeNameHandling = TypeNameHandling.None,
            };
        }

        /// <summary>
        ///     Specified settings for the JSON serializer.
        /// </summary>
         public JsonSerializerSettings SerializerSettings { get; set; }
    }
}