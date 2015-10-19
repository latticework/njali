using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jali.Core;
using Newtonsoft.Json.Linq;

namespace Jali.Serve.Server
{
    internal sealed class ServiceManager
    {
        public ServiceManager(JaliServer server, IService service)
        {
            if (server == null) throw new ArgumentNullException(nameof(server));
            if (service == null) throw new ArgumentNullException(nameof(service));

            this.Running = false;

            this._resourcesManagers = new Dictionary<string, ResourceManager>();

            this.Context = new ServiceContext
            {
                Manager = this,
                Definition = service.Definition,
            };

            Server = server;
            this.Service = service;
        }

        public bool Running { get; private set; }

        public ServiceContext Context { get; }

        public JaliServer Server { get; }
        public IService Service { get; }

        public async Task Run()
        {
            if (this.Running)
            {
                throw new InvalidOperationException("'ServiceManager' is already running.");
            }

            await this.Service.Init(new ExecutionContext(), this.Context);

            this.Running = true;
        }

        public async Task<IServiceMessage> SendMethod(
            string resourceName, string method, ServiceMessage<JObject> request, string key = null)
        {
            if (resourceName == null) throw new ArgumentNullException(nameof(resourceName));
            if (method == null) throw new ArgumentNullException(nameof(method));
            if (request == null) throw new ArgumentNullException(nameof(request));

            if (!this.Running)
            {
                var message = $"Operation '{nameof(SendMethod)}' called when Jali server is not running.'";
                throw new InvalidOperationException(message);
            }

            var resourceManager = await this.GetResourceManager(resourceName);

            return await resourceManager.SendMethod(method, request, key);
        }

        private async Task<ResourceManager> GetResourceManager(string resourceName)
        {
            if (resourceName == null) throw new ArgumentNullException(nameof(resourceName));

            var result = await this._resourcesManagers.GetOrCreateValueAsync(resourceName, async () =>
            {
                var context = new ExecutionContext();

                var resource = await this.Service.GetResource(context, resourceName);

                var manager = new ResourceManager(this, resource);

                await manager.Run();

                return manager;
            });

            if (!result.Found && result.Value == null)
            {
                var message = $"Invalid Service Manager State: Could not create a '{nameof(ResourceManager)}'";
                throw new InternalErrorException(message);
            }

            return result.Value;
        }

        private readonly IDictionary<string, ResourceManager> _resourcesManagers;
    }
}
