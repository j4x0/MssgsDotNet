using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MssgsDotNet.Responses
{
    public class UsernameInfo : MssgsResponse
    {
        public string Username { get; private set; }
        public bool Valid { get; private set; }
        public bool Used { get; private set; }

        public UsernameInfo(string username, bool valid, bool used)
        {
            this.Username = username;
            this.Valid = valid;
            this.Used = used;
        }
    }
}
