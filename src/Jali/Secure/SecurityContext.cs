using System.Collections.Generic;

namespace Jali.Secure
{
    /// <summary>
    ///     The security context for Jali application code.
    /// </summary>
    public sealed class SecurityContext : ISecurityContext
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="SecurityContext"/> class.
        /// </summary>
        /// <param name="identities">
        ///     A sequence of idenitities.
        /// </param>
        public SecurityContext(params SecurityIdentity[] identities)
            : this((IEnumerable<SecurityIdentity>)identities)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="SecurityContext"/> class.
        /// </summary>
        /// <param name="identities">
        ///     A sequence of idenitities.
        /// </param>
        public SecurityContext(IEnumerable<SecurityIdentity> identities)
        {
            this.User = new SecurityPrincipal(identities);
            this.Impersonator = null;
            this.Deputy = null;
        }
        /// <summary>
        ///     Represents the end user.
        /// </summary>
        public SecurityPrincipal User { get; }

        /// <summary>
        ///     Represents the impersonator or <see langword="null"/> if no impersonator exists.
        /// </summary>
        public SecurityPrincipal Impersonator { get; }

        /// <summary>
        ///     Represents the deputy or service account or <see langword="null"/> if no deputy exists.
        /// </summary>
        public SecurityPrincipal Deputy { get; }
    }
}