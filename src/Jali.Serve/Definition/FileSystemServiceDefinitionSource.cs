using System.Threading.Tasks;

namespace Jali.Serve.Definition
{
    public class FileSystemServiceDefinitionSource : IServiceDefinitionSource
    {
        public async Task Load(ServiceLoadContext context)
        {
            await Task.FromResult(true);
            throw new System.NotImplementedException();
        }
    }
}