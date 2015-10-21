using System;

namespace Jali
{
    /// <summary>
    ///     Represents a security claim.
    /// </summary>
    public class Claim
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Claim"/> class.
        /// </summary>
        /// <param name="type">
        ///     The claim type.
        /// </param>
        /// <param name="value">
        ///     The claim value.
        /// </param>
        public Claim(string type, string value)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (value == null) throw new ArgumentNullException(nameof(value));

            Type = type;
            Value = value;
        }

        /// <summary>
        ///     The claim type.
        /// </summary>
        public string Type { get; }

        /// <summary>
        ///     The claim value.
        /// </summary>
        public string Value { get; }
    }
}
