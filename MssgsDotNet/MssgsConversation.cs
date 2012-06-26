using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MssgsDotNet.Commands;
using MssgsDotNet.Responses;

namespace MssgsDotNet
{
    public class MssgsConversation : IEnumerable<UserMessagePair>
    {
        public ConversationInfo Info { get; private set; }

        public string Id { get; private set; }
        public bool Secured { get; set; }
        public string Password { get; set; }

        private List<MssgsMessage> messages;
        private List<MssgsUser> users;

        public MssgsConversation(string conversationId)
        {
            this.Id = conversationId;
            this.messages = new List<MssgsMessage>();
            this.users = new List<MssgsUser>();
        }

        public void AddMessage(MssgsMessage message)
        {
            this.messages.Add(message);
        }

        public void AddUser(MssgsUser user)
        {
            this.users.Add(user);
        }

        public void DeleteUser(MssgsUser user)
        {
            this.users.Remove(user);
        }

        public MssgsUser GetUser(string username)
        {
            foreach (var user in this.users)
            {
                if (user.Name == username)
                    return user;
            }
            throw new Exception("An user with name \"" + username + "\"  doesn't exist in the conversation");
        }

        public void RemoveUser(string username)
        {
            foreach (var user in this.users)
            {
                if (user.Name == username)
                {
                    this.users.Remove(user);
                    return;
                }
            }
        }

        public bool HasUser(string username)
        {
            foreach (var user in this.users)
            {
                if (user.Name == username)
                    return true;
            }
            return false;
        }

        public void RenameUser(string oldname, string newname)
        {
            var found = false;
            foreach (var user in this.users)
            {
                if (user.Name == oldname)
                {
                    found = true;
                    user.Name = newname;
                }
            }
            foreach (var msg in this.messages)
            {
                if (msg.Username == oldname)
                    msg.Username = newname;
            }
            if (!found) throw new Exception("An user with name: \"" + oldname + "\" doesn't exist!");
        }

        public MssgsUser this[string username]
        {
            get
            {
                return this.GetUser(username);
            }
            set
            {
                foreach (var user in this.users)
                {
                    if (user.Name == username)
                        this.users.Remove(user);
                }
                this.AddUser(value);
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            foreach (var msg in this.messages)
            {
                MssgsUser user = null;
                foreach (var usr in this.users)
                {
                    if (usr.Name == msg.Username)
                        user = usr;
                }
                yield return new UserMessagePair(user, msg);
            }

        }

        IEnumerator<UserMessagePair> IEnumerable<UserMessagePair>.GetEnumerator()
        {
            foreach (var msg in this.messages)
            {
                MssgsUser user = null;
                foreach (var usr in this.users)
                {
                    if (usr.Name == msg.Username)
                        user = usr;
                }
                yield return new UserMessagePair(user, msg);
            }
        }
    }

    public class UserMessagePair
    {
        public MssgsUser User { get; private set; }
        public MssgsMessage Message { get; private set; }

        public UserMessagePair(MssgsUser user, MssgsMessage message)
        {
            this.User = user;
            this.Message = message;
        }
    }
}
