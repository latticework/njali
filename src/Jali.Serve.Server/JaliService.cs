using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jali.Core;
using Jali.Serve.Definition;
using Jali.Serve.Server.ServiceDescription;
using Jali.Serve.Server.User;

namespace Jali.Serve.Server
{
    internal class JaliService : ServiceBase
    {
        public JaliService(JaliServerOptions serverOptions, IServiceContext serviceContext) 
            : base(JaliService.GetDefinition(), serviceContext)
        {
            if (serverOptions == null) throw new ArgumentNullException(nameof(serverOptions));

            this.ServerOptions = serverOptions;
        }

        public JaliServerOptions ServerOptions { get; }

        public static Service GetDefinition()
        {
            var jaliserveUrl = new Uri("http://jali.io/serve/v0.0.1/service");

            var jaliService = new Service
            {
                Name = "jaliserve",
                Url = jaliserveUrl,
                Version = "0.0.1",
                Description = "Internal Jali services.",
                Resources =
                {
                    [ServiceDescriptionResource.Name] = ServiceDescriptionResource.GetDefinition(jaliserveUrl),
                    [UserResource.Name] = UserResource.GetDefinition(jaliserveUrl),
                },
            };
            return jaliService;
        }

        protected override async Task<ResourceBase> CreateResource(string name, IResourceContext resourceContext)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));


            var resourceResult = this.Definition.Resources.GetValueOrDefault(name);


            if (!resourceResult.Found)
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

            return await Task.FromResult(resourceFactory(this, resourceContext, this.ServerOptions));
        }

        static JaliService()
        {
            JaliService._resourceFactories = new Dictionary<string, Func<ServiceBase, IResourceContext, JaliServerOptions, ResourceBase>>
            {
                [ServiceDescriptionResource.Name] = (s, c, opt) => new ServiceDescriptionResource(s, c, opt),
                [UserResource.Name] = (s, c, opt) => new UserResource(s, c, opt),
            };
        }

        private static IDictionary<string, Func<ServiceBase, IResourceContext, JaliServerOptions, ResourceBase>> _resourceFactories;
    }
}