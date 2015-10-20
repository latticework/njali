using System;
using Jali.Serve.Definition;

namespace Jali.Serve.Samples.HelloServices.GreetingData
{
    public class GreetingDataChangeNotification
    {
        public string GreetingDataId { get; set; }
        public string UserId { get; set; }
        public string ImpersonatorId { get; set; }
        public string DeputyId { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
    }
}