using System;
using System.Collections.Generic;
using Jali.Secure;

namespace Jali
{
    /// <summary>
    ///     The security context for Jali application code.
    /// </summary>
    public interface ISecurityContext
    {
        /// <summary>
        ///     Represents the end user.
        /// </summary>
        SecurityPrincipal User { get; }

        /// <summary>
        ///     Represents the impersonator or <see langword="null"/> if no impersonator exists.
        /// </summary>
        SecurityPrincipal Impersonator { get; }

        /// <summary>
        ///     Represents the deputy or service account or <see langword="null"/> if no deputy exists.
        /// </summary>
        SecurityPrincipal Deputy { get; }
    }
}
