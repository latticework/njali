using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jali.Core;
using Jali.Serve.Definition;

namespace Jali.Serve
{
    public abstract class ResourceBase : AsyncInitializedBase, IResource
    {
        public IResourceContext Context { get; private set; }

        public async Task<IRoutine> GetRoutine(IExecutionContext context, string name, IRoutineContext routineContext)
        {
            // TODO: ServiceBase.GetResource: Determine action if CreateRoutine returns null.
            var result = await this._routines.GetOrCreateValueAsync(name, async () => 
                await this.CreateRoutine(name, routineContext));

            return result.Value;
        }

        public Resource Definition { get; }
        public ServiceBase Service { get; }

        protected ResourceBase(ServiceBase service, Resource definition, IResourceContext resourceContext)
        {
            this._initializeTask = null;
            this._routines = new Dictionary<string, RoutineBase>();

            this.Definition = definition;
            this.Service = service;
            this.Context = resourceContext;
        }

        protected override async Task InitializeCore(IExecutionContext context)
        {
            await Task.FromResult(true);
        }

        protected override string GetAsyncInitializedInstanceName()
        {
            return $"Jali Resource '{this.Definition.Name}'";
        }

        protected virtual async Task<RoutineBase> CreateRoutine(string name, IRoutineContext routineContext)
        {
            return await Task.FromResult((RoutineBase)null);
        }

        private Task _initializeTask;
        private readonly IDictionary<string, RoutineBase> _routines;
    }

}