using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jali.Serve
{
    public class MessageConnection
    {
        public EndpointPartition Sender { get; set; }
        public EndpointPartition Receiver { get; set; }
    }
}
