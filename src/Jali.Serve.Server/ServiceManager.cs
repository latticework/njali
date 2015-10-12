using System;
using System.Threading.Tasks;
using Jali.Serve.Definition;

namespace Jali.Serve.Server
{
    public class ServiceManager
    {
        public ServiceManager(Service service)
        {
            if (service == null) throw new ArgumentNullException(nameof(service));


            this.Service = service;
        }

        public Service Service { get; }

        public async Task Run()
        {
            await Task.FromResult(true);
        }
    }
}
