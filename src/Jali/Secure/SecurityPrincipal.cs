using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Jali.Secure
{
    /// <summary>
    ///     Represents a Jali security principal containing zero or more identities.
    /// </summary>
    public class SecurityPrincipal
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="SecurityPrincipal"/> class.
        /// </summary>
        /// <param name="identities">
        ///     Security identities.
        /// </param>
        public SecurityPrincipal(params SecurityIdentity[] identities) 
            : this((IEnumerable<SecurityIdentity>)identities) { }


        /// <summary>
        ///     Initializes a new instance of the <see cref="SecurityPrincipal"/> class.
        /// </summary>
        /// <param name="identities">
        ///     Security identities.
        /// </param>
        public SecurityPrincipal(IEnumerable<SecurityIdentity> identities)
        {
            this._identities = new List<SecurityIdentity>(identities ?? new[] { new SecurityIdentity() });
        }

        /// <summary>
        ///     Gets a value indicating whether the principal is authenticated.
        /// </summary>
        public bool Authenticated => this._identities.Any(id => id.Authenticated);

        /// <summary>
        ///     Gets a collection of all the principal identities' claims.
        /// </summary>
        public IEnumerable<Claim> Claims => this.Identities.SelectMany(i => i.Claims);


        /// <summary>
        ///     Gets a sequence of the principal's identities.
        /// </summary>
        public IEnumerable<SecurityIdentity> Identities => new ReadOnlyCollection<SecurityIdentity>(this._identities);

        /// <summary>
        ///     Returns a value that indicates whether the specified claim is held by the principal.
        /// </summary>
        /// <param name="type">
        ///     The claim type.
        /// </param>
        /// <param name="value">
        ///     The claim value.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if the principal holds the specified claim; otherwise, <see langword="false"/>.
        /// </returns>
        public bool HasClaim(string type, string value)
        {
            return this._identities.Any(i => i.HasClaim(type, value));
        }

        private readonly IList<SecurityIdentity> _identities;
    }

}