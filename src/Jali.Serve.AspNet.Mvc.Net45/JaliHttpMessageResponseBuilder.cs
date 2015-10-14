using System;
using System.Net;
using System.Net.Http;
using Jali.Notification;

namespace Jali.Serve.AspNet.Mvc
{
    public class JaliHttpMessageResponseBuilder
    {
        public JaliHttpMessageResponseBuilder(IServiceMessage message, HttpRequestMessage request)
        {
            this.Messages = new NotificationMessageCollection();

            this.Message = message;
            this.Request = request;
        }

        public void Build()
        {
            var messages = new NotificationMessageCollection();

            this.GetStatus();
            this.GetReason();

            this.Response = new HttpResponseMessage
            {
                RequestMessage = this.Request,
                StatusCode = this.Status,
                ReasonPhrase = this.Reason,
            };

            this.SetHeaders();
            this.SetContent();
        }

        public IServiceMessage Message { get; }
        public HttpRequestMessage Request { get; }
        public HttpResponseMessage Response { get; private set; }

        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        private NotificationMessageCollection Messages { get; }
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        private string Reason { get; set; }
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        private HttpStatusCode Status { get; set; }

        private void SetContent()
        {
            throw new NotImplementedException();
        }

        private void GetReason()
        {
            throw new NotImplementedException();
        }

        private void GetStatus()
        {
            throw new NotImplementedException();
        }

        private void SetHeaders()
        {
            throw new NotImplementedException();
        }
    }
}
