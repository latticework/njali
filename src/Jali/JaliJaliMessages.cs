using Jali.Note;
using Jali.Note.Definition;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Jali
{
    /// <summary>
    ///     Notification messages defined by the Jali package.
    /// </summary>
    public static class JaliJaliMessages
    {
        //private const string _authorityCode = "0000";
        //private const string _domainCode = "00";
        //private const string _libraryCode = "01";
        //private const string _messagePrefix = "1000000001";

        /// <summary>
        ///     Error messages.
        /// </summary>
        public static class Errors
        {
            /// <summary>
            ///     Informs that the requester is not an authenticated user.
            /// </summary>
            public static class AuthenticationError
            {
                /// <summary>
                ///     Args for the AuthenticationError message.
                /// </summary>
                [JsonObject(MemberSerialization.OptIn)]
                public class Args
                {
                    /// <summary>
                    ///     Gets the name of the resource being accessed.
                    /// </summary>
                    public string ResourceName { get; set; }
                }

                /// <summary>
                ///     Informs that the requester is not an authenticated user. Authority jali, Domain jali, Library jali, 
                ///     Priority High, Severity Error, BaseCode 001.
                /// </summary>
                public const string Code = "1000000001EE0001";

                /// <summary>
                ///     The message definition.
                /// </summary>
                public static readonly MessageDefinition Definition;

                /// <summary>
                ///     Creates an instance of the InvalidError message.
                /// </summary>
                /// <param name="resourceName">
                ///     The name of the resource being accessed.
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
                    string resourceName = null,
                    string objectKey = null,
                    string objectPointer = null,
                    params string[] propertyNames)
                {
                    var args = new Args
                    {
                        ResourceName = resourceName,
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
                        message:
                            $"Access to requested resource{(args.ResourceName != null ? $" '{args.ResourceName}'" : "")} requires an authenticated user.",
                        args: args,
                        objectKey: objectKey,
                        objectPointer: objectPointer,
                        propertyNames: propertyNames);
                }


                static AuthenticationError()
                {
                    Definition = new MessageDefinition
                    {
                        BaseCode = "0001",
                        Name = "AuthenticationError",
                        Description =
                            "Informs that the requester is not an authenticated user.",
                        Priority = MessagePriority.High,
                        Severity = MessageSeverity.Error,
                        Message = null,
                        Template =
                            "Access to requested resource${args.resourceName != null ? \" '${args.resourceName}'\" : \"\"} requires an authenticated user.",
                        ArgumentSchema = JObject.Parse(@"{
  ""$schema"": ""http://json-schema.org/draft-04/schema#"",
  ""type"": ""object"",
  ""properties"": {
    ""resourceName"": {
    ""type"": ""object"",
    ""description"":  ""The name of the resource being accessed.""
    }
}"),
                    };

                }
            }

            /// <summary>
            ///     Informs that the requester is not an authenticated user.
            /// </summary>
            public static class AuthorizationError
            {
                /// <summary>
                ///     Args for the AuthenticationError message.
                /// </summary>
                [JsonObject(MemberSerialization.OptIn)]
                public class Args
                {
                    /// <summary>
                    ///     Gets or sets the Jali UserId of the authenticated user.
                    /// </summary>
                    public string UserId { get; set; }

                    /// <summary>
                    ///     Gets or sets the type of the required claim.
                    /// </summary>
                    public string ClaimType { get; set; }

                    /// <summary>
                    ///     Gets or sets the required claim value.
                    /// </summary>
                    public string ClaimValue { get; set; }
                }

                /// <summary>
                ///     Informs that the authenticated user is not authorized for the requested resource access. 
                ///     Authority jali, Domain jali, Library jali, Priority High, Severity Error, BaseCode 002.
                /// </summary>
                public const string Code = "1000000001EE0002";

                /// <summary>
                ///     The message definition.
                /// </summary>
                public static readonly MessageDefinition Definition;

                /// <summary>
                ///     Creates an instance of the InvalidError message.
                /// </summary>
                /// <param name="userId">
                ///     The Jali UserId of the authenticated user.
                /// </param>
                /// <param name="claimType">
                ///     The type of the required claim.
                /// </param>
                /// <param name="claimValue">
                ///     The required claim value.
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
                    string userId,
                    string claimType,
                    string claimValue,
                    string objectKey = null,
                    string objectPointer = null,
                    params string[] propertyNames)
                {
                    var args = new Args
                    {
                        UserId = userId,
                        ClaimType = claimType,
                        ClaimValue = claimValue,
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
                        message:
                            $"User '{args.UserId}'is not authorized to access resource '{args.ClaimValue}' of type '${args.ClaimType}'.",
                        args: args,
                        objectKey: objectKey,
                        objectPointer: objectPointer,
                        propertyNames: propertyNames);
                }


                static AuthorizationError()
                {
                    Definition = new MessageDefinition
                    {
                        Name = "AuthorizationError",
                        Priority = MessagePriority.High,
                        Severity = MessageSeverity.Error,
                        BaseCode = "0002",
                        Description =
                            "Informs that the authenticated user is not authorized for the requested resource access.",
                        Message = null,
                        Template =
                            "User '${args.userId}'is not authorized to access resource '${args.claimValue}' of type '${args.claimType}'.",
                        ArgumentSchema = JObject.Parse(@"{
  ""$schema"": ""http://json-schema.org/draft-04/schema#"",
  ""type"": ""object"",
  ""properties"": {
    ""userId"": {
    ""type"": ""string"",
    ""description"": ""The Jali UserId of the authenticated user.""
    },
    ""claimType"": {
    ""type"": ""string"",
    ""description"": ""The type of the required claim.""
    },
    ""claimValue"": {
    ""type"": ""string"",
    ""description"": ""The required claim value.""
    }
  },
  ""required"": [ ""userId"", ""claimType"", ""claimValue"" ]
}"),
                    };

                }
            }
        }

        static JaliJaliMessages()
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
                    Name = "jali",
                    Code = "01",
                },
                Messages =
                {
                    [Errors.AuthenticationError.Definition.Name] = Errors.AuthenticationError.Definition
                }
            };
        }

        /// <summary>
        ///     Represents a message definition document.
        /// </summary>
        public static MessageDefinitionDocument Document { get; }
    }
}
