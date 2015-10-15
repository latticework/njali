using System.Collections.Generic;

namespace Jali.Notification
{
    public interface INotificationMessage
    {
        string MessageCode { get; }
        MessagePriority Priority { get; }
        MessageSeverity Severity { get; }
        string Message { get; }
        bool IdentifyingMessage { get; }
        IList<object> Args { get; }
        IList<int> IdentifyingArgs { get; }
        string ObjectKey { get; }
        IList<string> PropertyNames { get; }
    }
}
