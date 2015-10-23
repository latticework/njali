// ReSharper disable All
// Decompiled with JetBrains decompiler
// Type: System.Net.Http.Formatting.FormDataCollection
// Assembly: System.Net.Http.Formatting, Version=5.2.3.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: DD5D1303-5E4D-4A2E-812D-07EC2F066876
// Assembly location: C:\git\latticework\njali\Jali.Pcl\packages\Microsoft.AspNet.WebApi.Client.5.2.3\lib\net45\System.Net.Http.Formatting.dll

using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Formatting.Parsers;
using System.Text;
using Jali.Serve.Decompiled;
using JaliResources = Jali.Serve.Decompiled.Resources;

namespace System.Net.Http.Formatting
{
    /// <summary>
    /// Represent the collection of form data.
    /// </summary>
    public class FormDataCollection : IEnumerable<KeyValuePair<string, string>>, IEnumerable
    {
        private readonly IEnumerable<KeyValuePair<string, string>> _pairs;

        /// <summary>
        /// Initializes a new instance of <see cref="T:System.Net.Http.Formatting.FormDataCollection"/> class.
        /// </summary>
        /// <param name="pairs">The pairs.</param>
        public FormDataCollection(IEnumerable<KeyValuePair<string, string>> pairs)
        {
            if (pairs == null)
                throw Error.ArgumentNull("pairs");
            this._pairs = pairs;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="T:System.Net.Http.Formatting.FormDataCollection"/> class.
        /// </summary>
        /// <param name="uri">The URI</param>
        public FormDataCollection(Uri uri)
        {
            if (uri == (Uri)null)
                throw Error.ArgumentNull("uri");
            string query = uri.Query;
            if (query != null && query.Length > 0 && (int)query[0] == 63)
                query = query.Substring(1);
            this._pairs = FormDataCollection.ParseQueryString(query);
        }

        /// <summary>
        /// Initializes a new instance of <see cref="T:System.Net.Http.Formatting.FormDataCollection"/> class.
        /// </summary>
        /// <param name="query">The query.</param>
        public FormDataCollection(string query)
        {
            this._pairs = FormDataCollection.ParseQueryString(query);
        }

        private static IEnumerable<KeyValuePair<string, string>> ParseQueryString(string query)
        {
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            if (string.IsNullOrWhiteSpace(query))
                return (IEnumerable<KeyValuePair<string, string>>)list;
            byte[] bytes = Encoding.UTF8.GetBytes(query);
            FormUrlEncodedParser urlEncodedParser = new FormUrlEncodedParser((ICollection<KeyValuePair<string, string>>)list, long.MaxValue);
            int bytesConsumed = 0;
            if (urlEncodedParser.ParseBuffer(bytes, bytes.Length, ref bytesConsumed, true) != ParserState.Done)
                throw Error.InvalidOperation(JaliResources.FormUrlEncodedParseError, (object)bytesConsumed);
            return (IEnumerable<KeyValuePair<string, string>>)list;
        }

        /// <summary>
        /// Gets an enumerable that iterates through the collection.
        /// </summary>
        /// 
        /// <returns>
        /// The enumerable that iterates through the collection.
        /// </returns>
        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return this._pairs.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this._pairs.GetEnumerator();
        }
    }
}
