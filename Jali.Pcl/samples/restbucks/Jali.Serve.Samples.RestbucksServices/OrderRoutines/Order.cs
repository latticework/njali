using System.Collections.Generic;
using Newtonsoft.Json;

namespace Jali.Serve.Samples.RestbucksServices.OrderRoutines
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Order
    {
        [JsonProperty("items")]
        public IEnumerable<OrderItem> Items { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("cost")]
        public decimal Cost { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }
}