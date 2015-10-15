
using System.Collections.Generic;
using Jali.Notification;

namespace Jali.Serve
{
    public interface IServiceMessage
    {
        MessageContract Contract { get; set; }
        object Data { get; set; }
        IEnumerable<NotificationMessage> Messages { get; }
        MessageIdentity Identity { get; set; }
        MessageConnection Connection { get; set; }
        TenantIdentity Tenant { get; set; }
    }

    public interface IServiceMessage<TData>: IServiceMessage
    {
        new TData Data { get; set; }
    }
}