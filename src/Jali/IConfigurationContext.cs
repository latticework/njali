using System;

namespace Jali
{
    public interface IConfigurationContext
    {
        // TODO: IConfigurationContext: Add Setters.
        // TODO: IConfigurationContext: Consider getter for JSON object.

        DateTimeOffset GetDate(string key, DateTimeOffset? defaultValue = null);
        decimal GetDecimal(string key, decimal? defaultValue = null);
        TimeSpan GetDuration(string key, TimeSpan? defaultValue = null);
        int GetInteger(string key, int? defaultValue = null);
        int GetString(string key, string defaultValue = null);
        DateTimeOffset GetTimestamp(string key, DateTimeOffset? defaultValue = null);
    }
}
