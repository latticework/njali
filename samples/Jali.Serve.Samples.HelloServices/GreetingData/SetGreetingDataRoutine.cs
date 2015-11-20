using System;
using System.Threading.Tasks;
using Jali.Serve.Definition;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Jali.Serve.Samples.HelloServices.GreetingData
{
    public class SetGreetingDataRoutine : RoutineBase<JObject, GreetingDataChangeResult, GreetingDataKey>
    {
        public const string Name = "set-greetingdata";

        public SetGreetingDataRoutine(ResourceBase resource, IRoutineContext routineContext)
            : base(resource, GetDefinition(resource.Definition.Url), routineContext)
        {
        }

        // HTTP Patch: https://tools.ietf.org/html/rfc5789
        // JSON Patch: http://tools.ietf.org/html/rfc6902
        // http://williamdurand.fr/2014/02/14/please-do-not-patch-like-an-idiot/
        protected override async Task ExecuteProcedureCore(
            IExecutionContext context, 
            RoutineProcedureContext<JObject, GreetingDataChangeResult, GreetingDataKey> procedureContext)
        {
            if (procedureContext.Key == null)
            {
                // TODO: SetGreetingDataRoutine.ExecuteProcedure: Replace with DomainErrorException.
                var message =
                    $"Resource '{this.Resource.Definition.Name}' " +
                    $"routine '{this.Definition.Name}' does not support Jali REST methods without a specified key.";

                throw new InvalidOperationException(message);
            }

            var resource = GreetingDataResource.GetGreetingDataByKey(procedureContext.Key.Id);

            // TODO: SetGreetingDataRoutine.ExecuteProcedure: Replace with DomainErrorException.
            if (resource == null)
            {
                var key = JObject.FromObject(procedureContext.Key).ToString(Formatting.None);

                var message =
                    $"Jali resource '{this.Resource.Definition.Name}' " +
                    $"routine '{this.Definition.Name}' cannot find the resource with key '{key}'.";

                throw new InvalidOperationException(message);
            }

            var changes = procedureContext.Request.Data["0"];

            foreach (var change in changes)
            {
                var changeObject = (JObject) change;
                var op = changeObject["op"].Value<string>();
                var path = changeObject["path"].Value<string>();
                var value = changeObject["value"].Value<string>();

                switch (path)
                {
                    case "/lang":
                        VerifyOperation(procedureContext.Key, "test", op, path);
                        break;
                    case "/greeting":
                        VerifyOperation(procedureContext.Key, "replace", op, path);

                        resource.Greeting = value;
                        break;
                    case "/separator":
                        VerifyOperation(procedureContext.Key, "replace", op, path);

                        resource.Separator = value;
                        break;
                    default:
                        // TODO: SetGreetingDataRoutine.ExecuteProcedure: Replace with DomainErrorException.
                        var key = JObject.FromObject(procedureContext.Key).ToString(Formatting.None);

                        var message =
                            $"Jali resource '{this.Resource.Definition.Name}' " +
                            $"routine '{this.Definition.Name}' cannot perform opartion '{op}' on unknown property " +
                            $"'{path}' for resource with key '{key}'.";

                        throw new InvalidOperationException(message);
                }
            }

            GreetingDataResource.ChangeGreetingData(resource);

            procedureContext.Response = procedureContext.Request.CreateOutboundMessage(
                new MessageCredentials(),
                new GreetingDataChangeResult
                {
                    GreetingDataId = procedureContext.Key.Id,
                },
                null);
        }

        public static Routine GetDefinition(Uri resourceUrl)
        {
            var url = new Uri(resourceUrl, $"routines/{Name}");
            return new Routine
            {
                Name = Name,
                Url = url,
                Description = "Changes greetingdata according to the specified request.",
                Messages =
                {
                    ["set-greetingdata-bykey"] = new RoutineMessage
                    {
                        Action = Name,
                        Description = "Add greeting data by unique identifier.",
                        Direction = MessageDirection.Inbound,
                        Schema = new SchemaReference
                        {
                            SchemaType = SchemaType.Patch,
                        },
                    },
                    ["set-greetingdata-result"] = new RoutineMessage
                    {
                        Action = "set-greetingdata-result",
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

        private void VerifyOperation(
            GreetingDataKey keyObject, 
            string expectedOp, 
            string actualOp,
            string path)
        {
            if (actualOp == expectedOp) { return; }

            // TODO: SetGreetingDataRoutine.ExecuteProcedure: Replace with DomainErrorException.
            var key = JObject.FromObject(keyObject).ToString(Formatting.None);

            var message =
                $"Jali resource '{this.Resource.Definition.Name}' " +
                $"routine '{this.Definition.Name}' cannot perform opartion '{actualOp}' on property '{path}' " +
                $"for resource with key '{key}'.";

            throw new InvalidOperationException(message);
        }
    }
}