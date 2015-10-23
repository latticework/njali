namespace Jali.Serve.Definition
{
    /// <summary>
    ///     Represents an HTTP method specification of a <see cref="Resource"/>.
    /// </summary>
    public class RestMethod
    {
        /// <summary>
        ///     The HTTP Method.
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        ///     The method description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     The name of the Jali routine implementing the method.
        /// </summary>
        public string Routine { get; set; }

        /// <summary>
        ///     The routine inbound method specification.
        /// </summary>
        public RestMethodRequest Request { get; set; }

        /// <summary>
        ///     The routine inbound method specification for key URLs.
        /// </summary>
        public RestMethodRequest KeyRequest { get; set; }

        /// <summary>
        ///     The routine outbound method specification.
        /// </summary>
        public RestMethodResponse Response { get; set; }
    }
}
