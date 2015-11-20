using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jali.Serve.Definition;
using Newtonsoft.Json.Linq;

namespace Jali.Serve.Samples.HelloServices.GreetingData
{
    public class NewGreetingDataRoutine : RoutineBase<GreetingData, GreetingDataChangeResult>
    {
        public const string Name = "new-greetingdata";

        public NewGreetingDataRoutine(ResourceBase resource, IRoutineContext routineContext) 
            : base(resource, GetDefinition(resource.Definition.Url), routineContext)
        {
        }

        protected override async Task ExecuteProcedureCore(
            IExecutionContext context, 
            RoutineProcedureContext<GreetingData, GreetingDataChangeResult, JObject> 
                procedureContext)
        {
            var result = GreetingDataResource.AddGreetingData(procedureContext.Request.Data);

            var response = procedureContext.Request.CreateOutboundMessage(new MessageCredentials(), result, null);

            procedureContext.Response = response;

            await Task.FromResult(true);
        }

        public GreetingDataResource GreetingDataResource => (GreetingDataResource) this.Resource;

        public static Routine GetDefinition(Uri resourceUrl)
        {
            var url = new Uri(resourceUrl, $"routines/{Name}");
            return new Routine
            {
                Name = Name,
                Url = url,
                Description = "Add greetingdata according to the specified request.",
                Messages =
                {
                    ["new-greetingdata-request"] = new RoutineMessage
                    {
                        Action = Name,
                        Description = "Add greeting data by unique identifier.",
                        Direction = MessageDirection.Inbound,
                        Schema = new SchemaReference
                        {
                            SchemaType = SchemaType.Resource,
                        },
                    },

                    ["new-greetingdata-result"] = new RoutineMessage
                    {
                        Action = "new-greetingdata-result",
                        Direction = MessageDirection.Outbound,
                        Description = "The operation result.",
                        Schema = new SchemaReference
                        {
                            SchemaType = SchemaType.Result,
                        },
                    },
                },
            };
        }
    }
}