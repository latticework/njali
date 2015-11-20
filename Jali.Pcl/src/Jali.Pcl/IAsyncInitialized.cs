using System.Threading.Tasks;

namespace Jali
{
    /// <summary>
    ///     Represents an asnycronously initialized instance. No members of this instance should be invoked, except for 
    ///     configuration and settings, until <see cref="Initialize"/> is called.
    /// </summary>
    public interface IAsyncInitialized
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
        Task Initialize(IExecutionContext context);
    }
}