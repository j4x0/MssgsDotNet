using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MssgsDotNet
{
    public class PacketBuilder
    {
        public string Packet { get; private set; }

        //public int Length { get; private set; }

        public PacketBuilder()
        {
            //this.Length = length;
            this.Packet = "";
        }

        public void Append(string str)
        {
            if (this.Built())
                throw new Exception("This packet is built already!");
            //if (str.StartsWith("length"))
            //    throw new Exception("New packet discovered while building a packet!");
            var filtered = this.Filter(str).Trim();
            if (filtered.IsFrikkinEmpty()) return;
            this.Packet += filtered;
        }

        public string Filter(string str)
        {
            return str.Replace("\r", "").Replace("\n", "");
        }

        public bool Built()
        {
            try
            {
                SimpleJson.SimpleJson.DeserializeObject(this.Packet);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public override string ToString()
        {
            return this.Packet;
        }
    }
}
