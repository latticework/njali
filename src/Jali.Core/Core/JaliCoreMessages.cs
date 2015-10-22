using Jali.Note;

namespace Jali.Core
{
    /// <summary>
    ///     Notification messages defined by the Jali Core package.
    /// </summary>
    public static class JaliCoreMessages
    {
        //private const string _authorityCode = "0000";
        //private const string _domainCode = "00";
        //private const string _libraryCode = "00";
        //private const string _messagePrefix = "1000000000";

        /// <summary>
        ///     Communicates that a critical internal application error has occurred and application should terminate.
        /// </summary>
        /// <param name="message">
        ///     The description of the internal error.
        /// </param>
        /// <param name="objectKey">
        ///     The key of the object the message pertains to or <see langword="null"/> if the message does not refer 
        ///     to one identifiable object.
        /// </param>
        /// <param name="propertyNames">
        ///     The names of the object properties that the message pertains to or <see langword="null"/> if the 
        ///     message does not refer to a specific set of object properties.
        /// </param>
        /// <returns>
        ///     The new message.
        /// </returns>
        public static NotificationMessage CreateInternalError(
            string message, string objectKey = null, params string[] propertyNames)
        {
            var result = new NotificationMessage
            {
                MessageCode = "1000000000FF0001",
                Args = { message },
                Message = $"Internal error: {message}",
                ObjectKey = objectKey,
            };

            result.PropertyNames.AddRange(propertyNames);

            return result;
        }
    }
}
