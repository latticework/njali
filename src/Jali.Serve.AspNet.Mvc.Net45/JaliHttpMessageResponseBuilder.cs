using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Jali.Notification;

namespace Jali.Serve.AspNet.Mvc
{
    public class JaliHttpMessageResponseBuilder
    {
        public JaliHttpMessageResponseBuilder(ServiceMessage message, HttpRequestMessage request)
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

        public ServiceMessage Message { get; }
        public HttpRequestMessage Request { get; }
        public HttpResponseMessage Response { get; private set; }

        private NotificationMessageCollection Messages { get; }
        private string Reason { get; set; }
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
