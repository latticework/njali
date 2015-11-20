using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jali.Serve.Definition;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using SchemaReference = Jali.Serve.Definition.SchemaReference;

namespace Jali.Serve.Server.ServiceDescription
{
    public class GetServiceDescriptionRoutine 
        : ServerRoutineBase<GetServiceDescriptionRequest, GetServiceDescriptionResponse>
    {
        public const string Name = "get-servicedescription";

        public GetServiceDescriptionRoutine(
            ResourceBase resource, IRoutineContext routineContext, JaliServerOptions options)
            : base(resource, GetDescription(resource.Definition.Url), routineContext, options)
        {
            
        }

        public static Routine GetDescription(Uri resourceUrl)
        {
            return new Routine
            {
                Name = Name,
                Url = new Uri(resourceUrl, $"routines/{Name}"),
                Description = "Get an html representation of the Jali service.",
                DefaultAuthentication = AuthenticationRequirement.Ignored,
                Messages =
                {
                    ["get-servicedescription-request"] = new RoutineMessage
                    {
                        Action = "get-servicedescription-request",
                        Direction = MessageDirection.Inbound,
                        Description = "The get-servicedescription request message",
                        Schema = new SchemaReference
                        {
                            SchemaType = SchemaType.Direct,
                            Schema = JSchema.Parse(@"{
  ""$schema"": ""http://json-schema.org/draft-04/schema#"",

  ""type"": ""object"",
  ""properties"": {
  }
}"),
                        },
                    },
                    ["get-servicedescription-response"] = new RoutineMessage
                    {
                        Action = "get-servicedescription-request",
                        Direction = MessageDirection.Inbound,
                        Description = "The get-servicedescription request message",
                        Schema = new SchemaReference
                        {
                            SchemaType = SchemaType.Resource,
                        },
                    },
                },
            };
        }

        protected override async Task ExecuteProcedureCore(
            IExecutionContext context, 
            RoutineProcedureContext<GetServiceDescriptionRequest, GetServiceDescriptionResponse, JObject> procedureContext)
        {
            var serviceDefinition = procedureContext.Request.Data.Service;
            var serviceTitle = $"{serviceDefinition.Name} Service Version {serviceDefinition.Version}";
            var serviceUrl = serviceDefinition.Url;

            var resourceTableData = serviceDefinition.Resources.Select(kvp => new
            {
                Name = kvp.Value.Name,
                Version = kvp.Value.Version,
                Location = kvp.Value.Url,
                Description = kvp.Value.Description,
            });

            var htmlBuilder = new StringBuilder();

            htmlBuilder.AppendLine($"<!DOCTYPE html>");
            htmlBuilder.AppendLine($"<html>");
            htmlBuilder.AppendLine($"  <head>");
            htmlBuilder.AppendLine($"    <title>{serviceTitle}</title>");
            htmlBuilder.AppendLine($"  </head>");
            htmlBuilder.AppendLine($"  <body>");
            htmlBuilder.AppendLine($"    <h1>{serviceTitle}</h1>");
            htmlBuilder.AppendLine($"    <hr>");
            htmlBuilder.AppendLine($"    <p><i>Location:</i> <a href='{serviceUrl}'>{serviceUrl}</a></p>");
            htmlBuilder.AppendLine($"    <h2>Resources</h2>");
            htmlBuilder.AppendLine($"    <table style='width:100%'>");
            htmlBuilder.AppendLine($"      <tr>");
            htmlBuilder.AppendLine($"        <th>Name</th>");
            htmlBuilder.AppendLine($"        <th>Version</th>");
            htmlBuilder.AppendLine($"        <th>Location</th>");
            htmlBuilder.AppendLine($"        <th>Description</th>");
            htmlBuilder.AppendLine($"      </tr>");

            foreach (var resourceRowData in resourceTableData)
            {
                htmlBuilder.AppendLine($"      <tr>");
                htmlBuilder.AppendLine($"        <th>{resourceRowData.Name}</th>");
                htmlBuilder.AppendLine($"        <th>{resourceRowData.Version}</th>");
                htmlBuilder.AppendLine($"        <th>{resourceRowData.Location}</th>");
                htmlBuilder.AppendLine($"        <th>{resourceRowData.Description}</th>");
                htmlBuilder.AppendLine($"      </tr>");
            }

            htmlBuilder.AppendLine($"    </table>");
            htmlBuilder.AppendLine($"  </body>");
            htmlBuilder.AppendLine($"</html>");

            var data = new GetServiceDescriptionResponse
            {
                Html = htmlBuilder.ToString(),
            };

            procedureContext.Response = procedureContext.Request.CreateOutboundMessage(new MessageCredentials(), data, null);

            await Task.FromResult(true);
        }
    }
}