using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Threading.Tasks;

namespace Jali.Serve.Definition
{
    public class RestMethod
    {
        public string Method { get; set; }
        public Routine Routine { get; set; }
        public RestMethodRequest Request { get; set; }
        public RestMethodResponse Response { get; set; }
    }
}
