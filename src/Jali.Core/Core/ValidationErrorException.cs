using System;
using System.Collections.Generic;
using Jali.Note;

namespace Jali.Core
{
#if !DNX && !PCL
    [Serializable]
#endif
    public class ValidationErrorException : DomainErrorException
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public ValidationErrorException()
            : this(null, null)
        {
        }

        public ValidationErrorException(Exception innerException)
            : this(null, innerException)
        {
        }

        public ValidationErrorException(IEnumerable<INotificationMessage> messages)
            : this(messages, null)
        {
        }

        public ValidationErrorException(IEnumerable<INotificationMessage> messages, Exception innerException)
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