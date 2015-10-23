using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using Jali.Core;
using Jali.Note;
using Newtonsoft.Json;

namespace Jali.Serve.Server
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
            this.ComputeStatus();
            this.ComputeReason();

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

        // TODO: JaliHttpMessageResponseBuilder: Implement builder messages or remove.
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        private NotificationMessageCollection Messages { get; }

        private string Reason { get; set; }

        private HttpStatusCode Status { get; set; }

        private void SetContent()
        {
            var writer = new StringWriter();

            JsonSerializer.Create().Serialize(writer, this.Message);

            this.Response.Content = new StringContent(writer.ToString(), Encoding.UTF8, "application/json");
        }

        private void ComputeReason()
        {
            // TODO: JaliHttpMessageResponseBuilder.ComputeReason: Think through all the Status types.
            // http://www.w3.org/Protocols/rfc2616/rfc2616-sec6.html
            switch (this.Status)
            {
                case HttpStatusCode.OK:
                    this.Reason = "OK";
                    break;
                case HttpStatusCode.Created:
                    this.Reason = "Created";
                    break;
                case HttpStatusCode.BadRequest:
                    this.Reason = "Bad Request";
                    break;
                case HttpStatusCode.InternalServerError:
                    this.Reason = "Internal Server Error";
                    break;
                default:
                    throw new InternalErrorException("Jail Server has set a status code that it does not support.");
            }
        }

        private void ComputeStatus()
        {
            this.Status = this.Request.Method.Method == "POST" ? HttpStatusCode.Created : HttpStatusCode.OK;

            if (!this.Message.Messages.Any())
            {
                return;
            }

            var severity = this.Message.Messages.Min(m => m.Severity);

            // TODO: JaliHttpMessageResponseBuilder.ComputeStatus: Think through all the Status types.
            // http://www.w3.org/Protocols/rfc2616/rfc2616-sec6.html
            switch (severity)
            {
                case MessageSeverity.Critical:
                    this.Status = HttpStatusCode.InternalServerError;
                    break;
                case MessageSeverity.Error:
                    this.Status = HttpStatusCode.BadRequest;
                    break;
            }
        }

        private void SetHeaders()
        {
            // TODO: JaliHttpMessageResponseBuilder.SetHeaders: Implement.
        }
    }
}
