using System;
using Jali.Serve.Definition;
using Newtonsoft.Json.Schema;
using SchemaReference = Jali.Serve.Definition.SchemaReference;

namespace Jali.Serve.Samples.HelloServices
{
    public class HelloResource : ResourceBase
    {
        public const string Name = "hello";

        public HelloResource(ServiceBase service, Resource resource) : base(service, resource)
        {
        }

        public static Resource GetDefinition(Uri helloServiceUrl)
        {
            var helloResourceUrl = new Uri(helloServiceUrl, "resources/hello/v0.0.1");


            var helloResource = new Resource
            {
                Name = Name,
                Url = helloResourceUrl,
                Version = "1.0.0",
                Description = "The Hello sample resource.",
                Schema = JSchema.Parse(@"
  ""$schema"": ""http://json-schema.org/draft-04/schema#"",

  ""type"": ""object"",
  ""properties"": {
    ""message"": {""type"": ""string""}
  },
  ""required"": [ ""message"" ]
"),
                Routines =
                {
                    ["get-hello"] = new Routine
                    {
                        Name = "get-hello",
                        Url = new Uri(helloResourceUrl, "routines/get-hello"),
                        Description = "Retrieves the hello message.",
                        Messages =
                        {
                            ["get-hello-request"] = new RoutineMessage
                            {
                                Action = "get-hello-request",
                                Direction = ActionDirection.To,
                                Description = "Criteria for the get-hello routine",
                                Schema = new SchemaReference
                                {
                                    SchemaType = SchemaType.Direct,
                                    Schema = JSchema.Parse(@"
""$schema"": ""http://json-schema.org/draft-04/schema#"",

""type"": ""object"",
""properties"": {
""name"": {""type"": ""string"", ""description"": ""Name of requester to say hello to.""}
},
"),
                                }
                            }
                        }
                    }
                }
            };

            return helloResource;
        }
    }
}