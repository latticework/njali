using Jali.Serve.Definition;

namespace Jali.Serve.Server.User
{
    public static class GetUserResponse
    {
        public const string Action = "get-user-response";

        public static RoutineMessage GetDefinition()
        {
            return new RoutineMessage
            {
                Action = "get-user-response",
                Direction = MessageDirection.Outbound,
                Description = "Returns the specified user.",
                Schema = new SchemaReference
                {
                    SchemaType = SchemaType.Resource,
                },
            };
        }
    }

}