using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Jali.Core;

namespace Jali.Serve.Server
{
    internal sealed class ResourceManager
    {
        public ResourceManager(ServiceManager serviceManager, IResource resource)
        {
            if (serviceManager == null) throw new ArgumentNullException(nameof(serviceManager));
            if (resource == null) throw new ArgumentNullException(nameof(resource));


            this._routineManagers = new Dictionary<string, RoutineManager>();


            this.ServiceManager = serviceManager;
            this.Resource = resource;

            this.Context = new ResourceContext
            {
                ServiceContext = this.ServiceManager.Context,
                Manager = this,
                Definition = resource.Definition,
            };
        }

        public ResourceContext Context { get; }

        public ServiceManager ServiceManager { get; }

        public IResource Resource { get; }

        public async Task Run(IExecutionContext context)
        {
            if (this.Running)
            {
                var message =
                    $"'{nameof(ResourceManager)}' for Resource '{this.Resource.Definition.Name}' of Service '{this.ServiceManager.Service.Definition.Name}' is already running.";

                throw new InvalidOperationException(message);
            }

            await this.Resource.Init(context, this.Context);

            this.Running = true;
        }

        public bool Running { get; set; }

        public async Task<HttpResponseMessage> Send(
            IExecutionContext context, HttpRequestParseResult parseResult, HttpRequestMessage request)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (parseResult == null) throw new ArgumentNullException(nameof(parseResult));
            if (request == null) throw new ArgumentNullException(nameof(request));

            var methodResult = this.Resource.Definition.Methods.GetValueOrDefault(parseResult.Method);

            if (!methodResult.Found)
            {
                var message = 
                    $"Method '{parseResult.Method}' for Resource '{this.Resource.Definition.Name}' of Service '{this.ServiceManager.Service.Definition.Name}' was not found.";

                throw new ArgumentException(message, nameof(parseResult));
            }

            if (!this.Running)
            {
                var message =
                    $"Can only call '{nameof(Send)}' when '{nameof(ResourceManager)}' is '{nameof(Running)}'.";

                throw new InvalidOperationException(message);
            }

            var resourceKey = (parseResult.ResourceKey == null)
                ? null
                : this.ServiceManager.Server.Options.KeyConverter.ToResourceKey(
                    this.Resource.Definition.KeySchema, parseResult.ResourceKey);

            // TODO: ResourceManager.Send: Implement direct routine invocation.
            if (parseResult.RoutineName != null)
            {
                throw new NotImplementedException("Jali Server has not yet implemented direct route message support.");
            }

            var routineManager = await this.GetRoutineManager(context, methodResult.Value.Routine);

            var requestAction = (parseResult.ResourceKey == null)
                ? methodResult.Value.Request.Message.Action
                : methodResult.Value.KeyRequest.Message.Action;

            var responseAction = methodResult.Value.Response.Message.Action;

            var result = await routineManager.ExecuteProcedure(
                context, parseResult, request, requestAction, responseAction, resourceKey);

            return result;
        }

        private readonly IDictionary<string, RoutineManager> _routineManagers;

        private async Task<RoutineManager> GetRoutineManager(IExecutionContext context, string routineName)
        {
            if (routineName == null) throw new ArgumentNullException(nameof(routineName));

            var result = await this._routineManagers.GetOrCreateValueAsync(routineName, async () =>
            {
                var routine = await this.Resource.GetRoutine(context, routineName);

                var manager = new RoutineManager(this, routine);

                // TODO: ResourceManager.GetRoutineManager: Should RoutineManager define Run()?
                // await manager.Run();

                return await Task.FromResult(manager);
            });

            if (!result.Found && result.Value == null)
            {
                var message = $"Invalid Service Manager State: Could not create a '{nameof(RoutineManager)}'";
                throw new InternalErrorException(message);
            }

            return result.Value;
        }
    }
}
