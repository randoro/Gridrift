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
        protected int byteDataLength;
        protected byte[] byteData;

        public Packet(PacketID packetID, int byteDataLength, byte[] byteData)
        {
            this.packetID = packetID;
            this.byteDataLength = byteDataLength;
            this.byteData = byteData;
        }

        public Packet(byte[] realArray)
        {
            setPacketArray(realArray);
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

        public byte[] getPacketArray()
        {
            byte[] realArray = new byte[5 + byteDataLength];
            byte ID = (byte)packetID;
            realArray[0] = ID;
            byte[] lengthBytes = BitConverter.GetBytes(byteDataLength);
            Array.Copy(lengthBytes, 0, realArray, 1, sizeof(int));

            Array.Copy(byteData, 0, realArray, 5, byteDataLength);

            return realArray;
        }

        public void setPacketArray(byte[] realArray)
        {
            packetID = (PacketID)realArray[0];
            byteDataLength = BitConverter.ToInt32(realArray, 1);
            byteData = new byte[byteDataLength];
            Array.Copy(realArray, 5, byteData, 0, byteDataLength);
        }


    }
}
