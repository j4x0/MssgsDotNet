using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MssgsDotNet
{
    public abstract class MssgsResponse
    {
        public string Method { get; set; }
        public Dictionary<string, string> Data { get; set; }

        public MssgsResponse(string method, Dictionary<string, string> data)
        {
            this.Method = method;
            this.Data = data;
        }

    }
}
