using System;
using System.Threading.Tasks;

namespace Jali
{
    /// <summary>
    ///     Provides asyncronous initialization functionality. No members of this instance should be invoked, except 
    ///     for configuration and settings, until <see cref="Initialize"/> is called.
    /// </summary>
    public abstract class AsyncInitializedBase
    {
        /// <summary>
        ///     Initialized this instance. No members of this instance should be invoked, except for configuration and
        ///     settings, until <see cref="Initialize"/> is called.
        /// </summary>
        /// <param name="context">
        ///     The execution context.
        /// </param>
        /// <returns>
        ///     A <see cref="Task"/> representing the instance initialization process.
        /// </returns>
        public async Task Initialize(IExecutionContext context)
        {
            if (this._initializeTask == null)
            {
                this._initializeTask = this.InitializeCore(context);
            }

            await this._initializeTask;
        }

        /// <summary>
        ///     Ensures that the instance initialization process has commenced and completed.
        /// </summary>
        /// <returns>
        ///     A <see cref="Task"/> representing the instance initialization process.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///     <see cref="Initialize"/> has not been called.
        /// </exception>
        protected async Task EnsureInitialized()
        {
            if (this._initializeTask == null)
            {
                var message = $"This {this.GetAsyncInitializedInstanceName()} has not been initialized.";
                throw new InvalidOperationException(message);
            }

            await this._initializeTask;
        }

        /// <summary>
        ///     When raising the <see cref="InvalidOperationException"/>, <see cref="EnsureInitialized"/> uses this 
        ///     value to communicate what instance was not initialized.
        /// </summary>
        /// <returns>
        ///     A <see cref="string"/> representing the instance that was not initialized.
        /// </returns>
        protected virtual string GetAsyncInitializedInstanceName()
        {
            return $"'{this.GetType().Name}'";
        }

        /// <summary>
        ///     Provides the initialization process of the instance.
        /// </summary>
        /// <param name="context">
        ///     The execution context.
        /// </param>
        /// <returns>
        ///     A <see cref="Task"/> representing the instance initialization process.
        /// </returns>
        protected abstract Task InitializeCore(IExecutionContext context);

        private Task _initializeTask;
    }

}