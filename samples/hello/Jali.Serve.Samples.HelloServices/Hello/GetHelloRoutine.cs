using System.Threading.Tasks;
using Jali.Serve.Definition;
using Jali.Serve.Samples.HelloServices.GreetingData;
using Newtonsoft.Json.Linq;

namespace Jali.Serve.Samples.HelloServices
{
    public class GetHelloRoutine : RoutineBase<GetHelloRequest, GetHelloResponse>
    {
        public const string Name = "get-hello";

        public GetHelloRoutine(ResourceBase resource, Routine definition, IRoutineContext routineContext) 
            : base(resource, definition, routineContext)
        {
        }

        protected override async Task ExecuteProcedureCore(
            IExecutionContext context, 
            RoutineProcedureContext<GetHelloRequest, GetHelloResponse, JObject> procedureContext)
        {
            var greetingData = GreetingDataResource.GetGreetingDataByLanguage(
                procedureContext.Request.Data?.Lang ?? "en");

            var name = procedureContext.Request.Data?.Name;

            var nameClause = string.IsNullOrEmpty(name) ? " World" : $"{greetingData.Separator} {name}";

            var data = new GetHelloResponse
            {
                Message = $"{greetingData.Greeting}{nameClause}!",
            };

            procedureContext.Response = procedureContext.Request.CreateOutboundMessage(new MessageCredentials(), data, null);

            await Task.FromResult(true);
        }
    }
}