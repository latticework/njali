using System;
using System.Threading.Tasks;

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

        public abstract Task<IServiceMessage> Execute(IExecutionContext context, IServiceMessage request);

        protected virtual async Task InitCore()
        {
            await Task.FromResult(true);
        }

        async Task<IServiceMessage> IRoutine.Execute(IExecutionContext context, IServiceMessage request)
        {
            return await this.Execute(context, (IServiceMessage) request);
        }
    }

    public abstract class RoutineBase<TRequestMessage, TResponseMessage> : RoutineBase
        where TRequestMessage : IServiceMessage
        where TResponseMessage : IServiceMessage
    {
        public override async Task<IServiceMessage> Execute(IExecutionContext context, IServiceMessage request)
        {
            return await this.Execute(context, (TRequestMessage) request);
        }

        private async Task<TResponseMessage> Execute(IExecutionContext context, TRequestMessage request)
        {
            this.Request = request;

            await this.ExecuteCore(context);

            return this.Response;
        }

        protected abstract Task ExecuteCore(IExecutionContext context);

        protected TRequestMessage Request { get; private set; }

        protected TResponseMessage Response { get; set; }
    }
}