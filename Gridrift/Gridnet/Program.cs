using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Gridnet
{
    class Program
    {
        static long lastUpdate;
        static long secondCounter;
        static int ticks;
        static bool isListening;
        static Thread listeningThread;
        static AsynchronousSocketListener listener;
        //static Dictionary<int, Thread> clientsThreads;
        //static Dictionary<int, HandleClient> clients;

        static void Main(string[] args)
        {
            lastUpdate = DateTime.Now.Ticks;
            secondCounter = DateTime.Now.Ticks;
            ticks = 0;

            Console.WriteLine("Starting Gridnet Gridrift server...");
            //clients = new Dictionary<int, HandleClient>();
            //clientsThreads = new Dictionary<int, Thread>();



            try
            {
                isListening = true;
                listeningThread = new Thread(new ThreadStart(AsynchronousSocketListener.StartListening));
                listeningThread.Start();

                while (true)
                {
                    long newNow = DateTime.Now.Ticks;

                    if (newNow > secondCounter + (TimeSpan.TicksPerSecond))
                    {
                        secondCounter = newNow;
                        Console.WriteLine("TPS:" + ticks);
                        ticks = 0;
                    }

                    if (newNow > lastUpdate + (TimeSpan.TicksPerSecond / 20))
                    {
                        ticks++;
                        lastUpdate = newNow;
                    }
                }
            }
            finally
            {
                isListening = false;
                //listeningThread.Abort();
            }
        }


        private static void StartListening()
        {
        }

        //private static void StartClientFunc(HandleClient client, TcpClient clientSocket, int counter)
        //{
        //    client.startClient(clientSocket, counter);

        //}
    }
}
