using System;
using Jali.Serve.Definition;
using Newtonsoft.Json.Schema;

namespace Jali.Serve.Samples.RestbucksServices.ReceiptRoutines
{
    public class ReceiptResource : ResourceBase
    {
        public const string Name = "receipt";

        public ReceiptResource(ServiceBase service, IResourceContext resourceContext)
            : base(service, GetDefinition(service.Definition.Url), resourceContext)
        {
        }

        public static Resource GetDefinition(Uri serviceUrl)
        {
            if (serviceUrl == null) throw new ArgumentNullException(nameof(serviceUrl));

            var url = serviceUrl.Combine($"resources/{Name}");
            return new Resource
            {
                Name = Name,
                Version = "1.0.0",
                Url = url,
                Description = "XXXX",
                Schema = Schema,
                KeySchema = KeySchema,
                Routines =
                {
                    [GetReceiptRoutine.Name] = GetReceiptRoutine.GetDefinition(url),
                },
                Methods =
                {
                    [RestMethodVerbs.Get] = new RestMethod
                    {
                        Method = RestMethodVerbs.Get,
                        Description = "XXXX",
                        Routine = GetReceiptRoutine.Name,
                        Request = new RestMethodRequest
                        {
                            Message = new RoutineMessageReference
                            {
                                Routine = GetReceiptRoutine.Name,
                                Action = GetReceiptRequest.Action,
                            },
                            Mode = DataTransmissionModes.Full,
                        },
                        Response = new RestMethodResponse
                        {
                            Message = new RoutineMessageReference
                            {
                                Routine = GetReceiptRoutine.Name,
                                Action = GetReceiptRequest.Action,
                            },
                            Mode = DataTransmissionModes.Full,
                        }
                    }
                },
            };
        }

        public static JSchema Schema => _schema.Value;
        public static JSchema KeySchema => _keySchema.Value;

        static ReceiptResource()
        {
            _schema = new Lazy<JSchema>(() => JSchema.Parse(@"{
  ""$schema"": ""http://json-schema.org/draft-04/schema#"",

  ""title"": ""ReceiptKey"",
  ""type"": ""object"",
  ""properties"": {
    ""location"": {
        ""type"": ""string"", 
        ""description"": ""Where the order is to be consumed."",
        ""enum"": [""takeAway"", ""inShop""]
    }
  }
}"));

            _keySchema = new Lazy<JSchema>(() => JSchema.Parse(@"{
  ""$schema"": ""http://json-schema.org/draft-04/schema#"",

  ""title"": ""ReceiptKey"",
  ""type"": ""object"",
  ""properties"": {
    ""0"": {
        ""type"": ""string"", 
        ""title"": ""id"", 
        ""description"": ""Unique identifier for the order."",
        ""pattern"": ""^[0-9]*$""
    }
  }
}"));
        }

        private static readonly Lazy<JSchema> _schema;

        private static readonly Lazy<JSchema> _keySchema;
    }
}