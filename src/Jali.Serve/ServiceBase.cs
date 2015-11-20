using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Jali.Core;
using Jali.Serve.Definition;

namespace Jali.Serve
{
    public abstract class ServiceBase : AsyncInitializedBase, IService
    {

        public IServiceContext Context { get; private set; }

        public Service Definition { get; }

        public async Task<IResource> GetResource(IExecutionContext context, string name, IResourceContext resourceContext)
        {
            // TODO: ServiceBase.GetResource: Determine action if CreateRoutine returns null.
            var result = await this._resources.GetOrCreateValueAsync(name, async () => await this.CreateResource(name, resourceContext));

            return result.Value;
        }

        protected ServiceBase(Service definition, IServiceContext serviceContext)
        {
            this._resources = new Dictionary<string, ResourceBase>();

            this.Definition = definition;
            this.Context = serviceContext;
        }

        protected async virtual Task<ResourceBase> CreateResource(string name, IResourceContext resourceContext)
        {
            return await Task.FromResult((ResourceBase) null);
        }

        protected override string GetAsyncInitializedInstanceName()
        {
            return $"Jali Service '{this.Definition.Name}'";
        }

        protected override async Task InitializeCore(IExecutionContext context)
        {
            await Task.FromResult(true);
        }

        private readonly IDictionary<string, ResourceBase> _resources;
    }
}