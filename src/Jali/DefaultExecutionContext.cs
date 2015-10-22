using Jali.Secure;

namespace Jali
{
    /// <summary>
    ///     Explicit cross-cutting concerns provided to all Jali code.
    /// </summary>
    public class DefaultExecutionContext : IExecutionContext
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultExecutionContext"/> class.
        /// </summary>
        /// <param name="identity"></param>
        public DefaultExecutionContext(SecurityIdentity identity)
        {
            var identities = identity == null ? new SecurityIdentity[] {} : new[] {identity};
            this.Security = new SecurityContext(identities);
        }

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

        /// <summary>
        ///     Creates an execution context from the current execution context.
        /// </summary>
        /// <param name="securityContext">
        ///     The security context to replace
        /// </param>
        /// <returns>
        ///     The new execution context.
        /// </returns>
        public virtual IExecutionContext MakeContext(ISecurityContext securityContext)
        {
            // TODO: DefaultExecutionContex.MakeContext: The security identity null parameter has code smell.
            return new DefaultExecutionContext(null)
            {
                Security = securityContext,
                Configuration = this.Configuration,
                Log = this.Log,
                Metrics = this.Metrics,
            };
        }
    }
}
