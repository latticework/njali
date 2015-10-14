using System;
using System.Collections.Generic;

namespace Jali.Serve.Definition
{
    public class ResourceConverter
    {
        public ResourceConverter()
        {
            this.Conversions = new Dictionary<string, SchemaConversion>();
        }

        public Uri Url { get; set; }
        public string Version { get; set; }
        public IDictionary<string, SchemaConversion> Conversions { get; }
    }
}
