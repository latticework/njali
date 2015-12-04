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

        public HelloResource(ServiceBase service, Resource definition, IResourceContext resourceContext) 
            : base(service, definition, resourceContext)
        {
        }

        public static Resource GetDefinition(Uri helloServiceUrl)
        {
            var helloResourceUrl = helloServiceUrl.Combine("resources/hello/v1.0.0");

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
    ""lang"": {""type"": ""string"", ""description"": ""Language to say hello in.""},
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
                        Url = helloResourceUrl.Combine("routines/get-hello"),
                        Description = "Retrieves the hello message.",
                        Messages =
                        {
                            ["get-hello-request"] = new RoutineMessage
                            {
                                Action = "get-hello-request",
                                Direction = MessageDirection.Inbound,
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
                                Direction = MessageDirection.Outbound,
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

        protected override async Task<RoutineBase> CreateRoutine(string name, IRoutineContext routineContext)
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

            return await Task.FromResult(routineFactory(this, routine, routineContext));
        }

        static HelloResource()
        {
            HelloResource._routineFactories = new Dictionary<string, Func<ResourceBase, Routine, IRoutineContext, RoutineBase>>
            {
                [GetHelloRoutine.Name] = (s, r, c) => new GetHelloRoutine(s, r, c),
            };
        }

        private static IDictionary<string, Func<ResourceBase, Routine, IRoutineContext, RoutineBase>> _routineFactories;
    }
}