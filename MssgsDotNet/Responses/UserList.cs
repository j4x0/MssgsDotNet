using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MssgsDotNet.Responses
{
    public class UserList : MssgsResponse
    {
        public List<MssgsUser> Users { get; private set; }

        public UserList(List<MssgsUser> users)
        {
            this.Users = users;
        }
    }
}
