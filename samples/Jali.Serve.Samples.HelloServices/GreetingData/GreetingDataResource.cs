using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Jali.Serve.Definition;
using Newtonsoft.Json.Schema;

namespace Jali.Serve.Samples.HelloServices.GreetingData
{
    public class GreetingDataResource : ResourceBase
    {
        public const string Name = "greetingdata";

        public GreetingDataResource(ServiceBase service, Resource definition, IResourceContext resourceContext) 
            : base(service, definition, resourceContext)
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
                    [NewGreetingDataRoutine.Name] = NewGreetingDataRoutine.GetDefinition(url),
                    [GetGreetingDataRoutine.Name] = GetGreetingDataRoutine.GetDefinition(url),
                    [SetGreetingDataRoutine.Name] = SetGreetingDataRoutine.GetDefinition(url),
                    [RemoveGreetingDataRoutine.Name] = RemoveGreetingDataRoutine.GetDefinition(url),
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
                        Method = RestMethodVerbs.Post,
                        Description = "Adds the greetingdata resource",
                        Routine = NewGreetingDataRoutine.Name,
                        Request = new RestMethodRequest
                        {
                            Message = new RoutineMessageReference
                            {
                                Routine = NewGreetingDataRoutine.Name,
                                Action = "new-greetingdata-request",
                            },
                            Mode = DataTransmissionModes.Full,
                        },
                        Response = new RestMethodResponse
                        {
                            Message = new RoutineMessageReference
                            {
                                Routine = NewGreetingDataRoutine.Name,
                                Action = "new-greetingdata-result",
                            },
                            Mode = DataTransmissionModes.Result
                        }
                    },
                    #endregion POST
                    #region PATCH
                    [RestMethodVerbs.Patch] = new RestMethod
                    {
                        Method = RestMethodVerbs.Patch,
                        Description = "Changes the greetingdata resource",
                        Routine = SetGreetingDataRoutine.Name,
                        KeyRequest = new RestMethodRequest
                        {
                            Message = new RoutineMessageReference
                            {
                                Routine = SetGreetingDataRoutine.Name,
                                Action = "set-greetingdata-bykey",
                            },
                            Mode = DataTransmissionModes.Patch,
                        },
                        Response = new RestMethodResponse
                        {
                            Message = new RoutineMessageReference
                            {
                                Routine = SetGreetingDataRoutine.Name,
                                Action = "set-greetingdata-result",
                            },
                            Mode = DataTransmissionModes.Result
                        }
                    },
                    #endregion PATCH
                    #region DELETE
                    [RestMethodVerbs.Delete] = new RestMethod
                    {
                        Method = RestMethodVerbs.Delete,
                        Description = "Removes the greetingdata resource",
                        Routine = RemoveGreetingDataRoutine.Name,
                        KeyRequest = new RestMethodRequest
                        {
                            Message = new RoutineMessageReference
                            {
                                Routine = RemoveGreetingDataRoutine.Name,
                                Action = "set-greetingdata-request-bykey",
                            },
                            Mode = DataTransmissionModes.Patch,
                        },
                        Response = new RestMethodResponse
                        {
                            Message = new RoutineMessageReference
                            {
                                Routine = RemoveGreetingDataRoutine.Name,
                                Action = "set-greetingdata-result",
                            },
                            Mode = DataTransmissionModes.Result
                        }
                    },
                    #endregion DELETE
                }
            };
        }

        public static JSchema GetSchema()
        {
            return JSchema.Parse(@"{
  ""$schema"": ""http://json-schema.org/draft-04/schema#"",

  ""type"": ""object"",
  ""properties"": {
    ""lang"": {""type"": ""string""},
    ""greeting"": {""type"": ""string""},
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
        ""title"": ""id"", 
        ""description"": ""Name of requester to say hello to."",
        ""pattern"": ""^[0-9]*$""
    }
  }
}");
        }

        // TODO: GreetingDataResource: Replace store accessors with repository.

        internal static GreetingDataChangeResult AddGreetingData(GreetingData resource)
        {
            if (_resourceStore.ContainsKey(resource.Lang))
            {
                // TODO: GreetingDataResource.AddGreetingData: Change to DomainErrorException.
                var message = $"Resource '{Name}' already has an entry for 'lang' of '{resource.Lang}'.";
                throw new InvalidOperationException(message);
            }

            var maxValue = _resourceStore.Values.Max(gd => int.Parse(gd.GreetingDataId));
            resource.GreetingDataId =  (maxValue + 1).ToString();

            _resourceStore[resource.Lang] = resource;

            return new GreetingDataChangeResult
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
            var lang = GetGreetingDataKeyById(id);

            if (lang == "en")
            {
                throw new InvalidOperationException("You cannot remove the 'en' language greeting data.");
            }

            if (lang != null)
            {
                _resourceStore.Remove(lang);
            }
        }

        protected override async Task<RoutineBase> CreateRoutine(string name, IRoutineContext routineContext)
        {
            RoutineBase routine;
            switch (name)
            {
                case NewGreetingDataRoutine.Name:
                    routine = new NewGreetingDataRoutine(this, routineContext);
                    break;
                case GetGreetingDataRoutine.Name:
                    routine = new GetGreetingDataRoutine(this, routineContext);
                    break;
                case SetGreetingDataRoutine.Name:
                    routine = new SetGreetingDataRoutine(this, routineContext);
                    break;
                case RemoveGreetingDataRoutine.Name:
                    routine = new RemoveGreetingDataRoutine(this, routineContext);
                    break;
                default:
                    // TODO: GreetingDataResource.CreateRoutine: Replace with DomainErrorException.
                    throw new ArgumentOutOfRangeException(nameof(name));
            }

            return await Task.FromResult(routine);
        }

        private static string GetGreetingDataKeyById(string id)
        {
            var lang = _resourceStore
                .Where(r => r.Value.GreetingDataId == id)
                .Select(r => r.Value.Lang)
                .SingleOrDefault();
            return lang;
        }

        private static readonly IDictionary<string, GreetingData> _resourceStore = 
            new ConcurrentDictionary<string, GreetingData>
        {
            ["en"] = new GreetingData {GreetingDataId = "1000", Lang = "en", Greeting = "Hello", Separator = ","},
        };

        public static GreetingData GetGreetingDataByKey(string id)
        {
            var lang = GetGreetingDataKeyById(id);

            return lang == null ? null : _resourceStore[lang];
        }

        public static GreetingData GetGreetingDataByLanguage(string lang)
        {
            var result = _resourceStore[lang];
            return result ?? _resourceStore[lang];
        }

        public static IEnumerable<GreetingData> GetAllGreetingData()
        {
            return _resourceStore.Values;
        }
    }
}