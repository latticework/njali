using System.Threading.Tasks;
using Jali.Serve.Definition;

namespace Jali.Serve
{
    public interface IRoutine
    {
        Routine Definition { get; }

        Task Init(IExecutionContext context, IRoutineContext routineContext);

        Task<IServiceMessage> ExecuteProcedure(
            IExecutionContext context, string requestAction, string responseAction, IServiceMessage request);
    }

    // TODO: IRoutine: Determine whether typed routine interface makes sense.

    //public interface IRoutine<in TRequestMessage, TResponseMessage> : IRoutine
    //    where TRequestMessage : IServiceMessage
    //    where TResponseMessage : IServiceMessage
    //{
    //    Task<TResponseMessage> Execute(IExecutionContext context, TRequestMessage request);
    //}
}