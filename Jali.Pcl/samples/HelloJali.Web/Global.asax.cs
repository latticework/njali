using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Jali.Serve.Definition;
using Newtonsoft.Json.Schema;

namespace HelloJali.Web
{
    using JaliSchemaReference = Jali.Serve.Definition.SchemaReference;

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }


        public static Lazy<Service> ServiceDefinition;

        private static Service GetServiceDefinition()
        {
            return new Service
            {
                Name = "hello",
                Url = new Uri("http://hello.servcies.tempuri.org"),
                Version = "0.1.0",
                Resources =
                    {
                        ["hello"] = new Resource
                        {
                            Name = "hello",
                            Url = new Uri("resources/hello"),
                            Version = "0.1.0",
                            Schema = JSchema.Parse(@"
{
  'type': 'object', 
  'properties': {
    'id': {'type': 'string'},
    'text': {'type': 'string'}
  }
}"),
                            Events =
                            {
                                ["added-hello"] = new ResourceEvent
                                {
                                    Url = new Uri("events/added-hello"),
                                    Name = "added-hello",
                                    Schema = new JaliSchemaReference
                                    {
                                        SchemaType = SchemaType.Resource,
                                        Schema = null,
                                        Event = null,
                                    },
                                    SupportedModes = DataTransmissionModes.All,
                                }
                            },
                            Routines =
                            {
                                ["get-hello"] = new Routine
                                {
                                    Name = "get-hello",
                                    Url = new Uri("resources/get-hello"),
                                    Messages =
                                    {
                                        ["get-hello-request"] = new RoutineMessage
                                        {
                                            Action = "get-hello-request",
                                            Direction = ActionDirection.To,
                                            Schema = new JaliSchemaReference
                                            {
                                                SchemaType = SchemaType.Direct,
                                                Schema = JSchema.Parse(@"
{
  'type': 'object',
  'properties': {
    'id': {'type': 'string'}
  }
}"),
                                                Event = null,
                                            }
                                        },
                                        ["get-hello-response"] = new RoutineMessage
                                        {
                                            Action = "result",
                                            Direction = ActionDirection.From,
                                            Schema = new JaliSchemaReference
                                            {
                                                SchemaType = SchemaType.Resource,
                                                Schema = null,
                                                Event = null,
                                            }
                                        }
                                    }
                                },
                                ["new-hello"] = new Routine
                                {
                                    Name = "create-hello",
                                    Url =  new Uri("routines/new-hello"),
                                    Messages =
                                    {
                                        ["new-hello-request"] = new RoutineMessage
                                        {
                                            Action = "new-hello-request",
                                            Direction = ActionDirection.To,
                                            Schema = new JaliSchemaReference
                                            {
                                                SchemaType = SchemaType.Resource,
                                                Schema = null,
                                                Event = null,
                                            }
                                        },
                                        ["new-hello-response"] = new RoutineMessage
                                        {
                                            Action = "new-hello-response",
                                            Direction = ActionDirection.To,
                                            Schema = new JaliSchemaReference
                                            {
                                                SchemaType = SchemaType.Event,
                                                Schema = null,
                                                Event = "added-hello"
                                            }
                                        }
                                    },
                                },
                            },
                            Methods =
                            {
                                ["GET"] = new RestMethod
                                {
                                    Method = "GET",
                                    Routine = "get-hello",
                                    Request = new RestMethodRequest
                                    {
                                        Message = new RoutineMessageReference
                                        {
                                            Routine = "get-hello",
                                            Action = "get-hello-request",
                                        },
                                        Mode = DataTransmissionModes.Full,
                                    },
                                    Response = new RestMethodResponse
                                    {
                                        Message = new RoutineMessageReference
                                        {
                                            Routine = "get-hello",
                                            Action = "get-hello-response",
                                        },
                                        Mode = DataTransmissionModes.Full,
                                    },
                                },
                                ["POST"] = new RestMethod
                                {
                                    Method = "POST",
                                    Routine = "new-hello",
                                    Request = new RestMethodRequest
                                    {
                                        Message = new RoutineMessageReference
                                        {
                                            Routine = "new-hello",
                                            Action = "new-hello-request",
                                        },
                                        Mode = DataTransmissionModes.Full,
                                    },
                                    Response = new RestMethodResponse
                                    {
                                        Message = new RoutineMessageReference
                                        {
                                            Routine = "new-hello",
                                            Action = "new-hello-response",
                                        },
                                        Mode = DataTransmissionModes.Notify,
                                    },
                                },
                            },
                            Converters =
                            {

                            },
                        },
                    },
            };
        }

        static MvcApplication()
        {
            ServiceDefinition = new Lazy<Service>(GetServiceDefinition);
        }
    }
}
