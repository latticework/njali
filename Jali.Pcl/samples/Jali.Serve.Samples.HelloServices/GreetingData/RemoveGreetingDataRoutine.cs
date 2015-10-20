using System.Threading.Tasks;
using System.Xml;
using Jali.Serve.Definition;
using Newtonsoft.Json.Linq;

namespace Jali.Serve.Samples.HelloServices.GreetingData
{
    public class RemoveGreetingDataRoutine : RoutineBase<JObject, GreetingDataChangeNotification, GreetingDataKey>
    {
        public const string Name = "remove-greetingdata";


        public RemoveGreetingDataRoutine(ResourceBase resource, Routine routine) : base(resource, routine)
        {
        }

        protected override async Task ExecuteProcedureCore(IExecutionContext context, RoutineProcedureContext<JObject, GreetingDataChangeNotification, GreetingDataKey> procedureContext)
        {
            throw new System.NotImplementedException();
        }
    }
}