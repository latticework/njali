using System.Net.Http;
using System.Threading.Tasks;
using Jali.Serve.Definition;
using Newtonsoft.Json.Linq;

namespace Jali.Serve
{
    /// <summary>
    ///     A Jali resource routine implementation.
    /// </summary>
    public abstract class RoutineBase : IRoutine
    {
        /// <summary>
        ///     The routine context.
        /// </summary>
        public IRoutineContext Context { get; private set; }

        /// <summary>
        ///     The routine definition.
        /// </summary>
        public Routine Definition { get; }

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
        public virtual AuthenticationRequirement GetAuthenticationRequirement(
            IExecutionContext context, string requestAction, JObject key = null)
        {
            var message = this.Definition.Messages[requestAction];
            return (message.Authentication != AuthenticationRequirement.Inherited)
                ? message.Authentication
                : (this.Definition.DefaultAuthentication != AuthenticationRequirement.Inherited)
                    ? this.Definition.DefaultAuthentication
                    : this.Resource.Definition.DefaultAuthentication;
        }

        /// <summary>
        ///     The parent Jali resource implementation.
        /// </summary>
        public ResourceBase Resource { get; }

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
        public async Task Init(IExecutionContext context, IRoutineContext routineContext)
        {
            this.Context = routineContext;

            await InitCore(context);
        }

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
        public abstract Task<ExecuteProcedureResult> ExecuteProcedure(
            IExecutionContext context,
            HttpRequestMessage httpRequest,
            string requestAction, 
            string responseAction, 
            ServiceMessage<JObject> request,
            JObject key = null);

        /// <summary>
        ///     Initializes a new instance of the <see cref="RoutineBase"/> class.
        /// </summary>
        /// <param name="resource">
        ///     The parent Jali resource implementation.
        /// </param>
        /// <param name="routine">
        ///     The Jali routine definition.
        /// </param>
        protected RoutineBase(ResourceBase resource, Routine routine)
        {
            this.Definition = routine;
            this.Resource = resource;
        }

        /// <summary>
        ///     Overridable method for initialization.
        /// </summary>
        /// <param name="context">
        ///     The execution context.
        /// </param>
        /// <returns>
        ///     A <see cref="Task"/>.
        /// </returns>
        protected virtual async Task InitCore(IExecutionContext context)
        {
            await Task.FromResult(true);
        }
    }

    /// <summary>
    ///     A typed Jali resource routine implementation.
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
    public abstract class RoutineBase<TRequestData, TResponseData, TResourceKey> : RoutineBase
        where TRequestData : class
        where TResponseData : class
        where TResourceKey : class
    {
        public override async Task<ExecuteProcedureResult> ExecuteProcedure(
            IExecutionContext context,
            HttpRequestMessage httpRequest,
            string requestAction, 
            string responseAction, 
            ServiceMessage<JObject> request,
            JObject key = null)
        {
            var typedRequest = request.ToTypedMessages<TRequestData>();
            var typedKey = (key == null) ? null : ToTypedKey(key);

            return await this.ExecuteProcedure(
                context, httpRequest, requestAction, responseAction, typedRequest, typedKey);
        }

        private TResourceKey ToTypedKey(JObject key)
        {
            // TODO: RoutineBase.ToTypedKey: Determine whether any json serialization settings are needed.
            return (typeof(TResourceKey) == typeof(JObject))
                ? (TResourceKey)(object)key
                : key?.ToObject<TResourceKey>();
        }

        private RoutineProcedureContext<TRequestData, TResponseData, TResourceKey> CreateProcedureContext()
        {
            return new RoutineProcedureContext<TRequestData, TResponseData, TResourceKey>();
        }

        /// <summary>
        ///     Implementation for a procedure. This implementation calls <see cref="ExecuteProcedureCore"/>.
        /// </summary>
        /// <param name="context">
        ///     The execution context.
        /// </param>
        /// <param name="httpRequest">
        ///     The http request of the request routine message.
        /// </param>
        /// <param name="requestAction">
        ///     The action name of the request routine message.
        /// </param>
        /// <param name="responseAction">
        ///     The action name of the response routine message.
        /// </param>
        /// <param name="request">
        ///     The service request message of the request routine message.
        /// </param>
        /// <param name="key">
        ///     The optional resource key.
        /// </param>
        /// <returns>
        ///     A <see cref="Task"/> that will result in a <see cref="ExecuteProcedureResult"/>.
        /// </returns>
        private async Task<ExecuteProcedureResult> ExecuteProcedure(
            IExecutionContext context, 
            HttpRequestMessage httpRequest,
            string requestAction, 
            string responseAction, 
            ServiceMessage<TRequestData> request,
            TResourceKey key = null)
        {
            var procedureContext = this.CreateProcedureContext();

            procedureContext.RequestMessageDefinition = this.Definition.Messages[requestAction];
            procedureContext.ResponseMessageDefinition = this.Definition.Messages[responseAction];
            procedureContext.Key = key;
            procedureContext.Request = request;
            procedureContext.HttpRequest = httpRequest;

            await this.ExecuteProcedureCore(context, procedureContext);

            // TODO: RoutineBase.ExecuteProcedure: Throw Domain Error if not Response set xor HttpResponse set.

            if (procedureContext.Response != null)
            {
                return new ExecuteProcedureResult(procedureContext.Response);
            }

            return new ExecuteProcedureResult(procedureContext.HttpResponse);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="RoutineBase{TRequestData, TResponseData, TResourceKey}"/> 
        ///     class.
        /// </summary>
        /// <param name="resource">
        ///     The parent Jali resource implementation.
        /// </param>
        /// <param name="routine">
        ///     The Jali routine definition.
        /// </param>
        protected RoutineBase(ResourceBase resource, Routine routine) : base(resource, routine)
        {
            
        }

        /// <summary>
        ///     Implementation for the procedure.
        /// </summary>
        /// <param name="context">
        ///     The execution context.
        /// </param>
        /// <param name="procedureContext">
        ///     The procedure context.
        /// </param>
        /// <returns>
        ///     A <see cref="Task"/>.
        /// </returns>
        protected abstract Task ExecuteProcedureCore(
            IExecutionContext context, 
            RoutineProcedureContext<TRequestData, TResponseData, TResourceKey> procedureContext);
    }

    /// <summary>
    ///     A typed Jali resource routine implementation for routines that do not use the resource key
    /// </summary>
    /// <typeparam name="TRequestData">
    ///     The type of the request Jali routine inbound message object.
    /// </typeparam>
    /// <typeparam name="TResponseData">
    ///     The type of the request Jali routine outbound message object.
    /// </typeparam>
    public abstract class RoutineBase<TRequestData, TResponseData> : RoutineBase<TRequestData, TResponseData, JObject>
        where TRequestData : class
        where TResponseData : class
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RoutineBase{TRequestData, TResponseData, TResourceKey}"/> 
        ///     class.
        /// </summary>
        /// <param name="resource">
        ///     The parent Jali resource implementation.
        /// </param>
        /// <param name="routine">
        ///     The Jali routine definition.
        /// </param>
        protected RoutineBase(ResourceBase resource, Routine routine) : base(resource, routine)
        {
        }
    }
}