using System.Threading.Tasks;
using Jali.Serve.Definition;

namespace Jali.Serve
{
    public interface IService
    {
        Service Definition { get; }

        Task Init(IExecutionContext context, IServiceContext serviceContext);
        Task<IResource> GetResource(IExecutionContext context, string name);
    }

}