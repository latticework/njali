using System;
using System.Collections.Generic;

namespace Jali.Notification.Definition
{
    public class MessageLibraryRegistry
    {
        public MessageLibraryRegistry()
        {
            this.Authorities = new List<MessageAuthority>();
        }

        public Uri Url { get; set; }
        public string Version { get; set; }

        public int Schema { get; set; }
        public string SchemaVersion { get; set; }
        public IList<MessageAuthority> Authorities { get; }
    }
}
