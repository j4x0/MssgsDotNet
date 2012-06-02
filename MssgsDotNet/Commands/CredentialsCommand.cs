using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MssgsDotNet.Responses;

namespace MssgsDotNet.Commands
{
    public class CredentialsCommand : IMssgsCommand<CredentialsResponse>
    {
        private AppCredentials creds;

        public string Method { get; set; }

        public CredentialsCommand(AppCredentials appCreds)
        {
            this.creds = appCreds;
            this.Method = "credentials";
        }

        public CredentialsResponse CreateResponse(string method, Dictionary<string, string> data)
        {
            return new CredentialsResponse(method, data);
        }
    }
}
