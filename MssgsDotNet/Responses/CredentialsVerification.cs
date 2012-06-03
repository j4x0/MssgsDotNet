using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MssgsDotNet.Responses
{
    public class CredentialsVerification : MssgsResponse
    {
        public bool Valid { get; private set; }

        public CredentialsVerification(bool valid)
        {
            this.Valid = valid;
        }
    }
}
