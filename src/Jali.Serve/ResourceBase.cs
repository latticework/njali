using System.Collections.Generic;
using System.Threading.Tasks;
using Jali.Core;
using Jali.Serve.Definition;

namespace Jali.Serve
{
    public abstract class ResourceBase : IResource
    {
        public virtual async Task Init(IExecutionContext context, IResourceContext resourceContext)
        {
            this.Context = resourceContext;

            await InitCore();
        }

        public IResourceContext Context { get; private set; }

        public async Task<IRoutine> GetRoutine(IExecutionContext context, string name)
        {
            // TODO: ServiceBase.GetResource: Determine action if CreateRoutine returns null.
            var result = await this._routines.GetOrCreateValueAsync(name, async () => await this.CreateRoutine(name));

            return result.Value;
        }

        public Resource Definition { get; }
        public ServiceBase Service { get; }

        protected virtual async Task InitCore()
        {
            await Task.FromResult(true);
        }

        protected async virtual Task<RoutineBase> CreateRoutine(string name)
        {
            return await Task.FromResult((RoutineBase)null);
        }

        protected ResourceBase(ServiceBase service, Resource resource)
        {
            this._routines = new Dictionary<string, RoutineBase>();

            this.Definition = resource;
            this.Service = service;
        }

        private readonly IDictionary<string, RoutineBase> _routines;
    }

}