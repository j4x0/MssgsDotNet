using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MssgsDotNet
{
    public class MssgsUser : MssgsResponse
    {
        public string Name { get; set; }
        public bool Op { get; set; }

        public MssgsUser(string name, bool op)
        {
            this.Name = name;
            this.Op = op;
        }
        public MssgsUser(string name) : this(name, false) { }
    }
}
