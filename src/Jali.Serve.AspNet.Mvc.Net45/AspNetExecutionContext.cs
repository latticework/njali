using System.Linq;
using System.Security.Principal;
using Jali.Core;
using Jali.Secure;

// ReSharper disable once CheckNamespace
namespace Jali
{
    /// <summary>
    ///     Privides a Windows AppPool identity as the default execution context.
    /// </summary>
    public class AspNetExecutionContext : DefaultExecutionContext
    {
        /// <summary>
        ///     Initializes a new instance of the <seealso cref="AspNetExecutionContext"/> class with claims from the 
        ///     AppPool's <seealso cref="WindowsIdentity"/>.
        /// </summary>
        public AspNetExecutionContext() : base(GetWindowsIdentity())
        {
        }

        private static SecurityIdentity GetWindowsIdentity()
        {
            var claims = WindowsIdentity.GetCurrent()?.Claims.Select(c => new Claim(c.Type, c.Value));

            if (claims == null)
            {
                throw new InternalErrorException("The AppPool identity is not assigned. Cannot create Jali Service");
            }

            return new SecurityIdentity(claims);
        }
    }
}
