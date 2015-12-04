using System;
using Jali.Serve.Definition;
using Newtonsoft.Json.Schema;
using SchemaReference = Jali.Serve.Definition.SchemaReference;

namespace Jali.Serve.Samples.RestbucksServices.ReceiptRoutines
{
    public class GetReceiptRequest
    {
        public const string Action = GetReceiptRoutine.Name + "-request";

        public static RoutineMessage GetDefinition()
        {
            return new RoutineMessage
            {
                Action = Action,
                Direction = MessageDirection.Inbound,
                Description = "Specifies critiera for CampaignRepairReceiptDetails.",
                Schema = new SchemaReference
                {
                    SchemaType = SchemaType.Direct,
                    Schema = Schema,
                },
            };
        }

        public static JSchema Schema => _schema.Value;

        static GetReceiptRequest()
        {
            _schema = new Lazy<JSchema>(() => JSchema.Parse(@"{
}"));
        }

        private static readonly Lazy<JSchema> _schema;
    }
}