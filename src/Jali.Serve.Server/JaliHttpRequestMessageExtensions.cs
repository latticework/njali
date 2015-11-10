using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Jali.Serve;
using Newtonsoft.Json.Linq;
using System.Net.Http.Formatting;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http.ModelBinding;
using Jali.Core;
using Jali.Note;
using Jali.Secure;
using Jali.Serve.Definition;
using Jose;

namespace System.Net.Http
{
    /// <summary>
    ///     Provides Jali-specific extensions to the <see cref="HttpRequestMessage"/> class.
    /// </summary>
    public static class JaliHttpRequestMessageExtensions
    {
        /// <summary>
        /// Retrieves the parsed query string as a collection of key-value pairs.
        /// </summary>
        /// <param name="request">The <see cref="HttpRequestMessage"/></param>
        /// <returns>The query string as a collection of key-value pairs.</returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures",
            Justification = "NameValuePairsValueProvider takes an IEnumerable<KeyValuePair<string, string>>")]
        [SuppressMessage("ReSharper", "All")]
        public static IEnumerable<KeyValuePair<string, string>> GetQueryNameValuePairs(this HttpRequestMessage request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            Uri uri = request.RequestUri;

            // Unit tests may not always provide a Uri in the request
            if (uri == null || String.IsNullOrEmpty(uri.Query))
            {
                return Enumerable.Empty<KeyValuePair<string, string>>();
            }

            IEnumerable<KeyValuePair<string, string>> queryStringData;
#pragma warning disable 0168
            string cachedQueryString;
#pragma warning restore 0168

            FormDataCollection formData = new FormDataCollection(uri);

            // The ToArray call here avoids reparsing the query string, and avoids storing an Enumerator state
            // machine in the request state.
            queryStringData = formData.GetJQueryNameValuePairs().ToArray();
            return queryStringData;
        }

        // Outbound https://weblog.west-wind.com/posts/2013/Apr/15/WebAPI-Getting-Headers-QueryString-and-Cookie-Values
        /// <summary>
        ///     Parses the message's query string and converts it to a dictionary.
        /// </summary>
        /// <param name="request">
        ///     The http request.
        /// </param>
        /// <returns>
        ///     A dictionary of query string parameters.
        /// </returns>
        public static IDictionary<string, string> GetQueryStrings(this HttpRequestMessage request)
        {
            return request.GetQueryNameValuePairs()
                .ToDictionary(kv => kv.Key, kv => kv.Value, StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        ///     Gets the Jali Jason Web Token (JWT) from the request message.
        /// </summary>
        /// <param name="request">
        ///     The http request.
        /// </param>
        /// <returns>
        ///     the JWT token string or <c>null</c> if no Jali token exists.
        /// </returns>
        public static string GetSecurityToken(this HttpRequestMessage request)
        {
            // TODO: JaliHttpRequestMessageExtensions.GetSecurityToken: Move implementation.
            var authorization = request.Headers.Authorization;

            if (authorization == null || authorization.Scheme.EqualsOrdinalIgnoreCase("Bearer"))
            {
                return null;
            }

            var token = authorization.Parameter;

            return token;
        }

        /// <summary>
        ///     Parses the request for Jali Message information
        /// </summary>
        /// <param name="request">
        ///     The http request.
        /// </param>
        /// <returns>
        ///     The parse result.
        /// </returns>
        public static async Task<HttpRequestParseResult> JaliParse(this HttpRequestMessage request)
        {
            var messages = new NotificationMessageCollection();

            var requestMethod = request.Method.Method.ToUpperInvariant();

            string method;
            switch (requestMethod)
            {
                case RestMethodVerbs.Get:
                case RestMethodVerbs.Post:
                case RestMethodVerbs.Patch:
                case RestMethodVerbs.Delete:
                    method = requestMethod;
                    break;

                default:
                    // TODO: JaliHttpRequestMessageExtensions.JaliParse: Replace with DomainException
                    var message =
                        $"Jali Server cannot handle HTTP '{request.Method.Method}' methods for url {request.RequestUri}. Use another HTTP Method.";
                    messages.Append(new InternalErrorException(message).Messages);
                    return new HttpRequestParseResult(messages);
            }

            var uri = request.RequestUri;

            var components = uri.GetComponents(UriComponents.Path, UriFormat.Unescaped);


            var resourceMatch = Regex.Match(
                components,
                @"^resources/(?<resourceName>[_a-zA-Z][_a-zA-Z0-9]*)(/(?<resourceKey>[_a-zA-Z0-9]+))?(/routines/(?<routineName>[_a-zA-Z][_a-zA-Z0-9]*)(?<messageAction>)[_a-zA-Z][_a-zA-Z0-9]*)?$");

            if (!resourceMatch.Success)
            {
                // TODO: JaliHttpRequestMessageExtensions.Parse: Change to Domain error.
                var message =
                    $"Jali Server cannot handle route HTTP {request.Method.Method} {request.RequestUri}. The request should not have been forwarded to it.";
                messages.Append(new InternalErrorException(message).Messages);
                return new HttpRequestParseResult(messages);
            }

            var resourceName = GetCaptureValue(resourceMatch, "resourceName");
            var resourceKey = GetCaptureValue(resourceMatch, "resourceKey");
            var routineName = GetCaptureValue(resourceMatch, "routineName");
            var messageAction = GetCaptureValue(resourceMatch, "messageAction");

            // TODO: JaliHttpRequestMessageExtensions.JaliParse: Set Domain Errors if URL semantics incorrect.

            string payload;
            if (method == RestMethodVerbs.Get || method == RestMethodVerbs.Delete)
            {
                var query = request.GetQueryStrings();

                if (query.Keys.Count >= 1 || !query.Keys.Contains("json"))
                {
                    // TODO: JaliHttpRequestMessageExtensions.JaliParse: Replace with DomainException.
                    var message =
                        $"Jali Server requires that all '{method}' methods have a single query parameter named 'json'";

                    messages.Append(new InternalErrorException(message).Messages);
                    return new HttpRequestParseResult(messages);
                }

                payload = query["json"];
            }
            else
            {
                payload = await request.Content.ReadAsStringAsync();
            }

            return new HttpRequestParseResult(
                method: method,
                resourceName: resourceName,
                payload: payload,
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
