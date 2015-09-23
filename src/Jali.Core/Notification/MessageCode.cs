using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jali.Notification
{
    public class MessageCode
    {
        public static int SchemaCode = 1;
        public static int SchemaVersion = 0;

        public MessageCode(string code)
        {
            // TODO: MessageCode.ctor: Validate SchemaCode and SchemaVersion.
            this.Code = code;
        }

        public string Code { get; private set; }


    }
}
