using System.Collections.Generic;
using System.Linq;

namespace Jali.Note
{
    public static class NotificationMessageExtensions
    {
        public static MessagePriority GetPriority(this IEnumerable<NotificationMessage> messages)
        {
            return (messages.Any()) ? messages.Min(m => m.Priority) : (MessagePriority)15;
        }

        public static MessageSeverity GetSeverity(this IEnumerable<NotificationMessage> messages)
        {
            return (messages.Any()) ? messages.Min(m => m.Severity) : (MessageSeverity)15;
        }

        public static bool HasErrors(this IEnumerable<NotificationMessage> messages)
        {
           return messages.GetSeverity() <= MessageSeverity.Error;
        }
    }
}