using System.Collections.Generic;

namespace Jali.Note.Definition
{
    public class MessageDefinition
    {    
        public MessageDefinition()
        {
            this.Arguments = new List<ArgumentDefinition>();
        }

        public string BaseCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Priority { get; set; }
        public string Severity { get; set; }
        public string Message { get; set; }
        public string Template { get; set; }
        public IList<ArgumentDefinition> Arguments { get; }
        public bool Identifying { get; set; }
    }
}
