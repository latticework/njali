using System.Collections.Generic;
using System.Linq;

namespace Jali.Notification
{
    public static class NotificationMessageExtensions
    {
        public static MessagePriority GetPriority(this IEnumerable<NotificationMessage> messages)
        {
            return messages.Min(m => m.Priority);
        }

        public static MessageSeverity GetSeverity(this IEnumerable<NotificationMessage> messages)
        {
            return messages.Min(m => m.Severity);
        }

        public static bool HasErrors(this IEnumerable<NotificationMessage> messages)
        {
           return messages.GetSeverity() <= MessageSeverity.Error;
        }
    }
}