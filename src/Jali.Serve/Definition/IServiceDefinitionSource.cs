using System.Threading.Tasks;

namespace Jali.Serve.Definition
{
    public interface IServiceDefinitionSource
    {
        Task Load(ServiceLoadContext context);
    }
}