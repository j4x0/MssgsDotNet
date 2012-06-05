using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebSocket4Net;
using MssgsDotNet.Commands;
using MssgsDotNet.Responses;
using SimpleJson;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

namespace MssgsDotNet
{
    public class MssgsClient : IDisposable
    {
        public static readonly string MSSGS_API_URI = "api.mss.gs";
        public static readonly int MSSGS_API_PORT = 8101;

        public AppCredentials AppCreds { get; private set; }
        public bool IsAuthenticated { get; private set; }

        public MssgsConversation Conversation { get; private set; }
        public bool InConversation { get; private set; }

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

        private int port;
        private string host;

        private Queue<string> OutgoingData;
        private PacketBuilder currentPacket;
        private bool stop = false;

        private Thread clientThread;

        public delegate void Handler();
        public delegate void Handler<T>(T obj);
        public event Handler Authenticated;
        public event Handler Connected;
        public event Handler<WelcomeMessage> Welcomed;

        public bool IsConnected { get; private set; }
        public bool IsWelcomed { get; private set; }

        public MssgsClient(string mssgsApiHost, int mssgsApiPort, string name)
        {
            this.IsAuthenticated = false;
            this.IsConnected = false;
            this.IsWelcomed = false;
            this.factoryMethods = new Dictionary<string, Func<RawMssgsResponse, MssgsResponse>>();
            this.Name = name;
            this.OutgoingData = new Queue<string>();
            this.host = mssgsApiHost;
            this.port = mssgsApiPort;
            this.RegisterResponseFactory<WelcomeMessage>("welcome", (RawMssgsResponse response) =>
                {
                    response.Data.AssureHas("socketAPI");
                    response.Data.AssureHas("webAPI");
                    response.Data.AssureHas("credentials");
                    return new WelcomeMessage(response.Data["credentials"], response.Data["webAPI"], response.Data["socketAPI"]);
                }
            );

            this.ResponseReceived += (MssgsResponse response) =>
                {
                    if (response.Method == "welcome")
                    {
                        this.IsWelcomed = true;
                        if (this.Welcomed != null)
                            this.Welcomed((WelcomeMessage) response);
                    }
                };
        }

        public MssgsClient() : this(MssgsClient.MSSGS_API_URI, MssgsClient.MSSGS_API_PORT, String.Empty) { }
        public MssgsClient(string name) : this(MssgsClient.MSSGS_API_URI, MssgsClient.MSSGS_API_PORT, name) { }

        public void Start()
        {
            this.clientThread = new Thread(() =>
                {
                    using (TcpClient client = new TcpClient())
                    {
                        client.Connect(this.host, this.port);
                        this.IsConnected = true;
                        if (this.Connected != null)
                            this.Connected();
                        using (NetworkStream stream = client.GetStream())
                        {
                            using (StreamReader reader = new StreamReader(stream))
                            {
                                using (StreamWriter writer = new StreamWriter(stream))
                                {
                                    Thread readThread = new Thread(() =>
                                        {
                                          while(!this.stop && client.Connected)
                                          {

                                              Console.WriteLine("read");
                                              string incoming = reader.ReadLine();
                                              Console.WriteLine("block ");
                                              if (incoming != null )
                                              {

                                        
                                                  if (incoming.StartsWith("length"))
                                                      this.currentPacket = new PacketBuilder(Convert.ToInt32(incoming.Replace("length:", "").Trim()));
                                                  else if (this.currentPacket != null)
                                                      this.currentPacket.Append(incoming);

                                                  if (this.currentPacket != null && this.currentPacket.Built())
                                                  {
                                                      var data = this.currentPacket.Packet;
                                                      this.HandleIncomingData(data);
                                                      this.currentPacket = null;
                                                      this.currentPacket = null;
                                                  }
                                               }
                                          }
                                        }
                                        );
                                    readThread.Start();
                                    while (!this.stop && client.Connected)
                                    {
                                        if (this.OutgoingData.Count > 0)
                                        {
                                            var str = this.OutgoingData.Dequeue();

                                            var data = "length: " + str.Length + "\r\n" + str + "\r\n\r\n";
                                            writer.Write(data);
                                            writer.Flush();
                                            Console.WriteLine("<< " + data);
                                        }
                                    }
                                    readThread.Abort();
                                    Console.WriteLine("died");
                                }
                            }
                        }
                    }
                }
             );
            this.clientThread.Start();
        }

