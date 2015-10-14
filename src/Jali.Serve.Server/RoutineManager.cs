using System;
using System.Threading.Tasks;

namespace Jali.Serve.Server
{
    internal sealed class RoutineManager
    {
        public RoutineManager(ResourceManager resourceManager, IRoutine routine)
        {
            if (resourceManager == null) throw new ArgumentNullException(nameof(resourceManager));
            if (routine == null) throw new ArgumentNullException(nameof(routine));


            this.ResourceManager = resourceManager;
            this.Routine = routine;

            this.Context = new RoutineContext
            {
                ResourceContext = resourceManager.Context,
                Manager = this,
                Definition = routine.Definition,
            };
        }

        public IRoutine Routine { get; }

        public RoutineContext Context { get; }

        public ResourceManager ResourceManager { get; }

        public async Task<IServiceMessage> ExecuteProcedure(
            string requestAction, string responseAction, IServiceMessage request)
        {
            // TODO: RoutineManager.ExecuteProcedure: Should Init be called by a Run method instead?
            await this.Routine.Init(new ExecutionContext(), this.Context);

            return await this.Routine.ExecuteProcedure(new ExecutionContext(), requestAction, responseAction, request);
        }
    }
}
