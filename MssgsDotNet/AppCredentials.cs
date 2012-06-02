using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MssgsDotNet
{
    struct AppCredentials
    {
        private string ID { get; set; }

        private string Secret { get; set; }

        public AppCredentials()
        {
            this.ID = "";
            this.Secret = "";
        }
    }
}
