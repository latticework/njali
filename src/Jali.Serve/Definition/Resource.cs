using System;
using System.Collections.Generic;
using Newtonsoft.Json.Schema;

namespace Jali.Serve.Definition
{
    public class Resource
    {
        public Resource()
        {
            this.Routines = new Dictionary<string, Routine>();
            this.Methods = new Dictionary<string, RestMethod>();
            this.Events = new Dictionary<string, ResourceEvent>();
            this.Converters = new Dictionary<string, ResourceConverter>();
        }

        public Uri Url { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public JSchema Schema { get; set; }
        public IDictionary<string, Routine> Routines { get; }
        public IDictionary<string, RestMethod> Methods { get; }
        public IDictionary<string, ResourceEvent> Events { get; }
        public IDictionary<string, ResourceConverter> Converters { get; }
    }
}
