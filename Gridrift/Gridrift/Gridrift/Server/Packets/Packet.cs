using Gridrift.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gridrift.Server.Packets
{

    class Packet
    {
        protected PacketID packetID;
        protected byte[] byteData;

        public Packet(PacketID packetID, byte[] byteData)
        {
            this.packetID = packetID;
            this.byteData = byteData;
        }

        public PacketID getID()
        {
            return packetID;
        }

        public byte[] getData()
        {
            return byteData;
        }

        public void setID(PacketID newID)
        {
            packetID = newID;
        }

        public void setData(byte[] newData)
        {
            byteData = newData;
        }


    }
}
