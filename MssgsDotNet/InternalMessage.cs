using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MssgsDotNet
{
    public class InternalMessage
    {
        public InternalMessageType Type { get; private set; }
        public IDictionary<string, string> Data { get; private set; }

        public InternalMessage(InternalMessageType type, IDictionary<string, string> data)
        {
            this.Type = type;
            this.Data = data;
        }

        public InternalMessage(InternalMessageType type) : this(type, null) {}

        public static InternalMessage Parse(string data)
        {
            var delimeter = new char[] { ':' };
            string[] splitted = data.Split(delimeter, 2);
            if (splitted.Length != 2)
                throw new Exception("Can't parse InternalMessage, corrupt json: \"" + data + "\"");
            IDictionary<string, string> msgData = null;
            try
            {
                var obj = (IDictionary<string, object>)SimpleJson.SimpleJson.DeserializeObject(splitted[1].Trim());
                msgData = obj.ToDictionary(k => k.Key.ToString(), k => k.Value.ToString());
            }
            catch
            {
                throw new Exception("Can't parse InternalMessage, corrupt json: \"" + data + "\"");
            }
            switch (splitted[0].ToLower().Trim())
            {
                case "join":
                    return new InternalMessage(InternalMessageType.Join, msgData);
                case "opunlock":
                    return new InternalMessage(InternalMessageType.OpUnlock);
                case "oppassword":
                    return new InternalMessage(InternalMessageType.OpPassword);
                case "reservedname":
                    return new InternalMessage(InternalMessageType.ReservedName);
                case "usernamechange":
                    return new InternalMessage(InternalMessageType.UsernameChange, msgData);
                case "leave":
                    return new InternalMessage(InternalMessageType.Leave, msgData);
                case "banned":
                    return new InternalMessage(InternalMessageType.Banned, msgData);
                case "kick":
                    return new InternalMessage(InternalMessageType.Kicked, msgData);
                case "unbanned":
                    return new InternalMessage(InternalMessageType.Unbanned, msgData);
                case "warnflood":
                    return new InternalMessage(InternalMessageType.WarnFlood);
                case "message":
                    return new InternalMessage(InternalMessageType.Message, msgData);
                case "options":
                    return new InternalMessage(InternalMessageType.Options, msgData);
                case "op":
                    return new InternalMessage(InternalMessageType.Op, msgData);
                case "me":
                    return new InternalMessage(InternalMessageType.Me, msgData);
                default:
                    break;
            }
            throw new Exception("Can't parse InternalMessage, unknown internal message type!");
        }

        public enum InternalMessageType
        {
            Join,
            OpUnlock,
            OpPassword,
            ReservedName,
            UsernameChange,
            Leave,
            Banned,
            Kicked,
            Unbanned,
            WarnFlood,
            Message,
            Options,
            Op,
            Me
        }
    }
}
