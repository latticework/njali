using System.Threading.Tasks;
using Jali.Serve.Definition;

namespace Jali.Serve.Samples.HelloServices
{
    public class GetHelloRoutine : RoutineBase<ServiceMessage<GetHelloRequest>, ServiceMessage<GetHelloResponse>>
    {
        public GetHelloRoutine(ResourceBase resource, Routine routine) : base(resource, routine)
        {
        }

        protected override async Task ExecuteProcedureCore(
            IExecutionContext context, 
            RoutineProcedureContext<ServiceMessage<GetHelloRequest>, ServiceMessage<GetHelloResponse>> procedureContext)
        {
            var name = procedureContext.Request.Data.Name;

            var nameClause = string.IsNullOrEmpty(name) ? " World" : $", {name}";

            var data = new GetHelloResponse
            {
                Message = $"Hello{nameClause}!",
            };

            procedureContext.Response = procedureContext.Request.CreateFromMessage(data, null);

            await Task.FromResult(true);
        }
    }
}