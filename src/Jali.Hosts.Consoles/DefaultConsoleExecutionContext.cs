using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jali.Hosts.Console
{
    public class DefaultConsoleExecutionContext : IExecutionContext
    {
        public IConfigurationContext Configuration { get; }
        public ILogContext Log { get; }
        public IMetricsContext Metrics { get; }
    }
}
