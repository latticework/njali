using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Jali.Serve.Definition;

namespace Jali.Serve.Server.ServiceDescription
{
    public class GetServiceDescriptionRoutine : RoutineBase<
        ServiceMessage<GetServiceDescriptionRequest>, ServiceMessage<GetServiceDescriptionResponse>>
    {
        public const string Name = "get-servicedescription";

        public GetServiceDescriptionRoutine(ResourceBase resource, Routine routine) : base(resource, routine)
        {
            
        }

        protected override async Task ExecuteProcedureCore(
            IExecutionContext context, 
            RoutineProcedureContext<ServiceMessage<GetServiceDescriptionRequest>, ServiceMessage<GetServiceDescriptionResponse>> procedureContext)
        {
            var serviceDefinition = this.Resource.Service.Definition;
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
                Html = WebUtility.HtmlEncode(htmlBuilder.ToString()),
            };

            procedureContext.Response = procedureContext.Request.CreateFromMessage(data, null);

            await Task.FromResult(true);
        }
    }
}