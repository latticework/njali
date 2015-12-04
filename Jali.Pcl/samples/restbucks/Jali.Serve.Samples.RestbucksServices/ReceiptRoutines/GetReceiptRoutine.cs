using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jali.Serve.Definition;

namespace Jali.Serve.Samples.RestbucksServices.ReceiptRoutines
{
    using GetReceiptRoutineBase = RoutineBase<GetReceiptRequest, IEnumerable<Receipt>, ReceiptKey>;
    using GetReceiptProcedureContext = RoutineProcedureContext<GetReceiptRequest, IEnumerable<Receipt>, ReceiptKey>;

    public class GetReceiptRoutine : GetReceiptRoutineBase
    {
        public const string Name = "get-" + ReceiptResource.Name;

        public GetReceiptRoutine(ResourceBase resource, IRoutineContext routineContext) 
            : base(resource, GetDefinition(resource.Definition.Url), routineContext)
        {
        }

        protected override Task ExecuteProcedureCore(
            IExecutionContext context, GetReceiptProcedureContext procedureContext)
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
                    [GetReceiptRequest.Action] =
                        GetReceiptRequest.GetDefinition(),
                    [GetReceiptResponse.Action] =
                        GetReceiptResponse.GetDefinition(),
                },
            };
        }
    }
}