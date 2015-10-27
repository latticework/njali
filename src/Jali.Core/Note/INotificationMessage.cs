using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Jali.Note
{
    /// <summary>
    ///     Represents a Jali Notification Message used to communicate semantic messages such as errors and warnings to
    ///     service operation consumers. Can also be used within a stack layer for validations or other business rule
    ///     purposes.
    /// </summary>
    public interface INotificationMessage
    {
        /// <summary>
        ///     Gets the unique identity of the of the message type.
        /// </summary>
        string MessageCode { get; }

        /// <summary>
        ///     Gets the priority of the message.
        /// </summary>
        MessagePriority Priority { get; }

        /// <summary>
        ///     Gets the severity of the message.
        /// </summary>
        MessageSeverity Severity { get; }

        /// <summary>
        ///     Gets the diagnostic description of the message.
        /// </summary>
        string Message { get; }

        /// <summary>
        ///     Gets the JSON representation of the message arguments or <see langword="null"/> if the message has no
        ///     arguments.
        /// </summary>
        JObject Args { get; }

        /// <summary>
        ///     Gets the JSON representation of the key to the resource or root object to which this message refers or 
        ///     <see langword="null"/> if no resource is referenced.
        /// </summary>
        string ObjectKey { get; }

        /// <summary>
        ///     Gets the JSON pointer reference to the object within the resource or root to which this message refers 
        ///     or <see langword="null"/> if no object is referenced.
        /// </summary>
        string ObjectPointer { get; }

        /// <summary>
        ///     Gets the names of the object properties to which this message refers or <see langword="null"/> if no 
        ///     object properties are referenced.
        /// </summary>
        IEnumerable<string> PropertyNames { get; }
    }

    /// <summary>
    ///     Jali Notification Message used to communicate semantic messages such as errors and warnings to
    ///     service operation consumers. Can also be used within a stack layer for validations or other business rule
    ///     purposes.
    /// </summary>
    /// <typeparam name="TArgs">
    ///     The JSON serializable type used to represent the message arguments.
    /// </typeparam>
    public interface INotificationMessage<TArgs> : INotificationMessage
    {
        /// <summary>
        ///     Gets the typed representation of the message arguments or <see langword="null"/> if the message has no
        ///     arguments.
        /// </summary>
        new TArgs Args { get; set; }
    }
}
