namespace Jali.Serve.Definition
{
    /// <summary>
    ///     Specifies user authentication needs of a Jali message.
    /// </summary>
    public enum AuthenticationRequirement
    {
        /// <summary>Specifies that user authentication is required.</summary>
        Required = 0,

        /// <summary>Specifies that user authentication requirement is inherited from a default.</summary>
        Inherited = 1,

        /// <summary>Specifies that user authentication negotication will be attempted.</summary>
        Requested = 2,

        /// <summary>Specifies that user authentication is permitted but not requested.</summary>
        Permitted = 3,

        /// <summary>Specifies that user authentication is not processed.</summary>
        Ignored = 4,
    }

}