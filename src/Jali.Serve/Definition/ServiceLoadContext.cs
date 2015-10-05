using System;
using Jali.Notification;

namespace Jali.Serve.Definition
{
    public class ServiceLoadContext
    {
        public ServiceLoadContext(Uri url)
        {
            this.Url = url;
            this.Messsages = new NotificationMessageCollection();
        }

        public Uri Url { get; }

        public Service Service { get; private set; }

        public NotificationMessageCollection Messsages { get; }

        public void SetService(Service service)
        {
            if (this.Service != null)
            {
                throw new InvalidOperationException("The Jali service has already been loaded.");
            }

            this.Service = service;
        }
    }
}
