namespace Jali.Core
{
    /// <summary>
    ///     Represents the result of a 'TryGet...' operation.
    /// </summary>
    /// <typeparam name="TResult">
    ///     The desired result type.
    /// </typeparam>
    public class TryGetResult<TResult>
    {
        /// <summary>
        ///     Gets or sets the attempted result value.
        /// </summary>
        public TResult Value { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether the value was found.
        /// </summary>
        /// <value>
        ///     <see langword="true"/> if the value was found; otherwise, <see langword="false"/>.
        /// </value>
        public bool Found { get; set; }
    }
}