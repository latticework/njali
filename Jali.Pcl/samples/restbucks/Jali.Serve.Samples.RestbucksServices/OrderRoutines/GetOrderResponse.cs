using Jali.Serve.Definition;

namespace Jali.Serve.Samples.RestbucksServices.OrderRoutines
{
    public class GetOrderResponse
    {
        public const string Action = GetOrderRoutine.Name + "-response";

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