using System.Threading.Tasks;
using Jali.Serve.Definition;

namespace Jali.Serve
{
    public interface IService : IAsyncInitialized
    {
        Service Definition { get; }

        Task<IResource> GetResource(IExecutionContext context, string name, IResourceContext resourceContext);
    }

}