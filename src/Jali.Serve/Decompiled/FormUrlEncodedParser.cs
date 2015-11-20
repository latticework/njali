// ReSharper disable All
#pragma warning disable 0469
#pragma warning disable 0168
// Decompiled with JetBrains decompiler
// Type: System.Net.Http.Formatting.Parsers.FormUrlEncodedParser
// Assembly: System.Net.Http.Formatting, Version=5.2.3.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: DD5D1303-5E4D-4A2E-812D-07EC2F066876
// Assembly location: C:\git\latticework\njali\Jali.Pcl\packages\Microsoft.AspNet.WebApi.Client.5.2.3\lib\net45\System.Net.Http.Formatting.dll

using System.Collections.Generic;
using System.Text;
using System.Web.Http;
using Jali.Serve.Decompiled;

namespace System.Net.Http.Formatting.Parsers
{
    internal class FormUrlEncodedParser
    {
        private const int MinMessageSize = 1;
        private long _totalBytesConsumed;
        private long _maxMessageSize;
        private FormUrlEncodedParser.NameValueState _nameValueState;
        private ICollection<KeyValuePair<string, string>> _nameValuePairs;
        private readonly FormUrlEncodedParser.CurrentNameValuePair _currentNameValuePair;

        public FormUrlEncodedParser(ICollection<KeyValuePair<string, string>> nameValuePairs, long maxMessageSize)
        {
            if (maxMessageSize < 1L)
                throw Error.ArgumentMustBeGreaterThanOrEqualTo("maxMessageSize", (object)maxMessageSize, (object)1);
            if (nameValuePairs == null)
                throw Error.ArgumentNull("nameValuePairs");
            this._nameValuePairs = nameValuePairs;
            this._maxMessageSize = maxMessageSize;
            this._currentNameValuePair = new FormUrlEncodedParser.CurrentNameValuePair();
        }

        public ParserState ParseBuffer(byte[] buffer, int bytesReady, ref int bytesConsumed, bool isFinal)
        {
            if (buffer == null)
                throw Error.ArgumentNull("buffer");
            ParserState parseState1 = ParserState.NeedMoreData;
            if (bytesConsumed >= bytesReady)
            {
                if (isFinal)
                    parseState1 = this.CopyCurrent(parseState1);
                return parseState1;
            }
            ParserState parseState2;
            try
            {
                parseState2 = FormUrlEncodedParser.ParseNameValuePairs(buffer, bytesReady, ref bytesConsumed, ref this._nameValueState, this._maxMessageSize, ref this._totalBytesConsumed, this._currentNameValuePair, this._nameValuePairs);
                if (isFinal)
                    parseState2 = this.CopyCurrent(parseState2);
            }
            catch (Exception ex)
            {
                parseState2 = ParserState.Invalid;
            }
            return parseState2;
        }

        private static ParserState ParseNameValuePairs(byte[] buffer, int bytesReady, ref int bytesConsumed, ref FormUrlEncodedParser.NameValueState nameValueState, long maximumLength, ref long totalBytesConsumed, FormUrlEncodedParser.CurrentNameValuePair currentNameValuePair, ICollection<KeyValuePair<string, string>> nameValuePairs)
        {
            int num1 = bytesConsumed;
            ParserState parserState = ParserState.DataTooBig;
            long num2 = maximumLength <= 0L ? long.MaxValue : maximumLength - totalBytesConsumed + (long)num1;
            if ((long)bytesReady < num2)
            {
                parserState = ParserState.NeedMoreData;
                num2 = (long)bytesReady;
            }
            switch (nameValueState)
            {
                case FormUrlEncodedParser.NameValueState.Name:
                    do
                    {
                        int index = bytesConsumed;
                        while ((int)buffer[bytesConsumed] != 61 && (int)buffer[bytesConsumed] != 38)
                        {
                            if ((long)++bytesConsumed == num2)
                            {
                                string @string = Encoding.UTF8.GetString(buffer, index, bytesConsumed - index);
                                currentNameValuePair.Name.Append(@string);
                                goto label_19;
                            }
                        }
                        if (bytesConsumed > index)
                        {
                            string @string = Encoding.UTF8.GetString(buffer, index, bytesConsumed - index);
                            currentNameValuePair.Name.Append(@string);
                        }
                        if ((int)buffer[bytesConsumed] == 61)
                        {
                            nameValueState = FormUrlEncodedParser.NameValueState.Value;
                            if ((long)++bytesConsumed != num2)
                                goto case 1;
                            else
                                break;
                        }
                        else
                            currentNameValuePair.CopyNameOnlyTo(nameValuePairs);
                    }
                    while ((long)++bytesConsumed != num2);
                    break;
                case FormUrlEncodedParser.NameValueState.Value:
                    int index1 = bytesConsumed;
                    while ((int)buffer[bytesConsumed] != 38)
                    {
                        if ((long)++bytesConsumed == num2)
                        {
                            string @string = Encoding.UTF8.GetString(buffer, index1, bytesConsumed - index1);
                            currentNameValuePair.Value.Append(@string);
                            goto label_19;
                        }
                    }
                    if (bytesConsumed > index1)
                    {
                        string @string = Encoding.UTF8.GetString(buffer, index1, bytesConsumed - index1);
                        currentNameValuePair.Value.Append(@string);
                    }
                    currentNameValuePair.CopyTo(nameValuePairs);
                    nameValueState = FormUrlEncodedParser.NameValueState.Name;
                    if ((long)++bytesConsumed != num2)
                        goto case 0;
                    else
                        break;
            }
            label_19:
            totalBytesConsumed += (long)(bytesConsumed - num1);
            return parserState;
        }

        private ParserState CopyCurrent(ParserState parseState)
        {
            if (this._nameValueState == FormUrlEncodedParser.NameValueState.Name)
            {
                if (this._totalBytesConsumed > 0L)
                    this._currentNameValuePair.CopyNameOnlyTo(this._nameValuePairs);
            }
            else
                this._currentNameValuePair.CopyTo(this._nameValuePairs);
            if (parseState != ParserState.NeedMoreData)
                return parseState;
            return ParserState.Done;
        }

        private enum NameValueState
        {
            Name,
            Value,
        }

        private class CurrentNameValuePair
        {
            private readonly StringBuilder _name = new StringBuilder(128);
            private readonly StringBuilder _value = new StringBuilder(2048);
            private const int DefaultNameAllocation = 128;
            private const int DefaultValueAllocation = 2048;

            public StringBuilder Name
            {
                get
                {
                    return this._name;
                }
            }

            public StringBuilder Value
            {
                get
                {
                    return this._value;
                }
            }

            public void CopyTo(ICollection<KeyValuePair<string, string>> nameValuePairs)
            {
                string key = UriQueryUtility.UrlDecode(this._name.ToString());
                string str = UriQueryUtility.UrlDecode(this._value.ToString());
                nameValuePairs.Add(new KeyValuePair<string, string>(key, str));
                this.Clear();
            }

            public void CopyNameOnlyTo(ICollection<KeyValuePair<string, string>> nameValuePairs)
            {
                string key = UriQueryUtility.UrlDecode(this._name.ToString());
                string str = string.Empty;
                nameValuePairs.Add(new KeyValuePair<string, string>(key, str));
                this.Clear();
            }

            private void Clear()
            {
                this._name.Clear();
                this._value.Clear();
            }
        }
    }
}
