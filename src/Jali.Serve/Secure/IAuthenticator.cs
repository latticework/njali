using System.Diagnostics.Contracts;
using System.Net.Http;
using System.Threading.Tasks;
using Jali.Serve;

namespace Jali.Secure
{
    /// <summary>
    ///     Represents a utility that provides a security context based on the current http request.
    /// </summary>
    public interface IAuthenticator
    {
        // TODO: IAuthenticator.Authenticate: Add serviceContext parameter.
        ///// <param name="serviceContext">
        /////     The Jali Service context.
        ///// </param>
        /// <summary>
        ///     Provides a security context based on the current http request.
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
        Task<ISecurityContext> Authenticate(
            IExecutionContext context, /*IServiceContext serviceContext,*/ HttpRequestMessage request);
    }
}