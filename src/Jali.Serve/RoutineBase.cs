using System.Threading.Tasks;
using Jali.Serve.Definition;

namespace Jali.Serve
{
    public abstract class RoutineBase : IRoutine
    {
        public virtual async Task Init(IExecutionContext context, IRoutineContext routineContext)
        {
            this.Context = routineContext;

            await InitCore();
        }

        public IRoutineContext Context { get; private set; }

        public Routine Definition { get; }
        public ResourceBase Resource { get; }

        public abstract Task<IServiceMessage> ExecuteProcedure(
            IExecutionContext context, string requestAction, string responseAction, IServiceMessage request);

        protected RoutineBase(ResourceBase resource, Routine routine)
        {
            this.Definition = routine;
            this.Resource = resource;
        }

        protected virtual async Task InitCore()
        {
            await Task.FromResult(true);
        }
    }

    public abstract class RoutineBase<TRequestMessage, TResponseMessage> : RoutineBase
        where TRequestMessage : IServiceMessage
        where TResponseMessage : IServiceMessage
    {
        public override async Task<IServiceMessage> ExecuteProcedure(
            IExecutionContext context, string requestAction, string responseAction, IServiceMessage request)
        {
            return await this.ExecuteProcedure(context, requestAction, responseAction, (TRequestMessage) request);
        }

        private RoutineProcedureContext<TRequestMessage, TResponseMessage> CreateProcedureContext()
        {
            return new RoutineProcedureContext<TRequestMessage, TResponseMessage>();
        }  

        private async Task<TResponseMessage> ExecuteProcedure(
            IExecutionContext context, string requestAction, string responseAction, TRequestMessage request)
        {
            var procedureContext = this.CreateProcedureContext();

            procedureContext.RequestMessageDefinition = this.Definition.Messages[requestAction];
            procedureContext.ResponseMessageDefinition = this.Definition.Messages[responseAction];
            procedureContext.Request = request;

            await this.ExecuteProcedureCore(context, procedureContext);

            return procedureContext.Response;
        }

        protected RoutineBase(ResourceBase resource, Routine routine) : base(resource, routine)
        {
            
        }

        protected abstract Task ExecuteProcedureCore(
            IExecutionContext context, RoutineProcedureContext<TRequestMessage, TResponseMessage> procedureContext);
    }
}