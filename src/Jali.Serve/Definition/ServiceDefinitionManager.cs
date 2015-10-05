using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Jali.Serve.Definition
{
    public sealed class ServiceDefinitionManager
    {
        public ServiceDefinitionManager()
        {
            this.Sources = new List<IServiceDefinitionSource>();
        }

        public IList<IServiceDefinitionSource> Sources { get; }

        public async Task<ServiceLoadResult> Load(Uri url)
        {
            var context = new ServiceLoadContext(url);

            foreach (var source in this.Sources)
            {
                await source.Load(context);

                if (context.Service != null)
                {
                    break;
                }
            }

            // TODO: throw error if no source handles load.
            // TODO: Propegate messages.
            var result = new ServiceLoadResult
            {
                Service = context.Service,
            };

            result.Messages.Append(context.Messsages);

            return result;
        }
    }
}
