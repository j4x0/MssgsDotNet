using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MssgsDotNet.Responses;

namespace MssgsDotNet.Commands
{
    public class CredentialsCommand : IMssgsCommand<CredentialsVerification>
    {
        public string Method { get; set; }

        public Dictionary<string, string> Data { get; set; }

        public CredentialsCommand(AppCredentials appCreds)
        {
            this.Method = "credentials";
            this.Data = new Dictionary<string, string>();
            this.Data["id"] = appCreds.id;
            this.Data["secret"] = appCreds.secret;
        }

        public CredentialsVerification CreateResponse(RawMssgsResponse rawResponse)
        {
            rawResponse.Data.AssureHas("valid");
            return new CredentialsVerification(Convert.ToBoolean(rawResponse.Data["valid"]));
        }
    }
}
