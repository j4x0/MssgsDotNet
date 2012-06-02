using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MssgsDotNet
{
    public struct RawMssgsResponse
    {
        public string Method { get; set; }

        public IDictionary<string, string> Data { get; set; }
    }
}
