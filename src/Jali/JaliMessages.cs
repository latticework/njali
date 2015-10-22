using System;
using Jali.Core;
using Jali.Note;

/// <summary>
///     Notification messages defined by the Jali Core package.
/// </summary>
public static class JaliMessages
{
    //private const string _authorityCode = "0000";
    //private const string _domainCode = "00";
    //private const string _libraryCode = "01";
    //private const string _messagePrefix = "1000000001";

    /// <summary>
    ///     Provides Jali library error message codes.
    /// </summary>
    public static class Errors
    {
        /// <summary>
        ///     Informs that the requester is not an authenticated user.
        /// </summary>
        public const string AuthenticationError = "1000000001EE0001";

        /// <summary>
        ///     Informs that the authenticated user is not authorized for the requested resource access.
        /// </summary>
        public const string AuthorizationError = "1000000001EE0002";
    }

    /// <summary>
    ///     Informs that the authenticated user is not authorized for the requested resource access.
    /// </summary>
    /// <param name="resourceName">
    ///     The name of the resource being accessed.
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
    public static NotificationMessage CreateAuthenticationError(
        string resourceName = null, string objectKey = null, params string[] propertyNames)
    {
        var result = new NotificationMessage
        {
            MessageCode = Errors.AuthenticationError,
            Args = { resourceName },
            Message = 
                $"Access to requested resource{(resourceName != null ? $" '{resourceName}'" : "")} requires an " +
                $"authenticated user.",
            ObjectKey = objectKey,
        };

        result.PropertyNames.AddRange(propertyNames);

        return result;
    }

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
    public static NotificationMessage CreateAuthorizationError(
        string userId, string claimType, string claimValue, string objectKey = null, params string[] propertyNames)
    {
        if (userId == null) throw new ArgumentNullException(nameof(userId));
        if (claimType == null) throw new ArgumentNullException(nameof(claimType));
        if (claimValue == null) throw new ArgumentNullException(nameof(claimValue));

        var result = new NotificationMessage
        {
            MessageCode = Errors.AuthorizationError,
            Args = { userId, claimType, claimValue },
            Message = $"User '{userId}'is not authorized to access resource '{claimValue}' of type '{claimType}'.",
            ObjectKey = objectKey,
        };

        result.PropertyNames.AddRange(propertyNames);

        return result;
    }
}
