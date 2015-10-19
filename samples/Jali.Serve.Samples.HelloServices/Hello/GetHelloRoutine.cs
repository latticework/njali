using System.Threading.Tasks;
using Jali.Serve.Definition;

namespace Jali.Serve.Samples.HelloServices
{
    public class GetHelloRoutine : RoutineBase<GetHelloRequest, GetHelloResponse>
    {
        public const string Name = "get-hello";

        public GetHelloRoutine(ResourceBase resource, Routine routine) : base(resource, routine)
        {
        }

        protected override async Task ExecuteProcedureCore(
            IExecutionContext context, 
            RoutineProcedureContext<GetHelloRequest, GetHelloResponse> procedureContext)
        {
            var name = procedureContext.Request.Data.Name;

            var nameClause = string.IsNullOrEmpty(name) ? " World" : $", {name}";

            var data = new GetHelloResponse
            {
                Message = $"Hello{nameClause}!",
            };

            procedureContext.Response = procedureContext.Request.CreateOutboundMessage(new MessageCredentials(), data, null);

            await Task.FromResult(true);
        }
    }
}