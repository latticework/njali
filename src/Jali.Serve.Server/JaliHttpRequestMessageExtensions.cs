using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Jali.Serve;
using Newtonsoft.Json.Linq;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web.Http.ModelBinding;

namespace System.Net.Http
{
    public static class JaliHttpRequestMessageExtensions
    {
        /// <summary>
        /// Retrieves the parsed query string as a collection of key-value pairs.
        /// </summary>
        /// <param name="request">The <see cref="HttpRequestMessage"/></param>
        /// <returns>The query string as a collection of key-value pairs.</returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "NameValuePairsValueProvider takes an IEnumerable<KeyValuePair<string, string>>")]
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
            string cachedQueryString;

            FormDataCollection formData = new FormDataCollection(uri);

            // The ToArray call here avoids reparsing the query string, and avoids storing an Enumerator state
            // machine in the request state.
            queryStringData = formData.GetJQueryNameValuePairs().ToArray();
            return queryStringData;
        }

        // Outbound https://weblog.west-wind.com/posts/2013/Apr/15/WebAPI-Getting-Headers-QueryString-and-Cookie-Values
        public static IDictionary<string, string> GetQueryStrings(this HttpRequestMessage request)
        {
            return request.GetQueryNameValuePairs()
                          .ToDictionary(kv => kv.Key, kv => kv.Value, StringComparer.OrdinalIgnoreCase);
        }

        // TODO: JaliHttpRequestMessageExtensions.AsServiceMessage: Implement.
        public static async Task<ServiceMessage<JObject>> AsServiceMessage(this HttpRequestMessage request)
        {
            var method = request.Method.Method;

            string jsonString;

            if (method == "GET" || method == "DELETE")
            {
                var query = request.GetQueryStrings();

                if (query.Keys.Count != 1 || !query.Keys.Contains("json"))
                {
                    var message =
                        $"Jali Server requires that all '{method}' methods have a single query parameter names 'json'";

                    // TODO: JaliHttpRequestMessageExtensions.AsServiceMessage: Replace with BusinessDomainException.
                    throw new Exception(message);
                }

                jsonString = query["json"];
            }
            else
            {
                jsonString = await request.Content.ReadAsStringAsync();
            }

            var json = JObject.Parse(jsonString);


            return new ServiceMessage<JObject>
            {
                Connection = new MessageConnection
                {
                    Receiver = new EndpointPartition
                    {
                        DataCenterId = null,
                        ServiceCenterId = null,
                    },
                    Sender = new EndpointPartition
                    {
                        DataCenterId = null,
                        ServiceCenterId = null,
                    }
                },
                Contract = new MessageContract
                {
                    Url = null,
                    ConsumerId = null,
                    Version = null,
                },
                Data = json,
                Identity = new MessageIdentity
                {
                    ConversationId = null,
                    MessageId = null,
                    MessageTransmissionId = null,
                    SessionId = null,
                    TransactionId = null,
                },
                Tenant = new TenantIdentity
                {
                    TenantId = null,
                    TenantOrgId = null,
                },
            };
        }
    }
}
