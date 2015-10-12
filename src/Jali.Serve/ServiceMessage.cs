using System;
using System.Collections.Generic;
using Jali.Notification;

namespace Jali.Serve
{
    public class ServiceMessage<TData> : IServiceMessage<TData>
    {
        public ServiceMessage()
        {
            this.Messages = new NotificationMessageCollection();
        }

        public MessageContract Contract { get; set; }

        public TData Data { get; set; }
        public NotificationMessageCollection Messages { get; }
        public MessageIdentity Identity { get; set; }
        public MessageConnection Connection { get; set; }
        public TenantIdentity Tenant { get; set; }

        IEnumerable<NotificationMessage> IServiceMessage.Messages { get; }

        public ServiceMessage<TData> CreateResponseMessage(TData data, IEnumerable<NotificationMessage> messages)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            var response = new ServiceMessage<TData>
            {
                Connection = this.Connection.CreateResponseConnection(),
                Data = data,
                Contract = this.Contract,
                Identity = this.Identity.CreateResponseIdentity(),
                Tenant = this.Tenant,
            };

            if (messages != null)
            {
                response.Messages.Append(messages);
            }

            return response;
        }

        object IServiceMessage.Data
        {
            get { return Data; }
            set { Data = (TData)value; }
        }
    }
}