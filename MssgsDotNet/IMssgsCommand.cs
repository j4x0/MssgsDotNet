using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MssgsDotNet
{
    public interface IMssgsCommand<T> where T : MssgsResponse
    {
        string Method { get; set; }

        T CreateResponse(RawMssgsResponse rawResponse);
    }

}
