using System;
using Jali.Serve.Definition;

namespace Jali.Serve.Server
{
    public class ResourceManager
    {
        public ResourceManager(ServiceManager serviceManager, Resource resource)
        {
            if (serviceManager == null) throw new ArgumentNullException(nameof(serviceManager));
            if (resource == null) throw new ArgumentNullException(nameof(resource));


            this.ServiceManager = serviceManager;
            this.Resource = resource;
        }

        public ServiceManager ServiceManager { get; }

        public Resource Resource { get; }
    }
}
