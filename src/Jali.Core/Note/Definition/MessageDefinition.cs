using System.Collections.Generic;

namespace Jali.Note.Definition
{
    public class MessageDefinition
    {    
        public MessageDefinition()
        {
            this.IdentifyingArgs = new List<string>();
        }

        public string BaseCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Priority { get; set; }
        public string Severity { get; set; }
        public string Message { get; set; }
        public string Template { get; set; }
        public string Arguments { get; set; }
        public IList<string> IdentifyingArgs { get; }
    }
}
