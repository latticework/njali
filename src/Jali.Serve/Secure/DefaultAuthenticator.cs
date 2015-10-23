using System.Net.Http;
using System.Threading.Tasks;

namespace Jali.Secure
{
    /// <summary>
    ///     A utility that provides a security context based on the current http request. This implementation supplies 
    ///     an empty identity.
    /// </summary>
    public class DefaultAuthenticator : IAuthenticator
    {
        // TODO: IAuthenticator.Authenticate: Add serviceContext parameter.
        ///// <param name="serviceContext">
        /////     The Jali Service context.
        ///// </param>
        /// <summary>
        ///     Provides a security context based on the current http request. Ths implementation supplies an empty 
        ///     identity.
        /// </summary>
        /// <param name="context">
        ///     The authenticator's execution context.
        /// </param>
        /// <param name="request">
        ///     The http request.
        /// </param>
        /// <returns>
        ///     The new security context.
        /// </returns>
        public virtual async Task<ISecurityContext> Authenticate(IExecutionContext context, HttpRequestMessage request)
        {
            return await Task.FromResult(new SecurityContext(new SecurityIdentity()));
        }
    }

}