using Newtonsoft.Json.Linq;

namespace Jali.Serve
{
    /// <summary>
    ///     Provides extension methods to the <see cref="ServiceMessage{TData}"/> class.
    /// </summary>
    public static class ServiceMessageExtensions
    {
        // TODO: ServiceMessageExtensions.ToTypedMessages Add JsonSerializationSettings parameter.
        /// <summary>
        ///     Converts from an untyped service messgae to a typed service message.
        /// </summary>
        /// <typeparam name="TData">
        ///     The service message type.
        /// </typeparam>
        /// <param name="receiver">
        ///     An untyped service message.
        /// </param>
        /// <returns>
        ///     The new typed service message.
        /// </returns>
        public static ServiceMessage<TData> ToTypedMessages<TData>(this ServiceMessage<JObject> receiver)
            where TData : class
        {
            var data = receiver.Data?.ToObject<TData>();

            var message = new ServiceMessage<TData>
            {
                Contract = receiver.Contract,
                Data = data,
                Connection = receiver.Connection,
                Identity = receiver.Identity,
                Tenant = receiver.Tenant
            };

            message.Messages.Append(receiver.Messages);

            return message;
        }
    }
}
