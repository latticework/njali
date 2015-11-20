using System.Threading.Tasks;
using Jali.Core;
using Jali.Serve.Definition;

namespace Jali.Serve
{
    public interface IResource : IAsyncInitialized
    {
        Resource Definition { get; }

        Task<IRoutine> GetRoutine(IExecutionContext context, string name, IRoutineContext routineContext);
    }
}