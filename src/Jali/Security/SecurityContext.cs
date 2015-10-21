using System.Collections.Generic;

namespace Jali.Security
{
    /// <summary>
    ///     The security context for Jali application code.
    /// </summary>
    public sealed class SecurityContext : ISecurityContext
    {
        /// <summary>
        ///     Claims of the end user.
        /// </summary>
        public IEnumerable<Claim> UserClaims
        {
            get { return _userClaims; }
        }

        /// <summary>
        ///     Claims of the impersonator or <see langword="null"/> if no impersonator exists.
        /// </summary>
        public IEnumerable<Claim> ImpersonatorClaims
        {
            get { return _impersonatorClaims; }
        }

        /// <summary>
        ///     Claims of any deputy or service account or <see langword="null"/> if no deputy exists.
        /// </summary>
        public IEnumerable<Claim> DeputyClaims
        {
            get { return _deputyClaims; }
        }

        private IEnumerable<Claim> _userClaims;
        private IEnumerable<Claim> _impersonatorClaims;
        private IEnumerable<Claim> _deputyClaims;
    }
}