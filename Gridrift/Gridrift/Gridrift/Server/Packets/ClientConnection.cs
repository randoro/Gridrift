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

        public ClientConnection(string hostName, int port)
        {
            client = new TcpClient();
            client.Connect(IPAddress.Parse("77.105.222.65"), port); //will pause here if no conn found
        }

        public void startListening()
        {
            
            try
            {
                NetworkStream s = client.GetStream();
                //StreamReader sr = new StreamReader(s);
                //StreamWriter sw = new StreamWriter(s);
                //sw.AutoFlush = true;
                byte[] byte4 = new byte[4];
                
                //Console.WriteLine("CS: "+sr.ReadLine());
                while (true)
                {
                    s.Read(byte4, 0, 4);
                    int bytesInt = BitConverter.ToInt32(byte4, 0);
                    Console.WriteLine("CS: size:" + bytesInt);
                    byte[] result = new byte[bytesInt];
                    s.Read(result, 0, bytesInt);
                    Console.Write("CS: result: ");
                    for (int i = 0; i < bytesInt; i++)
			        {
                        Console.Write(result[i] + ", ");
			        }
                    Console.WriteLine(" ");
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
                
                // code in finally block is guranteed 
                // to execute irrespective of 
                // whether any exception occurs or does 
                // not occur in the try block
                client.Close();
            }
        }


        //private Queue<Packet> sendQueue;
        //private Object sendLock = new Object();

        //private Queue<Packet> recieveQueue;
        //private Object recieveLock = new Object();

        //// ManualResetEvent instances signal completion.
        //private static ManualResetEvent connectDone =
        //    new ManualResetEvent(false);
        //private static ManualResetEvent sendDone =
        //    new ManualResetEvent(false);
        //private static ManualResetEvent receiveDone =
        //    new ManualResetEvent(false);

        //// The response from the remote device.
        //private static byte[] response;

        //private String host;
        //private int connectionPort;
        //private Socket client;

        //public bool isRunning;

        //public ClientConnection(String host, int connectionPort)
        //{
        //    sendQueue = new Queue<Packet>();
        //    recieveQueue = new Queue<Packet>();
        //    this.host = host;
        //    this.connectionPort = connectionPort;
        //}

        //public void addPacket(Packet newPacket)
        //{
        //    lock (sendLock)
        //    {
        //        sendQueue.Enqueue(newPacket);
        //    }
        //}

        //public void start()
        //{

        //    // Connect to a remote device.
        //    try
        //    {
        //        // Establish the remote endpoint for the socket.
        //        // The name of the 
        //        // remote device is "host.contoso.com".
        //        IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
        //        IPAddress ipAddress = ipHostInfo.AddressList[0];
        //        //IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
        //        IPEndPoint remoteEP = new IPEndPoint(ipAddress, connectionPort);

        //        // Create a TCP/IP socket.
        //        client = new Socket(AddressFamily.InterNetwork,
        //            SocketType.Stream, ProtocolType.Tcp);

        //        // Connect to the remote endpoint.
        //        client.BeginConnect(remoteEP,
        //            new AsyncCallback(ConnectCallback), client);
        //        connectDone.WaitOne();

        //        isRunning = true;
        //        while (isRunning)
        //        {
                    


        //            if (sendQueue.Count > 0)
        //            {
        //                Packet sendPacket = sendQueue.Peek();
        //                byte packetID = (byte)sendPacket.getID();
        //                byte[] data = sendPacket.getData();
        //                byte[] IDplusData = new byte[data.Length + 1];
        //                IDplusData[0] = packetID;
        //                Array.Copy(data, 0, IDplusData, 1, data.Length);

        //                // Send test data to the remote device.
        //                Send(client, IDplusData);
        //                sendDone.WaitOne();
        //                lock (sendLock)
        //                {
        //                    sendQueue.Dequeue();
        //                }
                        


        //            }



        //            // Receive the response from the remote device.
        //            Receive(client);
        //            receiveDone.WaitOne();

        //            if (response != null && response.Length > 0)
        //            {
        //                PacketID newID = (PacketID)response[0];
        //                byte[] newData = new byte[response.Length - 1];
        //                Array.Copy(response, 1, newData, 0, newData.Length);
        //                Packet newPacket = new Packet(newID, newData);
        //                lock (recieveLock)
        //                {
        //                    sendQueue.Enqueue(newPacket);
        //                }
        //                response = null;
        //            }


        //        }

        //        // Release the socket.
        //        //client.Shutdown(SocketShutdown.Both);
        //        //client.Close();

        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.ToString());
        //    }



        //    //isRunning = true;

        //    //while (isRunning)
        //    //{
        //    //    //lock (sendLock)
        //    //    //{
        //    //    //    if (sendQueue.Count > 0)
        //    //    //    {
        //    //    //        Packet sendPacket = sendQueue.Dequeue();
        //    //    //        byte packetID = (byte)sendPacket.getID();
        //    //    //        byte[] data = sendPacket.getData();
        //    //    //        byte[] IDplusData = new byte[data.Length + 1];
        //    //    //        IDplusData[0] = packetID;
        //    //    //        Array.Copy(data, 0, IDplusData, 1, data.Length);
        //    //    //        clientStream.Write(IDplusData, 0, IDplusData.Length);
        //    //    //    }
        //    //    //}

        //    //    //lock (recieveLock)
        //    //    //{

        //    //    //        Packet sendPacket = recieveQueue.Dequeue();
        //    //    //        byte packetID = (byte)sendPacket.getID();
        //    //    //        byte[] data = sendPacket.getData();
        //    //    //        byte[] IDplusData = new byte[data.Length + 1];
        //    //    //        IDplusData[0] = packetID;
        //    //    //        Array.Copy(data, 0, IDplusData, 1, data.Length);
        //    //    //        clientStream.Write(IDplusData, 0, IDplusData.Length);
        //    //    //    }
        //    //    }

        //}



        //private static void ConnectCallback(IAsyncResult ar)
        //{
        //    try
        //    {
        //        // Retrieve the socket from the state object.
        //        Socket client = (Socket)ar.AsyncState;

        //        // Complete the connection.
        //        client.EndConnect(ar);

        //        Console.WriteLine("CL Socket connected to {0}",
        //            client.RemoteEndPoint.ToString());

        //        // Signal that the connection has been made.
        //        connectDone.Set();
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.ToString());
        //    }
        //}

        //private static void Receive(Socket client)
        //{
        //    try
        //    {
        //        // Create the state object.
        //        StateObject state = new StateObject();
        //        state.workSocket = client;

        //        // Begin receiving the data from the remote device.
        //        client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
        //            new AsyncCallback(ReceiveCallback), state);
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.ToString());
        //    }
        //}

        //private static void ReceiveCallback(IAsyncResult ar)
        //{
        //    try
        //    {
        //        // Retrieve the state object and the client socket 
        //        // from the asynchronous state object.
        //        StateObject state = (StateObject)ar.AsyncState;
        //        Socket client = state.workSocket;

        //        // Read data from the remote device.
        //        int bytesRead = client.EndReceive(ar);

        //        if (bytesRead > 0)
        //        {
        //            // There might be more data, so store the data received so far.
        //            state.ms.Write(state.buffer, 0, bytesRead);

        //            // Get the rest of the data.
        //            client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
        //                new AsyncCallback(ReceiveCallback), state);
        //        }
        //        else
        //        {
        //            // All the data has arrived; put it in response.
        //            if (state.ms.Length > 1)
        //            {
        //                response = state.ms.ToArray();
        //            }
        //            // Signal that all bytes have been received.
        //            receiveDone.Set();
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.ToString());
        //    }
        //}

        //private static void Send(Socket client, byte[] data)
        //{
        //    // Convert the string data to byte data using ASCII encoding.
        //    //byte[] byteData = Encoding.ASCII.GetBytes(data);

        //    // Begin sending the data to the remote device.
        //    client.BeginSend(data, 0, data.Length, 0,
        //        new AsyncCallback(SendCallback), client);
        //}

        //private static void SendCallback(IAsyncResult ar)
        //{
        //    try
        //    {
        //        // Retrieve the socket from the state object.
        //        Socket client = (Socket)ar.AsyncState;

        //        // Complete sending the data to the remote device.
        //        int bytesSent = client.EndSend(ar);
        //        Console.WriteLine("CL Sent {0} bytes to server.", bytesSent);

        //        // Signal that all bytes have been sent.
        //        sendDone.Set();
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.ToString());
        //    }
        //}

    }
}
