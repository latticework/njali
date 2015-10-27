using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Jali.Note.Definition
{
    /// <summary>
    ///     Represents a Jali Notification Message type used to communicate semantic messages such as errors and 
    ///     warnings to service operation consumers. Can also be used within a stack layer for validations or other business rule
    ///     purposes.
    /// </summary>
    public class MessageDefinition
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="MessageDefinition"/> class.
        /// </summary>
        public MessageDefinition()
        {
            this.IdentifyingArgs = new List<string>();
        }

        /// <summary>
        ///     The base code that, when combined with the registered identities of the message authority, domian,
        ///     and library and with the message severity and priority, specifies the message code of the message
        ///     definition.
        /// </summary>
        public string BaseCode { get; set; }

        /// <summary>
        ///     The unique name of the message within the message library.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     A description of the message's purpose.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     The message priority.
        /// </summary>
        public MessagePriority Priority { get; set; }

        /// <summary>
        ///     The message severity.
        /// </summary>
        public MessageSeverity Severity { get; set; }

        /// <summary>
        ///     The static diagostic message that applies to all messages of this type.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        ///     A ES6 template literal that references argument properties to create the diagnostic message string.
        /// </summary>
        public string Template { get; set; }

        /// <summary>
        ///     A JSON Schema string representation of the arguments used by this message or <see langword="null"/> if 
        ///     the message has no arguments.
        /// </summary>
        public JObject ArgumentSchema { get; set; }

        /// <summary>
        ///     A sequence JSON pointer paths to arguments properties that contain Personally Identifiable Information 
        ///     (PII) or Business Identifiable Information (BII). If a pointer points to the root args object itself,
        ///     the entire message is considered identifying.
        /// </summary>
        public IList<string> IdentifyingArgs { get; }
    }
}
