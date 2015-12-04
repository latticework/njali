using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jali.Serve.Definition;

namespace Jali.Serve.Samples.RestbucksServices.OrderRoutines
{
    using GetOrderRoutineBase = RoutineBase<GetOrderRequest, IEnumerable<Order>, OrderKey>;
    using GetOrderProcedureContext = RoutineProcedureContext<GetOrderRequest, IEnumerable<Order>, OrderKey>;

    public class GetOrderRoutine : GetOrderRoutineBase
    {
        public const string Name = "get-" + OrderResource.Name;

        public GetOrderRoutine(ResourceBase resource, IRoutineContext routineContext) 
            : base(resource, GetDefinition(resource.Definition.Url), routineContext)
        {
        }

        protected override Task ExecuteProcedureCore(
            IExecutionContext context, GetOrderProcedureContext procedureContext)
        {
            throw new System.NotImplementedException();
        }

        public static Routine GetDefinition(Uri resourceUrl)
        {
            var url = resourceUrl.Combine($"routines/{Name}");
            return new Routine
            {
                Name = Name,
                Url = url,
                Description = "XXXX",
                Messages =
                {
                    [GetOrderRequest.Action] =
                        GetOrderRequest.GetDefinition(),
                    [GetOrderResponse.Action] =
                        GetOrderResponse.GetDefinition(),
                },
            };
        }
    }
}