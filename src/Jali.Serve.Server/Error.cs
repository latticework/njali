using System;
// ReSharper disable All

namespace Jali.Serve.Server
{
    internal static class Error
    {
        public static Exception Argument(string paramName, string message)
        {
            return new ArgumentException(message, paramName);
        }

        public static Exception ArgumentNull(string paramName)
        {
            return new ArgumentNullException(paramName);
        }

        public static Exception InvalidOperation(string format, object parameter)
        {
            return new InvalidOperationException(string.Format(format, parameter));
        }

        public static Exception ArgumentMustBeGreaterThanOrEqualTo(string paramName, object actual, object expected)
        {
            return new ArgumentOutOfRangeException(
                paramName, actual, $"Argument must be greater or equal to '{expected}'.");
        }
    }

    internal static class Resources
    {
        public const string FormUrlEncodedParseError = "Could not parse query string at position '{0}'.";
    }

    internal static class SRResources
    {
        public const string JQuerySyntaxMissingClosingBracket = "Missing closing bracket in query string.";
        public const string MaxHttpCollectionKeyLimitReached = "Your query string has more than the limit of '{0}' collection keys.";
    }

    internal enum ParserState
    {
        NeedMoreData,
        Done,
        Invalid,
        DataTooBig,
    }


}