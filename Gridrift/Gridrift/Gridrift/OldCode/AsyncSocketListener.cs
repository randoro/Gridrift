﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Gridrift.OldCode
{
    public delegate void MessageReceivedHandler(int id, string msg);
    public delegate void MessageSubmittedHandler(int id, bool close);

    public sealed class AsyncSocketListener : IAsyncSocketListener
    {
        private const ushort Port = 13080;
        private const ushort Limit = 250;

        private static readonly IAsyncSocketListener instance = new AsyncSocketListener();

        private readonly ManualResetEvent mre = new ManualResetEvent(false);
        private readonly IDictionary<int, IStateObject> clients = new Dictionary<int, IStateObject>();

        public event MessageReceivedHandler MessageReceived;

        public event MessageSubmittedHandler MessageSubmitted;

        private AsyncSocketListener()
        {
            MessageReceived = new MessageReceivedHandler(recievedData);
        }

        public static IAsyncSocketListener Instance
        {
            get
            {
                return instance;
            }
        }

        /* Starts the AsyncSocketListener */
        public void StartListening()
        {
            var host = Dns.GetHostEntry(string.Empty);
            var ip = host.AddressList[0];
            var endpoint = new IPEndPoint(ip, Port);

            try
            {
                using (var listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    listener.Bind(endpoint);
                    listener.Listen(Limit);
                    while (true)
                    {
                        this.mre.Reset();
                        listener.BeginAccept(this.OnClientConnect, listener);
                        this.mre.WaitOne();
                    }
                }
            }
            catch (SocketException)
            {
                // TODO:
            }
        }

        /* Gets a socket from the clients dictionary by his Id. */
        private IStateObject GetClient(int id)
        {
            IStateObject state;

            return this.clients.TryGetValue(id, out state) ? state : null;
        }

        /* Checks if the socket is connected. */
        public bool IsConnected(int id)
        {
            var state = this.GetClient(id);

            return !(state.Listener.Poll(1000, SelectMode.SelectRead) && state.Listener.Available == 0);
        }

        /* Add a socket to the clients dictionary. Lock clients temporary to handle multiple access.
         * ReceiveCallback raise a event, after the message receive complete. */
        #region Receive data
        public void OnClientConnect(IAsyncResult result)
        {
            try
            {
                this.mre.Set();


                IStateObject state;

                lock (this.clients)
                {
                    var id = !this.clients.Any() ? 1 : this.clients.Keys.Max() + 1;

                    state = new StateObject(((Socket)result.AsyncState).EndAccept(result), id);
                    this.clients.Add(id, state);
                    Console.WriteLine("Client connected. Get Id " + id);
                }

                state.Listener.BeginReceive(state.Buffer, 0, state.BufferSize, SocketFlags.None, this.ReceiveCallback, state);
            }
            catch (SocketException)
            {
                // TODO:
            }
            catch (ObjectDisposedException)
            {
                // TODO:
            }

        }

        public void ReceiveCallback(IAsyncResult result)
        {
            try
            {
            
                var state = (IStateObject)result.AsyncState;

            
                var receive = state.Listener.EndReceive(result);

                if (receive > 0)
                {
                    state.Append(Encoding.UTF8.GetString(state.Buffer, 0, receive));
                }

                if (receive == state.BufferSize)
                {
                    state.Listener.BeginReceive(state.Buffer, 0, state.BufferSize, SocketFlags.None, this.ReceiveCallback, state);
                }
                else
                {
                    var messageReceived = this.MessageReceived;

                    if (messageReceived != null)
                    {
                        messageReceived(state.Id, state.Text);
                        Console.WriteLine("Server recieved: " + state.Text);
                    }

                    state.Reset();
                }
            }
            catch (SocketException)
            {
                // TODO:
            }
            catch (ObjectDisposedException)
            {
                // TODO:
            }
        }
        #endregion

        /* Send(int id, String msg, bool close) use bool to close the connection after the message sent. */
        #region Send data
        public void Send(int id, string msg, bool close)
        {
            var state = this.GetClient(id);

            if (state == null)
            {
                throw new Exception("Client does not exist.");
            }

            if (!this.IsConnected(state.Id))
            {
                throw new Exception("Destination socket is not connected.");
            }

            try
            {
                var send = Encoding.UTF8.GetBytes(msg);

                state.Close = close;
                state.Listener.BeginSend(send, 0, send.Length, SocketFlags.None, this.SendCallback, state);
            }
            catch (SocketException)
            {
                // TODO:
            }
            catch (ArgumentException)
            {
                // TODO:
            }
        }

        private void SendCallback(IAsyncResult result)
        {
            var state = (IStateObject)result.AsyncState;

            try
            {
                state.Listener.EndSend(result);
            }
            catch (SocketException)
            {
                // TODO:
            }
            catch (ObjectDisposedException)
            {
                // TODO:
            }
            finally
            {
                var messageSubmitted = this.MessageSubmitted;

                if (messageSubmitted != null)
                {
                    messageSubmitted(state.Id, state.Close);
                }
            }
        }
        #endregion

        public void Close(int id)
        {
            var state = this.GetClient(id);

            if (state == null)
            {
                throw new Exception("Client does not exist.");
            }

            try
            {
                state.Listener.Shutdown(SocketShutdown.Both);
                state.Listener.Close();
            }
            catch (SocketException)
            {
                // TODO:
            }
            finally
            {
                lock (this.clients)
                {
                    this.clients.Remove(state.Id);
                    Console.WriteLine("Client disconnected with Id {0}", state.Id);
                }
            }
        }

        public void Dispose()
        {
            //foreach (var id in this.clients.Keys)
            //{
                while (this.clients.Keys.Count > 0)
                {
                    int id = this.clients.ElementAt(0).Key;
                    this.Close(id);
                }
            this.mre.Dispose();
        }

        public void recievedData(int id, string msg)
        {
            Console.WriteLine("IS recieved data from client: " + msg);

            IStateObject state = GetClient(id);

            if (state != null)
            {
                state.Listener.BeginReceive(state.Buffer, 0, state.BufferSize, SocketFlags.None, this.ReceiveCallback, state);
            }
        }
    }
}
