using System;
using System.Collections.Generic;
using Jali.Note;

#if !DNX && !PCL
using System.Runtime.Serialization;
#endif

namespace Jali.Core
{
#if !DNX && !PCL
    [Serializable]
#endif
    public class DomainErrorException : NotificationMessageException
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //
        public DomainErrorException(IEnumerable<INotificationMessage> messages)
            : base(messages)
        {
            if (this.Messages == null) throw new ArgumentNullException(nameof(messages));

            if (this.Messages.GetSeverity() != MessageSeverity.Error)
            {
                var message = $"'{nameof(messages)}' argument must contain errors that are only non-critical.";

                throw new ArgumentException(message, nameof(messages));
            }
        }

        public static NotificationMessageException CreateException(IEnumerable<INotificationMessage> messages)
        {

            var collection = NotificationMessageCollection.FromEnumerable(messages);

            switch (collection.GetSeverity())
            {
                case MessageSeverity.Critical:
                    return new InternalErrorException(collection);

                case MessageSeverity.Error:
                    return new DomainErrorException(collection);

                default:
                    var message = $"'{nameof(messages)}' argument must contain errors.";
                    throw new ArgumentException(message, nameof(messages));
            }
        }

#if !DNX && !PCL
        protected DomainErrorException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
#endif
    }
}