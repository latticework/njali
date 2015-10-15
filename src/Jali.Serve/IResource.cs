using System.Threading.Tasks;
using Jali.Serve.Definition;

namespace Jali.Serve
{
    public interface IResource
    {
        Resource Definition { get; }

        Task Init(IExecutionContext context, IResourceContext resourceContext);
        Task<IRoutine> GetRoutine(IExecutionContext context, string name);
    }
}