using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jali.Core;
using Jali.Serve.Definition;
using Newtonsoft.Json.Schema;
using SchemaReference = Jali.Serve.Definition.SchemaReference;

namespace Jali.Serve.Server.ServiceDescription
{
    public class ServiceDescriptionResource : ResourceBase
    {
        public const string Name = "servicedescription";

        public ServiceDescriptionResource(ServiceBase service, Resource resource) : base(service, resource)
        {
        }

        public static Resource GetDefinition(Uri jaliserveUrl)
        {
            var serviceDescriptionUrl = new Uri(jaliserveUrl, $"resources/servicedescription/v0.0.1");

            var serviceDescriptionResource = new Resource
            {
                Name = Name,
                Url = serviceDescriptionUrl,
                Version = "0.0.1",
                Description = "Povides documentation about a Jali service.",
                Schema = JSchema.Parse(@"{
  ""$schema"": ""http://json-schema.org/draft-04/schema#"",

  ""type"": ""object"",
  ""properties"": {
    ""html"": {""type"": ""string""}
  }
}"),
                Routines =
                {
                    ["get-servicedescription"] = new Routine
                    {
                        Name = "get-servicedescription",
                        Url = new Uri(serviceDescriptionUrl, "routines/get-servicedescription"),
                        Description = "Get an html representation of the Jali service.",
                        Messages =
                        {
                            ["get-servicedescription-request"] = new RoutineMessage
                            {
                                Action = "get-servicedescription-request",
                                Direction = ActionDirection.To,
                                Description = "The get-servicedescription request message",
                                Schema = new SchemaReference
                                {
                                    SchemaType = SchemaType.Direct,
                                    Schema = JSchema.Parse(@"{
  ""$schema"": ""http://json-schema.org/draft-04/schema#"",

  ""type"": ""object"",
  ""properties"": {
  }
}"),
                                },
                            },
                            ["get-servicedescription-response"] = new RoutineMessage
                            {
                                Action = "get-servicedescription-request",
                                Direction = ActionDirection.To,
                                Description = "The get-servicedescription request message",
                                Schema = new SchemaReference
                                {
                                    SchemaType = SchemaType.Resource,
                                },
                            },
                        },
                    }
                },
                Methods =
                        {
                            ["GET"] = new RestMethod
                            {
                                Method = "GET",
                                Description = "Retrieves the service description.",
                                Routine = "get-servicedescription",
                                Request = new RestMethodRequest
                                {
                                    Message = new RoutineMessageReference
                                    {
                                        Routine = "get-servicedescription",
                                        Action = "get-servicedescription-request"
                                    },
                                    Mode = DataTransmissionModes.Full,
                                },
                                Response = new RestMethodResponse
                                {
                                    Message = new RoutineMessageReference
                                    {
                                        Routine = "get-servicedescription",
                                        Action = "get-servicedescription-response"
                                    },
                                    Mode = DataTransmissionModes.Full,
                                },
                            },
                        },
            };

            return serviceDescriptionResource;
        }

        protected override async Task<RoutineBase> CreateRoutine(string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));


            var routineResult = this.Definition.Routines.GetValueOrDefault(name);


            if (!routineResult.Found)
            {
                throw new InternalErrorException(
                    $"Jail server internal resource '{nameof(ServiceDescriptionResource)}' has been requested to create invalid routine '{name}'.");
            }


            var routine = routineResult.Value;

            var routineFactory = ServiceDescriptionResource._routineFactories[routine.Name];


            if (routineFactory == null)
            {
                throw new InternalErrorException(
                    $"Jail server internal resource '{nameof(ServiceDescriptionResource)}' has not implemented correctly specified requested routine '{name}'.");
            }

            return await Task.FromResult(routineFactory(this, routine));
        }

        static ServiceDescriptionResource()
        {
            ServiceDescriptionResource._routineFactories = 
                new Dictionary<string, Func<ResourceBase, Routine, RoutineBase>>
            {
                [GetServiceDescriptionRoutine.Name] = 
                    (resource, routine) => new GetServiceDescriptionRoutine(resource, routine),
            };
        }

        private static IDictionary<string, Func<ResourceBase, Routine, RoutineBase>> _routineFactories;
    }
}