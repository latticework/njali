using System;
using Jali.Serve.Definition;
using Newtonsoft.Json.Schema;
using SchemaReference = Jali.Serve.Definition.SchemaReference;

namespace Jali.Serve.Samples.RestbucksServices.OrderRoutines
{
    public class GetOrderRequest
    {
        public const string Action = GetOrderRoutine.Name + "-request";

        public static RoutineMessage GetDefinition()
        {
            return new RoutineMessage
            {
                Action = Action,
                Direction = MessageDirection.Inbound,
                Description = "Specifies critiera for CampaignRepairOrderDetails.",
                Schema = new SchemaReference
                {
                    SchemaType = SchemaType.Direct,
                    Schema = Schema,
                },
            };
        }

        public static JSchema Schema => _schema.Value;

        static GetOrderRequest()
        {
            _schema = new Lazy<JSchema>(() => JSchema.Parse(@"{
}"));
        }

        private static readonly Lazy<JSchema> _schema;
    }
}