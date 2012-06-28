using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MssgsDotNet.Responses
{
    public class ConversationInfo : MssgsResponse
    {
        public string ConversationId { get; set; }
        public bool ReadOnly { get; set; }
        public bool PasswordProtected { get; set; }
        public bool Exists { get; set; }
        public bool SocialAuth { get; set; }
        public bool RobotPassword { get; set; }
        public bool Channel { get; set; }


    }
}
