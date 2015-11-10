using Newtonsoft.Json.Schema;

namespace Jali.Serve.Definition
{
    public class SchemaReference
    {
        public SchemaType SchemaType { get; set; }
        public JSchema Schema { get; set; }
        public string Event { get; set; }
        public SchemaReferenceOptions Options { get; set; }
    }
}
