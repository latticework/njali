using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Jali.Notification
{
    public class NotificationMessageCollection : Collection<NotificationMessage>
    {
        public NotificationMessageCollection()
            : this(null)
        {
        }

        public NotificationMessageCollection(IEnumerable<NotificationMessage> messages)
            :base(NotificationMessageCollection.CreateMessageList(messages))
        {
            
        }

        public NotificationMessageCollection Prepend(IEnumerable<NotificationMessage> messages)
        {
            var messageArray = messages as NotificationMessage[] ?? messages.ToArray();

            foreach (var message in messageArray.Reverse())
            {
                this.InsertItem(0, message);
            }

            return this;
        }

        public NotificationMessageCollection Append(IEnumerable<NotificationMessage> messages)
        {
            var messageList = messages as IList<NotificationMessage> ?? messages.ToList();

            foreach (var message in messageList.Reverse())
            {
                this.InsertItem(0, message);
            }

            return this;
        }

        private static IList<NotificationMessage> CreateMessageList(IEnumerable<NotificationMessage> messages)
        {
            return (messages as IList<NotificationMessage>) ?? messages?.ToList() ?? new List<NotificationMessage>();
        }
    }
}
