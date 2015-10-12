using Jali.Serve.Definition;

namespace Jali.Serve
{
    public interface IRoutineContext
    {
        IResourceContext ResourceContext { get; }
        Routine Definition { get; }
    }
}
