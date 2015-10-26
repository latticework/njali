using System.Collections.Generic;
using System.Linq;
using Jali.Note.Definition;
using Newtonsoft.Json.Linq;

namespace Jali.Note
{
    using static MessageCode;
    /// <summary>
    ///     Represents a Jali Notification Message used to communicate semantic messages such as errors and warnings to
    ///     service operation consumers. Can also be used within a stack layer for validations or other business rule
    ///     purposes.
    /// </summary>
    /// <typeparam name="TArgs">
    ///     The type used to represent the Args property.
    /// </typeparam>
    public class NotificationMessage<TArgs> : INotificationMessage
    {
        /// <summary>
        ///     The unique identity of the of the message type.
        /// </summary>
        public string MessageCode { get; internal set; }

        /// <summary>
        ///     Gets the priority of the message.
        /// </summary>
        public MessagePriority Priority => GetPriority(this.MessageCode);

        /// <summary>
        ///     Gets the severity of the message.
        /// </summary>
        public MessageSeverity Severity => GetSeverity(this.MessageCode);

        /// <summary>
        ///     The diagnostic description of the message.
        /// </summary>
        public string Message { get; internal set; }

        /// <summary>
        ///     Gets a value indicating whether the message is considered Personally Identifiable Information 
        ///     (PII) or Business Identifiable Information (BII).
        /// </summary>
        public bool IdentifyingMessage => this.IdentifyingArgs.Any(a => a == "#");

        /// <summary>
        ///     Gets the message arguments or <see langword="null"/> if the message has no arguments.
        /// </summary>
        public TArgs Args { get; internal set; }

        /// <summary>
        ///     Gets a sequence JSON pointer paths to arguments properties that contain Personally Identifiable 
        ///     Information (PII) or Business Identifiable Information (BII). If a pointer points to the root args 
        ///     object itself, the entire message is considered identifying.
        /// </summary>
        public IEnumerable<string> IdentifyingArgs { get; internal set; }

        /// <summary>
        ///     Gets and sets the JSON representation of the key to the resource or root object to which this message 
        ///     refers or <see langword="null"/> if no resource is referenced.
        /// </summary>
        public string ObjectKey { get; internal set; }

        /// <summary>
        ///     Gets the JSON pointer reference to the object within the resource or root to which this message refers 
        ///     or <see langword="null"/> if no object is referenced.
        /// </summary>
        public string ObjectPointer { get; internal set; }

        /// <summary>
        ///     Gets the names of the object properties to which this message refers or <see langword="null"/> if no 
        ///     object properties are referenced.
        /// </summary>
        public IEnumerable<string> PropertyNames { get; internal set; }

        /// <summary>
        ///     Gets the JSON representation of the message arguments or <see langword="null"/> if the message has no
        ///     arguments.
        /// </summary>
        JObject INotificationMessage.Args => JObject.FromObject(this.Args);

        internal NotificationMessage()
        {
            
        } 
    }

    /// <summary>
    ///     Creates instances of the <see cref="NotificationMessage{TArgs}"/> class or the 
    ///     <see cref="NotificationMessage"/> class if the message has no arguments.
    /// </summary>
    public class NotificationMessage : NotificationMessage<JObject>
    {
        /// <summary>
        ///     Creates an instance of the <see cref="NotificationMessage{TArgs}"/> class.
        /// </summary>
        /// <typeparam name="TArgs">
        ///     The type used to represent the Args property.
        /// </typeparam>
        /// <param name="definition">
        ///     The <see cref="MessageDefinition"/> of the message being created.
        /// </param>
        /// <param name="messageCode">
        ///     The unique identity of the of the message type.
        /// </param>
        /// <param name="message">
        ///     The diagnostic description of the message.
        /// </param>
        /// <param name="args">
        ///     The message arguments or <see langword="null"/> if the message has no arguments.
        /// </param>
        /// <param name="objectKey">
        ///     The JSON representation of the key to the resource or root object to which this message refers or 
        ///     <see langword="null"/> if no resource is referenced.
        /// </param>
        /// <param name="objectPointer">
        ///     Tthe names of the object properties to which this message refers or <see langword="null"/> if no 
        ///     object properties are referenced.
        /// </param>
        /// <param name="propertyNames"></param>
        /// <returns>
        ///     A new <see cref="NotificationMessage{TArgs}"/>
        /// </returns>
        public static NotificationMessage<TArgs> CreateMessage<TArgs>(
            MessageDefinition definition,
            string messageCode,
            string message,
            TArgs args,
            string objectKey = null,
            string objectPointer = null,
            params string[] propertyNames)
        {
            return CreateMessage(
                definition,
                messageCode, 
                message, 
                args, 
                objectKey, 
                objectPointer, 
                (IEnumerable<string>)propertyNames);
        }

