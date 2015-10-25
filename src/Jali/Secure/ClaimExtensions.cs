using System;
using System.Collections.Generic;
using System.Linq;
using Jali.Core;

namespace Jali.Secure
{

    /// <summary>
    ///     Provides extensions for the <see cref="Claim"/> class.
    /// </summary>
    public static class ClaimExtensions
    {
        /// <summary>
        ///     Raises an authorization error if the specified claim is not found in the sequence of claims.
        /// </summary>
        /// <param name="claims">
        ///     The claims to be reviewed.
        /// </param>
        /// <param name="userId">
        ///     The Jali UserID of the authorized user.
        /// </param>
        /// <param name="type">
        ///     The type of the required claim.
        /// </param>
        /// <param name="value">
        ///     The required claim's value.
        /// </param>
        /// <param name="objectKey">
        ///     The JSON representation of the key to the resource or root object to which this message 
        ///     refers or <see langword="null"/> if no resource is referenced.
        /// </param>
        /// <param name="objectPointer">
        ///     The JSON pointer reference to the object within the resource or root to which this message refers 
        ///     or <see langword="null"/> if no object is referenced.
        /// </param>
        /// <param name="propertyNames">
        ///     The names of the object properties to which this message refers or <see langword="null"/> if no 
        ///     object properties are referenced.
        /// </param>
        public static void DemandClaim(this IEnumerable<Claim> claims, 
            string userId, 
            string type, 
            string value, 
            string objectKey = null,
            string objectPointer = null,
            params string[] propertyNames)
        {
            if (!claims.HasClaim(type, value))
            {
                throw new DomainErrorException(new[]
                {
                    JaliJaliMessages.Errors.AuthorizationError.Create(
                        userId, type, value, objectKey, objectPointer, propertyNames)
                });
            }

        }

        /// <summary>
        ///     Returns the value of the first claim that matches the specified type.
        /// </summary>
        /// <param name="claims">
        ///     The sequence of claims.
        /// </param>
        /// <param name="type">
        ///     The claim type being matched.
        /// </param>
        /// <returns>
        ///     A claim value or <see langword="null"/> if the type is not matched.
        /// </returns>
        public static string GetClaimValue(this IEnumerable<Claim> claims, string type)
        {
            return claims.FirstOrDefault(c => c.Type.EqualsOrdinalIgnoreCase(type))?.Value;
        }

        /// <summary>
        ///     Returns the value of all the cliams that match teh specified type. 
        /// </summary>
        /// <param name="claims">
        ///     The sequence of claims.
        /// </param>
        /// <param name="type">
        ///     The claim type being matched.
        /// </param>
        /// <returns>
        ///     A list of claim values.
        /// </returns>
        public static IEnumerable<string> GetClaimValues(this IEnumerable<Claim> claims, string type)
        {
            return claims.Where(c => c.Type.EqualsOrdinalIgnoreCase(type)).Select(c => c.Value);
        }


        /// <summary>
        ///     Returns a value indicating whether the specified claim is held by the sequence of claims.
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
        /// <returns>
        ///     <see langword="true"/> if the sequence of claims holds the specified claim; otherwise, 
        ///     <see langword="false"/>.
        /// </returns>
        public static bool HasClaim(this IEnumerable<Claim> claims, string type, string value)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (value == null) throw new ArgumentNullException(nameof(value));

            return claims.Any(c => 
                c.Type.EqualsOrdinalIgnoreCase(type) && c.Value.EqualsOrdinalIgnoreCase(value));
        }
    }
}