namespace Jali.Serve
{
    /// <summary>
    ///     The security context of a message.
    /// </summary>
    public class MessageCredentials
    {
        /// <summary>
        ///     The sending party's credentials.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        ///     An impersonator's credentials for messages that support impersonation.
        /// </summary>
        public string ImpersonatorId { get; set; }

        /// <summary>
        ///     The credentials of the deputy (i.e. service) account that is sending the message on behalf of the user.
        /// </summary>
        public string DeputyId { get; set; }
    }
}