using System;
using System.Globalization;
using Jali.Note.Definition;

namespace Jali.Note
{
    //             1
    // 0123 4567 8901 2345
    // |||    |  | || |
    // CVAA AADD LLPS BBBB 
    // |||    |  | || |    Pos From   To   Part Name
    // |||    |  | || |    --- ----   ---- ----------------------
    // |||    |  | || +--- 12  0000 - FFFF Base Code
    // |||    |  | |+----- 11     0 -    F Severity
    // |||    |  | +------ 10     0 -    F Priority
    // |||    |  +--------  8    00 -   FF Library
    // |||    +-----------  6    00 -   FF Domain
    // ||+----------------  2  0000 - FFFF Authority (Registered)
    // |+-----------------  1     0      F Schema Version
    // +------------------  0     1        Schema

    public class MessageCode
    {
        public static int Schema = 1;
        public static int SchemaVersion = 0;
        public static int Length = 16;

        public MessageCode(string code)
        {
            // TODO: MessageCode.ctor: Validate Schema and SchemaVersion.
            this.Code = code;
        }

        public static MessageCode Build(
            int authority,
            int domain,
            int library,
            MessageSeverity severity,
            int baseCode,
            MessagePriority? priority = null)
        {
            if (priority == null)
            {
                priority = MessageCode.GetDefaultPriority(severity);
            }

            var schemaCode = MessageCode.FormatCodePart(MessageCode.Schema, CodePartData<int>.SchemaData);

            var schemaVersionCode = 
                MessageCode.FormatCodePart(MessageCode.SchemaVersion, CodePartData<int>.SchemaVersionData);

            var authorityCode = MessageCode.FormatCodePart(authority, CodePartData<int>.AuthorityData);
            var domainCode = MessageCode.FormatCodePart(domain, CodePartData<int>.DomainData);
            var libraryCode = MessageCode.FormatCodePart(library, CodePartData<int>.LibraryData);
            var priorityCode = MessageCode.FormatCodePart(priority.Value, CodePartData<MessagePriority>.PriorityData);
            var severityCode = MessageCode.FormatCodePart(severity, CodePartData<MessageSeverity>.SeverityData);
            var baseCodeCode = MessageCode.FormatCodePart(baseCode, CodePartData<int>.BaseCodeData);

            return new MessageCode(string.Concat(
                schemaCode, 
                schemaVersionCode, 
                authorityCode, domainCode, 
                libraryCode, 
                priorityCode, 
                severityCode, 
                baseCodeCode));
        }

        public static int GetSchema(string messageCode) =>
            MessageCode.ParseCodePart(messageCode, CodePartData<int>.SchemaData);

        public static int GetSchemaVersion(string messageCode) =>
            MessageCode.ParseCodePart(messageCode, CodePartData<int>.SchemaVersionData);

        public static int GetAuthority(string messageCode) =>
            MessageCode.ParseCodePart(messageCode, CodePartData<int>.AuthorityData);

        public static int GetDomain(string messageCode) =>
            MessageCode.ParseCodePart(messageCode, CodePartData<int>.DomainData);

        public static int GetLibrary(string messageCode) =>
            MessageCode.ParseCodePart(messageCode, CodePartData<int>.LibraryData);

        public static MessagePriority GetPriority(string messageCode) =>
            MessageCode.ParseCodePart(messageCode, CodePartData<MessagePriority>.PriorityData);

        public static MessageSeverity GetSeverity(string messageCode) =>
            MessageCode.ParseCodePart(messageCode, CodePartData<MessageSeverity>.SeverityData);

        public static int GetBaseCode(string messageCode) =>
            MessageCode.ParseCodePart(messageCode, CodePartData<int>.BaseCodeData);

