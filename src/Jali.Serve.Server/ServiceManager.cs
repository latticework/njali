using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Jali.Core;

namespace Jali.Serve.Server
{
    internal sealed class ServiceManager : AsyncInitializedBase
    {
        public ServiceManager(JaliServer server, Func<IServiceContext, Task<IService>> assignNewService)
        {
            if (server == null) throw new ArgumentNullException(nameof(server));
            if (assignNewService == null) throw new ArgumentNullException(nameof(assignNewService));

            this.Running = false;

            this.Server = server;
            this._resourcesManagers = new Dictionary<string, ResourceManager>();

            _assignNewService = assignNewService;

            this.Context = new ServiceContext
            {
                Manager = this,
            };
        }

        public bool Running { get; private set; }

        public ServiceContext Context { get; }

        public JaliServer Server { get; }
        public IService Service { get; private set; }

        public async Task Run(IExecutionContext context)
        {
            await EnsureInitialized();

            if (this.Running)
            {
                throw new InvalidOperationException("'ServiceManager' is already running.");
            }

            await this.Service.Initialize(context);

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
                var manager = new ResourceManager(this, async ctx => 
                    await this.Service.GetResource(context, resourceName, ctx));

                await manager.Initialize(context);
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
        protected override async Task InitializeCore(IExecutionContext context)
        {
            this.Service = await this._assignNewService(this.Context);
        }

        private readonly IDictionary<string, ResourceManager> _resourcesManagers;

        private readonly Func<IServiceContext, Task<IService>> _assignNewService;
    }
}
