using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Jali.Core;

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
        ///     Gets the unique identifier for a deputy (i.e. service) account or <c>null</c> if no impersonator is 
        ///     present.
        /// </summary>
        public string DeputyId => this.GetFirstClaimValue(JaliClaimTypes.DeputySid);

        /// <summary>
        ///     Gets the login name for a deputy (i.e. service) account or <c>null</c> if no impersonator is present.
        /// </summary>
        public string DeputyUserName => this.GetFirstClaimValue(JaliClaimTypes.DeputyNameIdentifier);

        /// <summary>
        ///     Gets the given name for a deputy (i.e. service) account or <c>null</c> if no impersonator is present.
        /// </summary>
        public string DeputyGivenName => this.GetFirstClaimValue(JaliClaimTypes.DeputyGivenName);

        /// <summary>
        ///     Gets the surname for a deputy (i.e. service) account or <c>null</c> if no impersonator is present.
        /// </summary>
        public string DeputySurname => this.GetFirstClaimValue(JaliClaimTypes.DeputySurname);

        /// <summary>
        ///     Gets the unique identifier for an impersonating user or <c>null</c> if no impersonator is present.
        /// </summary>
        public string ImpersonatorId => this.GetFirstClaimValue(JaliClaimTypes.ImpersonatorSid);

        /// <summary>
        ///     Gets the login name for an impersonating user or <c>null</c> if no impersonator is present.
        /// </summary>
        public string ImpersonatorName => this.GetFirstClaimValue(JaliClaimTypes.ImpersonatorNameIdentifier);

        /// <summary>
        ///     Gets the given name for an impersonating user or <c>null</c> if no impersonator is present.
        /// </summary>
        public string ImpersonatorGivenName => this.GetFirstClaimValue(JaliClaimTypes.ImpersonatorGivenName);

        /// <summary>
        ///     Gets the surname for an impersonating user or <c>null</c> if no impersonator is present.
        /// </summary>
        public string ImpersonatorSurname => this.GetFirstClaimValue(JaliClaimTypes.ImpersonatorSurname);

        /// <summary>
        ///     Gets the unique identifier for the Jali Service tenant with which the user is associated.
        /// </summary>
        public string TenantId => this.GetFirstClaimValue(JaliClaimTypes.TenantId);

        /// <summary>
        ///     Gets the friendly name of the Jali Service tenant with which the user is associated.
        /// </summary>
        public string TenantName => this.GetFirstClaimValue(JaliClaimTypes.TenantName);

        /// <summary>
        ///     Gets the unique identifier for the Jali Service tenant for which the user is authorized.
        /// </summary>
        public string TenantOrgId => this.GetFirstClaimValue(JaliClaimTypes.TenantOrgId);

        /// <summary>
        ///     Gets the friendly name of the Jali Service tenant for which the user is authorized.
        /// </summary>
        public string TenantOrgName => this.GetFirstClaimValue(JaliClaimTypes.TenantOrgName);

        /// <summary>
        ///     Gets the unique identifier for the user represented by this <see cref="SecurityIdentity"/>.
        /// </summary>
        public string UserId => this.GetFirstClaimValue(WellKnownClaimTypes.Sid);

        /// <summary>
        ///     Gets the login name for the user represented by this <see cref="SecurityIdentity"/>.
        /// </summary>
        public string UserName => this.GetFirstClaimValue(WellKnownClaimTypes.NameIdentifier);

        /// <summary>
        ///     Gets the given name for the user represented by this <see cref="SecurityIdentity"/>.
        /// </summary>
        public string UserGivenName => this.GetFirstClaimValue(WellKnownClaimTypes.GivenName);

        /// <summary>
        ///     Gets the surname for the user represented by this <see cref="SecurityIdentity"/>.
        /// </summary>
        public string UserSurname => this.GetFirstClaimValue(WellKnownClaimTypes.Surname);

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

        private string GetFirstClaimValue(string type)
        {
            return this.Claims.FirstOrDefault(c => c.Type.EqualsOrdinalIgnoreCase(type))?.Value;
        }
    }

}