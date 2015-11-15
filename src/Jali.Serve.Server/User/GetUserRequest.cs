using Jali.Serve.Definition;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using SchemaReference = Jali.Serve.Definition.SchemaReference;

namespace Jali.Serve.Server.User
{
    [JsonObject(MemberSerialization.OptIn)]
    public class GetUserRequest
    {
        public const string Action = "get-user-request";

        [JsonProperty("redirectUrl")]
        public string RedirectUrl { get; set; }

        public static RoutineMessage GetDefinition()
        {
            return new RoutineMessage
            {
                Action = Action,
                Direction = MessageDirection.Inbound,
                Description = "Requests an anuthenticated and authorized user object.",
                Schema = new SchemaReference
                {
                    SchemaType = SchemaType.Direct,
                    Schema = JSchema.Parse(@"{
  ""$schema"": ""http://json-schema.org/draft-04/schema#"",

  ""type"": ""object"",
  ""properties"": {
    ""redirectUrl"": {""type"": ""string""}
  }
}"),
                    Options = new SchemaReferenceOptions
                    {
                        Typed = false,
                    },
                },
            };
        }
    }

}