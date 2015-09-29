using System;

namespace Jali.Serve
{
    public class MessageContract
    {
        public Uri Url { get; set; }
        public string ConsumerId { get; set; }
        public string Version { get; set; }
    }
}