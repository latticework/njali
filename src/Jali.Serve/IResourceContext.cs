using Jali.Serve.Definition;

namespace Jali.Serve
{
    public interface IResourceContext
    {
        IServiceContext ServiceContext { get; }
        Resource ResourceDefinition { get; }
    }
}