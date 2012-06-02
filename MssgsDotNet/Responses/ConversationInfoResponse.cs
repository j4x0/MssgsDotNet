using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MssgsDotNet.Responses
{
    class ConversationInfoResponse : MssgsResponse
    {
        public bool ReadOnly { get; private set; }
        public bool PasswordProtected { get; private set; }

        public ConversationInfoResponse(string method, Dictionary<string, string> data)
            : base(method, data)
        {
            if (!data.ContainsKey("password"))
                throw new MssgsApiException("Data doesn't contain element with \"password\" key");
            if (!data.ContainsKey("readonly"))
                throw new MssgsApiException("Data doesn't contain element with \"readonly\" key");
            this.PasswordProtected = Convert.ToBoolean(data["password"]);
            this.ReadOnly = Convert.ToBoolean(data["readonly"]);
        }

    }
}
