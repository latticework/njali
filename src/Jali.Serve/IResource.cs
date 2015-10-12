using System.Threading.Tasks;

namespace Jali.Serve
{
    public interface IResource
    {
        Task Init(IExecutionContext context, IResourceContext resourceContext);
        Task<IRoutine> GetRoutine(IExecutionContext context, string name);
    }
}