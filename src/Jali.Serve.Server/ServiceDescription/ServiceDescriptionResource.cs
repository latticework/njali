using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jali.Core;
using Jali.Serve.Definition;
using Newtonsoft.Json.Schema;

namespace Jali.Serve.Server.ServiceDescription
{
    public class ServiceDescriptionResource : ServerResourceBase
    {
        public const string Name = "servicedescription";

        public ServiceDescriptionResource(ServiceBase service, JaliServerOptions serverOptions) 
            : base(service, GetDefinition(service.Definition.Url), serverOptions)
        {
        }

        public static Resource GetDefinition(Uri serviceUrl)
        {
            var url = new Uri(serviceUrl, $"resources/{Name}");

            return new Resource
            {
                Name = Name,
                Url = url,
                Version = "0.0.1",
                Description = "Povides documentation about a Jali service.",
                DefaultAuthentication = AuthenticationRequirement.Ignored,
                Schema = JSchema.Parse(@"{
  ""$schema"": ""http://json-schema.org/draft-04/schema#"",

  ""type"": ""object"",
  ""properties"": {
    ""html"": {""type"": ""string""}
  }
}"),
                Routines =
                {
                    [GetServiceDescriptionRoutine.Name] =  GetServiceDescriptionRoutine.GetDescription(url),

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
                                        Routine = GetServiceDescriptionRoutine.Name,
                                        Action = "get-servicedescription-request",
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

            return await Task.FromResult(routineFactory(this, this.ServerOptions));
        }

        static ServiceDescriptionResource()
        {
            ServiceDescriptionResource._routineFactories = 
                new Dictionary<string, Func<ResourceBase, JaliServerOptions, RoutineBase>>
            {
                [GetServiceDescriptionRoutine.Name] = 
                    (resource, serverOptions) => new GetServiceDescriptionRoutine(resource, serverOptions),
            };
        }

        private static IDictionary<string, Func<ResourceBase, JaliServerOptions, RoutineBase>> _routineFactories;
    }
}