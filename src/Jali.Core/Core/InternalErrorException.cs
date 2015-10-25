using System;

#if !DNX && !PCL
using System.Runtime.Serialization;
#endif

namespace Jali.Core
{
#if !DNX && !PCL
    [Serializable]
#endif
    public class InternalErrorException : NotificationMessageException
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public InternalErrorException()
            :this(_defaultMessage, null)
        {
        }

        public InternalErrorException(Exception innerException)
            : this(_defaultMessage, innerException)
        {
        }

        public InternalErrorException(string message)
            : this(message, null)
        {
        }

        public InternalErrorException(string message, Exception innerException) : base(innerException)
        {
            this.Messages.Add(JaliCoreMessages.Errors.InternalError.Create(message));
        }

#if !DNX && !PCL
        protected InternalErrorException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
#endif

        private const string _defaultMessage = "The application encountered an internal error.";
    }
}
