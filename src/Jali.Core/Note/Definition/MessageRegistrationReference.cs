namespace Jali.Note.Definition
{
    /// <summary>
    ///     References a Notifcation Message registry entry.
    /// </summary>
    public class MessageRegistrationReference
    {
        /// <summary>
        ///     Specifies the type of registry to which a <see cref="MessageRegistrationReference"/> refers.
        /// </summary>
        public MessageRegistryType Type { get; set; }

        /// <summary>
        ///     The unique name of the registration within the registry.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     The unique registration code within the registry.
        /// </summary>
        public string Code { get; set; }
    }

}