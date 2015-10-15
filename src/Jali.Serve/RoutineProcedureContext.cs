using Jali.Serve.Definition;

namespace Jali.Serve
{
    public class RoutineProcedureContext<TRequestData, TResponseData>
    {
        public RoutineMessage RequestMessageDefinition { get; internal set; }
        public RoutineMessage ResponseMessageDefinition { get; internal set; }

        public ServiceMessage<TRequestData> Request { get; internal set; }
        public ServiceMessage<TResponseData> Response { get; set; }
    }
}