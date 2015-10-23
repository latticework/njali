using System;
using System.Collections.Generic;

namespace Jali.Note.Definition
{
    public class MessageDomain
    {
        public MessageDomain()
        {
            this.Libraries = new List<MessageLibrary>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public Uri Url { get; set; }
        public string Version { get; set; }

        public IList<MessageLibrary> Libraries { get; }
    }
}