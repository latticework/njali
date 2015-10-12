using System.Threading.Tasks;

namespace Jali.Serve.Server.ServiceDescription
{
    public class GetServiceDescriptionRoutine : RoutineBase<
        ServiceMessage<GetServiceDescriptionRequest>, ServiceMessage<GetServiceDescriptionResponse>>
    {
        protected override Task ExecuteCore(IExecutionContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}