        /// <summary>
        ///     Creates an instance of the <see cref="NotificationMessage{TArgs}"/> class.
        /// </summary>
        /// <typeparam name="TArgs">
        ///     The type used to represent the Args property.
        /// </typeparam>
        /// <param name="definition">
        ///     The <see cref="MessageDefinition"/> of the message being created.
        /// </param>
        /// <param name="messageCode">
        ///     The unique identity of the of the message type.
        /// </param>
        /// <param name="message">
        ///     The diagnostic description of the message.
        /// </param>
        /// <param name="args">
        ///     The message arguments or <see langword="null"/> if the message has no arguments.
        /// </param>
        /// <param name="objectKey">
        ///     The JSON representation of the key to the resource or root object to which this message refers or 
        ///     <see langword="null"/> if no resource is referenced.
        /// </param>
        /// <param name="objectPointer">
        ///     Tthe names of the object properties to which this message refers or <see langword="null"/> if no 
        ///     object properties are referenced.
        /// </param>
        /// <param name="propertyNames"></param>
        /// <returns>
        ///     A new <see cref="NotificationMessage{TArgs}"/>
        /// </returns>
        public static NotificationMessage<TArgs> CreateMessage<TArgs>(
            MessageDefinition definition,
            string messageCode,
            string message,
            TArgs args,
            string objectKey = null,
            string objectPointer = null,
            IEnumerable<string> propertyNames = null)
        {
            Validate(definition, messageCode);

            return new NotificationMessage<TArgs>
            {
                MessageCode = messageCode,
                Message = message,
                Args = args,
                IdentifyingArgs = definition.IdentifyingArgs,
                ObjectKey = objectKey,
                // TODO: NotificationMessage.CreateMessage: May want to validate the object pointer.
                ObjectPointer = objectPointer,
                PropertyNames = propertyNames,
            };
        }

        /// <summary>
        ///     Creates an instance of the <see cref="NotificationMessage"/> class.
        /// </summary>
        /// <param name="definition">
        ///     The <see cref="MessageDefinition"/> of the message being created.
        /// </param>
        /// <param name="messageCode">
        ///     The unique identity of the of the message type.
        /// </param>
        /// <param name="message">
        ///     The diagnostic description of the message.
        /// </param>
        /// <param name="objectKey">
        ///     The JSON representation of the key to the resource or root object to which this message refers or 
        ///     <see langword="null"/> if no resource is referenced.
        /// </param>
        /// <param name="objectPointer">
        ///     Tthe names of the object properties to which this message refers or <see langword="null"/> if no 
        ///     object properties are referenced.
        /// </param>
        /// <param name="propertyNames"></param>
        /// <returns>
        ///     A new <see cref="NotificationMessage"/>
        /// </returns>
        public static NotificationMessage CreateMessage(
            MessageDefinition definition,
            string messageCode,
            string message,
            string objectKey = null,
            string objectPointer = null,
            IEnumerable<string> propertyNames = null)
        {
            Validate(definition, messageCode);

            return new NotificationMessage
            {
                MessageCode = messageCode,
                Message = message,
                Args = null,
                IdentifyingArgs = definition.IdentifyingArgs,
                ObjectKey = objectKey,
                // TODO: NotificationMessage.CreateMessage: May want to validate the object pointer.
                ObjectPointer = objectPointer,
                PropertyNames = propertyNames,
            };
        }

        private NotificationMessage()
        {

        }
    }
}
