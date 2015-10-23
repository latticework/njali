using System;
using System.Collections.Generic;
using Jali.Note;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Jali.Serve
{    /// <summary>
     ///     Represents a Jali service message.
     /// </summary>
    [JsonConverter(typeof(ServiceMesssageConverter))]
    public sealed class ServiceMessage<TData> : IServiceMessage<TData>
    {
        /// <summary>
        ///     Initializes a new instance of <see cref="ServiceMessage{TData}"/>.
        /// </summary>
        public ServiceMessage()
        {
            this.Messages = new NotificationMessageCollection();
        }

        /// <summary>
        ///     The client contract used to define this message.
        /// </summary>
        public MessageContract Contract { get; set; }

        /// <summary>
        ///     The sender's credentials.
        /// </summary>
        public MessageCredentials Credentials { get; set; }

        /// <summary>
        ///     The JSON-serializable message data. <see langword="null"/> if <see cref="Messages"/> has errors.
        /// </summary>
        public TData Data { get; set; }

        /// <summary>
        ///     A list of <see cref="NotificationMessage"/> objects accompanying the service message.
        /// </summary>
        public NotificationMessageCollection Messages { get; }

        /// <summary>
        ///     A collenction of unique identifiers representing this message.
        /// </summary>
        public MessageIdentity Identity { get; set; }

        /// <summary>
        ///     The data center partitions negociated between the communicating parties.
        /// </summary>
        public MessageConnection Connection { get; set; }

        /// <summary>
        ///     The service tenant on whose behalf the message is being sent.
        /// </summary>
        public TenantIdentity Tenant { get; set; }

        IEnumerable<NotificationMessage> IServiceMessage.Messages
        {
            get { return this.Messages; }
            set
            {
                this.Messages.Clear();
                this.Messages.Append(value);
            }
        }

        public ServiceMessage<TData> Clone()
        {
            var clone = new ServiceMessage<TData>
            {
                Contract = new MessageContract
                {
                    Url = this.Contract?.Url,
                    ConsumerId = this.Contract?.ConsumerId,
                    Version = this.Contract?.Version,
                },
                Credentials = new MessageCredentials
                {
                    UserId = this.Credentials?.UserId,
                    ImpersonatorId = this.Credentials?.ImpersonatorId,
                    DeputyId = this.Credentials?.DeputyId,
                },
                Data = this.Data,
                Identity = new MessageIdentity
                {
                    TransactionId = this.Identity?.TransactionId,
                    SessionId = this.Identity?.SessionId,
                    ConversationId = this.Identity?.ConversationId,
                    MessageId = this.Identity?.MessageId,
                    MessageTransmissionId = this.Identity?.MessageTransmissionId,
                },
                Connection = new MessageConnection
                {
                    Sender = new EndpointPartition
                    {
                        ServiceCenterId = this.Connection?.Sender?.ServiceCenterId,
                        DataCenterId = this.Connection?.Sender?.DataCenterId,
                    },
                    Receiver = new EndpointPartition
                    {
                        ServiceCenterId = this.Connection?.Receiver?.ServiceCenterId,
                        DataCenterId = this.Connection?.Receiver?.DataCenterId,
                    },
                },
                Tenant = new TenantIdentity
                {
                    TenantId = this.Tenant?.TenantId,
                    TenantOrgId = this.Tenant?.TenantOrgId,
                },
            };

            ((IServiceMessage) clone).Messages = this.Messages;

            return clone;
        }

        public ServiceMessage<TResponseData> CreateOutboundMessage<TResponseData>(
            MessageCredentials credentials, TResponseData data, IEnumerable<NotificationMessage> messages)
        {
            if (credentials == null) throw new ArgumentNullException(nameof(credentials));
            if (data == null) throw new ArgumentNullException(nameof(data));

            // TODO: ServiceMessage.CreateOutboundMessage: Create copies of everything.
            var response = new ServiceMessage<TResponseData>
            {
                Connection = this.Connection.CreateOutboundConnection(),
                Credentials = credentials,
                Data = data,
                Contract = this.Contract,
                Identity = this.Identity.CreateOutboundIdentity(),
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

        IServiceMessage IServiceMessage.Clone()
        {
            return Clone();
        }
    }
}