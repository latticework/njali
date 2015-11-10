using System.Net.Http;
using System.Threading.Tasks;
using Jali.Serve.Definition;
using Newtonsoft.Json.Linq;

namespace Jali.Serve
{
    /// <summary>
    ///     Represents a Jali resource routine.
    /// </summary>
    public interface IRoutine
    {
        /// <summary>
        ///     The routine definition.
        /// </summary>
        Routine Definition { get; }

        /// <summary>
        ///     Initializes the routine.
        /// </summary>
        /// <param name="context">
        ///     The execution context.
        /// </param>
        /// <param name="routineContext">
        ///     The routine context.
        /// </param>
        /// <returns>
        ///     A <see cref="Task"/>.
        /// </returns>
        Task Init(IExecutionContext context, IRoutineContext routineContext);

        /// <summary>
        ///     Executes a Jali REST method.
        /// </summary>
        /// <param name="context">
        ///     The execution context.
        /// </param>
        /// <param name="httpRequest">
        ///     The http request of the request routine message.
        /// </param>
        /// <param name="requestAction">
        ///     The name of the routine inbound message to process.
        /// </param>
        /// <param name="responseAction">
        ///     The name of the routine outbound message to return.
        /// </param>
        /// <param name="request">
        ///     The routine inbound message content to process.
        /// </param>
        /// <param name="key">
        ///     The optional resource key to operate on.
        /// </param>
        /// <returns>
        ///     The routine outbound message content to return or an http response to send.
        /// </returns>
        Task<ExecuteProcedureResult> ExecuteProcedure(
            IExecutionContext context,
            HttpRequestMessage httpRequest,
            string requestAction, 
            string responseAction, 
            ServiceMessage<JObject> request,
            JObject key = null);

        /// <summary>
        ///     Gets the routine's authentication requirement.
        /// </summary>
        /// <param name="context">
        ///     The execution context.
        /// </param>
        /// <param name="requestAction">
        ///     The name of the routine inbound message to process.
        /// </param>
        /// <param name="key">
        ///     The optional resource key to operate on.
        /// </param>
        /// <returns>
        ///     The authentication requirement.
        /// </returns>
        AuthenticationRequirement GetAuthenticationRequirement(
            IExecutionContext context, string requestAction, JObject key = null);
    }
}