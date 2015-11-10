using System;
using System.Collections.Generic;
using System.Linq;
using Jali.Note;

#if !DNX && !PCL
using System.Runtime.Serialization;
#endif

namespace Jali.Core
{
#if !DNX && !PCL
    [Serializable]
#endif
    public class NotificationMessageException : Exception
    {
        public NotificationMessageException()
            : this(null, null)
        {
        }

        public NotificationMessageException(Exception innerException)
            : this(null, innerException)
        {
        }

        public NotificationMessageException(IEnumerable<INotificationMessage> messages)
            :this(messages, null)
        {
        }

        public NotificationMessageException(IEnumerable<INotificationMessage> messages, Exception innerException)
            : base(null, innerException)
        {
            this.Messages = NotificationMessageCollection.FromEnumerable(messages);
        }

#if !DNX && !PCL
        protected NotificationMessageException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
#endif

        public override string Message => this.Error.Message;

        public IEnumerable<INotificationMessage> Errors => this.Messages
            .Where(m => m.Severity <= MessageSeverity.Error).OrderByDescending(m => m.Severity);

        public INotificationMessage Error => this.Errors.First();

        public MessageSeverity Severity => this.Error.Severity;

        public NotificationMessageCollection Messages { get; }
    }
}
