using System;
using System.Collections.Generic;

namespace Jali.Serve.Definition
{
    public class Routine
    {
        public Routine()
        {
            this.Messages = new Dictionary<string, RoutineMessage>();
        }

        public Uri Url { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public AuthenticationRequirement DefaultAuthentication { get; set; }
        public Uri EntryPoint { get; set; }
        public IDictionary<string, RoutineMessage> Messages { get; }
    }
}
