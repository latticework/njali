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
                Sender = new EndpointPartition
                {
                    ServiceCenterId = this.Receiver.ServiceCenterId,
                    DataCenterId = this.Receiver.DataCenterId,
                },
                Receiver = new EndpointPartition
                {
                    ServiceCenterId = this.Sender.ServiceCenterId,
                    DataCenterId = this.Sender.DataCenterId,
                },
            };
        }
    }
}
