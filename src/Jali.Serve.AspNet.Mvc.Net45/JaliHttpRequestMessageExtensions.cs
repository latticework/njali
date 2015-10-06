using Jali.Serve;

namespace System.Net.Http
{
    public static class JaliHttpRequestMessageExtensions
    {
        public static ServiceMessage AsServiceMessage(this HttpRequestMessage request)
        {
            return new ServiceMessage
            {
                Connection = new MessageConnection
                {
                    Receiver = new EndpointPartition
                    {
                        DataCenterId = null,
                        ServiceCenterId = null,
                    },
                    Sender = new EndpointPartition
                    {
                        DataCenterId = null,
                        ServiceCenterId = null,
                    }
                },
                Contract = new MessageContract
                {
                    Url = null,
                    ConsumerId = null,
                    Version = null,
                },
                Data = null,
                Identity = new MessageIdentity
                {
                    ConversationId = null,
                    DeputyId = null,
                    ImpersonatorId = null,
                    MessageId = null,
                    MessageTransmissionId = null,
                    SessionId = null,
                    TransactionId = null,
                    UserId = null,
                },
                Tenant = new TenantIdentity
                {
                    TenantId = null,
                    TenantOrgId = null,
                },
            };
        }
    }
}
