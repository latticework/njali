namespace Jali.Secure
{
    /// <summary>
    ///     Identifies claims related to Jali Services.
    /// </summary>
    /// <seealso href="https://msdn.microsoft.com/en-us/library/jj552963(v=vs.85).aspx"/>
    public static class JaliClaimTypes
    {
        /// <summary>
        ///     Specifies the Security Identifier (SID) for an impersonating user.
        /// </summary>
        public const string ImpersonatorSid = _claimTypePrefix + "impersonatorSid";

        /// <summary>
        ///     Specifies the given name for an impersonating user.
        /// </summary>
        public const string ImpersonatorNameIdentifier = _claimTypePrefix + "impersonatorNameIdentifier";

        /// <summary>
        ///     Specifies the given name for an impersonating user.
        /// </summary>
        public const string ImpersonatorGivenName = _claimTypePrefix + "impersonatorGivenName";

        /// <summary>
        ///     Specifies the surname for an impersonating user.
        /// </summary>
        public const string ImpersonatorSurname = _claimTypePrefix + "impersonatorSurname";

        /// <summary>
        ///     Specifies the Security Identifier (SID) for a deputy (i.e. service) account.
        /// </summary>
        public const string DeputySid = _claimTypePrefix + "deputySid";

        /// <summary>
        ///     Specifies the unique name for a deputy (i.e. service) account.
        /// </summary>
        public const string DeputyNameIdentifier = _claimTypePrefix + "deputyNameIdentifier";

        /// <summary>
        ///     Specifies the given name for a deputy (i.e. service) account.
        /// </summary>
        public const string DeputyGivenName = _claimTypePrefix + "deputyGivenName";

        /// <summary>
        ///     Specifies the surname for an impersonating user.
        /// </summary>
        public const string DeputySurname = _claimTypePrefix + "deputySurname";

        /// <summary>
        ///     Identifies the Jali Service tenant with which the user session is associated.
        /// </summary>
        public const string TenantId = _claimTypePrefix + "tenantid";

        /// <summary>
        ///     Identifies the Jali Service tenant organization with which the user session is authorized.
        /// </summary>
        public const string TenantOrgId = _claimTypePrefix + "tenantorgid";

        /// <summary>
        ///     Specifies the friendly name of the tenant.
        /// </summary>
        public const string TenantName = _claimTypePrefix + "tenantname";

        /// <summary>
        ///     Specifies the friendly name of the tenant organization.
        /// </summary>
        public const string TenantOrgName = _claimTypePrefix + "tenantorgname";

        private const string _claimTypePrefix = "http://schemas.jali.io/ws/2015/11/identity/claims/";
    }
}
