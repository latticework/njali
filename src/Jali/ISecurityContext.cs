using System;
using System.Collections.Generic;

namespace Jali
{
    /// <summary>
    ///     The security context for Jali application code.
    /// </summary>
    public interface ISecurityContext
    {
        /// <summary>
        ///     Claims of the end user.
        /// </summary>
        IEnumerable<Claim> UserClaims { get; }

        /// <summary>
        ///     Claims of any impersonator.
        /// </summary>
        IEnumerable<Claim> ImpersonatorClaims { get; }

        /// <summary>
        ///     Claims of any deputy or service account.
        /// </summary>
        IEnumerable<Claim> DeputyClaims { get; }
    }
}
