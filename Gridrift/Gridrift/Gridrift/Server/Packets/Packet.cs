using Gridrift.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Gridrift.Server.Packets
{

    class Packet
    {
        public PacketID packetID { get; set; }
        public int byteDataLength { get; set; }
        public byte[] byteData { get; set; }

        #region constructors
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

        public Packet()
        {

        }
        #endregion


        private byte getIDbytes()
        {
            return (byte)packetID;
        }

        private byte[] getDataLengthBytes()
        {
            return BitConverter.GetBytes(byteDataLength);
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

        public static void sendPacket(Packet packet, Stream stream)
        {
            stream.WriteByte(packet.getIDbytes());
            stream.Write(packet.getDataLengthBytes(), 0, sizeof(int));
            if (packet.byteDataLength > 0)
            {
            stream.Write(packet.byteData, 0, packet.byteDataLength);
            }
        }

        public static Packet recievePacket(Stream stream)
        {
            
            PacketID packetID = (PacketID)stream.ReadByte(); //check for -1 later
            byte[] byte4 = new byte[4];
            stream.Read(byte4, 0, sizeof(int));
            int byteDataLength = BitConverter.ToInt32(byte4, 0);
            if (byteDataLength > 0)
            {
                byte[] byteData = new byte[byteDataLength];
                stream.Read(byteData, 0, byteDataLength);

                return new Packet(packetID, byteDataLength, byteData);
            }
            else
            {
                return new Packet(packetID, byteDataLength, null);
            }
            
        }

    }
}
