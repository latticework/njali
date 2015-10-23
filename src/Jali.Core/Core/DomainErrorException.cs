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

        public DomainErrorException()
            : this(null, null)
        {
        }

        public DomainErrorException(Exception innerException)
            : this(null, innerException)
        {
        }

        public DomainErrorException(IEnumerable<NotificationMessage> messages)
            : this(messages, null)
        {
        }

        public DomainErrorException(IEnumerable<NotificationMessage> messages, Exception innerException)
            : base(messages, innerException)
        {
        }

#if !DNX && !PCL
        protected InternalErrorException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
#endif
    }
}