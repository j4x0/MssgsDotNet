using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MssgsDotNet
{
    public class MssgsApiException : Exception
    {
        public MssgsApiException() : base() { }

        public MssgsApiException(string text) : base(text) { }

        public MssgsApiException(string text, Exception inner) : base(text, inner) { }
    }
}
