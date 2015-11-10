using System.Net.Http;
using Jali.Serve.Definition;

namespace Jali.Serve
{
    /// <summary>
    ///     REST Method procedure exeuction context.
    /// </summary>
    /// <typeparam name="TRequestData">
    ///     The type of the request Jali routine inbound message object.
    /// </typeparam>
    /// <typeparam name="TResponseData">
    ///     The type of the request Jali routine outbound message object.
    /// </typeparam>
    /// <typeparam name="TResourceKey">
    ///     The type of the resource key.
    /// </typeparam>
    public class RoutineProcedureContext<TRequestData, TResponseData, TResourceKey>
    {
        /// <summary>
        ///     The definition of the Jali routine inbound message.
        /// </summary>
        public RoutineMessage RequestMessageDefinition { get; internal set; }

        /// <summary>
        ///     The definition of the Jali routine outbound message.
        /// </summary>
        public RoutineMessage ResponseMessageDefinition { get; internal set; }

        // TODO: RoutineProcedureContext: Determine method to convert to/from HTTP Request/Response generically.

        /// <summary>
        ///     The inbound http request to process.
        /// </summary>
        public HttpRequestMessage HttpRequest { get; set; }

        /// <summary>
        ///     The inbound message to process.
        /// </summary>
        public ServiceMessage<TRequestData> Request { get; internal set; }

        /// <summary>
        ///     The outbound message to send.
        /// </summary>
        public ServiceMessage<TResponseData> Response { get; set; }

        /// <summary>
        ///     The outbound http response to send.
        /// </summary>
        public HttpResponseMessage HttpResponse { get; set; }

        /// <summary>
        ///     The resource key that this method applies to.
        /// </summary>
        public TResourceKey Key { get; set; }
    }
}