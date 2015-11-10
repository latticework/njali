using System;
using Jali.Serve.Definition;
using Jali.Serve.Server;

namespace Jali.Serve
{
    /// <summary>
    ///     Provides base functionality for a Jali Server resource.
    /// </summary>
    public abstract class ServerResourceBase : ResourceBase
    {
        /// <summary>
        ///     Gets the initialization options of the Jali Server.
        /// </summary>
        public JaliServerOptions ServerOptions { get; }

        /// <summary>
        ///     Intializes a new instance of the <see cref="ServerResourceBase"/> class.
        /// </summary>
        /// <param name="service">
        ///     The parent service.
        /// </param>
        /// <param name="definition">
        ///     The resource defintion corresponding to this resource implementation.
        /// </param>
        /// <param name="serverOptions">
        ///     The initialization options of the Jali Server.
        /// </param>
        protected ServerResourceBase(ServiceBase service, Resource definition,  JaliServerOptions serverOptions) 
            : base(service, definition)
        {
            if (serverOptions == null) throw new ArgumentNullException(nameof(serverOptions));

            this.ServerOptions = serverOptions;
        }
    }

}