namespace Jali.Serve
{
    public class MessageIdentity
    {
        public string TransactionId { get; set; }
        public string SessionId { get; set; }
        public string ConversationId { get; set; }
        public string MessageId { get; set; }
        public string MessageTransmissionId { get; set; }
        public string UserId { get; set; }
        public string ImpersonatorId { get; set; }
        public string DeputyId { get; set; }
    }
}