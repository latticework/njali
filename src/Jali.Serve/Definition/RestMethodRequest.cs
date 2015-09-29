using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jali.Serve.Definition
{
    public class RestMethodRequest
    {
        public RoutineMessage Message { get; set; }
        public DataTransmissionModes Mode { get; set; }
    }
}
