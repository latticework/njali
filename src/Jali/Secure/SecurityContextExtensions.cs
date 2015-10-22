using System.Linq;

namespace Jali.Secure
{
    /// <summary>
    ///     Extension methods for the <see cref="ISecurityContext"/> class.
    /// </summary>
    public static class SecurityContextExtensions
    {
        /// <summary>
        ///     Returns a value that indicates whether the security context user holds the specified claim.
        /// </summary>
        /// <param name="context">
        ///     The security context.
        /// </param>
        /// <param name="type">
        ///     The claim type.
        /// </param>
        /// <param name="value">
        ///     The claim value.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if the claim was found for the user; otherwise <see langword="false"/>.
        /// </returns>
        public static bool HasClaim(this ISecurityContext context, string type, string value)
        {
            return context.User.HasClaim(type, value);
        }

        /// <summary>
        ///     Returns a value indicating whether the security context has a deputy.
        /// </summary>
        /// <param name="context">
        ///     The security context.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if the security context has a deputy; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool HasImpersonator(this ISecurityContext context)
        {
            return context.Impersonator != null;
        }

        /// <summary>
        ///     Returns a value indicating whether the security context has an impersonator.
        /// </summary>
        /// <param name="context">
        ///     The security context.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if the security context has an impersonator; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool HasDeputy(this ISecurityContext context)
        {
            return context.Deputy != null;
        }
    }

}