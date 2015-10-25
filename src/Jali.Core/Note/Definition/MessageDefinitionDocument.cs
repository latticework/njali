using System;
using System.Collections.Generic;

namespace Jali.Note.Definition
{
    /// <summary>
    ///     Represents a collection of messages defined for a source code package as part of a message library.
    /// </summary>
    public class MessageDefinitionDocument
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="MessageDefinitionDocument"/> class.
        /// </summary>
        public MessageDefinitionDocument()
        {
            this.Messages = new Dictionary<string, MessageDefinition>();
        }

        /// <summary>
        ///     The message definition document's name that is unique within the message library.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     The message definition document's location.
        /// </summary>
        public Uri Url { get; set; }

        // TODO: MessageDefinition Document: It doesn't seem useful for the messge library or document to have a version. 
        /// <summary>
        ///  The message library's version number.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        ///     The registration reference of the authority registered with Jali that assigned the message domain.
        /// </summary>
        public MessageRegistrationReference Authority { get; set; }

        /// <summary>
        ///     The registration reference of the domain registered  with the authority that assigned the message 
        ///     library.
        /// </summary>
        public MessageRegistrationReference Domain { get; set; }

        /// <summary>
        ///     The registration reference of the message library that this message is a part of.
        /// </summary>
        public MessageRegistrationReference Library { get; set; }

        /// <summary>
        ///     Gets the messages that are part of this message definition document.
        /// </summary>
        public IDictionary<string, MessageDefinition> Messages { get; }
    }
}