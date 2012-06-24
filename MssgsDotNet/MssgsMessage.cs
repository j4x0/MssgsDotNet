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
        public InternalMessage InternalMessage { get; private set; }
        public string Username { get; set; }
        public bool Op { get; set; }
        public int Date { get; private set; }
        public string Message { get; set; }
        public string ConversationId { get; private set; }
        public bool New { get; private set; }

        public MssgsMessage(
            string message, 
            string conversationId, 
            int date, 
            string username, 
            bool op, 
            bool isInternal, 
            bool newMessage)
        {
            this.Message = message;
            this.ConversationId = conversationId;
            this.Date = date;
            this.Username = username;
            this.Op = op;
            this.Internal = isInternal;
            this.New = newMessage;
        }

        public MssgsMessage(string message, string conversationId)
        {
            this.ConversationId = conversationId;
            this.Message = message;
        }

        public static MssgsMessage Parse(IDictionary<string,string> data, bool newMessage)
        {
            bool isInternal = data["internal"].ToBoolean();
            string msg = data["message"];
            var message =  new MssgsMessage(
                msg,
                data["conversation"],
                Convert.ToInt32(data["date"]),
                data["username"],
                data["op"].ToBoolean(),
                isInternal,
                newMessage
                );
            if(isInternal)
                message.InternalMessage = InternalMessage.Parse(msg);
            return message;
        }
    }
}
