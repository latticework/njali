namespace Jali.Serve.Definition
{
    /// <summary>
    ///     The type of schema specified by a <see cref="SchemaReference"/>.
    /// </summary>
    public enum SchemaType
    {
        /// <summary>No schema specified.</summary>
        None = 0,
        
        /// <summary>Specifies the resource result schema.</summary>
        Result = 1,
        
        /// <summary>Specifies the resource schema.</summary>
        Resource = 2,
        
        /// <summary>Specifies the resource key schema.</summary>
        Key = 3,

        /// <summary>Specifies the shcema of an event should be used.</summary>
        Event = 4,

        /// <summary>Specifies the schema will be supplied.</summary>
        Direct = 5,

        /// <summary>Specifies the JSON Patch schema.</summary>
        Patch = 5,
    }
}
