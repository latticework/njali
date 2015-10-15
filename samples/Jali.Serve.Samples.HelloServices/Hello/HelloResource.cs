using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jali.Core;
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

            var resourceSchema = JSchema.Parse(@"{
  ""$schema"": ""http://json-schema.org/draft-04/schema#"",

  ""type"": ""object"",
  ""properties"": {
    ""message"": {""type"": ""string""}
  },
  ""required"": [ ""message"" ]
}");
            var getRoutineRequestMessageSchema = JSchema.Parse(@"{
  ""$schema"": ""http://json-schema.org/draft-04/schema#"",

  ""type"": ""object"",
  ""properties"": {
    ""name"": {""type"": ""string"", ""description"": ""Name of requester to say hello to.""}
  }
}");

            var helloResource = new Resource
            {
                Name = Name,
                Url = helloResourceUrl,
                Version = "1.0.0",
                Description = "The Hello sample resource.",
                Schema = resourceSchema,
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
                                    Schema = getRoutineRequestMessageSchema,
                                }
                            },
                            ["get-hello-result"] = new RoutineMessage
                            {
                                Action = "get-hello-result",
                                Direction = ActionDirection.From,
                                Description = "The hello message.",
                                Schema = new SchemaReference
                                {
                                    SchemaType = SchemaType.Resource,
                                }
                            },
                        }
                    }
                },
                Methods =
                {
                    ["GET"] = new RestMethod
                    {
                        Method = "GET",
                        Description = "Gets the hello message.",
                        Routine = "get-hello",
                        Request = new RestMethodRequest
                        {
                            Message = new RoutineMessageReference
                            {
                                Routine = "get-hello",
                                Action = "get-hello-request",
                            },
                            Mode = DataTransmissionModes.Full,
                        },
                        Response = new RestMethodResponse
                        {
                            Message = new RoutineMessageReference
                            {
                                Routine = "get-hello",
                                Action = "get-hello-result",
                            },
                            Mode = DataTransmissionModes.Full,
                        }
                    }
                }
            };

            return helloResource;
        }

        protected override async Task<RoutineBase> CreateRoutine(string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));


            var routineResult = this.Definition.Routines.GetValueOrDefault(name);


            if (!routineResult.Found)
            {
                throw new InternalErrorException(
                    $"The hello routine of the hello service has been requested to create invalid routine '{name}'.");
            }


            var routine = routineResult.Value;

            var routineFactory = HelloResource._routineFactories[routine.Name];


            if (routineFactory == null)
            {
                throw new InternalErrorException(
                    $"Hello service has not implemented correctly specified requested routine '{name}'.");
            }

            return await Task.FromResult(routineFactory(this, routine));
        }

        static HelloResource()
        {
            HelloResource._routineFactories = new Dictionary<string, Func<ResourceBase, Routine, RoutineBase>>
            {
                [GetHelloRoutine.Name] = (s, r) => new GetHelloRoutine(s, r),
            };
        }

        private static IDictionary<string, Func<ResourceBase, Routine, RoutineBase>> _routineFactories;
    }
}