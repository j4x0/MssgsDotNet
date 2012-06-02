using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebSocket4Net;
using MssgsDotNet.Commands;
using MssgsDotNet.Responses;
using SimpleJson;

namespace MssgsDotNet
{
    public class MssgsClient : WebSocket
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
                var response = this.ExecuteMssgsCommand(new CredentialsCommand(value));
                if (!response.Valid)
                    throw new MssgsApiException("Credentials invalid");
                this.appCreds = value;
                this.AppAuthenticated = true;
            }
        }

        public bool AppAuthenticated { get; private set; }


        public bool InConversation { get; private set; }

        public MssgsConversation Conversation { get; private set; }


        private string name;

        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                if (this.InConversation)
                    throw new Exception("Can't change name while in conversation");
                this.name = value;
            }
        }


        public delegate void MssgsDataReceivedHandler(RawMssgsResponse resp);

        public event MssgsDataReceivedHandler MssgsDataReceived;


        public MssgsClient(string mssgsApiUri, AppCredentials appCreds) : base(mssgsApiUri)
        {
            this.AppAuthenticated = false;
            this.MessageReceived += new EventHandler<MessageReceivedEventArgs>(this.HandleIncomingData);
        }

        public MssgsClient(AppCredentials appCreds) : this(MssgsClient.MSSGS_API_URI, appCreds)
        {
        }

        private void HandleIncomingData(object sender, MessageReceivedEventArgs args)
        {
            if (args.Message.IsFrikkinEmpty()) return;

            var data = (IDictionary<string, object>)SimpleJson.SimpleJson.DeserializeObject(args.Message);
            data.AssureHas("method");
            data.AssureHas("data");
            if (this.MssgsDataReceived != null)
                this.MssgsDataReceived(
                    new RawMssgsResponse
                    {
                        Method = (string)data["method"],
                        Data = (IDictionary<string, string>)data["data"]
                    }
                    );
        }

        public T ExecuteMssgsCommand<T>(IMssgsCommand<T> command) where T : MssgsResponse
        {
            return command.CreateResponse(null, null);
        }

        public void JoinConversation(MssgsConversation conversation)
        {
        }

        public void JoinConversation(MssgsConversation conversation, string name)
        {
            this.Name = name;
            this.JoinConversation(conversation);
        }
    }
}
