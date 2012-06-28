using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MssgsDotNet.Responses
{
    public class MessageList : MssgsResponse
    {
        public List<MssgsMessage> Messages { get; private set; }

        public MessageList(List<MssgsMessage> messages)
        {
            this.Messages = messages;
        }
    }
}
