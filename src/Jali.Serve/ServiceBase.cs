using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using Jali.Core;
using Jali.Serve.Definition;

namespace Jali.Serve
{
    public abstract class ServiceBase : IService
    {
        public virtual async Task Init(IExecutionContext context, IServiceContext serviceContext)
        {
            this.Context = serviceContext;

            await InitCore();
        }

        public IServiceContext Context { get; private set; }

        public async Task<IResource> GetResource(IExecutionContext context, string name)
        {
            // TODO: ServiceBase.GetResource: Determine action if CreateRoutine returns null.
            var result = await this._resources.GetOrCreateValueAsync(name, async () => await this.CreateResource(name));

            return result.Value;
        }

        protected virtual async Task InitCore()
        {
            await Task.FromResult(true);
        }

        protected async virtual Task<ResourceBase> CreateResource(string name)
        {
            return await Task.FromResult((ResourceBase) null);
        }

        protected ServiceBase(Service definition)
        {
            this._resources = new Dictionary<string, ResourceBase>();

            this.Definition = definition;
        }

        public Service Definition { get; }

        private readonly IDictionary<string, ResourceBase> _resources;
    }
}