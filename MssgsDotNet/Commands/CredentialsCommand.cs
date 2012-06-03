using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MssgsDotNet.Responses;

namespace MssgsDotNet.Commands
{
    public class CredentialsCommand : IMssgsCommand<CredentialsVerification>
    {
        private AppCredentials creds;

        public string Method { get; set; }

        public CredentialsCommand(AppCredentials appCreds)
        {
            this.creds = appCreds;
            this.Method = "credentials";
        }

        public CredentialsVerification CreateResponse(RawMssgsResponse rawResponse)
        {
            rawResponse.Data.AssureHas("valid");
            return new CredentialsVerification(Convert.ToBoolean(rawResponse.Data["valid"]));
        }
    }
}
