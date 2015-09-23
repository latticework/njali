namespace Jali
{
    public class ExecutionContext : IExecutionContext
    {
        public IConfigurationContext Configuration { get; set; }
        public ILogContext Log { get; set; }
        public IMetricsContext Metrics { get; set; }
    }
}
