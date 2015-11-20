using System.Text.RegularExpressions;
using Jali.Core;
using Jali.Note;
using Jali.Serve;

// ReSharper disable once CheckNamespace
namespace System
{
    /// <summary>
    ///     Provides Jali-specific extensions to the <see cref="Uri"/> class.
    /// </summary>
    public static class JaliUriExtensions
    {
        /// <summary>
        ///     Parses the request for Jali Message information
        /// </summary>
        /// <param name="uri">
        ///     The request URL.
        /// </param>
        /// <param name="rootUrl">
        ///     A relative URL that represents the service root.
        /// </param>
        /// <returns>
        ///     The parse result.
        /// </returns>
        public static HttpRequestParseResult JaliParse(this Uri uri, string rootUrl = null)
        {
            if (uri == null) throw new ArgumentNullException(nameof(uri));

            rootUrl = rootUrl ?? string.Empty;

            if (rootUrl.Length > 0 && rootUrl[0] == '/')
            {
                rootUrl = rootUrl.Substring(1);
            }

            var messages = new NotificationMessageCollection();

            var path = uri.GetComponents(UriComponents.Path, UriFormat.Unescaped);

            if (!path.StartsWith(rootUrl, StringComparison.OrdinalIgnoreCase))
            {
                // TODO: JaliHttpRequestMessageExtensions.Parse: Change to Domain error.
                var message =
                    $"Route '{uri}' is not rooted with the Jali Server url, which is '{rootUrl}'. The request should not have been forwarded to it.";
                messages.Append(new InternalErrorException(message).Messages);
                return new HttpRequestParseResult(messages);
            }
            else
            {
                path = path.Substring(rootUrl.Length);

                if (path.Length > 0 && path[0] == '/')
                {
                    path = path.Substring(1);
                }
            }

            var resourceMatch = Regex.Match(
                path,
                @"^resources/(?<resourceName>[_a-zA-Z][_a-zA-Z0-9]*)(/(?<resourceKey>[_a-zA-Z0-9]+))?(/routines/(?<routineName>[_a-zA-Z][_a-zA-Z0-9]*)(?<messageAction>)[_a-zA-Z][_a-zA-Z0-9]*)?$");

            if (!resourceMatch.Success)
            {
                // TODO: JaliHttpRequestMessageExtensions.Parse: Change to Domain error.
                var message =
                    $"Jali Server cannot handle route '{uri}'. The request should not have been forwarded to it.";
                messages.Append(new InternalErrorException(message).Messages);
                return new HttpRequestParseResult(messages);
            }

            var resourceName = GetCaptureValue(resourceMatch, "resourceName");
            var resourceKey = GetCaptureValue(resourceMatch, "resourceKey");
            var routineName = GetCaptureValue(resourceMatch, "routineName");
            var messageAction = GetCaptureValue(resourceMatch, "messageAction");

            return new HttpRequestParseResult(
                method: null,
                resourceName: resourceName,
                payload: null,
                resourceKey: resourceKey,
                routineName: routineName,
                messageAction: messageAction,
                messages: messages);
        }

        private static string GetCaptureValue(Match match, string name)
        {
            var value = match.Groups[name].Value;

            return (value == string.Empty) ? null : value;
        }
    }
}
