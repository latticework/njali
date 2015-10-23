using Jali.Serve.Definition;
using Newtonsoft.Json.Schema;
using SchemaReference = Jali.Serve.Definition.SchemaReference;

namespace Jali.Serve.Samples.HelloServices.GreetingData
{
    public class GetGreetingDataRequest
    {
        public const string Name = "get-greetingdata-request";

        public string Lang { get; set; }

        public static RoutineMessage GetDefinition()
        {
            return new RoutineMessage
            {
                Action = Name,
                Description = "The selection critera to retrieve greetingdata.",
                Direction = MessageDirection.Inbound,
                Schema = new SchemaReference
                {
                    SchemaType = SchemaType.Direct,
                    Schema = GetSchema(),
                },
            };
        }

        private static JSchema GetSchema()
        {
            return JSchema.Parse(@"{
  ""$schema"": ""http://json-schema.org/draft-04/schema#"",

  ""type"": ""object"",
  ""properties"": {
    ""lang"": {""type"": ""string""}
  },
  ""required"": [ ]
}");
        }
    }
}