using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MssgsDotNet
{
    public interface IMssgsCommand<T>
    {
        public string Method { get; set; }

        public Dictionary<string, T> Data { get; set; }
    }

    public interface IMssgsCommand : IMssgsCommand<string> { }
}
