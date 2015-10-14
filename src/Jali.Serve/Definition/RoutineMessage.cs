namespace Jali.Serve.Definition
{
    public class RoutineMessage
    {
        public string Action { get; set; }
        public ActionDirection Direction { get; set; }
        public string Description { get; set; }
        public SchemaReference Schema { get; set; }
    }
}
