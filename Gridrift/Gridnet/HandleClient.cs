using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Gridnet
{
    class HandleClient
    {
        
        public TcpClient clientSocket;
        int clientID;
        int msgNr = 0;
        


        public void startClient(TcpClient inClientSocket, int clientID)
        {
            this.clientSocket = inClientSocket;
            this.clientID = clientID;
            //Thread ctThread = new Thread(new ThreadStart(sendRecieveLoop));
            //ctThread.Start();
            sendRecieveLoop();
        }
        private void sendRecieveLoop()
        {
            while (true)
            {
                
                try
                {

                    if (clientID == 3)
                    {
                        clientSocket.Close();
                        return;
                    }
                    while (!clientSocket.Connected)
                    {
                        //wait for connection
                    }
                    NetworkStream networkStream = clientSocket.GetStream();
                    if (networkStream != null)
                    {
                        byte[] outBytes = new byte[4];
                        networkStream.Read(outBytes, 0, 4);
                        msgNr++;

                        String str = "Message Nr:" + msgNr + " recieved from ID:" + clientID + " { " + outBytes[0] + ", " + outBytes[1] + ", " + outBytes[2] + ", " + outBytes[3] + " }";
                        Console.WriteLine(str);
                        for (int i = 0; i < 4; i++)
                        {
                            outBytes[i] += 1;
                        }
                        outBytes[0] = (byte)(clientID + 10);
                        networkStream.Write(outBytes, 0, 4);
                        Console.WriteLine("Message Nr:" + msgNr + " sent from ID:" + clientID + " { " + outBytes[0] + ", " + outBytes[1] + ", " + outBytes[2] + ", " + outBytes[3] + " }");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(" >> " + ex.ToString());
                }
            }
        }
    }
}
