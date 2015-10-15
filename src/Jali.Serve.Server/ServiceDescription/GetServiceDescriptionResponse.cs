using Newtonsoft.Json;

namespace Jali.Serve.Server.ServiceDescription
{
    public class GetServiceDescriptionResponse
    {
        [JsonProperty("html")]
        public string Html { get; set; }
    }
}