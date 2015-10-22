using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

namespace Jali.Secure
{
    /// <summary>
    ///     Represents an authenticated or authorized user by a security authority.
    /// </summary>
    public class SecurityIdentity
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="SecurityIdentity"/> class as unauthenticated.
        /// </summary>
        /// <param name="claims">
        ///     The list of authorized claims.
        /// </param>
        public SecurityIdentity(params Claim[] claims)  : this((IEnumerable<Claim>) claims)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="SecurityIdentity"/> class.
        /// </summary>
        /// <param name="claims">
        ///     The list of authorized claims.
        /// </param>
        public SecurityIdentity(IEnumerable<Claim> claims)
        {
            var list = (claims == null)
                ? (IEnumerable<Claim>)new Claim[] { }
                : (IEnumerable<Claim>)new List<Claim>(claims);

            this.Claims = new ReadOnlyCollection<Claim>(claims?.ToList() ?? new List<Claim>());
        }

        /// <summary>
        ///     Gets a value indicating whether the identity is authenticated.
        /// </summary>
        public bool Authenticated => this.Claims.Any();

        /// <summary>
        ///     Claims of the identity.
        /// </summary>
        public IEnumerable<Claim> Claims { get; }

        /// <summary>
        ///     Returns a value indicating whether whether the specified claim is held by the identity.
        /// </summary>
        /// <param name="type">
        ///     The claim type.
        /// </param>
        /// <param name="value">
        ///     The claim value.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if the identity holds the specified claim; otherwise, <see langword="false"/>.
        /// </returns>
        public bool HasClaim(string type, string value)
        {
            return this.Claims.HasClaim(type, value);
        }
    }

}