using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jali.Serve.Definition
{
    public class RoutineMessage
    {
        public string Action { get; set; }
        public ActionDirection Direction { get; set; }
        public SchemaReference Schema { get; set; }
    }
}
