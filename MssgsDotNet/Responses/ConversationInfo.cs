using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MssgsDotNet.Responses
{
    public class ConversationInfo : MssgsResponse
    {
        public bool ReadOnly { get; private set; }
        public bool PasswordProtected { get; private set; }
        public bool Exists { get; private set; }

        public ConversationInfo(bool passwordProtected, bool readOnly, bool exists)
        {
            this.PasswordProtected = passwordProtected;
            this.ReadOnly = readOnly;
            this.Exists = exists;
        }

    }
}
