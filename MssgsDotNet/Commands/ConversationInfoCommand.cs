using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MssgsDotNet.Responses;

namespace MssgsDotNet.Commands
{
    public class ConversationInfoCommand : IMssgsCommand<ConversationInfo>
    {
        public string Method { get; set; }

        public Dictionary<string, string> Data { get; set; }

        public ConversationInfoCommand(string conversationId)
        {
            this.Method = "conversation info";
            this.Data = new Dictionary<string, string>();
            this.Data["id"] = conversationId;
        }

        public ConversationInfo CreateResponse(RawMssgsResponse rawResponse)
        {
            return new ConversationInfo(
                Convert.ToBoolean(rawResponse["password"]),
                Convert.ToBoolean(rawResponse["readonly"]),
                Convert.ToBoolean(rawResponse["password"])
            );
        }
    }
}
