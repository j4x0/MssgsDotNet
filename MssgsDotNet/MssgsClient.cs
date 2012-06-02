using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebSocket4Net;
using MssgsDotNet.Commands;

namespace MssgsDotNet
{
    class MssgsClient : JsonWebSocket
    {
        public static readonly string MSSGS_API_URI = "ws://api.mss.gs:8101";

        private AppCredentials appCreds;

        public AppCredentials AppCreds
        {
            get
            {
                return this.appCreds;
            }
            set
            {
                var credCmd = new CredentialsCommand(value);
                this.appCreds = value;
            }
        }

        public MssgsClient(string conversationId, string mssgsApiUri, AppCredentials appCreds) : base(mssgsApiUri)
        {
        }

        public MssgsClient(string conversationId, AppCredentials appCreds) : this(conversationId, MssgsClient.MSSGS_API_URI, appCreds)
        {
        }
    }
}
