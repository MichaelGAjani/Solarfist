namespace Jund.CommunicationHelper
{
    using System;
    using System.IO;
    using System.Net.Sockets;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Threading;

    internal class Transceiver
    {
        private Socket client;
        private byte[] flagValue;
        private const int SegmentSize = 0x400;

        public event ProgressHandler ReceiveProgressChanged;

        public event ProgressHandler SendProgressChanged;

        public Transceiver(Socket client)
        {
            if (client == null)
            {
                throw new ArgumentNullException("client");
            }
            this.client = client;
        }

        private byte[] GetBytes(object obj)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream serializationStream = new MemoryStream();
            formatter.Serialize(serializationStream, obj);
            return serializationStream.ToArray();
        }

        private object GetObject(byte[] bytes)
        {
            MemoryStream serializationStream = new MemoryStream(bytes);
            BinaryFormatter formatter = new BinaryFormatter();
            return formatter.Deserialize(serializationStream);
        }

        protected void OnReceiveProgressChanged(Guid id, int bytesLoaded, int bytesTotal)
        {
            if (this.ReceiveProgressChanged != null)
            {
                ProgressEventArgs e = new ProgressEventArgs {
                    RequestId = id,
                    BytesLoaded = bytesLoaded,
                    BytesTotal = bytesTotal
                };
                this.ReceiveProgressChanged(e);
            }
        }

        protected void OnSendProgressChanged(Guid id, int bytesLoaded, int bytesTotal)
        {
            if (this.SendProgressChanged != null)
            {
                ProgressEventArgs e = new ProgressEventArgs {
                    RequestId = id,
                    BytesLoaded = bytesLoaded,
                    BytesTotal = bytesTotal
                };
                this.SendProgressChanged(e);
            }
        }

        private byte[] ReceiveBytes()
        {
            int num3;
            byte[] buffer = new byte[4];
            this.client.Receive(buffer);
            this.client.Send(this.FlagBytes);
            byte[] buffer2 = new byte[BitConverter.ToInt32(buffer, 0)];
            for (int i = 0; i < buffer2.Length; i += num3)
            {
                num3 = buffer2.Length - i;
                if (num3 > 0x400)
                {
                    num3 = 0x400;
                }
                num3 = this.client.Receive(buffer2, i, num3, SocketFlags.None);
                this.client.Send(this.FlagBytes);
            }
            this.client.Receive(this.FlagBytes);
            return buffer2;
        }

        private byte[] ReceiveBytesWithProgress(Guid id)
        {
            int num4;
            byte[] buffer = new byte[4];
            this.client.Receive(buffer);
            this.client.Send(this.FlagBytes);
            byte[] buffer2 = new byte[BitConverter.ToInt32(buffer, 0)];
            int bytesLoaded = 0;
            this.OnReceiveProgressChanged(id, bytesLoaded, buffer2.Length);
            for (int i = 0; i < buffer2.Length; i += num4)
            {
                num4 = buffer2.Length - i;
                if (num4 > 0x400)
                {
                    num4 = 0x400;
                }
                num4 = this.client.Receive(buffer2, i, num4, SocketFlags.None);
                this.client.Send(this.FlagBytes);
                bytesLoaded += num4;
                this.OnReceiveProgressChanged(id, bytesLoaded, buffer2.Length);
            }
            this.client.Receive(this.FlagBytes);
            return buffer2;
        }

        private object ReceiveObject()
        {
            byte[] b = this.ReceiveBytes();
            Guid id = new Guid(b);
            byte[] bytes = this.ReceiveBytesWithProgress(id);
            return this.GetObject(bytes);
        }

        public Request ReceiveRequest() => 
            ((Request) this.ReceiveObject());

        public Response ReceiveResponse() => 
            ((Response) this.ReceiveObject());

        private void SendBytes(byte[] bytes)
        {
            int num2;
            byte[] buffer = BitConverter.GetBytes(bytes.Length);
            this.client.Send(buffer);
            this.client.Receive(this.FlagBytes);
            for (int i = 0; i < bytes.Length; i += num2)
            {
                num2 = bytes.Length - i;
                if (num2 > 0x400)
                {
                    num2 = 0x400;
                }
                this.client.Send(bytes, i, num2, SocketFlags.None);
                this.client.Receive(this.FlagBytes);
            }
            this.client.Send(this.FlagBytes);
        }

        private void SendBytesWithProgress(byte[] bytes, Guid id)
        {
            int num3;
            byte[] buffer = BitConverter.GetBytes(bytes.Length);
            int bytesLoaded = 0;
            this.OnSendProgressChanged(id, bytesLoaded, bytes.Length);
            this.client.Send(buffer);
            this.client.Receive(this.FlagBytes);
            for (int i = 0; i < bytes.Length; i += num3)
            {
                num3 = bytes.Length - i;
                if (num3 > 0x400)
                {
                    num3 = 0x400;
                }
                this.client.Send(bytes, i, num3, SocketFlags.None);
                this.client.Receive(this.FlagBytes);
                bytesLoaded += num3;
                this.OnSendProgressChanged(id, bytesLoaded, bytes.Length);
            }
            this.client.Send(this.FlagBytes);
        }

        private void SendObject(object obj, Guid id)
        {
            byte[] bytes = id.ToByteArray();
            byte[] buffer2 = this.GetBytes(obj);
            this.SendBytes(bytes);
            this.SendBytesWithProgress(buffer2, id);
        }

        public void SendRequest(Request request)
        {
            this.SendObject(request, request.Id);
        }

        public void SendResponse(Response response)
        {
            this.SendObject(response, response.RequestId);
        }

        private byte[] FlagBytes
        {
            get
            {
                if (this.flagValue == null)
                {
                    this.flagValue = new byte[] { 0x55 };
                }
                return this.flagValue;
            }
        }
    }
}

