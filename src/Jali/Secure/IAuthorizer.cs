using System.Collections.Generic;
using System.Threading.Tasks;
using Jali.Secure;

namespace Jali.Secure
{
    /// <summary>
    ///     Represents a utility that provides application claims based on the current http request and the current 
    ///     message security context.
    /// </summary>
    public interface IAuthorizer
    {
        /// <summary>
        ///     Provides the <see cref="ISecurityContext"/> to be authorized.
        /// </summary>
        /// <param name="context">
        ///     The security context.
        /// </param>
        /// <param name="securityContext">
        ///     The security context.
        /// </param>
        /// <returns>
        ///     The result of the authorization operation.
        /// </returns>
        Task<AuthorizationResult> Authorize(IExecutionContext context, ISecurityContext securityContext);
    }

}