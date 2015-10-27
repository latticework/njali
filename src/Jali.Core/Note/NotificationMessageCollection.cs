using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Jali.Note
{
    public class NotificationMessageCollection : Collection<INotificationMessage>
    {
        public NotificationMessageCollection()
            : this(null)
        {
        }

        public NotificationMessageCollection(IEnumerable<INotificationMessage> messages)
            :base(NotificationMessageCollection.CreateMessageList(messages))
        {
            
        }

        public NotificationMessageCollection Prepend(IEnumerable<INotificationMessage> messages)
        {
            var messageArray = messages as INotificationMessage[] ?? messages.ToArray();

            foreach (var message in messageArray.Reverse())
            {
                this.InsertItem(0, message);
            }

            return this;
        }

        public NotificationMessageCollection Append(IEnumerable<INotificationMessage> messages)
        {
            var messageList = messages as IList<INotificationMessage> ?? messages.ToList();

            foreach (var message in messageList.Reverse())
            {
                this.InsertItem(0, message);
            }

            return this;
        }

        private static IList<INotificationMessage> CreateMessageList(IEnumerable<INotificationMessage> messages)
        {
            return (messages as IList<INotificationMessage>) ?? messages?.ToList() ?? new List<INotificationMessage>();
        }
    }
}
