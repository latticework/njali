using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jali.Serve.Definition;

namespace Jali.Serve.Samples.HelloServices.GreetingData
{
    public class GetGreetingDataRoutine : 
        RoutineBase<GetGreetingDataRequest, IEnumerable<GreetingData>, GreetingDataKey>
    {
        public const string Name = "get-greetingdata";

        public GetGreetingDataRoutine(ResourceBase resource) : base(resource, GetDefinition(resource.Definition.Url))
        {
        }

        protected override async Task ExecuteProcedureCore(
            IExecutionContext context, 
            RoutineProcedureContext<GetGreetingDataRequest, IEnumerable<GreetingData>, GreetingDataKey> 
                procedureContext)
        {
            var result = new List<GreetingData>();

            if (procedureContext.Key != null)
            {
                result.Add(GreetingDataResource.GetGreetingDataByKey(procedureContext.Key.Id));
            }
            else if (procedureContext.Request.Data.Lang != null)
            {
                result.Add(GreetingDataResource.GetGreetingDataByLanguage(procedureContext.Request.Data.Lang));
            }
            else
            {
                result.AddRange(GreetingDataResource.GetAllGreetingData());
            }

            procedureContext.Response = procedureContext.Request.CreateOutboundMessage(
                new MessageCredentials(), (IEnumerable<GreetingData>)result, null);

            await Task.FromResult(true);
        }

        public static Routine GetDefinition(Uri resourceUrl)
        {
            var url = new Uri(resourceUrl, $"routines/{Name}");
            return new Routine
            {
                Name = Name,
                Url = url,
                Description = "Gets greetingdata according to the specified criteria.",
                Messages =
                {
                    ["get-greetingdata-bykey"] = new RoutineMessage
                    {
                        Action = Name,
                        Description = "Retrive greeting data by unique identifier.",
                        Direction = MessageDirection.Inbound,
                        Schema = new SchemaReference
                        {
                            SchemaType = SchemaType.Key,
                        },
                    },

                    [GetGreetingDataRequest.Name] = GetGreetingDataRequest.GetDefinition(),

                    ["get-greetingdata-result"] = new RoutineMessage
                    {
                        Action = "get-greetingdata-result",
                        Direction = MessageDirection.Outbound,
                        Description = "The selected greetingdata resources.",
                        Schema = new SchemaReference
                        {
                            SchemaType = SchemaType.Resource,
                        },
                    },
                },
            };
        }
    }
}