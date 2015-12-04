using Newtonsoft.Json;

namespace Jali.Serve.Samples.RestbucksServices.ReceiptRoutines
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Receipt
    {
        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("paid")]
        public string Paid { get; set; }
    }
}