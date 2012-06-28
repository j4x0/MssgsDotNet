using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MssgsDotNet.Responses
{
    public class ResponseTuple<TItem1, TItem2> : MssgsResponse 
        where TItem1 : MssgsResponse 
        where TItem2 : MssgsResponse
    {
        public TItem1 Item1 { get; set; }
        public TItem2 Item2 { get; set; }

        public ResponseTuple(TItem1 item1, TItem2 item2)
        {
            this.Item1 = item1;
            this.Item2 = item2;
        }
    }
}
