using System;
using System.Collections.Generic;
using System.Linq;

namespace Jali.Notification.Definition
{
    public class MessageLibrary
    {
        public MessageLibrary()
        {
            this.Files = new List<MessageDefinitionFile>();
        }

        public Uri Url { get; set; }
        public string Version { get; set; }

        public IList<MessageDefinitionFile> Files { get; }

        public IEnumerable<MessageDefinition> Messages => this.Files.SelectMany(f => f.Messages);
    }
}
