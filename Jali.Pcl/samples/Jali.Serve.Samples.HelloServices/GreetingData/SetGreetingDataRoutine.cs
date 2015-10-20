using System.Threading.Tasks;
using Jali.Serve.Definition;
using Newtonsoft.Json.Linq;

namespace Jali.Serve.Samples.HelloServices.GreetingData
{
    public class SetGreetingDataRoutine : RoutineBase<JObject, GreetingDataChangeNotification>
    {
        public const string Name = "set-greetingdata";

        public SetGreetingDataRoutine(ResourceBase resource, Routine routine) : base(resource, routine)
        {
        }

        protected override async Task ExecuteProcedureCore(
            IExecutionContext context, 
            RoutineProcedureContext<JObject, GreetingDataChangeNotification, JObject> procedureContext)
        {

            throw new System.NotImplementedException();
        }
    }
}