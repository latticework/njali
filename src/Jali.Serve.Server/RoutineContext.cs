using Jali.Serve.Definition;

namespace Jali.Serve.Server
{
    internal class RoutineContext : IRoutineContext
    {
        public IResourceContext ResourceContext { get; set; }
        public RoutineManager Manager { get; set; }
        public Routine Definition { get; set; }
    }
}