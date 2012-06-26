using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MssgsDotNet.Responses; 

namespace MssgsDotNet.Commands
{
    public class JoinConversationCommand : IMssgsCommand
    {
        public string Method { get; set; }

        public Dictionary<string, string> Data { get; set; }

        public JoinConversationCommand(string conversationId, string password)
        {
            this.Method = "join conversation";
            this.Data = new Dictionary<string, string>();
            this.Data["id"] = conversationId;
            this.Data["robot password"] = password;
        }
    }
}
