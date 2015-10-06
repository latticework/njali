using System.Collections.Generic;
using Jali.Notification;

namespace Jali.Serve
{
    public class ServiceMessage : IServiceMessage
    {
        public ServiceMessage()
        {
            this.Messages = new NotificationMessageCollection();
        }

        public MessageContract Contract { get; set; }
        public object Data { get; set; }
        public NotificationMessageCollection Messages { get; }
        public MessageIdentity Identity { get; set; }
        public MessageConnection Connection { get; set; }
        public TenantIdentity Tenant { get; set; }

        IEnumerable<NotificationMessage> IServiceMessage.Messages { get; }
    }
}