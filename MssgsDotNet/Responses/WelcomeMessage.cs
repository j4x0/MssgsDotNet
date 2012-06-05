using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MssgsDotNet.Responses
{
    public class WelcomeMessage : MssgsResponse
    {
        public string SocketApiInfo { get; private set; }
        public string WebApiInfo { get; private set; }
        public string Message { get; private set; }

        public WelcomeMessage(string message, string webApiInfo, string socketApiInfo)
        {
            this.SocketApiInfo = socketApiInfo;
            this.WebApiInfo = webApiInfo;
            this.Message = message;
        }
    }
}
