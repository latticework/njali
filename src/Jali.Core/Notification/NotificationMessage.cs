using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jali.Notification
{
    public class NotificationMessage : INotificationMessage
    {
        public int MessageCode
        {
            get { return _messageCode; }
            set
            {
                
            }
        }

        public MessagePriority Priority
        {
            get { return _priority; }
        }

        public MessageSeverity Severity
        {
            get { return _severity; }
        }

        public string Message
        {
            get { return _message; }
        }

        public IEnumerable<object> Args
        {
            get { return _args; }
        }

        private int _messageCode;
        private MessagePriority _priority;
        private MessageSeverity _severity;
        private string _message;
        private IEnumerable<object> _args;

        //             1
        // 0123 4567 8901 2345
        // CVAA AADD LLPS BBBB
        // |||    |  | || |
        // |||    |  | || +--- 0000 - FFFF Base Code
        // |||    |  | |+-----    0 -    F Severity
        // |||    |  | +------    0 -    F Priority
        // |||    |  +--------   00 -   FF Library
        // |||    +-----------   00 -   FF Domain
        // ||+---------------- 0000 - FFFF Authority (Registered)
        // |+-----------------    0      F Schema Version
        // +------------------    1        Schema
        private static int codeLength = 16;

        private static int[] _codeSchemaData = new[] {0, 1};
        private static int[] _codeSchemaVersionData = new[] { 1, 1 };
        private static int[] _codeAuthorityData = new[] { 2, 4 };
        private static int[] _codeDomainData = new[] { 6, 2 };
        private static int[] _codeLibraryData = new[] { 8, 2 };
        private static int[] _codePriorityData = new[] { 10, 1 };
        private static int[] _codeSeverityData = new[] { 11, 1 };
        private static int[] _codeBaseCodeData = new[] { 12, 4 };

        private static string GetCodePartCode(string code, int[] codePartData)
        {
            return code.Substring(codePartData[0], codePartData[1]);
        }

        private static int GetCodePartValue(string codePartCode)
        {
            // TODO: NotificationMessage.GetCodePartValue: Add error handling.
            return int.Parse(codePartCode);
        }

        private static int GetCodePartValueFromCode(string code, int[] codePartData)
        {
            return NotificationMessage.GetCodePartValue(NotificationMessage.GetCodePartCode(code, codePartData));
        }

    }
}