        public void Authenticate(AppCredentials appCreds)
        {
            if (this.IsAuthenticated) return;
            this.ExecuteCommand(new CredentialsCommand(appCreds), (CredentialsVerification ver) =>
            {
                if (!ver.Valid)
                {
                    this.IsAuthenticated = false;
                    throw new MssgsApiException("Credentials invalid");
                }
                else
                {
                    this.IsAuthenticated = true;
                    if (this.Authenticated != null)
                        this.Authenticated();
                }
            });
            this.AppCreds = appCreds;
        }

        private void HandleIncomingData(string data)
        {
            Console.WriteLine(">> " + data);
            if (data.IsFrikkinEmpty()) return;
            
            var obj = (IDictionary<string, object>)SimpleJson.SimpleJson.DeserializeObject(data);
            obj.AssureHas("method");
            obj.AssureHas("data");
            var dataDic = (IDictionary<string, object>)SimpleJson.SimpleJson.DeserializeObject(obj["data"].ToString());
            this.DispatchResponse(
                new RawMssgsResponse
                {
                    Method = (string)obj["method"],
                    Data = dataDic.ToDictionary(k => k.Key.ToString(), k => k.Value.ToString())
                }
                );
        }

        public void ExecuteCommand<T>(IMssgsCommand<T> command, Handler<T> callback) where T : MssgsResponse
        {
            if (!this.IsConnected)
                throw new Exception("Please connect first before you execute commands!");
            if (!this.IsWelcomed)
                throw new Exception("The server needs to welcome you first before you can execute commands!");
            this.UnregisterResponseFactory(command.Method);
            this.RegisterResponseFactory<T>(command.Method, command.CreateResponse);
            var jsonObject = new Dictionary<string, object>();
            jsonObject["method"] = command.Method;
            jsonObject["data"] = command.Data;
            string json = SimpleJson.SimpleJson.SerializeObject(jsonObject);
            this.OutgoingData.Enqueue(json);
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
            var response = this.factoryMethods[rawResponse.Method].Invoke(rawResponse);
            response.Method = rawResponse.Method;
            if (response == null) return;
            this.ResponseReceived(response);
        }

        public void RegisterResponseFactory<T>(string methodName, Func<RawMssgsResponse, T> factory) where T : MssgsResponse
        {
            if(this.factoryMethods.ContainsKey(methodName))
                throw new Exception("There is already a factory method registered for response \"" + methodName  + "\"");
            this.factoryMethods[methodName] = factory;
        }

        public void UnregisterResponseFactory(string methodName)
        {
            if(this.factoryMethods.ContainsKey(methodName))
                this.factoryMethods.Remove(methodName);
        }

        public void Close()
        {
            this.stop = true;
            this.IsConnected = false;
            this.IsAuthenticated = false;
            this.IsWelcomed = false;
            this.clientThread.Abort();
        }

        public void Dispose()
        {
            this.Close();
            GC.SuppressFinalize(this);
        }

        public void Join(MssgsConversation conversation)
        {
            if (this.InConversation)
                throw new Exception("Already in conversation!");
            this.ExecuteCommand(new JoinConversationCommand(conversation.Id, this.Name), (UsernameInfo info) =>
                {
                    if (info.Used)
                        throw new MssgsApiException("This username is already being used!");
                    if (!info.Valid)
                        throw new MssgsApiException("This username isn't valid!");
                    this.InConversation = true;
                    this.Conversation = conversation;
                }
            );
        }

        public void Join(MssgsConversation conversation, string name)
        {
            this.Name = name;
            this.Join(conversation);
        }
    }
}
