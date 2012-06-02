using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MssgsDotNet.Responses
{
    public class CredentialsResponse : MssgsResponse
    {
        public bool Valid { get; private set; }

        public CredentialsResponse(string method, Dictionary<string, string> data) : base(method, data)
        {
            if (!data.ContainsKey("valid"))
                throw new MssgsApiException("Data doesn't contain element with \"valid\" key");
            this.Valid = Convert.ToBoolean(data["valid"]);
        }

    }
}
