using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MssgsDotNet.Responses;

namespace MssgsDotNet.Commands
{
    public class AuthUserCommand : IMssgsCommand<UsernameInfo>
    {
        public string Method { get; set; }

        public Dictionary<string, string> Data { get; set; }

        public AuthUserCommand(string username, string avatarUrl)
        {
            this.Method = "auth";
            this.Data = new Dictionary<string, string>();
            this.Data["username"] = username;
            this.Data["avatar"] = avatarUrl;
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
