using System;
using System.Collections.Generic;

namespace Jali.Notification
{
    using static MessageCode;
    public class NotificationMessage : INotificationMessage
    {
        public NotificationMessage()
        {
            this.Args = new List<object>();
            this.IdentifyingArgs = new List<int>();
            this.PropertyNames = new List<string>();
        }

        public string MessageCode
        {
            get { return this._messageCode; }
            set
            {
                Validate(value);
                this._messageCode = value;
            }
        }

        public MessagePriority Priority => GetPriority(this.MessageCode);

        public MessageSeverity Severity => GetSeverity(this.MessageCode);

        public string Message
        {
            get
            {
                if (this._message == null)
                {
                    if (this.MessageTemplate == null)
                    {
                        throw new InvalidOperationException("Notification Message string not assigned.");
                    }

                    this._message = string.Format(this.MessageTemplate, this.Args);
                }

                return this._message;
            }
            set { this._message = value; }

        }

        public string MessageTemplate { get; set; }

        public bool IdentifyingMessage { get; set; }

        public IList<object> Args { get; }

        public IList<int> IdentifyingArgs { get; }

        public string ObjectKey { get; set; }

        public IList<string> PropertyNames { get; }

        private string _messageCode;
        private string _message;
    }
}
