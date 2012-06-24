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

        public string this[string index]
        {
            get
            {
                return this.Data[index];
            }
            set
            {
                this.Data[index] = value;
            }
        }
    }
}
