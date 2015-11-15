using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jali.Serve.Definition;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using SchemaReference = Jali.Serve.Definition.SchemaReference;

namespace Jali.Serve.Server.User
{
    public class GetUserRoutine : ServerRoutineBase<JObject, IEnumerable<User>>
    {
        public const string Name = "user";

        public GetUserRoutine(ResourceBase resource, JaliServerOptions options) 
            : base(resource, GetDefinition(resource.Definition.Url), options)
        {
        }

        protected override async Task ExecuteProcedureCore(
            IExecutionContext context, 
            RoutineProcedureContext<JObject, IEnumerable<User>, JObject> procedureContext)
        {
            var authenticationResult = await this.ServerOptions.Authenticator.AuthenticateUser(
                context, procedureContext.HttpRequest, procedureContext.Request);

            // TODO: GetUserRoutine.ExecuteProcedureCore: Make sure redirects include return address in JSON request data.
            if (authenticationResult.Response != null)
            {
                procedureContext.HttpResponse = authenticationResult.Response;
                return;
            }

            // TODO: GetUserRoutine.ExecuteProcedureCore: Implement Authorization.
            //var result = await this.ServerOptions.Authorizer.Authorize(context, authenticationResult.User);

            // TODO: GetUserRoutine.ExecuteProcedureCore: Determine method to expose client claims.
            var token = await this.ServerOptions.Authenticator.EncodeUser(authenticationResult.User);

            var user = new User
            {
                Token = token
            };

            // TODO: GetUserRoutine.ExecuteProcedureCore: Determine outbound credentials.
            var response = procedureContext.Request.CreateOutboundMessage(
                new MessageCredentials(), (IEnumerable<User>)new [] { user } , null);

            procedureContext.Response = response;
        }

        public static Routine GetDefinition(Uri resourceUrl)
        {
            var url = new Uri(resourceUrl, $"routines/{Name}");
            return new Routine
            {
                Name = Name,
                Url = url,
                Description = "Gets the specified Jali User.",
                DefaultAuthentication = AuthenticationRequirement.Ignored,
                Messages =
                {
                    [GetUserRequest.Action] = GetUserRequest.GetDefinition(),

                    // TODO: GetUserRoutine: Support 'get-user-bykey'.

                    ["get-user-response"] = new RoutineMessage
                    {
                        Action = "get-user-response",
                        Description = "Returns the specified user.",
                        Direction = MessageDirection.Outbound,
                        Schema = new SchemaReference
                        {
                            SchemaType = SchemaType.Resource,
                        }
                    }
                }
            };

        }
    }
}