using Jali.Serve.Definition;

namespace Jali.Serve
{
    public class RoutineProcedureContext<TRequestMessage, TResponseMessage>
    {
        public RoutineMessage RequestMessageDefinition { get; internal set; }
        public RoutineMessage ResponseMessageDefinition { get; internal set; }

        public TRequestMessage Request { get; internal set; }
        public TResponseMessage Response { get; set; }
    }
}