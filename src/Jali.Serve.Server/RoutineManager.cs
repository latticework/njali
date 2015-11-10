using System;
using System.Net.Http;
using System.Threading.Tasks;
using Jali.Core;
using Jali.Secure;
using Jali.Serve.Definition;
using Jali.Serve.Server.MessageConversion;
using Newtonsoft.Json.Linq;

namespace Jali.Serve.Server
{
    internal sealed class RoutineManager
    {
        public RoutineManager(ResourceManager resourceManager, IRoutine routine)
        {
            if (resourceManager == null) throw new ArgumentNullException(nameof(resourceManager));
            if (routine == null) throw new ArgumentNullException(nameof(routine));


            this.ResourceManager = resourceManager;
            this.Routine = routine;

            this.Context = new RoutineContext
            {
                ResourceContext = resourceManager.Context,
                Manager = this,
                Definition = routine.Definition,
            };
        }

        public IRoutine Routine { get; }

        public RoutineContext Context { get; }

        public ResourceManager ResourceManager { get; }

        public async Task<HttpResponseMessage> ExecuteProcedure(
            IExecutionContext context, 
            HttpRequestMessage request,
            string requestAction,
            string responseAction,
            JObject key)
        {
            // TODO: RoutineManager.ExecuteProcedure: Should Init be called by a Run method instead?
            await this.Routine.Init(context, this.Context);

            var serverOptions = this.ResourceManager.ServiceManager.Server.Options;

            var authenticationRequirement = this.Routine.GetAuthenticationRequirement(context, requestAction, key);

            ISecurityContext user;
            if (authenticationRequirement == AuthenticationRequirement.Ignored)
            {
                user = new SecurityContext();
            }
            else
            {
                var authenticationResult = await serverOptions.Authenticator.AuthenticateToken(context, request);

                if (authenticationResult.Response != null)
                {
                    // TODO: RoutineManager.ExecuteProcedure: Implement handling authentication request failure.
                    if (authenticationRequirement != AuthenticationRequirement.Requested)
                    {
                        var message =
                            $"Support for '{nameof(AuthenticationRequirement.Requested)}' authentication requirement not implemented yet";

                        throw new InternalErrorException(message);
                    }

                    if (authenticationRequirement != AuthenticationRequirement.Permitted)
                    {
                        return authenticationResult.Response;
                    }
                    else
                    {
                        user = new SecurityContext();
                    }
                }
                else
                {
                    user = authenticationResult.User;
                }

            }

            // TODO: RoutineManager.ExecuteProcedure: Implement Authorization.
            //var result = await serverOptions.Authorizer.Authorize(context, user);

            var conversionContext = new MessageConversionContext(user);

            var requestMessage = await serverOptions.MessageConverter.FromRequest(context, conversionContext, request);
            var userContext = context.MakeContext(user);

            var executeResult = await this.Routine.ExecuteProcedure(
                userContext, request, requestAction, responseAction, requestMessage, key);

            if (executeResult.Response != null)
            {
                return executeResult.Response;
            }

            return await serverOptions.MessageConverter.ToResponse(
                context, conversionContext, executeResult.Message, request);

            // TODO: JaliServer.Send: Remove these class files for 'AsResponse'.
            //return result.AsResponse(request);
        }
    }
}
