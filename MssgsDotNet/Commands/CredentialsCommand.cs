using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MssgsDotNet.Commands
{
    public class CredentialsCommand : IMssgsCommand
    {
        private AppCredentials creds;

        public CredentialsCommand(AppCredentials appCreds)
        {
            this.creds = appCreds;
        }
    }
}
