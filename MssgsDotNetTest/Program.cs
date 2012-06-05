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
            client.Start();
            client.Welcomed += (WelcomeMessage welcome) => {
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
            var devConversation = new MssgsConversation("dev");
            client.Join(devConversation, "abc clone.NET");
            while (true)
            {
                var text =  Console.ReadLine();
                if (text == "auth")
                {
                    client.Authenticate(creds);
                }
                if (text == "stop")
                {
                    client.Close();
                    Console.ReadLine();
                    return;
                }
            }
          
        }

        public static void Test(MssgsClient client)
        {
 
           // client.Close();
           // client.Start();
        }
        
    }
}
