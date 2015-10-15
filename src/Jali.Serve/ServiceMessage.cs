using System;
using System.Collections.Generic;
using Jali.Notification;
using Newtonsoft.Json.Linq;

namespace Jali.Serve
{
    public static class ServiceMessageExtensions
    {
        public static ServiceMessage<TData> ToTypedMessages<TData>(this ServiceMessage<JObject> receiver)
        {
            var data = receiver.Data.ToObject<TData>();

            var message = new ServiceMessage<TData>
            {
                Contract = receiver.Contract,
                Data = data,
                Connection = receiver.Connection,
                Identity = receiver.Identity,
                Tenant = receiver.Tenant
            };

            message.Messages.Append(receiver.Messages);

            return message;
        } 
    }

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

        IEnumerable<NotificationMessage> IServiceMessage.Messages => this.Messages;

        public ServiceMessage<TResponseData> CreateFromMessage<TResponseData>(
            TResponseData data, IEnumerable<NotificationMessage> messages)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            // TODO: ServiceMessage.CreateFromMessage: Rename.
            // TODO: ServiceMessage.CreateFromMessage: Create copies of everything.
            var response = new ServiceMessage<TResponseData>
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