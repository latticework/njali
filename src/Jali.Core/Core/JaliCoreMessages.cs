using Jali.Notification;

namespace Jali.Core
{
    public static class JaliCoreMessages
    {
        private const string _authorityCode = "0000";
        private const string _domainCode = "00";
        private const string _libraryCode = "00";
        private const string _messagePrefix = "0100000000";

        public static NotificationMessage CreateInternalError(
            string message, string objectKey = null, params string[] propertyNames)
        {
            var result = new NotificationMessage
            {
                MessageCode = "0100000000FF0001",
                Args = {message},
                MessageTemplate = "Internal error: ${message}",
                ObjectKey = objectKey,
            };

            result.PropertyNames.AddRange(propertyNames);

            return result;
        }
    }
}
