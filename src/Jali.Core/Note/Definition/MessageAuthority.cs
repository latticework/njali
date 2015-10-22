using System;
using System.Collections.Generic;

namespace Jali.Note.Definition
{
    public class MessageAuthority
    {
        public MessageAuthority()
        {
            this.Domains = new List<MessageDomain>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public Uri Url { get; set; }
        public string Version { get; set; }

        public IList<MessageDomain> Domains { get; }
    }
}
