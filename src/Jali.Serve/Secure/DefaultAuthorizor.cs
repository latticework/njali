using System;
using System.Threading.Tasks;

namespace Jali.Secure
{
    /// <summary>
    ///     A utility that provides application claims based on the current http request and the current 
    ///     message security context. This implementation provides no authorizations.
    /// </summary>
    public class DefaultAuthorizor : IAuthorizer
    {
        /// <summary>
        ///     Provides the <see cref="ISecurityContext"/> to be authorized. This implementation provides empty 
        ///     identities.
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
        public virtual async Task<AuthorizationResult> Authorize(IExecutionContext context, ISecurityContext securityContext)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            var result = new AuthorizationResult(
                new SecurityIdentity(),
                securityContext.Impersonator != null ? new SecurityIdentity() : null,
                securityContext.Deputy != null ? new SecurityIdentity() : null);

            return await Task.FromResult(result);
        }
    }
}