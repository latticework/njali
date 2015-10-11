using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jali.Serve.Definition
{
    public class RestMethodRequest
    {
        public RoutineMessageReference Message { get; set; }
        public DataTransmissionModes Mode { get; set; }
    }
}
