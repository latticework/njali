using System.Threading.Tasks;

namespace Jali.Serve
{
    public interface IService
    {
        Task Init(IExecutionContext context, IServiceContext serviceContext);
        Task<IResource> GetResource(IExecutionContext context, string name);
    }

}