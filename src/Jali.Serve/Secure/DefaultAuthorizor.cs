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
        /// <returns>
        ///     The result of the authorization operation.
        /// </returns>
        public virtual async Task<AuthorizationResult> Authorize(ISecurityContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            var result = new AuthorizationResult(
                new SecurityIdentity(),
                context.Impersonator != null ? new SecurityIdentity() : null,
                context.Deputy != null ? new SecurityIdentity() : null);

            return await Task.FromResult(result);
        }
    }
}