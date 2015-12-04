using Jali.Serve.Definition;

namespace Jali.Serve.Samples.RestbucksServices.ReceiptRoutines
{
    public class GetReceiptResponse
    {
        public const string Action = GetReceiptRoutine.Name + "-response";

        public static RoutineMessage GetDefinition()
        {
            return new RoutineMessage
            {
                Action = Action,
                Direction = MessageDirection.Outbound,
                Description = "XXXX",
                Schema = new SchemaReference
                {
                    SchemaType = SchemaType.Resource,
                }
            };
        }
    }
}