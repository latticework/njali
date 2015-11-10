using Jali.Note;
using Jali.Note.Definition;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Jali.Rule
{
    /// <summary>
    ///     Notification messages defined by the Jali Core package for standard validation rules.
    /// </summary>
    public static class JaliCoreRuleMessages
    {
        /// <summary>
        ///     Represents a message definition document.
        /// </summary>
        public static MessageDefinitionDocument Document { get; }

        /// <summary>
        ///     Error messages.
        /// </summary>
        public static class Errors
        {
            /// <summary>
            ///     A required value is missing.
            /// </summary>
            public static class RequiredValue
            {
                /// <summary>
                ///     A required value is missing. Authority jali, Domain jali, Library core, Priority Madatory, Severity Critical, 
                ///     BaseCode 0100
                /// </summary>
                public const string Code = "1000000000FF0100";

                /// <summary>
                ///     The message definition.
                /// </summary>
                public static readonly MessageDefinition Definition;

                /// <summary>
                ///     Creates an instance of the InvalidError message.
                /// </summary>
                /// <param name="objectKey">
                ///     The JSON representation of the key to the resource or root object to which this message 
                ///     refers or <see langword="null"/> if no resource is referenced.
                /// </param>
                /// <param name="objectPointer">
                ///     The JSON pointer reference to the object within the resource or root to which this message refers 
                ///     or <see langword="null"/> if no object is referenced.
                /// </param>
                /// <param name="propertyNames">
                ///     The names of the object properties to which this message refers or <see langword="null"/> if no 
                ///     object properties are referenced.
                /// </param>
                /// <returns>
                ///     An RequiredValue message.
                /// </returns>
                public static NotificationMessage Create(
                    string objectKey = null, string objectPointer = null, params string[] propertyNames)
                {
                    return NotificationMessage.CreateMessage(
                        definition: Definition,
                        messageCode: Code,
                        message: $"{(propertyNames == null ? "A value is required." : $"The value{(propertyNames.Length > 1 ? "s" : "")} '{string.Join(",", propertyNames)}' {(propertyNames.Length > 1 ? "are" : "is")} required ")}{(objectPointer != null ? $"for object at '{objectPointer}'" : "")}{(objectKey != null ? $" of '{objectKey}'" : "")}.",
                        objectKey: objectKey,
                        objectPointer: objectPointer,
                        propertyNames: propertyNames);
                }

                static RequiredValue()
                {
                    Definition = new MessageDefinition
                    {
                        BaseCode = "0100",
                        Name = "RequiredValue",
                        Description = "A required value is missing.",
                        Priority = MessagePriority.High,
                        Severity = MessageSeverity.Error,
                        Message = null,
                        Template = "${propertyNames === null ? \"A value is required.\" : \"The value${propertyNames.length > 1 ? \"s\" : \"\"} '${propertyNames.join()}'\" ${propertyNames.length > 1 ? \"are\" : \"is\"} required ${objectPointer !== null ? \"for object at '${objectPointer}'\" : \"\"}${objectKey !== null ? \" of '${objectKey}'\" : \"\"}.}",
                        ArgumentSchema = null,
                    };
                }
            }

            /// <summary>
            ///     A value is too long.
            /// </summary>
            public static class MaxLength
            {
                /// <summary>
                ///     Args for the MaxLength message.
                /// </summary>
                [JsonObject(MemberSerialization.OptIn)]
                public class Args
                {
                    /// <summary>
                    ///     Gets or sets the inclusive upper bound value length.
                    /// </summary>
                    [JsonProperty("maxLength")]
                    // ReSharper disable once MemberHidesStaticFromOuterClass
                    public int MaxLength { get; set; }

                    /// <summary>
                    ///     Gets or sets the actual value length.
                    /// </summary>
                    [JsonProperty("actualLength")]
                    public int ActualLength { get; set; }
                }

                /// <summary>
                ///     A value is too long. Authority jali, Domain jali, Library core, Priority Madatory, Severity Critical, 
                ///     BaseCode 0102
                /// </summary>
                public const string Code = "1000000000FF0102";

                /// <summary>
                ///     The message definition.
                /// </summary>
                public static readonly MessageDefinition Definition;

                /// <summary>
                ///     Creates an instance of the MaxLength message.
                /// </summary>
                /// <param name="maxLength">
                ///     The inclusive upper bound value length.
                /// </param>
                /// <param name="actualLength">
                ///     The actual value length.
                /// </param>
                /// <param name="objectKey">
                ///     The JSON representation of the key to the resource or root object to which this message 
                ///     refers or <see langword="null"/> if no resource is referenced.
                /// </param>
                /// <param name="objectPointer">
                ///     The JSON pointer reference to the object within the resource or root to which this message refers 
                ///     or <see langword="null"/> if no object is referenced.
                /// </param>
                /// <param name="propertyNames">
                ///     The names of the object properties to which this message refers or <see langword="null"/> if no 
                ///     object properties are referenced.
                /// </param>
                /// <returns>
                ///     An MaxLength message.
                /// </returns>
                public static NotificationMessage<Args> Create(
                    int maxLength, int actualLength, string objectKey = null, string objectPointer = null, params string[] propertyNames)
                {
                    var args = new Args
                    {
                        MaxLength = maxLength,
                        ActualLength = actualLength,
                    };

                    return Create(args, objectPointer, objectKey, propertyNames);
                }

                /// <summary>
                ///     Creates an instance of the MaxLength message.
                /// </summary>
                /// <param name="args">
                ///     The message arguments.
                /// </param>
                /// <param name="objectKey">
                ///     The JSON representation of the key to the resource or root object to which this message 
                ///     refers or <see langword="null"/> if no resource is referenced.
                /// </param>
                /// <param name="objectPointer">
                ///     The JSON pointer reference to the object within the resource or root to which this message refers 
                ///     or <see langword="null"/> if no object is referenced.
                /// </param>
                /// <param name="propertyNames">
                ///     The names of the object properties to which this message refers or <see langword="null"/> if no 
                ///     object properties are referenced.
                /// </param>
                /// <returns>
                ///     An MaxLength message.
                /// </returns>
                public static NotificationMessage<Args> Create(
                    Args args, string objectKey = null, string objectPointer = null, params string[] propertyNames)
                {
                    return NotificationMessage.CreateMessage(
                        args: args,
                        definition: Definition,
                        messageCode: Code,
                        message: $"{(propertyNames == null ? "A value" : $"The value{(propertyNames.Length > 1 ? "s" : "")} '{string.Join(",", propertyNames)}'")} {(propertyNames?.Length > 1 ? "are" : "is")} too long {(objectPointer != null ? "for object at '${objectPointer}'" : "")}{(objectKey != null ? " of '${objectKey}'" : "")}. Max length is '${args.MaxLength}'. Yours is '${args.ActualLength}'",
                        objectKey: objectKey,
                        objectPointer: objectPointer,
                        propertyNames: propertyNames);
                }

                static MaxLength()
                {
                    Definition = new MessageDefinition
                    {
                        BaseCode = "0102",
                        Name = "MaxLength",
                        Description = "A value is too long.",
                        Priority = MessagePriority.High,
                        Severity = MessageSeverity.Error,
                        Message = null,
                        Template = "${propertyNames === null ? \"A value\" : \"The value${propertyNames.length > 1 ? \"s\" : \"\"} '${propertyNames.join()}'\" ${propertyNames.length > 1 ? \"are\" : \"is\"} too long ${objectPointer !== null ? \"for object at '${objectPointer}'\" : \"\"}${objectKey !== null ? \" of '${objectKey}'\" : \"\"}. Max length is '${args.maxLength}'. Yours is '${args.actualLength}'}",
                        ArgumentSchema = JObject.Parse(@"{
  ""$schema"": ""http://json-schema.org/draft-04/schema#"",
  ""type"": ""object"",
  ""properties"": {
    ""maxLength"": {
      ""type"": ""integer"",
      ""description"": ""The inclusive upper bound value length.""
    },
    ""actualLength"": {
      ""type"": ""integer"",
      ""description"": ""The actual value length.""
    }
  },
  ""required"": [ ""maxLength"", ""actualLength"" ],
  ""identifyingArgs"": [ ]
}"),
                    };
                }
            }
        }

        static JaliCoreRuleMessages()
        {
            Document = new MessageDefinitionDocument
            {
                Url = null,
                Version = null,
                Authority = new MessageRegistrationReference
                {
                    Type = MessageRegistryType.Authority,
                    Name = "jali",
                    Code = "00",
                },
                Domain = new MessageRegistrationReference
                {
                    Type = MessageRegistryType.Domain,
                    Name = "jali",
                    Code = "00",
                },
                Library = new MessageRegistrationReference
                {
                    Type = MessageRegistryType.Library,
                    Name = "core",
                    Code = "00",
                },
                Messages =
                {
                    [Errors.RequiredValue.Definition.Name] = Errors.RequiredValue.Definition
                }
            };
        }
    }

}