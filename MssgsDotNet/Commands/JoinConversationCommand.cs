using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MssgsDotNet.Responses; 

namespace MssgsDotNet.Commands
{
    public class JoinConversationCommand : IMssgsCommand<UsernameInfo>
    {
        public string Method { get; set; }

        public Dictionary<string, string> Data { get; set; }

        public JoinConversationCommand(string conversationId, string username)
        {
            this.Method = "auth";
            this.Data = new Dictionary<string, string>();
            if (username.IsFrikkinEmpty())
                throw new Exception("Username can't be empty!");
            this.Data["username"] = username;
            this.Data["conversationId"] = conversationId;
        }

        public UsernameInfo CreateResponse(RawMssgsResponse rawResponse)
        {
            rawResponse.Data.AssureHas("valid");
            var valid = Convert.ToBoolean(rawResponse.Data["valid"]);
            if (valid)
            {
                rawResponse.Data.AssureHas("username");
                return new UsernameInfo(
                    rawResponse.Data["username"],
                    true,
                    false
                    );
            }
            else
            {
                rawResponse.Data.AssureHas("used");
                return new UsernameInfo(
                    String.Empty,
                    false,
                    Convert.ToBoolean(rawResponse.Data["used"])
                    );
            }
        }
    }
}
