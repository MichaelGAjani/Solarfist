namespace Jund.CommunicationHelper
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;

    public class NetClient : IDisposable
    {
        private Socket client;

        public event ProgressHandler RequestProgressChanged;

        public event ProgressHandler ResponseProgressChanged;

        public void Close()
        {
            if (!this.Connected)
            {
                throw new InvalidOperationException("Not connected.");
            }
            this.client.Shutdown(SocketShutdown.Both);
            this.client.Close();
        }

        public void Connect(IPEndPoint remoteEP)
        {
            if (this.Connected)
            {
                throw new InvalidOperationException("Connected.");
            }
            if (remoteEP == null)
            {
                throw new ArgumentNullException("remoteEP");
            }
            this.client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.client.Connect(remoteEP);
        }

        public void Connect(string address, int port)
        {
            IPAddress address2;
            if (this.Connected)
            {
                throw new InvalidOperationException("Connected.");
            }
            if (string.IsNullOrEmpty(address))
            {
                throw new ArgumentNullException("address");
            }
            this.client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            if (IPAddress.TryParse(address, out address2))
            {
                this.client.Connect(address2, port);
            }
            else
            {
                this.client.Connect(address, port);
            }
        }

        public void Dispose()
        {
            if (this.Connected)
            {
                this.Close();
            }
        }

        public Response GetResponse(Request request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }
            if (!this.Connected)
            {
                throw new InvalidOperationException("Not connected.");
            }
            Transceiver transceiver = new Transceiver(this.client);
            transceiver.SendProgressChanged += new ProgressHandler(this.OnRequestProgressChanged);
            transceiver.ReceiveProgressChanged += new ProgressHandler(this.OnResponseProgressChanged);
            transceiver.SendRequest(request);
            Response response = transceiver.ReceiveResponse();
            transceiver.SendProgressChanged -= new ProgressHandler(this.OnRequestProgressChanged);
            transceiver.ReceiveProgressChanged -= new ProgressHandler(this.OnResponseProgressChanged);
            return response;
        }

        protected void OnRequestProgressChanged(ProgressEventArgs e)
        {
            if (this.RequestProgressChanged != null)
            {
                this.RequestProgressChanged(e);
            }
        }

        protected void OnResponseProgressChanged(ProgressEventArgs e)
        {
            if (this.ResponseProgressChanged != null)
            {
                this.ResponseProgressChanged(e);
            }
        }

        public bool Connected =>
            ((this.client != null) && this.client.Connected);
    }
}

