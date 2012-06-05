using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MssgsDotNet.Commands;
using MssgsDotNet.Responses;

namespace MssgsDotNet
{
    public class MssgsConversation
    {
        public ConversationInfo Info { get; private set; }

        public string Id { get; private set; }

        public MssgsConversation(string conversationId)
        {
            /**client.ExecuteCommand(new ConversationInfoCommand(conversationId), (ConversationInfo cInfo) =>
                {
                    this.Info = cInfo;
                }
            );**/
            this.Id = conversationId;
        }

    }
}
