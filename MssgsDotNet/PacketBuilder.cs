using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MssgsDotNet
{
    public class PacketBuilder
    {
        public string Packet { get; private set; }

        public IDictionary<string, object> Data { get; private set; }

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
            return str.Replace("\r", "").Replace("\n", "").Replace("\0","");
        }

        public bool Built()
        {
            if (this.Packet.Length < 1)
                return false;
            if (!this.Packet.Trim().EndsWith("\"end\":true}"))
            {
                return false;
            }
            object data = null;
            try
            {
                data = SimpleJson.SimpleJson.DeserializeObject(this.Packet);
            }
            catch
            {
                return false;
            }
            try
            {
                this.Data = (IDictionary<string, object>)data;
                return true;
            }
            catch
            {
                throw new Exception("Bad packet: " + this.Packet);
            }
        }

        public override string ToString()
        {
            return this.Packet;
        }
    }
}
