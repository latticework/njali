﻿using Jali.Notification;

namespace Jali.Serve.Definition
{
    public class ServiceLoadResult
    {
        public Service Service { get; set; }
        public NotificationMessageCollection Messages { get; set; }
    }
}