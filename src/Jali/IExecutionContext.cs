namespace Jali
{
    /// <summary>
    ///     Explicit cross-cutting concerns provided to all Jali code.
    /// </summary>
    public interface IExecutionContext
    {
        /// <summary>
        ///     Provides security authentication and auntorization services.
        /// </summary>
        ISecurityContext Security { get; }

        /// <summary>
        ///     Provides configuration services.
        /// </summary>
        IConfigurationContext Configuration { get; }

        /// <summary>
        ///     Provides logging services.
        /// </summary>
        ILogContext Log { get; }

        /// <summary>
        ///     Provides metrics tracking services.
        /// </summary>
        IMetricsContext Metrics { get; }

        /// <summary>
        ///     Creates an execution context from the current execution context.
        /// </summary>
        /// <param name="securityContext">
        ///     The security context to replace
        /// </param>
        /// <returns>
        ///     The new execution context.
        /// </returns>
        IExecutionContext MakeContext(ISecurityContext securityContext);
    }
}
