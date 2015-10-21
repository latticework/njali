using System;
using System.Collections.Generic;
using System.Linq;

namespace Jali
{

    /// <summary>
    ///     Provides extensions for the <see cref="Claim"/> class.
    /// </summary>
    public static class ClaimExtensions
    {
        public static void AssertClaim(this IEnumerable<Claim> claims, string type, string value)
        {
            
        }

        /// <summary>
        ///     Returns a value indicating whether the specified claim is held by sequence of claims.
        /// </summary>
        /// <param name="claims">
        ///     The claims sequence to check.
        /// </param>
        /// <param name="type">
        ///     The claim type.
        /// </param>
        /// <param name="value">
        ///     The claim value.
        /// </param>
        /// <returns></returns>
        public static bool HasClaim(this IEnumerable<Claim> claims, string type, string value)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (value == null) throw new ArgumentNullException(nameof(value));

            return claims.Any(c => c.Type == type && c.Value == value);
        }
    }
}