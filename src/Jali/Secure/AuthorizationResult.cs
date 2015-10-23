namespace Jali.Secure
{
    /// <summary>
    ///     The result of the <see cref="IAuthorizer.Authorize"/> operation.
    /// </summary>
    public class AuthorizationResult
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AuthorizationResult"/> class.
        /// </summary>
        /// <param name="userAuthorizations">
        ///     A <see cref="SecurityIdentity"/> that represents the user authorizations provided by the authority.
        /// </param>
        /// <param name="impersonatorAuthorizations">
        ///     A <see cref="SecurityIdentity"/> that represents the impersonating user authorizations provided by the 
        ///     authority or <see langword="null"/> if no impersonator is present.
        /// </param>
        /// <param name="deputyAuthorizations">
        ///     A <see cref="SecurityIdentity"/> that represents the deputy account authorizations provided by the 
        ///     authority or <see langword="null"/> if no deputy is present.
        /// </param>
        public AuthorizationResult(
            SecurityIdentity userAuthorizations,
            SecurityIdentity impersonatorAuthorizations,
            SecurityIdentity deputyAuthorizations)
        {
            this.UserAuthorizations = userAuthorizations;
            this.ImpersonatorAuthorizations = impersonatorAuthorizations;
            this.DeputyAuthorizations = deputyAuthorizations;
        }

        public SecurityIdentity UserAuthorizations { get; }
        public SecurityIdentity ImpersonatorAuthorizations { get; }
        public SecurityIdentity DeputyAuthorizations { get; }
    }

}