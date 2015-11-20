using System;
using Jali.Serve.Definition;
using Jali.Serve.Server;

namespace Jali.Serve
{
    /// <summary>
    ///     Provides base functionality for a Jali Server routine.
    /// </summary>
    public abstract class ServerRoutineBase : RoutineBase
    {
        /// <summary>
        ///     Gets the initialization options of the Jali Server.
        /// </summary>
        public JaliServerOptions ServerOptions { get; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ServerRoutineBase"/> class.
        /// </summary>
        /// <param name="resource">
        ///     The parent Jali resource implementation.
        /// </param>
        /// <param name="definition">
        ///     The Jali routine definition.
        /// </param>
        /// <param name="routineContext">
        ///     The routine context.
        /// </param>
        /// <param name="serverOptions">
        ///     The initialization options of the Jali Server.
        /// </param>
        protected ServerRoutineBase(
            ResourceBase resource, Routine definition, IRoutineContext routineContext, JaliServerOptions serverOptions) 
            : base(resource, definition, routineContext)
        {
            if (serverOptions == null) throw new ArgumentNullException(nameof(serverOptions));

            ServerOptions = serverOptions;
        }
    }

    /// <summary>
    ///     Provides base functionality for a Jali Server routine.
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
    public abstract class ServerRoutineBase<TRequestData, TResponseData, TResourceKey> 
        : RoutineBase<TRequestData, TResponseData, TResourceKey>
        where TRequestData : class
        where TResponseData : class
        where TResourceKey : class
    {
        /// <summary>
        ///     Gets the initialization options of the Jali Server.
        /// </summary>
        public JaliServerOptions ServerOptions { get; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="RoutineBase{TRequestData, TResponseData, TResourceKey}"/> 
        ///     class.
        /// </summary>
        /// <param name="resource">
        ///     The parent Jali resource implementation.
        /// </param>
        /// <param name="definition">
        ///     The Jali routine definition.
        /// </param>
        /// <param name="routineContext">
        ///     The routine context.
        /// </param>
        /// <param name="serverOptions">
        ///     The initialization options of the Jali Server.
        /// </param>
        protected ServerRoutineBase(
            ResourceBase resource, Routine definition, IRoutineContext routineContext, JaliServerOptions serverOptions)
            : base(resource, definition, routineContext)
        {
            if (serverOptions == null) throw new ArgumentNullException(nameof(serverOptions));

            ServerOptions = serverOptions;
        }
    }


    /// <summary>
    ///     Provides base functionality for a Jali Server routine.
    /// </summary>
    /// <typeparam name="TRequestData">
    ///     The type of the request Jali routine inbound message object.
    /// </typeparam>
    /// <typeparam name="TResponseData">
    ///     The type of the request Jali routine outbound message object.
    /// </typeparam>
    public abstract class ServerRoutineBase<TRequestData, TResponseData>
        : RoutineBase<TRequestData, TResponseData>
        where TRequestData : class
        where TResponseData : class
    {
        /// <summary>
        ///     Gets the initialization options of the Jali Server.
        /// </summary>
        public JaliServerOptions ServerOptions { get; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="RoutineBase{TRequestData, TResponseData, TResourceKey}"/> 
        ///     class.
        /// </summary>
        /// <param name="resource">
        ///     The parent Jali resource implementation.
        /// </param>
        /// <param name="definition">
        ///     The Jali routine definition.
        /// </param>
        /// <param name="routineContext">
        ///     The routine context.
        /// </param>
        /// <param name="serverOptions">
        ///     The initialization options of the Jali Server.
        /// </param>
        protected ServerRoutineBase(
            ResourceBase resource, 
            Routine definition, 
            IRoutineContext routineContext, 
            JaliServerOptions serverOptions)
            : base(resource, definition, routineContext)
        {
            if (serverOptions == null) throw new ArgumentNullException(nameof(serverOptions));

            ServerOptions = serverOptions;
        }
    }
}