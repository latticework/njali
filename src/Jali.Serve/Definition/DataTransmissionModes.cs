using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jali.Serve.Definition
{
    [Flags]
    public enum DataTransmissionModes
    {
        None = 0,
        Result = 1,
        Notify = 2,
        Patch = 4,
        Full = 8,
        All = Result | Notify | Patch | Full,
    }
}
