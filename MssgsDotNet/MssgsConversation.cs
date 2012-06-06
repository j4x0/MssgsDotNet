using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MssgsDotNet.Commands;
using MssgsDotNet.Responses;

namespace MssgsDotNet
{
    public class MssgsConversation : IEnumerable<MssgsMessage>
    {
        public ConversationInfo Info { get; private set; }

        public string Id { get; private set; }

        public List<MssgsMessage> Messages { get; private set; }

        public delegate void Handler();
        public delegate void Handler<T>(T obj);

        public MssgsConversation(string conversationId)
        {
            this.Id = conversationId;
            this.Messages = new List<MssgsMessage>();
        }

        public void AddMessage(MssgsMessage message)
        {
            this.Messages.Add(message);
        }


        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            foreach (var message in this.Messages)
                yield return message;
        }

        IEnumerator<MssgsMessage> IEnumerable<MssgsMessage>.GetEnumerator()
        {
            foreach (var message in this.Messages)
                yield return message;
        }
    }
}
