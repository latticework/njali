namespace Jali.Note.Definition
{
    /// <summary>
    ///     Specifies the type of registry to which a <see cref="MessageRegistrationReference"/> refers.
    /// </summary>
    public enum MessageRegistryType
    {
        /// <summary>Specifies that no value has been assinged.</summary>
        None = 0,

        /// <summary>Specifies the authority registry.</summary>
        Authority = 1,

        /// <summary>Specifies the authority's domain registry.</summary>
        Domain = 2,

        /// <summary>Specifies the domain's library registry.</summary>
        Library = 3,
    }
}