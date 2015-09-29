using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Schema;

namespace Jali.Serve.Definition
{
    public class SchemaReference
    {
        public SchemaType SchemaType { get; set; }
        public JSchema Schema { get; set; }
        public ResourceEvent Event { get; set; }
    }
}
