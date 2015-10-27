using Jali.Note.Definition;
using Jali.Rule;

namespace Jali.Core
{
    /// <summary>
    ///     Messages of the Jali.Jali.Core message library.
    /// </summary>
    public static class JaliCoreMessageLibrary
    {
        /// <summary>
        ///     The message library.
        /// </summary>
        public static MessageLibrary Library { get; }

        static JaliCoreMessageLibrary()
        {
            Library = new MessageLibrary
            {
                Name = "core",
                Code = "00",
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
                    [JaliCoreMessages.Document.Name] = JaliCoreMessages.Document,
                    [JaliCoreRuleMessages.Document.Name] = JaliCoreMessages.Document,
                },
            };
        }
    }
}