namespace Jali
{
    public interface IExecutionContext
    {
        IConfigurationContext Configuration { get; }
        ILogContext Log { get; }
        IMetricsContext Metrics { get; }
    }
}
