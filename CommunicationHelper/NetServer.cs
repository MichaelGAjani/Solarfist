namespace Jund.CommunicationHelper
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Sockets;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;

    public class NetServer
    {
        private long clientIdSeed;
        private List<Socket> clients = new List<Socket>();
        private List<Thread> clientThreads = new List<Thread>();
        private bool isRunning;
        private Socket listener;
        private IPEndPoint localEP;
        private int receiveTimeout;
        private Thread runningThread;

        public event ConnectedHandler ClientConnected;

        public event RequestHandler ClientRequest;

        public NetServer(IPEndPoint localEP)
        {
            if (localEP == null)
            {
                throw new ArgumentNullException("localEP");
            }
            this.localEP = localEP;
        }

        private void CloseClient(Socket client)
        {
            try
            {
                this.clientThreads.Remove(Thread.CurrentThread);
                this.clients.Remove(client);
                client.Shutdown(SocketShutdown.Both);
                client.Close();
            }
            catch
            {
            }
        }

        private long GetNewClientId() => 
            Interlocked.Increment(ref this.clientIdSeed);

        protected virtual void OnClientConnected(ConnectedEventArgs e)
        {
            if (this.ClientConnected != null)
            {
                this.ClientConnected(e);
            }
        }

        protected virtual void OnClientRequest(RequestEventArgs e)
        {
            if (this.ClientRequest != null)
            {
                this.ClientRequest(e);
            }
        }

        private bool ProcessConnection(long clientId, IPEndPoint clientEP)
        {
            ConnectedEventArgs e = new ConnectedEventArgs(clientId, clientEP);
            this.OnClientConnected(e);
            return e.Allowed;
        }

        private void Processing(object parameter)
        {
            ClientState state = (ClientState) parameter;
            try
            {
                bool flag;
                Transceiver transceiver = new Transceiver(state.Client);
                do
                {
                    Request request = transceiver.ReceiveRequest();
                    Response response = this.ProcessRequest(state.Id, (IPEndPoint) state.Client.RemoteEndPoint, request, out flag);
                    transceiver.SendResponse(response);
                }
                while (!flag);
            }
            catch (ThreadAbortException)
            {
                return;
            }
            catch (Exception)
            {
            }
            this.CloseClient(state.Client);
        }

        private Response ProcessRequest(long clientId, IPEndPoint clientEP, Request request, out bool closeAfterResponse)
        {
            RequestEventArgs e = new RequestEventArgs(clientId, clientEP, request);
            Response response = new Response(request.Id);
            this.OnClientRequest(e);
            closeAfterResponse = e.CloseAfterResponse;
            response.Status = e.ResponseStatus;
            response.Message = e.ResponseMessage;
            response.SetData(e.ResponseData);
            return response;
        }

        private void Running()
        {
        Label_0000:
            try
            {
                Socket item = this.listener.Accept();
                long newClientId = this.GetNewClientId();
                if (this.ProcessConnection(newClientId, (IPEndPoint) item.RemoteEndPoint))
                {
                    item.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, this.receiveTimeout);
                    Thread thread = new Thread(new ParameterizedThreadStart(this.Processing)) {
                        IsBackground = true
                    };
                    this.clientThreads.Add(thread);
                    this.clients.Add(item);
                    ClientState parameter = new ClientState {
                        Id = this.GetNewClientId(),
                        Client = item
                    };
                    thread.Start(parameter);
                }
                else
                {
                    item.Close();
                }
                goto Label_0000;
            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception)
            {
                Thread.Sleep(0x3e8);
                goto Label_0000;
            }
        }

        public void Start()
        {
            if (this.isRunning)
            {
                throw new InvalidOperationException("Server is running.");
            }
            this.runningThread = new Thread(new ThreadStart(this.Running));
            this.runningThread.IsBackground = true;
            this.runningThread.Start();
            this.listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.listener.Bind(this.localEP);
            this.listener.Listen(0x7fffffff);
            this.isRunning = true;
        }

        public void Stop()
        {
            if (!this.isRunning)
            {
                throw new InvalidOperationException("Server is not running.");
            }
            try
            {
                if (this.runningThread != null)
                {
                    this.runningThread.Abort();
                    this.runningThread = null;
                }
                if (this.clients != null)
                {
                    foreach (Socket socket in this.clients)
                    {
                        try
                        {
                            socket.Shutdown(SocketShutdown.Both);
                            socket.Close();
                        }
                        catch
                        {
                        }
                    }
                    this.clients.Clear();
                }
                if (this.clientThreads != null)
                {
                    foreach (Thread thread in this.clientThreads)
                    {
                        thread.Abort();
                    }
                    this.clientThreads.Clear();
                }
                this.listener.Close();
                this.listener = null;
                this.isRunning = false;
            }
            catch (Exception)
            {
            }
        }

        public bool IsRunning =>
            this.isRunning;

        public int ReceiveTimeout
        {
            get => 
                this.receiveTimeout;
            set
            {
                this.receiveTimeout = value;
            }
        }

        private class ClientState
        {
            public Socket Client { get; set; }

            public long Id { get; set; }
        }
    }
}

