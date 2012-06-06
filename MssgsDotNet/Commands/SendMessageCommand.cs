using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MssgsDotNet.Commands
{
    public class SendMessageCommand : IMssgsCommand
    {
        public string Method { get; set; }

        public Dictionary<string, string> Data { get; set; }

        public SendMessageCommand(MssgsMessage message)
        {
            this.Method = "message";
            this.Data = new Dictionary<string, string>();
            this.Data["text"] = message.Message;
            this.Data["conversation"] = message.ConversationId;
        }
    }
}
