using System.Threading.Tasks;

namespace Jali.Serve
{
    public interface IRoutine
    {
        Task Init(IExecutionContext context, IRoutineContext routineContext);
        Task<IServiceMessage> Execute(IExecutionContext context, IServiceMessage request);
    }

    // TODO: IRoutine: Determine whether typed routine interface makes sense.

    //public interface IRoutine<in TRequestMessage, TResponseMessage> : IRoutine
    //    where TRequestMessage : IServiceMessage
    //    where TResponseMessage : IServiceMessage
    //{
    //    Task<TResponseMessage> Execute(IExecutionContext context, TRequestMessage request);
    //}
}