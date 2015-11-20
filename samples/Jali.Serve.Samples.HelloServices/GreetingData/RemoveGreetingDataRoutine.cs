using System;
using System.Threading.Tasks;
using System.Xml;
using Jali.Serve.Definition;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using SchemaReference = Jali.Serve.Definition.SchemaReference;

namespace Jali.Serve.Samples.HelloServices.GreetingData
{
    public class RemoveGreetingDataRoutine : RoutineBase<JObject, GreetingDataChangeResult, GreetingDataKey>
    {
        public const string Name = "remove-greetingdata";


        public RemoveGreetingDataRoutine(ResourceBase resource, IRoutineContext routineContext)
            : base(resource, GetDefinition(resource.Definition.Url), routineContext)
        {
        }

        protected override async Task ExecuteProcedureCore(
            IExecutionContext context, 
            RoutineProcedureContext<JObject, GreetingDataChangeResult, GreetingDataKey> procedureContext)
        {
            if (procedureContext.Key == null)
            {
                // TODO: RemoveGreetingDataRoutine.ExecuteProcedure: Replace with DomainErrorException.
                var message =
                    $"Resource '{this.Resource.Definition.Name}' " +
                    $"routine '{this.Definition.Name}' does not support Jali REST methods without a specified key.";

                throw new InvalidOperationException(message);
            }

            GreetingDataResource.RemoveGreetingData(procedureContext.Key.Id);

            var result = new GreetingDataChangeResult
            {
                GreetingDataId = procedureContext.Key.Id,
            };

            procedureContext.Response = procedureContext.Request.CreateOutboundMessage(
                new MessageCredentials(), result, null);

            await Task.FromResult(true);
        }

        public static Routine GetDefinition(Uri resourceUrl)
        {
            var url = new Uri(resourceUrl, $"routines/{Name}");
            return new Routine
            {
                Name = Name,
                Url = url,
                Description = "Removes greetingdata according to the specified key.",
                Messages =
                {
                    ["remove-greetingdata-bykey"] = new RoutineMessage
                    {
                        Action = Name,
                        Description = "Remove greeting data by unique identifier.",
                        Direction = MessageDirection.Inbound,
                        Schema = new SchemaReference
                        {
                            SchemaType = SchemaType.Key,
                        },
                    },

                    ["remove-greetingdata-result"] = new RoutineMessage
                    {
                        Action = "remove-greetingdata-result",
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