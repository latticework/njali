using Jali.Serve;
using Newtonsoft.Json.Linq;

namespace System.Net.Http
{
    public static class JaliHttpRequestMessageExtensions
    {
        // TODO: JaliHttpRequestMessageExtensions.AsServiceMessage: Implement.
        public static ServiceMessage<JObject> AsServiceMessage(this HttpRequestMessage request)
        {
            return new ServiceMessage<JObject>
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
