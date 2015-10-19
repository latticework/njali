namespace Jali.Serve.MessageConversion
{
    /// <summary>
    ///     Options used to initialize a <see cref="CompositeServiceMessageConverter"/>.
    /// </summary>
    public class CompositeServiceMessageConverterOptions
    {
        /// <summary>
        ///     The message serializer that serializes and deserializes a <see cref="ServiceMessage{JObject}"/>.
        /// </summary>
        public IServiceMessageSerializer Serializer { get; set; }

        /// <summary>
        ///     Represents a utility that converts between an http request, http response, and a service message 
        ///     contract.
        /// </summary>
        public IMessageContractConverter ContractConverter { get; set; }

        /// <summary>
        ///     Represents a utility that converts between an http request, http response, and service message 
        ///     credentials.
        /// </summary>
        public IMessageCredentialConverter CredentialsConverter { get; set; }

        /// <summary>
        ///     Represents a utility converts between an http request, http response, and an json object representing 
        ///     the service message data.
        /// </summary>
        public IServiceMessageDataConverter DataConverter { get; set; }

        /// <summary>
        ///     Represents a utility converts between an http request, an http response, and a sequence of Jali 
        ///     notification messages.
        /// </summary>
        public INotificationMessageConverter NotificationConverter { get; set; }

        /// <summary>
        ///     Represents a utility that converts between an http request, http response, and a Jali service message 
        ///     identity.
        /// </summary>
        public IMessageIdentityConverter IdentityConverter { get; set; }

        /// <summary>
        ///     Represents a utility that converts between an http request, http response, and a service message 
        ///     connection.
        /// </summary>
        public IMessageConnectionConverter ConnectionConverter { get; set; }

        /// <summary>
        ///     Represents a utility that converts between an http request, http response, and a service message tenant.
        /// </summary>
        public IServiceMessageTenantConverter TenantConverter { get; set; }
    }
}