using System;
using System.Threading.Tasks;
using Jali.Serve.Definition;
using Newtonsoft.Json.Schema;

namespace Jali.Serve.Server.User
{
    public class UserResource : ServerResourceBase
    {
        public const string Name = "user";

        public UserResource(ServiceBase service, IResourceContext resourceContext, JaliServerOptions serverOptions) 
            : base(service, GetDefinition(service.Definition.Url), resourceContext, serverOptions)
        {
        }

        protected override async Task<RoutineBase> CreateRoutine(string name, IRoutineContext routineContext)
        {
            await Task.FromResult(true);

            switch (name)
            {
                case GetUserRoutine.Name:
                    return new GetUserRoutine(this, routineContext, this.ServerOptions);
            }

            // TODO: UserResource.CreateRoutine: Replace with Domain Error Exception.
            var message = $"Resource '{Name}' cannot create undefined routine '{name}'.";
            throw new InvalidOperationException(message);
        }

        public static Resource GetDefinition(Uri serviceUrl)
        {
            var url = new Uri(serviceUrl, $"resources/{Name}");
            return new Resource
            {
                Name = Name,
                Url = url,
                Version = "0.0.1",
                Description = "Gets Jali User data relavant for this service",
                DefaultAuthentication = AuthenticationRequirement.Ignored,
                Schema = GetSchema(),
                Routines =
                {
                    [GetUserRoutine.Name] = GetUserRoutine.GetDefinition(url),
                },
                Methods =
                {
                    [RestMethodVerbs.Get] = new RestMethod
                    {
                        Method = RestMethodVerbs.Get,
                        Description = "Gets the user after the authenticator provides the security context.",
                        Routine = GetUserRoutine.Name,
                        Request = new RestMethodRequest
                        {
                            Message = new RoutineMessageReference
                            {
                                Routine = GetUserRoutine.Name,
                                Action = GetUserRequest.Action,
                            },
                            Mode = DataTransmissionModes.Full,
                        },
                        Response = new RestMethodResponse
                        {
                            Message = new RoutineMessageReference
                            {
                                Routine = GetUserRoutine.Name,
                                Action = GetUserRequest.Action,
                            },
                            Mode = DataTransmissionModes.Full,
                        },
                    }
                }
            };
        }

        public static JSchema GetSchema()
        {
            return JSchema.Parse(@"{
  ""$schema"": ""http://json-schema.org/draft-04/schema#"",

  ""type"": ""object"",
  ""properties"": {
    ""token"": {""type"": ""string""}
  }
}");
        }

    }
}