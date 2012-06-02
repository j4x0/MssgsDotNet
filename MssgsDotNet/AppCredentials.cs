using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MssgsDotNet
{
    public struct AppCredentials
    {
        public string id;

        public string secret;

        public AppCredentials(string id, string secret)
        {
            this.id = id;
            this.secret = secret;
        }
    }
}
