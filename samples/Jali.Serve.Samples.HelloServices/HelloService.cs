using System;
using System.Collections.Generic;
using Jali.Core;
using Jali.Serve.Definition;
using Jali.Serve.Samples.HelloServices.GreetingData;

namespace Jali.Serve.Samples.HelloServices
{
    using System.Threading.Tasks;

    public class HelloService : ServiceBase
    {
        public HelloService(IServiceContext serviceContext) : base(HelloService.GetDefinition(), serviceContext)
        {
        }

        public static Service GetDefinition()
        {
            var helloServiceUrl = new Uri("http://jali.io/serve/v0.0.1/samples/hello");
            var helloResource = HelloResource.GetDefinition(helloServiceUrl);
            var greetingDataResource = GreetingDataResource.GetDefinition(helloServiceUrl);

            var helloService = new Service
            {
                Name = "hello",
                Url = helloServiceUrl,
                Version = "1.0.0",
                Description = "Hello Jali service.",
                Resources =
                {
                    [helloResource.Name] = helloResource,
                    [greetingDataResource.Name] = greetingDataResource,
                }
            };

            return helloService;
        }

        protected override async Task<ResourceBase> CreateResource(string name, IResourceContext resourceContext)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));


            var resourceResult = this.Definition.Resources.GetValueOrDefault(name);


            if (!resourceResult.Found)
            {
                throw new InternalErrorException(
                    $"Hello service has been requested to create invalid resource '{name}'.");
            }


            var resource = resourceResult.Value;

            var resourceFactory = HelloService._resourceFactories[resource.Name];


            if (resourceFactory == null)
            {
                throw new InternalErrorException(
                    $"Hello service has not implemented correctly specified requested resource '{name}'.");
            }

            return await Task.FromResult(resourceFactory(this, resource, resourceContext));
        }

        static HelloService()
        {
            HelloService._resourceFactories = new Dictionary<string, Func<ServiceBase, Resource, IResourceContext, ResourceBase>>
            {
                [HelloResource.Name] = (s, c, r) => new HelloResource(s, c, r),
                [GreetingDataResource.Name] = (s, c, r) => new GreetingDataResource(s, c, r),
            };
        }

        private static IDictionary<string, Func<ServiceBase, Resource, IResourceContext, ResourceBase>>
            _resourceFactories;
    }
}