using Newtonsoft.Json;

namespace Jali.Serve.Samples.RestbucksServices.OrderRoutines
{
    [JsonObject(MemberSerialization.OptIn)]
    public class OrderItem
    {
        [JsonProperty("milk")]
        public string Milk { get; set; }

        [JsonProperty("size")]
        public string Size { get; set; }

        [JsonProperty("drink")]
        public string Drink { get; set; }
    }
}