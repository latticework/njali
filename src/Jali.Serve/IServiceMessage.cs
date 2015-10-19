using System.Collections.Generic;
using Jali.Notification;

namespace Jali.Serve
{
    /// <summary>
    ///     Represents a Jali service message.
    /// </summary>
    public interface IServiceMessage
    {
        /// <summary>
        ///     The client contract used to define this message.
        /// </summary>
        MessageContract Contract { get; set; }

        /// <summary>
        ///     The sender's credentials.
        /// </summary>
        MessageCredentials Credentials { get; set; }

        /// <summary>
        ///     The JSON-serializable message data. <see langword="null"/> if <see cref="Messages"/> has errors.
        /// </summary>
        object Data { get; set; }

        /// <summary>
        ///     A list of <see cref="NotificationMessage"/> objects accompanying the service message.
        /// </summary>
        IEnumerable<NotificationMessage> Messages { get; set; }

        /// <summary>
        ///     A collenction of unique identifiers representing this message.
        /// </summary>
        MessageIdentity Identity { get; set; }

        /// <summary>
        ///     The data center partitions negociated between the communicating parties.
        /// </summary>
        MessageConnection Connection { get; set; }

        /// <summary>
        ///     The service tenant on whose behalf the message is being sent.
        /// </summary>
        TenantIdentity Tenant { get; set; }

        /// <summary>
        ///     Makes a deep copy of the service message. The <see cref="Data"/> is shared, however.
        /// </summary>
        /// <returns>
        ///     A copy of the service message.
        /// </returns>
        IServiceMessage Clone();
    }

    /// <summary>
    ///     Represents a typed implementation of <see cref="IServiceMessage"/>.
    /// </summary>
    /// <typeparam name="TData">
    ///     Must be serializable to JSON.
    /// </typeparam>
    public interface IServiceMessage<TData>: IServiceMessage
    {
        /// <summary>
        ///     The JSON-serializable message data. <see langword="null"/> if messages has errors.
        /// </summary>
        new TData Data { get; set; }

        /// <summary>
        ///     Makes a deep copy of the service message. The <see cref="Data"/> is shared, however.
        /// </summary>
        /// <returns>
        ///     A copy of the service message.
        /// </returns>
        new ServiceMessage<TData> Clone();
    }
}