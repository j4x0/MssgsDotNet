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
            rawResponse.Data.AssureHas("readonly");
            rawResponse.Data.AssureHas("exists");
            rawResponse.Data.AssureHas("password");
            return new ConversationInfo(
                Convert.ToBoolean(rawResponse.Data["password"]),
                Convert.ToBoolean(rawResponse.Data["readonly"]),
                Convert.ToBoolean(rawResponse.Data["password"])
            );
        }
    }
}
