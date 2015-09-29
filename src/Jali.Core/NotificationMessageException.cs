using System;
#if !DNX
using System.Runtime.Serialization;
#endif

namespace Jali
{
#if !DNX
    [Serializable]
#endif
    public class NotificationMessageException : Exception
    {
        public NotificationMessageException()
        {
        }

#if !DNX
        protected NotificationMessageException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
#endif
    }
}
