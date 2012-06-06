using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MssgsDotNet;
using MssgsDotNet.Commands;
using MssgsDotNet.Responses;

namespace MssgsDotNetTest
{
    public class Program
    {
        public static AppCredentials creds = new AppCredentials(String.Empty, String.Empty);
        public static void Main(string[] args)
        {
            var client = new MssgsClient();
            client.Open();
            client.Welcomed += (WelcomeMessage welcome) =>
            {
                try
                {
                    client.Authenticate(creds);
                }
                catch (MssgsApiException e)
                {
                    Console.WriteLine(e);
                }
            };
            int dots = 0;
            int show = 0;
            while (!client.IsConnected)
            {
                if (show < 100000000)
                    show++;
                else
                {
                    show = 0;
                    string text = "Waiting for connection";
                    for (int i = 0; i < dots; i++)
                    {
                        text += ".";
                    }
                    Console.WriteLine(text);
                    if (dots > 4) dots = 0;
                    dots++;
                }
            }
            Console.WriteLine("Connected with the Mssgs API on: " + client.Host + ":" + client.Port);
            while (!client.IsWelcomed)
            {
                if (show < 100000000)
                    show++;
                else
                {
                    show = 0;
                    string text = "Waiting for welcome";
                    for (int i = 0; i < dots; i++)
                    {
                        text += ".";
                    }
                    Console.WriteLine(text);
                    if (dots > 4) dots = 0;
                    dots++;
                }
            }
            while (!client.IsAuthenticated)
            {
                if (show < 100000000)
                    show++;
                else
                {
                    show = 0;
                    string text = "Waiting for authentication";
                    for (int i = 0; i < dots; i++)
                    {
                        text += ".";
                    }
                    Console.WriteLine(text);
                    if (dots > 4) dots = 0;
                    dots++;
                }
            }
            Console.WriteLine("Authenticated!");
            Console.WriteLine("Please enter a name for the client...");
            string name = Console.ReadLine();
            Console.WriteLine("Ok, " + name + ". Please enter the id of the conversation you wish to join...");
            string conversationId = Console.ReadLine();
            if (conversationId == "debug channel")
                conversationId = "84919df97d610cf36868877051c42830";
            var conversation = new MssgsConversation(conversationId);
            bool parrot = false;
            client.MessageReceived += (MssgsMessage msg) =>
                {
                    string data = msg.Op ? "[!" : "[";
                    data += msg.Username + "] " + msg.Message;
                    data += " (" + msg.Date + ")";
                    Console.WriteLine(data);
                    if (msg.Message.ToLower().Contains("jaco") && msg.Username != client.Name && msg.New && !msg.Internal)
                    {
                        client.Send("Jaco is awesome ^_^");
                    }
                    else if (parrot && msg.Username != client.Name && msg.New && !msg.Internal)
                    {
                        client.Send(msg);
                    }

                    if (msg.Username == "abc" && !msg.Internal && msg.New)
                    {
                        if (msg.Message.StartsWith("--say "))
                        {
                            client.Send(msg.Message.Replace("--say ", ""));
                        }
                        else if (msg.Message == "--parrot" )
                        {
                            parrot = !parrot;
                            if (parrot)
                            {
                                client.Send("I'm now a parrot :-)");
                            }
                            else
                            {
                                client.Send("Stopped being a parrot");
                            } 
                        }
                    }
                };
            Console.WriteLine("Trying to join conversation: " + conversationId);
            try
            {
                client.Join(conversation, name);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            while (!client.InConversation)
            {
                if (show < 100000000)
                    show++;
                else
                {
                    show = 0;
                    string text = "Joining conversation";
                    for (int i = 0; i < dots; i++)
                    {
                        text += ".";
                    }
                    Console.WriteLine(text);
                    if (dots > 4) dots = 0;
                    dots++;
                }
            }
            Console.WriteLine("Joined!");
            client.Send("Hello!");
            while (true)
            {
                var text = Console.ReadLine();
                if (text == "/close")
                    break;
                else
                    client.Send(text);
            }
            client.Close();
            Console.WriteLine("Connection closed");
            Console.ReadLine();
        }
        
    }
}