        public static void Validate(MessageDefinition definition, string messageCode)
        {
            // TODO: MessagCode.Validate: Move to MessageDefinition and validate entire message against the definition.
            if (messageCode == null) { throw new ArgumentNullException(nameof(messageCode)); }

            if (messageCode.Length != MessageCode.Length)
            {
                throw new FormatException(
                    $"MessageCode must be of length '{MessageCode.Length}'. Yours, '{messageCode}', is of length '{messageCode.Length}'.");
            }

            long dummyResult;
            var succeeded = long.TryParse(
                messageCode, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out dummyResult);

            if (!succeeded)
            {
                throw new FormatException(
                    $"MessageCode must hexidecimal numeric string'. Yours, '{messageCode}', is not.");
            }

            var schema = MessageCode.GetSchema(messageCode);
            if (schema != MessageCode.Schema)
            {
                throw new FormatException(
                    $"Only MessageCode schema '{MessageCode.Schema}' is supported'. Yours, '{messageCode}', has a schema of '{schema}'.");
            }

            var schemaVersion = MessageCode.GetSchemaVersion(messageCode);
            if (schemaVersion != MessageCode.SchemaVersion)
            {
                throw new FormatException(
                    $"Only MessageCode schema version '{MessageCode.SchemaVersion}' is supported'. Yours, '{messageCode}', has a schema version of '{schemaVersion}'.");
            }
        }

        private static TPart ParseCodePart<TPart>(string messageCode, CodePartData<TPart> data)
        {
            var partCode = Convert.ToInt32(messageCode.Substring(data.Position, data.Length), 16);
            return data.ConvertTo(partCode);
        }

        private static string FormatCodePart<TPart>(TPart part, CodePartData<TPart> data)
        {
            return data.ConvertFrom(part).ToString("X").PadLeft(data.Length, '0');
        }

        public static MessagePriority GetDefaultPriority(MessageSeverity severity)
        {
            if (severity < MessageSeverity.Critical)
            {
                throw new ArgumentOutOfRangeException(nameof(severity));
            }

            return (severity <= MessageSeverity.Verbose)
                ? (MessagePriority) severity
                : MessagePriority.VeryLow;
        }

        public string Code { get; }

        public int Authority => MessageCode.GetAuthority(this.Code);
        public int Domain => MessageCode.GetDomain(this.Code);
        public int Library => MessageCode.GetLibrary(this.Code);
        public MessagePriority Priority => MessageCode.GetPriority(this.Code);
        public MessageSeverity Severity => MessageCode.GetSeverity(this.Code);
        public int BaseCode => MessageCode.GetBaseCode(this.Code);
    }

    internal class CodePartData<TPart>
    {
        // ReSharper disable once InconsistentNaming
        // ReSharper disable StaticMemberInGenericType
        private static readonly Func<int, int> _dummyConverter = code => code;

        public static readonly CodePartData<int> SchemaData = new CodePartData<int>
                { Position = 0, Length = 1, ConvertTo = _dummyConverter, ConvertFrom = _dummyConverter, };

                public static readonly CodePartData<int> SchemaVersionData = new CodePartData<int>
                { Position = 1, Length = 1, ConvertTo = _dummyConverter, ConvertFrom = _dummyConverter, };

                public static readonly CodePartData<int> AuthorityData = new CodePartData<int>
                { Position = 2, Length = 4, ConvertTo = _dummyConverter, ConvertFrom = _dummyConverter, };

                public static readonly CodePartData<int> DomainData = new CodePartData<int>
                { Position = 6, Length = 2, ConvertTo = _dummyConverter, ConvertFrom = _dummyConverter, };

                public static readonly CodePartData<int> LibraryData = new CodePartData<int>
                { Position = 8, Length = 2, ConvertTo = _dummyConverter, ConvertFrom = _dummyConverter, };

                public static readonly CodePartData<MessagePriority> PriorityData = new CodePartData<MessagePriority>
                {
                    Position = 10,
                    Length = 1,
                    ConvertTo = code => (MessagePriority)(15 - code),
                    ConvertFrom = priority => (int)(15 - priority),
                };

                public static readonly CodePartData<MessageSeverity> SeverityData = new CodePartData<MessageSeverity>
                {
                    Position = 11,
                    Length = 1,
                    ConvertTo = code => (MessageSeverity)(15 - code),
                    ConvertFrom = severity => (int)(15 - severity),
                };

                public static readonly CodePartData<int> BaseCodeData = new CodePartData<int>
                { Position = 12, Length = 4, ConvertTo = _dummyConverter, ConvertFrom = _dummyConverter, };
        // ReSharper restore StaticMemberInGenericType

        public int Position;
        public int Length;
        public Func<int, TPart> ConvertTo;
        public Func<TPart, int> ConvertFrom;
    }
}
