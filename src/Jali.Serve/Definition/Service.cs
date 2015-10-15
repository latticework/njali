using System;
using System.Collections.Generic;

namespace Jali.Serve.Definition
{
    public class Service
    {
        public Service()
        {
            this.Resources = new Dictionary<string, Resource>();
        }

        public Uri Url { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public string Description { get; set; }
        public IDictionary<string, Resource> Resources { get; }


    }
}
