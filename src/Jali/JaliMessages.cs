using Jali.Core;
using Jali.Notification;

/// <summary>
///     Notification messages defined by the Jali Core package.
/// </summary>
public static class JaliCoreMessages
{
    //private const string _authorityCode = "0000";
    //private const string _domainCode = "00";
    //private const string _libraryCode = "01";
    //private const string _messagePrefix = "1000000001";

    /// <summary>
    ///     Informs that the authenticated user is not authorized for the requested resource access.
    /// </summary>
    /// <param name="userId">
    ///     The Jali UserId of the authenticated user.
    /// </param>
    /// <param name="claimType">
    ///     The type of the required claim.
    /// </param>
    /// <param name="claimValue">
    ///     The value of the required claim.
    /// </param>
    /// <param name="objectKey">
    ///     The key of the object the message pertains to or <see langword="null"/> if the message does not refer 
    ///     to one identifiable object.
    /// </param>
    /// <param name="propertyNames">
    ///     The names of the object properties that the message pertains to or <see langword="null"/> if the 
    ///     message does not refer to a specific set of object properties.
    /// </param>
    /// <returns>
    ///     The new message.
    /// </returns>
    public static NotificationMessage CreateInternalError(
        string userId, string claimType, string claimValue, string objectKey = null, params string[] propertyNames)
    {
        var result = new NotificationMessage
        {
            MessageCode = "1000000001FF0001",
            Args = { userId, claimType, claimValue },
            Message = $"User '{userId}'is not authorized to access resource '{claimValue}' of type '{claimType}'.",
            ObjectKey = objectKey,
        };

        result.PropertyNames.AddRange(propertyNames);

        return result;
    }
}
