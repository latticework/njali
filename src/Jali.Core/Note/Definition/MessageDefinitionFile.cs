using System;
using System.Collections.Generic;

namespace Jali.Note.Definition
{
    public class MessageDefinitionFile
    {
        public MessageDefinitionFile()
        {
            this.Messages = new List<MessageDefinition>();
        }

        public Uri Url { get; set; }
        public string Version { get; set; }

        public string Authority { get; set; }
        public string Domain { get; set; }
        public string Library { get; set; }

        public IList<MessageDefinition> Messages { get; }
    }
}