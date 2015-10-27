// ReSharper disable All
// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Net.Http.Formatting;
using System.Text;
using Jali.Serve.Decompiled;

namespace System.Web.Http.ModelBinding
{
    public static class FormDataCollectionExtensions
    {
        // This is a helper method to use Model Binding over a JQuery syntax. 
        // Normalize from JQuery to MVC keys. The model binding infrastructure uses MVC keys
        // x[] --> x
        // [] --> ""
        // x[field]  --> x.field, where field is not a number
        internal static string NormalizeJQueryToMvc(string key)
        {
            if (key == null)
            {
                return String.Empty;
            }

            StringBuilder sb = null;
            int i = 0;
            while (true)
            {
                int indexOpen = key.IndexOf('[', i);
                if (indexOpen < 0)
                {
                    // Fast path, no normalization needed.
                    // This skips the string conversion and allocating the string builder.
                    if (i == 0)
                    {
                        return key;
                    }
                    sb = sb ?? new StringBuilder();
                    sb.Append(key, i, key.Length - i);
                    break; // no more brackets
                }

                sb = sb ?? new StringBuilder();
                sb.Append(key, i, indexOpen - i); // everything up to "["

                // Find closing bracket.
                int indexClose = key.IndexOf(']', indexOpen);
                if (indexClose == -1)
                {
                    throw Error.Argument("key", SRResources.JQuerySyntaxMissingClosingBracket);
                }

                if (indexClose == indexOpen + 1)
                {
                    // Empty bracket. Signifies array. Just remove. 
                }
                else
                {
                    if (Char.IsDigit(key[indexOpen + 1]))
                    {
                        // array index. Leave unchanged. 
                        sb.Append(key, indexOpen, indexClose - indexOpen + 1);
                    }
                    else
                    {
                        // Field name.  Convert to dot notation. 
                        sb.Append('.');
                        sb.Append(key, indexOpen + 1, indexClose - indexOpen - 1);
                    }
                }

                i = indexClose + 1;
                if (i >= key.Length)
                {
                    break; // end of string
                }
            }
            return sb.ToString();
        }

        internal static IEnumerable<KeyValuePair<string, string>> GetJQueryNameValuePairs(
            this FormDataCollection formData)
        {
            if (formData == null)
            {
                throw Error.ArgumentNull("formData");
            }

            int count = 0;

            foreach (KeyValuePair<string, string> kv in formData)
            {
                ThrowIfMaxHttpCollectionKeysExceeded(count);

                string key = NormalizeJQueryToMvc(kv.Key);
                string value = kv.Value ?? String.Empty;
                yield return new KeyValuePair<string, string>(key, value);

                count++;
            }
        }

        private static void ThrowIfMaxHttpCollectionKeysExceeded(int count)
        {
            if (count >= 1000)
            {
                throw Error.InvalidOperation(SRResources.MaxHttpCollectionKeyLimitReached, 1000);
            }
        }

    }
}
