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
                this.ExecuteMssgsCommand(new CredentialsCommand(value), (CredentialsVerification ver) =>
                {
                    if (!ver.Valid)
                    {
                        this.AppAuthenticated = false;
                        throw new MssgsApiException("Credentials invalid");
                    }
                    else
                        this.AppAuthenticated = true;
                });
                this.appCreds = value;
            }
        }
        public bool AppAuthenticated { get; private set; }

        public bool InConversation { get; private set; }
        public MssgsConversation Conversation { get; private set; }

        private Dictionary<string, Func<RawMssgsResponse, MssgsResponse>> factoryMethods;

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

        public delegate void ResponseReceivedHandler(MssgsResponse resp);
        public event ResponseReceivedHandler ResponseReceived;

        public delegate void AsyncCallback<T>(T obj);

        public MssgsClient(string mssgsApiUri, AppCredentials appCreds, string name) : base(mssgsApiUri)
        {
            this.AppAuthenticated = false;
            this.MessageReceived += new EventHandler<MessageReceivedEventArgs>(this.HandleIncomingData);
            this.factoryMethods = new Dictionary<string, Func<RawMssgsResponse, MssgsResponse>>();
            this.Name = name;
        }

        public MssgsClient(AppCredentials appCreds) : this(MssgsClient.MSSGS_API_URI, appCreds, String.Empty) { }
        public MssgsClient(AppCredentials appCreds, string name) : this(MssgsClient.MSSGS_API_URI, appCreds, name) { } 

        private void HandleIncomingData(object sender, MessageReceivedEventArgs args)
        {
            if (args.Message.IsFrikkinEmpty()) return;

            var data = (IDictionary<string, object>)SimpleJson.SimpleJson.DeserializeObject(args.Message);
            data.AssureHas("method");
            data.AssureHas("data");
            this.DispatchResponse(
                new RawMssgsResponse
                {
                    Method = (string)data["method"],
                    Data = (IDictionary<string, string>)data["data"]
                }
                );
        }

        public void ExecuteMssgsCommand<T>(IMssgsCommand<T> command, AsyncCallback<T> callback) where T : MssgsResponse
        {
            this.UnregisterResponseFactory(command.Method);
            this.RegisterResponseFactory<T>(command.Method, command.CreateResponse);
            this.ResponseReceived += (MssgsResponse response) =>
            {
                if (command.Method != response.Method) return;
                this.UnregisterResponseFactory(command.Method);
                callback((T)response);
            };
        }

        private void DispatchResponse(RawMssgsResponse rawResponse)
        {
            if (!this.factoryMethods.ContainsKey(rawResponse.Method) || this.ResponseReceived == null)
                return;
            
            this.ResponseReceived(this.factoryMethods[rawResponse.Method].Invoke(rawResponse));
        }

        public void RegisterResponseFactory<T>(string methodName, Func<RawMssgsResponse, T> factory) where T : MssgsResponse
        {
            if(this.factoryMethods.ContainsKey(methodName))
                throw new Exception("There is already a factory method registered for response \"" + methodName  + "\"");
            this.factoryMethods[methodName] = factory;
        }

        public void UnregisterResponseFactory(string methodName)
        {
            this.factoryMethods.AssureHas(methodName);
            this.factoryMethods.Remove(methodName);
        }
    }
}
