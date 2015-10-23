using System;

namespace Jali.Serve.Definition
{
    /// <summary>
    ///     Specifies the kind of data is passed in the message.
    /// </summary>
    [Flags]
    public enum DataTransmissionModes
    {
        /// <summary>No mode has been assigned.</summary>
        None = 0,

        /// <summary>Specifies the resource result schema.</summary>
        Result = 1,

        /// <summary>Specifies that only notifcation messages will be sent.</summary>
        Notify = 2,

        /// <summary>Specifies the JSON Patch schema.</summary>
        Patch = 4,

        /// <summary>Specifies the resource schema.</summary>
        Full = 8,

        /// <summary>Specifies all transmission modes are supported.</summary>
        All = Result | Notify | Patch | Full,
    }
}
