using Newtonsoft.Json;

namespace Jali.Serve.Server.User
{
    [JsonObject(MemberSerialization.OptIn)]
    public class User
    {
        [JsonProperty("token")]
        public string Token { get; set; }      
    }
}