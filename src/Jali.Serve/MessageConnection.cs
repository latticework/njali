namespace Jali.Serve
{
    public class MessageConnection
    {
        public EndpointPartition Sender { get; set; }
        public EndpointPartition Receiver { get; set; }

        public MessageConnection CreateResponseConnection()
        {
            return new MessageConnection
            {
                Sender = this.Receiver,
                Receiver = this.Sender,
            };
        }
    }
}
