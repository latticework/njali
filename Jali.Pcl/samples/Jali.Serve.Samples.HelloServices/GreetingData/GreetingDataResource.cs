using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Jali.Serve.Definition;
using Newtonsoft.Json.Schema;

namespace Jali.Serve.Samples.HelloServices.GreetingData
{
    public class GreetingDataResource : ResourceBase
    {
        public const string Name = "greetingdata";

        public GreetingDataResource(ServiceBase service, Resource resource) : base(service, resource)
        {
        }

        public static Resource GetDefinition(Uri helloServiceUrl)
        {
            var url = new Uri(helloServiceUrl, "resources/greetingdata/v1.0.0");
            var schema = GetSchema();
            var keySchema = GetKeySchema();

            return new Resource
            {
                Name = Name,
                Url = url,
                Version = "1.0.0",
                Description = "Greeting reference data.",
                Schema = schema,
                KeySchema = keySchema,
                Routines =
                {
                    [GetGreetingDataRoutine.Name] = GetGreetingDataRoutine.GetDefinition(url),
                },
                Methods =
                {
                    #region GET
                    [RestMethodVerbs.Get] = new RestMethod
                    {
                        Method = RestMethodVerbs.Get,
                        Description = "Get the greetingdata resource",
                        Routine = GetGreetingDataRoutine.Name,
                        Request = new RestMethodRequest
                        {
                            Message = new RoutineMessageReference
                            {
                                Routine = GetGreetingDataRoutine.Name,
                                Action = GetGreetingDataRequest.Name,
                            },
                            Mode = DataTransmissionModes.Full,
                        },
                        KeyRequest = new RestMethodRequest
                        {
                            Message = new RoutineMessageReference
                            {
                                Routine = GetGreetingDataRoutine.Name,
                                Action = "get-greetingdata-bykey",
                            },
                            Mode = DataTransmissionModes.Full,
                        },
                        Response = new RestMethodResponse
                        {
                            Message = new RoutineMessageReference
                            {
                                Routine = GetGreetingDataRoutine.Name,
                                Action = "get-greetingdata-result",
                            },
                            Mode = DataTransmissionModes.Full
                        },
                    },
                    #endregion GET
                    #region POST
                    [RestMethodVerbs.Post] = new RestMethod
                    {
                        Method = RestMethodVerbs.Get,
                        Description = "Get the greetingdata resource",
                        Routine = GetGreetingDataRoutine.Name,
                        Request = new RestMethodRequest
                        {
                            Message = new RoutineMessageReference
                            {
                                Routine = GetGreetingDataRoutine.Name,
                                Action = GetGreetingDataRequest.Name,
                            },
                            Mode = DataTransmissionModes.Full,
                        },
                        KeyRequest = new RestMethodRequest
                        {
                            Message = new RoutineMessageReference
                            {
                                Routine = GetGreetingDataRoutine.Name,
                                Action = "get-greetingdata-bykey",
                            },
                            Mode = DataTransmissionModes.Full,
                        },
                        Response = new RestMethodResponse
                        {
                            Message = new RoutineMessageReference
                            {
                                Routine = GetGreetingDataRoutine.Name,
                                Action = "get-greetingdata-result",
                            },
                            Mode = DataTransmissionModes.Full
                        }
                    },
                    #endregion POST
                }
            };
        }

        public static JSchema GetSchema()
        {
            return JSchema.Parse(@"{
  ""$schema"": ""http://json-schema.org/draft-04/schema#"",

  ""type"": ""object"",
  ""properties"": {
    ""lang"": {""type"": ""string""}
    ""greeting"": {""type"": ""string""}
    ""separator"": {""type"": ""string""}
  },
  ""required"": [ ""lang"", ""greeting"", ""separator"" ]
}");
        }

        public static JSchema GetKeySchema()
        {
            return JSchema.Parse(@"{
  ""$schema"": ""http://json-schema.org/draft-04/schema#"",

  ""type"": ""object"",
  ""properties"": {
    ""0"": {
        ""type"": ""string"", 
        ""title"": ""greetingDataId"", 
        ""description"": ""Name of requester to say hello to.""
    }
  }
}");
        }

        internal static GreetingDataChangeNotification AddGreetingData(GreetingData resource)
        {
            if (_resourceStore.ContainsKey(resource.Lang))
            {
                // TODO: GreetingDataResource.AddGreetingData: Change to DomainErrorException.
                var message = $"Resource '{Name}' already has an entry for 'lang' of '{resource.Lang}'.";
                throw new InvalidOperationException(message);
            }

            _resourceStore[resource.Lang] = resource;

            return new GreetingDataChangeNotification
            {
                GreetingDataId = resource.GreetingDataId,
            };
        }

        internal static void ChangeGreetingData(GreetingData resource)
        {
            if (!_resourceStore.ContainsKey(resource.Lang))
            {
                // TODO: GreetingDataResource.ChangeGreetingData: Change to DomainErrorException.
                var message = $"Resource '{Name}' has no entry for 'lang' of '{resource.Lang}'.";
                throw new InvalidOperationException(message);
            }

            _resourceStore[resource.Lang] = resource;
        }

        internal static void RemoveGreetingData(string id)
        {
            var lang = _resourceStore
                .Where(r => r.Value.GreetingDataId == id)
                .Select(r => r.Value.Lang)
                .SingleOrDefault();

            if (lang != null)
            {
                _resourceStore.Remove(lang);
            }
        }

        private static IDictionary<string, GreetingData> _resourceStore = new ConcurrentDictionary<string, GreetingData>
        {
            ["en"] = new GreetingData {Lang = "en", Greeting = "Hello", Separator = ","},
        };
    }
}