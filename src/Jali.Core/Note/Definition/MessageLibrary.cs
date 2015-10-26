using System;
using System.Collections.Generic;
using System.Linq;

namespace Jali.Note.Definition
{
    /// <summary>
    ///     Represents a Jali message library.
    /// </summary>
    public class MessageLibrary
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="MessageLibrary"/>
        /// </summary>
        public MessageLibrary()
        {
            this.Documents = new Dictionary<string, MessageDefinitionDocument>();
        }

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
        ///     Gets or sets the name of the message library unique within its domain.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets unique registration code within its domain.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        ///     Gets or sets the location of the message library.
        /// </summary>
        public Uri Url { get; set; }

        /// <summary>
        ///     Gets or sets the message library's version.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        ///     Gets the documents that are part of this message library.
        /// </summary>
        public IDictionary<string, MessageDefinitionDocument> Documents { get; }

        /// <summary>
        ///     Gets all the messages that are part of this message library.
        /// </summary>
        public IDictionary<string, MessageDefinition> Messages => this.Documents.Values
            .SelectMany(d => d.Messages.Values)
            .ToDictionary(md => md.Name);
    }
}
