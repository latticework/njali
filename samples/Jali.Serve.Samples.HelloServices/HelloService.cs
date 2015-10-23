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
        public HelloService() : base(HelloService.GetDefinition())
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

        protected override async Task<ResourceBase> CreateResource(string name)
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

            return await Task.FromResult(resourceFactory(this, resource));
        }

        static HelloService()
        {
            HelloService._resourceFactories = new Dictionary<string, Func<ServiceBase, Resource, ResourceBase>>
            {
                [HelloResource.Name] = (s, r) => new HelloResource(s, r),
                [GreetingDataResource.Name] = (s, r) => new GreetingDataResource(s, r),
            };
        }

        private static IDictionary<string, Func<ServiceBase, Resource, ResourceBase>> _resourceFactories;
    }
}