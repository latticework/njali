using System;
using System.Threading.Tasks;
using Jali.Serve.Definition;
using Jali.Serve.Samples.RestbucksServices.OrderRoutines;
using Jali.Serve.Samples.RestbucksServices.ReceiptRoutines;

namespace Jali.Serve.Samples.RestbucksServices
{
    public class RestbucksService : ServiceBase
    {
        public const string Name = "restbucks";

        public RestbucksService(Service definition, IServiceContext serviceContext) : base(definition, serviceContext)
        {
        }

        public static Service GetDefinition(Uri url)
        {
            return new Service
            {
                Name = Name,
                Version = "1.0.0",
                Url = url,
                Description = "Restbucks Point of Sale Services",
                Resources =
                {
                    
                }
            };
        }

        protected override async Task<ResourceBase> CreateResource(string name, IResourceContext resourceContext)
        {
            await Task.FromResult(true);

            switch (name)
            {
                case OrderResource.Name:
                    return new OrderResource(this, resourceContext);
                case ReceiptResource.Name:
                    return new ReceiptResource(this, resourceContext);
                default:
                    return null;
            }
        }
    }
}