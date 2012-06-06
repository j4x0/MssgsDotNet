using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MssgsDotNet.Responses;

namespace MssgsDotNet
{
    public class MssgsMessage : MssgsResponse
    {
        public bool Internal { get; private set; }
        public string Username { get; private set; }
        public bool Op { get; private set; }
        public int Date { get; private set; }
        public string Message { get; private set; }
        public string ConversationId { get; private set; }
        public bool New { get; private set; }

        public MssgsMessage(string message, string conversationId, int date, string username, bool op, bool internalMessage, bool newMessage)
        {
            this.Message = message;
            this.ConversationId = conversationId;
            this.Date = date;
            this.Username = username;
            this.Op = op;
            this.Internal = internalMessage;
            this.New = newMessage;
        }

        public MssgsMessage(string message, string conversationId)
        {
            this.ConversationId = conversationId;
            this.Message = message;
        }

        public static MssgsMessage Parse(IDictionary<string,string> data, bool newMessage)
        {
            return new MssgsMessage(
                data["message"],
                data["conversation"],
                Convert.ToInt32(data["date"]),
                data["username"],
                data.GetOrDefault("op").ToBoolean(),
                data.GetOrDefault("internal").ToBoolean(),
                newMessage
                );
        }
    }
}
