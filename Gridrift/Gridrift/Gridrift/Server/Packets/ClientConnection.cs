using Gridrift.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Gridrift.Server.Packets
{

    class ClientConnection
    {
        TcpClient client;
        private Queue<Packet> sendQueue;
        private Object sendLock = new Object();

        private Queue<Packet> recieveQueue;
        private Object recieveLock = new Object();

        public bool listening { get; private set; }


        public ClientConnection(string hostName, int port)
        {
            client = new TcpClient();
            client.Connect(IPAddress.Parse("77.105.222.65"), port); //will pause here if no conn found
            listening = false;
            sendQueue = new Queue<Packet>();
            recieveQueue = new Queue<Packet>();

        }


        public void startListening()
        {
            listening = true;
            try
            {
                NetworkStream s = client.GetStream();


                Packet startPacket = new Packet(PacketID.connect, 0, null);
                Packet.sendPacket(startPacket, s);

                //byte[] byte4 = new byte[4];
                //byte byte1ID;
                Packet incomingPacket;
                
                //Console.WriteLine("CS: "+sr.ReadLine());
                while (listening)
                {
                    incomingPacket = Packet.recievePacket(s);
                    Console.Write("CS: recieved new Packet:");
                    Console.Write(" PacketID =" + incomingPacket.packetID.ToString());
                    Console.Write(" PacketDataLength =" + incomingPacket.byteDataLength); 
                    Console.Write(" PacketData =");
                    for (int i = 0; i < incomingPacket.byteDataLength; i++)
			        {
                        Console.Write(incomingPacket.byteData[i]);
			        }
                    Console.WriteLine(" ");

                    //byte1ID = (byte)s.ReadByte();
                    //s.Read(byte4, 0, 4);
                    //int bytesInt = BitConverter.ToInt32(byte4, 0);
                    //Console.WriteLine("CS: size:" + bytesInt);
                    //byte[] result = new byte[bytesInt];
                    //s.Read(result, 0, bytesInt);

                    //Console.Write("CS: " + "Name: ");
                    //string name = "john";
                    //sw.WriteLine(name);
                    //if (name == "") break;
                    //Console.WriteLine("CS: " + sr.ReadLine());
                }
                s.Close();
            }
            finally
            {
                listening = false;
                // code in finally block is guranteed 
                // to execute irrespective of 
                // whether any exception occurs or does 
                // not occur in the try block
                client.Close();
            }
        }

        public void stopListening()
        {
            listening = false;
            client.Close();

        }
        


    }
}
