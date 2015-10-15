using System.Threading.Tasks;
using Jali.Serve.Definition;
using Newtonsoft.Json.Linq;

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
            IExecutionContext context, string requestAction, string responseAction, ServiceMessage<JObject> request);

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

    public abstract class RoutineBase<TRequestData, TResponseData> : RoutineBase
    {
        public override async Task<IServiceMessage> ExecuteProcedure(
            IExecutionContext context, string requestAction, string responseAction, ServiceMessage<JObject> request)
        {
            var typedRequest = request.ToTypedMessages<TRequestData>();

            return await this.ExecuteProcedure(context, requestAction, responseAction, typedRequest);
        }

        private RoutineProcedureContext<TRequestData, TResponseData> CreateProcedureContext()
        {
            return new RoutineProcedureContext<TRequestData, TResponseData>();
        }  

        private async Task<ServiceMessage<TResponseData>> ExecuteProcedure(
            IExecutionContext context, string requestAction, string responseAction, ServiceMessage<TRequestData> request)
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
            IExecutionContext context, RoutineProcedureContext<TRequestData, TResponseData> procedureContext);
    }
}