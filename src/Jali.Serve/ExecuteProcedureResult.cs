using System;
using System.Net.Http;

namespace Jali.Serve
{
    /// <summary>
    ///     Represents the result of the <see cref="IRoutine.ExecuteProcedure"/> method.
    /// </summary>
    public class ExecuteProcedureResult
    {
        /// <summary>
        ///     Initializes a new <see cref="ExecuteProcedureResult"/> with a service message result.
        /// </summary>
        /// <param name="message">
        ///     The service message response.
        /// </param>
        public ExecuteProcedureResult(IServiceMessage message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));

            this.Message = message;
        }

        /// <summary>
        ///     Initializes a new <see cref="ExecuteProcedureResult"/> with a http response result.
        /// </summary>
        /// <param name="response">
        ///     The http response.
        /// </param>
        public ExecuteProcedureResult(HttpResponseMessage response)
        {
            if (response == null) throw new ArgumentNullException(nameof(response));

            this.Response = response;
        }

        /// <summary>
        ///     Gets the service message response or <c>null</c> if this is an http response result.
        /// </summary>
        public IServiceMessage Message { get; }

        /// <summary>
        ///     Gets the http response or <c>null</c> if this is a service message result.
        /// </summary>
        public HttpResponseMessage Response { get; }
    }
}