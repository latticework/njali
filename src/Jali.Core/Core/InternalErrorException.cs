using System;

#if !DNX && !PCL
using System.Runtime.Serialization;
#endif

namespace Jali.Core
{
    /// <summary>
    ///     Represents an internal application error that cannot be recovered.
    /// </summary>
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

        /// <summary>
        ///     Initializes a new instance of the <see cref="InternalErrorException"/> class.
        /// </summary>
        public InternalErrorException()
            :this(_defaultMessage, null)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="InternalErrorException"/> class.
        /// </summary>
        /// <param name="innerException">
        ///     The execption that caused the internal error.
        /// </param>
        public InternalErrorException(Exception innerException)
            : this(_defaultMessage, innerException)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="InternalErrorException"/> class.
        /// </summary>
        /// <param name="message">
        ///     A description of the internal error.
        /// </param>
        public InternalErrorException(string message)
            : this(message, null)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="InternalErrorException"/> class.
        /// </summary>
        /// <param name="message">
        ///     A description of the internal error.
        /// </param>
        /// <param name="innerException">
        ///     The execption that caused the internal error.
        /// </param>
        public InternalErrorException(string message, Exception innerException) : base(innerException)
        {
            this.Messages.Add(JaliCoreMessages.Criticals.InternalError.Create(message));
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
