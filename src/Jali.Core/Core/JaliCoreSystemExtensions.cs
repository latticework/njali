using Jali.Core.Decompiled.System.Web;
using Jali.Core.Decompiled.System.Web.Util;

namespace System
{
    public static class JaliCoreSystemExtensions
    {
        public static bool EqualsOrdinal(this string receiver, string other)
        {
            return string.Equals(receiver, other, StringComparison.Ordinal);
        }

        public static bool EqualsOrdinalIgnoreCase(this string receiver, string other)
        {
            return string.Equals(receiver, other, StringComparison.OrdinalIgnoreCase);
        }

        public static string GetBaseUrl(this Uri receiver)
        {
            // TODO: JaliCoreExtensions.GetBaseUrl: Use GetLeftPart(UriPartial.Authority) when available.
            return receiver.Scheme + "://" + receiver.Authority;
        }

        /// <summary>
        ///     Combines two <see cref="Uri"/>s even if they are both relative.
        /// </summary>
        /// <param name="baseUri">
        ///     Relative or absolute base uri.
        /// </param>
        /// <param name="relativeUri">
        ///     Uri to be appended.
        /// </param>
        public static Uri Combine(this Uri baseUri, string relativeUri)
        {
            return baseUri.Combine(new Uri(relativeUri, UriKind.Relative));
        }

        /// <summary>
        ///     Combines two <see cref="Uri"/>s even if they are both relative.
        /// </summary>
        /// <param name="baseUri">
        ///     Relative or absolute base uri.
        /// </param>
        /// <param name="relativeUri">
        ///     Uri to be appended.
        /// </param>
        public static Uri Combine(this Uri baseUri, Uri relativeUri)
        {
            if (baseUri == null) throw new ArgumentNullException(nameof(baseUri));
            if (relativeUri == null) throw new ArgumentNullException(nameof(relativeUri));

            // From http://stackoverflow.com/a/33651894
            var baseUrl = UrlPath.AppendSlashToPathIfNeeded(baseUri.ToString());
            var combinedUrl = VirtualPathUtility.Combine(baseUrl, relativeUri.ToString());

            return new Uri(combinedUrl, baseUri.IsAbsoluteUri ? UriKind.Absolute : UriKind.Relative);
        }
    }

}