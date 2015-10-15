using Jali.Serve.Definition;

namespace Jali.Serve.Server
{
    internal class ServiceContext : IServiceContext
    {
        public ServiceManager Manager { get; set; }
        public Service Definition { get; set; }
    }
}