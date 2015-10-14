using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jali.Core;
using Jali.Serve.Definition;
using Jali.Serve.Server.ServiceDescription;

namespace Jali.Serve.Server
{
    internal class JaliService : ServiceBase
    {
        public JaliService() : base(JaliService.GetDefinition())
        {
        }

        public static Service GetDefinition()
        {
            var jaliserveUrl = new Uri("http://jali.io/serve/v0.0.1/service");
            var serviceDescriptionResource = ServiceDescriptionResource.GetDefinition(jaliserveUrl);

            var jaliService = new Service
            {
                Name = "jaliserve",
                Url = jaliserveUrl,
                Version = "0.0.1",
                Description = "Internal Jali services.",
                Resources =
                {
                    [serviceDescriptionResource.Name] = serviceDescriptionResource,
                },
            };
            return jaliService;
        }

        protected override async Task<ResourceBase> CreateResource(string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));


            var resourceResult = this.Definition.Resources.GetValueOrDefault(name);


            if (!resourceResult.Succeeded)
            {
                throw new InternalErrorException(
                    $"Jali server has been requested to create invalid internal resource '{name}'.");
            }


            var resource = resourceResult.Value;

            var resourceFactory = JaliService._resourceFactories[resource.Name];


            if (resourceFactory == null)
            {
                throw new InternalErrorException(
                    $"Jali server has not implemented correctly specified requested internal resource '{name}'.");
            }

            return await Task.FromResult(resourceFactory(this, resource));
        }

        static JaliService()
        {
            JaliService._resourceFactories = new Dictionary<string, Func<ServiceBase, Resource, ResourceBase>>
            {
                [ServiceDescriptionResource.Name] = (s, r) => new ServiceDescriptionResource(s, r),
            };
        }

        private static IDictionary<string, Func<ServiceBase, Resource, ResourceBase>> _resourceFactories;
    }
}