using System.Collections.Generic;
using System.Threading.Tasks;
using Jali.Serve.Definition;
using Newtonsoft.Json.Linq;

namespace Jali.Serve.Samples.HelloServices.GreetingData
{
    public class NewGreetingDataRoutine : RoutineBase<GreetingData, GreetingDataChangeNotification>
    {
        public NewGreetingDataRoutine(ResourceBase resource, Routine routine) : base(resource, routine)
        {
        }

        protected override async Task ExecuteProcedureCore(
            IExecutionContext context, 
            RoutineProcedureContext<GreetingData, GreetingDataChangeNotification, JObject> 
                procedureContext)
        {
            GreetingDataResource.AddGreetingData(procedureContext.Request.Data);

            throw new System.NotImplementedException();
        }

        public GreetingDataResource GreetingDataResource => (GreetingDataResource) this.Resource;
    }
}