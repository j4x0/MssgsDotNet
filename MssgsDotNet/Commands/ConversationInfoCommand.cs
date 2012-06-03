using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MssgsDotNet.Responses;

namespace MssgsDotNet.Commands
{
    class ConversationInfoCommand : IMssgsCommand<ConversationInfo>
    {
        public ConversationInfoCommand(string conversationId)
        {
        }
    }
}
