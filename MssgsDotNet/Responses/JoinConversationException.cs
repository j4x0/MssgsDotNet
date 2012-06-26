using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MssgsDotNet.Responses
{
    public class JoinConversationException : MssgsResponse
    {
        public string Message { get; private set; }
        public int ErrorCode { get; private set; }

        public JoinConversationException(string msg, int errorCode)
        {
            this.Message = msg;
            this.ErrorCode = errorCode;
        }

    }
}
