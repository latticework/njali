namespace Jali.Serve.Server.MessageConversion
{
    /// <summary>
    ///     The context under which message converson is executed.
    /// </summary>
    public class MessageConversionContext
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="MessageConversionContext"/> class.
        /// </summary>
        /// <param name="userContext">
        ///     The user's security context.
        /// </param>
        public MessageConversionContext(ISecurityContext userContext = null)
        {
            this.UserContext = userContext;
        }

        /// <summary>
        ///     Gets the user's security context.
        /// </summary>
        public ISecurityContext UserContext { get; }
    }

}