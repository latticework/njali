using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Jali.Core;

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
        ///     Adds the specified identity to the security principal.
        /// </summary>
        /// <param name="identity">
        ///     The identity to add.
        /// </param>
        public void AddIdentity(SecurityIdentity identity)
        {
            if (identity == null) throw new ArgumentNullException(nameof(identity));

            this.AddIdentities(new[] {identity});
        }

        /// <summary>
        ///     Adds the specified identities to the security principal.
        /// </summary>
        /// <param name="first">
        ///     The first identity.
        /// </param>
        /// <param name="second">
        ///     The second identity.
        /// </param>
        /// <param name="other">
        ///     Any more identities.
        /// </param>
        public void AddIdentities(SecurityIdentity first, SecurityIdentity second, params SecurityIdentity[] other)
        {
            if (first == null) throw new ArgumentNullException(nameof(first));
            if (second == null) throw new ArgumentNullException(nameof(second));

            var identities = new SecurityIdentity[2 + other.Length];

            identities[0] = first;
            identities[1] = second;
            other.CopyTo(identities, 0);

            AddIdentities(identities);
        }

        /// <summary>
        ///     Adds the specified identities to the security principal.
        /// </summary>
        /// <param name="identities">
        ///     The identities to add.
        /// </param>
        public void AddIdentities(IEnumerable<SecurityIdentity> identities)
        {
            if (identities == null) throw new ArgumentNullException(nameof(identities));

            this._identities.AddRange(identities);
        }

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