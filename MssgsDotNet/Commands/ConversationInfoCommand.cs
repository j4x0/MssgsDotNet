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
            this.Data["conversation"] = conversationId;
        }

        public ConversationInfo CreateResponse(RawMssgsResponse rawResponse)
        {
            var exists = rawResponse["exists"].ToBoolean();
            if (exists)
                return new ConversationInfo
                {
                    ConversationId = rawResponse["conversation"],
                    PasswordProtected = rawResponse["password"].ToBoolean(),
                    ReadOnly = rawResponse["readonly"].ToBoolean(),
                    Exists = exists,
                    SocialAuth = rawResponse["socialauth"].ToBoolean(),
                    RobotPassword = rawResponse["robotpassword"].ToBoolean()
                };
            else
                return new ConversationInfo
                {
                    Exists = exists
                };

        }
    }
}
