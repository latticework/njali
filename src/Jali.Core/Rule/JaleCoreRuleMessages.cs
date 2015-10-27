using Jali.Note;
using Jali.Note.Definition;

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
            ///     A critical internal application error has occurred and application should 
            ///     terminate.
            /// </summary>
            public static class RequiredValue
            {
                /// <summary>
                ///     A critical internal application error has occurred and application should 
                ///     terminate. Authority jali, Domain jali, Library core, Priority Madatory, Severity Critical, 
                ///     BaseCode 0001
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
                        Priority = MessagePriority.Mandatory,
                        Severity = MessageSeverity.Critical,
                        Message = null,
                        Template = "Internal error: ${args.message}",
                        ArgumentSchema = null,
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