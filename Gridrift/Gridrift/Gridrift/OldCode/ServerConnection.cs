﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Gridrift.OldCode
{
    class ServerConnection
    {

        //    public static ManualResetEvent allDone = new ManualResetEvent(false);


        //    public static void StartListening() {
        //    // Data buffer for incoming data.
        //    byte[] bytes = new Byte[1024];

        //    // Establish the local endpoint for the socket.
        //    // The DNS name of the computer
        //    // running the listener is "host.contoso.com".
        //    IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
        //    IPAddress ipAddress = ipHostInfo.AddressList[0];
        //    IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 13000);

        //    // Create a TCP/IP socket.
        //    Socket listener = new Socket(AddressFamily.InterNetwork,
        //        SocketType.Stream, ProtocolType.Tcp );

        //    // Bind the socket to the local endpoint and listen for incoming connections.
        //    try {
        //        listener.Bind(localEndPoint);
        //        listener.Listen(100);

        //        while (true) {
        //            // Set the event to nonsignaled state.
        //            allDone.Reset();

        //            // Start an asynchronous socket to listen for connections.
        //            Console.WriteLine("IS Waiting for a connection...");
        //            listener.BeginAccept( 
        //                new AsyncCallback(AcceptCallback),
        //                listener );

        //            // Wait until a connection is made before continuing.
        //            allDone.WaitOne();
        //        }

        //    } catch (Exception e) {
        //        Console.WriteLine(e.ToString());
        //    }

        //    //Console.WriteLine("\nPress ENTER to continue...");
        //    //Console.Read();

        //}

        //public static void AcceptCallback(IAsyncResult ar) {
        //    // Signal the main thread to continue.
        //    allDone.Set();

        //    // Get the socket that handles the client request.
        //    Socket listener = (Socket) ar.AsyncState;
        //    Socket handler = listener.EndAccept(ar);

        //    // Create the state object.
        //    StateObject state = new StateObject();
        //    state.workSocket = handler;
        //    handler.BeginReceive( state.buffer, 0, StateObject.BufferSize, 0,
        //        new AsyncCallback(ReadCallback), state);
        //}

        //public static void ReadCallback(IAsyncResult ar) {
        //    byte[] content;

        //    // Retrieve the state object and the handler socket
        //    // from the asynchronous state object.
        //    StateObject state = (StateObject) ar.AsyncState;
        //    Socket handler = state.workSocket;

        //    // Read data from the client socket. 
        //    int bytesRead = handler.EndReceive(ar);

        //    if (bytesRead > 0) {
        //        // There  might be more data, so store the data received so far.
        //        state.ms.Write(state.buffer, 0, bytesRead);

        //        // Check for end-of-file tag. If it is not there, read 
        //        // more data.
        //        content = state.ms.ToArray();
        //        if (content[content.Length - 1] == 0)
        //        {

        //            // All the data has been read from the 
        //            // client. Display it on the console.
        //            Console.WriteLine("IS Read {0} bytes from socket. \n Data : {1}",
        //                content.Length, content.ToString() );
        //            // Echo the data back to the client.
        //            Send(handler, content);
        //        } else {
        //            // Not all data received. Get more.
        //            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
        //            new AsyncCallback(ReadCallback), state);
        //        }
        //    }
        //}

        //private static void Send(Socket handler, byte[] data) {
        //    // Convert the string data to byte data using ASCII encoding.
        //    //byte[] byteData = Encoding.ASCII.GetBytes(data);

        //    // Begin sending the data to the remote device.
        //    handler.BeginSend(data, 0, data.Length, 0,
        //        new AsyncCallback(SendCallback), handler);
        //}

        //private static void SendCallback(IAsyncResult ar) {
        //    try {
        //        // Retrieve the socket from the state object.
        //        Socket handler = (Socket) ar.AsyncState;

        //        // Complete sending the data to the remote device.
        //        int bytesSent = handler.EndSend(ar);
        //        Console.WriteLine("IS Sent {0} bytes to client.", bytesSent);

        //        //handler.Shutdown(SocketShutdown.Both);
        //        //handler.Close();

        //    } catch (Exception e) {
        //        Console.WriteLine(e.ToString());
        //    }
        //}
        //}
    }
}
