using Jali.Note;
using Jali.Note.Definition;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Jali.Core
{
    /// <summary>
    ///     Notification messages defined by the Jali Core package.
    /// </summary>
    public static class JaliCoreMessages
    {
        /// <summary>
        ///     Represents a message definition document.
        /// </summary>
        public static MessageDefinitionDocument Document { get; }

        //private const string _authorityCode = "0000";
        //private const string _domainCode = "00";
        //private const string _libraryCode = "00";
        //private const string _messagePrefix = "1000000000";

        /// <summary>
        ///     Critical error messages.
        /// </summary>
        public static class Criticals
        {
            /// <summary>
            ///     A critical internal application error has occurred and application should 
            ///     terminate.
            /// </summary>
            public static class InternalError
            {
                /// <summary>
                ///     Args for the InternalError message.
                /// </summary>
                [JsonObject(MemberSerialization.OptIn)]
                public class Args
                {
                    /// <summary>
                    ///     Gets the description of the internal error.
                    /// </summary>
                    [JsonProperty("message")]
                    public string Message { get; set; }
                }

                /// <summary>
                ///     A critical internal application error has occurred and application should 
                ///     terminate. Authority jali, Domain jali, Library core, Priority Madatory, Severity Critical, 
                ///     BaseCode 0001
                /// </summary>
                public const string Code = "1000000000FF0001";

                /// <summary>
                ///     The message definition.
                /// </summary>
                public static readonly MessageDefinition Definition;

                /// <summary>
                ///     Creates an instance of the InvalidError message.
                /// </summary>
                /// <param name="message">
                ///     The description of the internal error.
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
                ///     An InternalError message.
                /// </returns>
                public static NotificationMessage<Args> Create(
                    string message, string objectKey = null, string objectPointer = null, params string[] propertyNames)
                {
                    var args = new Args
                    {
                        Message = message,
                    };

                    return Create(args, objectKey, objectPointer, propertyNames);
                }

                /// <summary>
                ///     Creates an instance of the InvalidError message.
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
                ///     An InternalError message.
                /// </returns>
                public static NotificationMessage<Args> Create(
                    Args args, string objectKey = null, string objectPointer = null, params string[] propertyNames)
                {
                    return NotificationMessage.CreateMessage(
                        definition: Definition,
                        messageCode: Code,
                        message: $"Internal error: {args.Message}",
                        args: args,
                        objectKey: objectKey,
                        objectPointer: objectPointer,
                        propertyNames: propertyNames);
                }

                static InternalError()
                {
                    Definition = new MessageDefinition
                    {
                        BaseCode = "0001",
                        Name = "InternalError",
                        Description =
                            "A critical internal application error has occurred and application should terminate.",
                        Priority = MessagePriority.Mandatory,
                        Severity = MessageSeverity.Critical,
                        Message = null,
                        Template = "Internal error: ${args.message}",
                        ArgumentSchema = JObject.Parse(@"{
  ""$schema"": ""http://json-schema.org/draft-04/schema#"",
  ""type"": ""object"",
  ""properties"": {
    ""message"": {
    ""type"": ""object"",
    ""description"":  ""The description of the internal error.""
    }
}"),
                    };
                }

            }
        }

        static JaliCoreMessages()
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
                    [Criticals.InternalError.Definition.Name] = Criticals.InternalError.Definition
                }
            };
        }
    }
}
