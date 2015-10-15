using Jali.Serve.Definition;

namespace Jali.Serve.Server
{
    internal class ResourceContext : IResourceContext
    {
        public IServiceContext ServiceContext { get; set; }
        public ResourceManager Manager { get; set; }
        public Resource Definition { get; set; }
    }
}