using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Jali.Core;

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

        public async Task Run(IExecutionContext context)
        {
            if (this.Running)
            {
                throw new InvalidOperationException("'ServiceManager' is already running.");
            }

            await this.Service.Init(context, this.Context);

            this.Running = true;
        }

        public async Task<HttpResponseMessage> SendMethod(
            IExecutionContext context, HttpRequestParseResult parseResult, HttpRequestMessage request)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (parseResult == null) throw new ArgumentNullException(nameof(parseResult));
            if (request == null) throw new ArgumentNullException(nameof(request));

            if (!this.Running)
            {
                var message = $"Operation '{nameof(SendMethod)}' called when Jali server is not running.'";
                throw new InvalidOperationException(message);
            }

            var resourceManager = await this.GetResourceManager(context, parseResult.ResourceName);

            return await resourceManager.Send(context, parseResult, request);
        }

        private async Task<ResourceManager> GetResourceManager(IExecutionContext context, string resourceName)
        {
            if (resourceName == null) throw new ArgumentNullException(nameof(resourceName));

            var result = await this._resourcesManagers.GetOrCreateValueAsync(resourceName, async () =>
            {
                var resource = await this.Service.GetResource(context, resourceName);

                var manager = new ResourceManager(this, resource);

                await manager.Run(context);

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
