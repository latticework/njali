using Jali.Note.Definition;

namespace Jali
{
    /// <summary>
    ///     Messages of the Jali.Jali.Core message library.
    /// </summary>
    public static class JaliJaliMessageLibrary
    {
        /// <summary>
        ///     The message library.
        /// </summary>
        public static MessageLibrary Library { get; }

        static JaliJaliMessageLibrary()
        {
            Library = new MessageLibrary
            {
                Name = "jali",
                Version = null,
                Url = null,
                Authority = new MessageRegistrationReference
                {
                    Type = MessageRegistryType.Authority,
                    Name = "jali",
                    Code = "00",
                },
                Domain = new MessageRegistrationReference
                {
                    Type = MessageRegistryType.Domain,
                    Name = "jali",
                    Code = "00",
                },
                Documents =
                {
                    [JaliJaliMessages.Document.Name] = JaliJaliMessages.Document,
                },
            };
        }
    }
}