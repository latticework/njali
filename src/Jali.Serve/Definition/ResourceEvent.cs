﻿using System;

namespace Jali.Serve.Definition
{
    public class ResourceEvent
    {
        public Uri Url { get; set; }
        public string Name { get; set; }
        public SchemaReference Schema { get; set; }
        public DataTransmissionModes SupportedModes { get; set; }
    }
}
