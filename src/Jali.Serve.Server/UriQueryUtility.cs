// ReSharper disable All
// Decompiled with JetBrains decompiler
// Type: System.Web.Http.UriQueryUtility
// Assembly: System.Net.Http.Formatting, Version=5.2.3.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: DD5D1303-5E4D-4A2E-812D-07EC2F066876
// Assembly location: C:\git\latticework\njali\Jali.Pcl\packages\Microsoft.AspNet.WebApi.Client.5.2.3\lib\net45\System.Net.Http.Formatting.dll

using System.Text;
using Jali.Serve.Decompiled;

namespace System.Web.Http
{
    internal static class UriQueryUtility
    {
        public static string UrlEncode(string str)
        {
            if (str == null)
                return (string)null;
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            //return Encoding.ASCII.GetString(UriQueryUtility.UrlEncode(bytes, 0, bytes.Length, false));
            return Encoding.UTF8.GetString(UriQueryUtility.UrlEncode(bytes, 0, bytes.Length, false), 0, bytes.Length);
        }

        public static string UrlDecode(string str)
        {
            if (str == null)
                return (string)null;
            return UriQueryUtility.UrlDecodeInternal(str, Encoding.UTF8);
        }

        private static byte[] UrlEncode(byte[] bytes, int offset, int count, bool alwaysCreateNewReturnValue)
        {
            byte[] numArray = UriQueryUtility.UrlEncode(bytes, offset, count);
            if (!alwaysCreateNewReturnValue || numArray == null || numArray != bytes)
                return numArray;
            return (byte[])numArray.Clone();
        }

        private static byte[] UrlEncode(byte[] bytes, int offset, int count)
        {
            if (!UriQueryUtility.ValidateUrlEncodingParameters(bytes, offset, count))
                return (byte[])null;
            int num1 = 0;
            int num2 = 0;
            for (int index = 0; index < count; ++index)
            {
                char ch = (char)bytes[offset + index];
                if ((int)ch == 32)
                    ++num1;
                else if (!UriQueryUtility.IsUrlSafeChar(ch))
                    ++num2;
            }
            if (num1 == 0 && num2 == 0)
                return bytes;
            byte[] numArray1 = new byte[count + num2 * 2];
            int num3 = 0;
            for (int index1 = 0; index1 < count; ++index1)
            {
                byte num4 = bytes[offset + index1];
                char ch = (char)num4;
                if (UriQueryUtility.IsUrlSafeChar(ch))
                    numArray1[num3++] = num4;
                else if ((int)ch == 32)
                {
                    numArray1[num3++] = (byte)43;
                }
                else
                {
                    byte[] numArray2 = numArray1;
                    int index2 = num3;
                    int num5 = 1;
                    int num6 = index2 + num5;
                    int num7 = 37;
                    numArray2[index2] = (byte)num7;
                    byte[] numArray3 = numArray1;
                    int index3 = num6;
                    int num8 = 1;
                    int num9 = index3 + num8;
                    int num10 = (int)(byte)UriQueryUtility.IntToHex((int)num4 >> 4 & 15);
                    numArray3[index3] = (byte)num10;
                    byte[] numArray4 = numArray1;
                    int index4 = num9;
                    int num11 = 1;
                    num3 = index4 + num11;
                    int num12 = (int)(byte)UriQueryUtility.IntToHex((int)num4 & 15);
                    numArray4[index4] = (byte)num12;
                }
            }
            return numArray1;
        }

        private static string UrlDecodeInternal(string value, Encoding encoding)
        {
            if (value == null)
                return (string)null;
            int length = value.Length;
            UriQueryUtility.UrlDecoder urlDecoder = new UriQueryUtility.UrlDecoder(length, encoding);
            for (int index = 0; index < length; ++index)
            {
                char ch = value[index];
                switch (ch)
                {
                    case '+':
                        ch = ' ';
                        goto default;
                    case '%':
                        if (index < length - 2)
                        {
                            int num1 = UriQueryUtility.HexToInt(value[index + 1]);
                            int num2 = UriQueryUtility.HexToInt(value[index + 2]);
                            if (num1 >= 0 && num2 >= 0)
                            {
                                byte b = (byte)(num1 << 4 | num2);
                                index += 2;
                                urlDecoder.AddByte(b);
                                break;
                            }
                            goto default;
                        }
                        else
                            goto default;
                    default:
                        if (((int)ch & 65408) == 0)
                        {
                            urlDecoder.AddByte((byte)ch);
                            break;
                        }
                        urlDecoder.AddChar(ch);
                        break;
                }
            }
            return urlDecoder.GetString();
        }

        private static int HexToInt(char h)
        {
            if ((int)h >= 48 && (int)h <= 57)
                return (int)h - 48;
            if ((int)h >= 97 && (int)h <= 102)
                return (int)h - 97 + 10;
            if ((int)h < 65 || (int)h > 70)
                return -1;
            return (int)h - 65 + 10;
        }

        private static char IntToHex(int n)
        {
            if (n <= 9)
                return (char)(n + 48);
            return (char)(n - 10 + 97);
        }

        private static bool IsUrlSafeChar(char ch)
        {
            if ((int)ch >= 97 && (int)ch <= 122 || (int)ch >= 65 && (int)ch <= 90 || (int)ch >= 48 && (int)ch <= 57)
                return true;
            switch (ch)
            {
                case '!':
                case '(':
                case ')':
                case '*':
                case '-':
                case '.':
                case '_':
                    return true;
                default:
                    return false;
            }
        }

        private static bool ValidateUrlEncodingParameters(byte[] bytes, int offset, int count)
        {
            if (bytes == null && count == 0)
                return false;
            if (bytes == null)
                throw Error.ArgumentNull("bytes");
            if (offset < 0 || offset > bytes.Length)
                throw new ArgumentOutOfRangeException("offset");
            if (count < 0 || offset + count > bytes.Length)
                throw new ArgumentOutOfRangeException("count");
            return true;
        }

        private class UrlDecoder
        {
            private int _bufferSize;
            private int _numChars;
            private char[] _charBuffer;
            private int _numBytes;
            private byte[] _byteBuffer;
            private Encoding _encoding;

            internal UrlDecoder(int bufferSize, Encoding encoding)
            {
                this._bufferSize = bufferSize;
                this._encoding = encoding;
                this._charBuffer = new char[bufferSize];
            }

            private void FlushBytes()
            {
                if (this._numBytes <= 0)
                    return;
                this._numChars += this._encoding.GetChars(this._byteBuffer, 0, this._numBytes, this._charBuffer, this._numChars);
                this._numBytes = 0;
            }

            internal void AddChar(char ch)
            {
                if (this._numBytes > 0)
                    this.FlushBytes();
                this._charBuffer[this._numChars++] = ch;
            }

            internal void AddByte(byte b)
            {
                if (this._byteBuffer == null)
                    this._byteBuffer = new byte[this._bufferSize];
                this._byteBuffer[this._numBytes++] = b;
            }

            internal string GetString()
            {
                if (this._numBytes > 0)
                    this.FlushBytes();
                if (this._numChars > 0)
                    return new string(this._charBuffer, 0, this._numChars);
                return string.Empty;
            }
        }
    }
}
