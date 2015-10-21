namespace Jali
{
    /// <summary>
    ///     Explicit cross-cutting concerns provided to all Jali code.
    /// </summary>
    public class ExecutionContext : IExecutionContext
    {
        /// <summary>
        ///     Provides security authentication and auntorization services.
        /// </summary>
        public ISecurityContext Security { get; set; }

        /// <summary>
        ///     Provides configuration services.
        /// </summary>
        public IConfigurationContext Configuration { get; set; }

        /// <summary>
        ///     Provides logging services.
        /// </summary>
        public ILogContext Log { get; set; }

        /// <summary>
        ///     Provides metrics tracking services.
        /// </summary>
        public IMetricsContext Metrics { get; set; }
    }
}
