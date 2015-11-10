using System;
using System.Collections.Generic;
using Jali.Note;

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

        /// <summary>
        ///     Initializes a new instance of the <see cref="InternalErrorException"/> class.
        /// </summary>
        /// <param name="messages">
        ///     A description of the internal error.
        /// </param>
        public InternalErrorException(IEnumerable<INotificationMessage> messages) : 
            base(VerifyCriticalErrors(messages))
        {
            if (this.Messages.GetSeverity() != MessageSeverity.Critical)
            {
                var message = $"'{nameof(messages)}' argument must contain critical errors.";
                throw new ArgumentException(message, nameof(messages));
            }
        }

#if !DNX && !PCL
        protected InternalErrorException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
#endif

        private const string _defaultMessage = "The application encountered an internal error.";

        private static NotificationMessageCollection VerifyCriticalErrors(IEnumerable<INotificationMessage> messages)
        {
            var collection = NotificationMessageCollection.FromEnumerable(messages);

            return collection;
        }

    }
}
