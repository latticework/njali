using Jali.Note;

namespace Jali.Serve
{
    public interface IStageContext
    {
        IRoutineContext RoutineContext { get; }
        // TODO: IStageContext: Add Stage property when implemented.
        // Stage Definition { get; }

        NotificationMessageCollection Messages { get; }
    }
}
