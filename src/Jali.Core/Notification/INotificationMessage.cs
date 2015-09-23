using System.Collections.Generic;

namespace Jali.Notification
{
    public interface INotificationMessage
    {
        int MessageCode { get; }
        MessagePriority Priority { get; }
        MessageSeverity Severity { get; }
        string Message { get; }
        IEnumerable<object> Args { get; }
    }
}
