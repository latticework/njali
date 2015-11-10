using System;
using System.Threading.Tasks;
using Jali.Serve.Definition;
using Newtonsoft.Json.Schema;

namespace Jali.Serve.Server.User
{
    public class UserResource : ServerResourceBase
    {
        public const string Name = "user";

        public UserResource(ServiceBase service, Resource resource, JaliServerOptions serverOptions) 
            : base(service, resource, serverOptions)
        {
        }

        protected override async Task<RoutineBase> CreateRoutine(string name)
        {
            await Task.FromResult(true);

            switch (name)
            {
                case GetUserRoutine.Name:
                    return new GetUserRoutine(this, this.ServerOptions);
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