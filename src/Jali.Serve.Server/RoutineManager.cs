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
    internal sealed class RoutineManager : AsyncInitializedBase
    {
        public RoutineManager(ResourceManager resourceManager, Func<IRoutineContext, Task<IRoutine>> assignNewRoutine)
        {
            if (resourceManager == null) throw new ArgumentNullException(nameof(resourceManager));
            if (assignNewRoutine == null) throw new ArgumentNullException(nameof(assignNewRoutine));


            this.ResourceManager = resourceManager;
            this._assignNewRoutine = assignNewRoutine;

            this.Context = new RoutineContext
            {
                ResourceContext = resourceManager.Context,
                Manager = this,
            };
        }

        public IRoutine Routine { get; private set; }

        public RoutineContext Context { get; }

        public ResourceManager ResourceManager { get; }

        public async Task<HttpResponseMessage> ExecuteProcedure(
            IExecutionContext context,
            HttpRequestParseResult parseResult,
            HttpRequestMessage request,
            string requestAction,
            string responseAction,
            JObject key)
        {
            await EnsureInitialized();

            // TODO: RoutineManager.ExecuteProcedure: Initialize should be called by a Run method instead.
            await this.Routine.Initialize(context);

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
                    if (authenticationRequirement == AuthenticationRequirement.Requested)
                    {
                        var message =
                            $"Support for '{nameof(AuthenticationRequirement.Requested)}' authentication requirement not implemented yet";

                        throw new InternalErrorException(message);
                    }

                    if (authenticationRequirement == AuthenticationRequirement.Required)
                    {
                        return authenticationResult.Response;
                    }
                    else if (authenticationRequirement == AuthenticationRequirement.Permitted)
                    {
                        user = new SecurityContext();
                    }
                    else
                    {
                        var message = $"Invalid AuthenticationRequirement '{authenticationRequirement}'";
                        throw new InternalErrorException(message);
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

            var requestMessage = await serverOptions.MessageConverter.FromRequest(
                context, conversionContext, parseResult, request);
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

        protected override async Task InitializeCore(IExecutionContext context)
        {
            this.Routine = await this._assignNewRoutine(this.Context);
        }

        private readonly Func<IRoutineContext, Task<IRoutine>> _assignNewRoutine;
    }
}
