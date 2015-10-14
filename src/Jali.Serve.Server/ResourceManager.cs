using System;
using System.Collections.Generic;
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

        public async Task Run()
        {
            if (this.Running)
            {
                var message =
                    $"'{nameof(ResourceManager)}' for Resource '{this.Resource.Definition.Name}' of Service '{this.ServiceManager.Service.Definition.Name}' is already running.";

                throw new InvalidOperationException(message);
            }

            await this.Resource.Init(new ExecutionContext(), this.Context);

            this.Running = true;
        }

        public bool Running { get; set; }

        public async Task<IServiceMessage> SendMethod(string method, IServiceMessage request)
        {
            if (method == null) throw new ArgumentNullException(nameof(method));
            if (request == null) throw new ArgumentNullException(nameof(request));

            var methodResult = this.Resource.Definition.Methods.GetValueOrDefault(method);

            if (!methodResult.Succeeded)
            {
                var message = $"Method '{method}' for Resource '{this.Resource.Definition.Name}' of Service '{this.ServiceManager.Service.Definition.Name}' was not found.";
                throw new ArgumentException(message, nameof(method));
            }

            if (!this.Running)
            {
                var message =
                    $"Can only call '{nameof(SendMethod)}' when '{nameof(ResourceManager)}' is '{nameof(Running)}'.";

                throw new InvalidOperationException(message);
            }

            var routineManager = await this.GetRoutineManager(methodResult.Value.Routine);

            var requestAction = methodResult.Value.Request.Message.Action;
            var responseAction = methodResult.Value.Response.Message.Action;

            var result = await routineManager.ExecuteProcedure(requestAction, responseAction, request);

            return result;
        }

        private readonly IDictionary<string, RoutineManager> _routineManagers;

        private async Task<RoutineManager> GetRoutineManager(string routineName)
        {
            if (routineName == null) throw new ArgumentNullException(nameof(routineName));

            var result = await this._routineManagers.GetOrCreateValueAsync(routineName, async () =>
            {
                var context = new ExecutionContext();

                var routine = await this.Resource.GetRoutine(context, routineName);

                var manager = new RoutineManager(this, routine);

                // TODO: ResourceManager.GetRoutineManager: Should RoutineManager define Run()?
                // await manager.Run();

                return await Task.FromResult(manager);
            });

            if (!result.Succeeded)
            {
                var message = $"Invalid Service Manager State: Could not create a '{nameof(RoutineManager)}'";
                throw new InternalErrorException(message);
            }

            return result.Value;
        }
    }
}
