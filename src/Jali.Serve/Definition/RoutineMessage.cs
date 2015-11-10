namespace Jali.Serve.Definition
{
    public class RoutineMessage
    {
        public RoutineMessage()
        {
            this.Authentication = AuthenticationRequirement.Inherited;
        }

        public string Action { get; set; }
        public MessageDirection Direction { get; set; }
        public string Description { get; set; }
        public AuthenticationRequirement Authentication { get; set; }
        public SchemaReference Schema { get; set; }
    }
}
