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
            var valid = Convert.ToBoolean(rawResponse["valid"]);
            if (valid)
            {
                return new UsernameInfo(
                    rawResponse["username"],
                    true,
                    false
                    );
            }
            else
            {
                return new UsernameInfo(
                    String.Empty,
                    false,
                    Convert.ToBoolean(rawResponse["used"])
                    );
            }
        }
    }
}
