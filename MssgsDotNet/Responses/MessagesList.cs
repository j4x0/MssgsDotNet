using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MssgsDotNet.Responses
{
    public class MessagesList : MssgsResponse
    {
        public List<MssgsMessage> Messages;

        public MessagesList(List<MssgsMessage> messages)
        {
            this.Messages = messages;
        }
    }
}
