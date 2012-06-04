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
        public static void Main(string[] args)
        {
            var client = new MssgsClient();
            client.Start();
            client.Connected += () => client.Authenticate(new AppCredentials("", ""));
            while (true)
            {
                var text =  Console.ReadLine();
                if (text == "stop")
                {
                    client.Close();
                    return;
                }
            }
          
        }
        
    }
}
