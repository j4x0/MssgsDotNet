using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MssgsDotNet.Responses
{
    public class ConversationAction : MssgsResponse
    {
        public IDictionary<string,string> Data { get; private set; }
        public ActionType Type { get; private set; }

        public enum ActionType
        {
            UserJoined,
            UserLeft,
            Removed,
            UserChangedName,
            ConversationSecured,
            ConversationUnsecured,
            UserOpped,
            UserUnopped
        }

        public ConversationAction(ActionType type, IDictionary<string, string> data)
        {
            this.Type = type;
            this.Data = data;
        }
    }
}
