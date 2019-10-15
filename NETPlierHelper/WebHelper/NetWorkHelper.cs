// FileInfo
// File:"NetWorkHelper.cs" 
// Solution:"Solarfist"
// Project:"DotNET Framework Helper" 
// Create:"2019-10-10"
// Author:"Michael G"
// https://github.com/MichaelGAjani/Solarfist
//
// License:GNU General Public License v3.0
// 
// Version:"1.0"
// Function:Socket
// 1.CheckPortValid(int port)
// 2.IPAddress ToIPAddress(string ip) 
// 3.LocalHostName
// 4.IP
// 5.GetClientIP(Socket clientSocket)
// 6.CreateIPEndPoint(string ip, int port) 
// 7.CreateTcpListener
// 8.CreateTcpListener(string ip, int port)
// 9.CreateTcpSocket
// 10.CreateUdpSocket
// 11.GetLocalPoint(TcpListener tcpListener) 
// 12.GetLocalPointIP(TcpListener tcpListener)
// 13.GetLocalPointPort(TcpListener tcpListener)
// 14.GetLocalPoint(Socket socket)
// 15.GetLocalPointIP(Socket socket) 
// 16.GetLocalPointPort(Socket socket)
// 17.BindEndPoint(Socket socket, IPEndPoint endPoint)
// 18.BindEndPoint(Socket socket, string ip, int port)
// 19.StartListen(Socket socket, int port, int backLog = 100)
// 20.StartListen(Socket socket, string ip, int port, int maxConnection)
// 21.Connect(Socket socket, string ip, int port)
// 22.SendMsg(Socket socket, byte[] msg) 
// 23.SendMsg(Socket socket, string msg)
// 24.ReceiveMsg(Socket socket, byte[] buffer) 
// 25.ReceiveMsg(Socket socket)
// 26.Close(Socket socket)
//
// File Lines:142

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Jund.NETHelper.WebHelper
{
    public class NetWorkHelper
    {
        public static bool CheckPortValid(int port) => port > 0 && port <= 65535; 
        public static IPAddress ToIPAddress(string ip)=> IPAddress.Parse(ip);
        public static string LocalHostName=> Dns.GetHostName();
        public static (string LANIP,string WANIP) IP
        {
            get
            {
                (string LANIP, string WANIP) ip=("","");
                //获取本机的IP列表,IP列表中的第一项是局域网IP，第二项是广域网IP
                IPAddress[] addressList = Dns.GetHostEntry(Dns.GetHostName()).AddressList;

                if (addressList.Length > 0) ip.LANIP = addressList[0].ToString();
                if (addressList.Length > 1) ip.WANIP = addressList[1].ToString();
                
                return ip;
            }
        }
        public static string GetClientIP(Socket clientSocket)
        {
            IPEndPoint client = (IPEndPoint)clientSocket.RemoteEndPoint;
            return client.Address.ToString();
        }
        public static IPEndPoint CreateIPEndPoint(string ip, int port) => new IPEndPoint(ToIPAddress(ip), port);
        public static TcpListener CreateTcpListener() => new TcpListener(new IPEndPoint(IPAddress.Any, 0));         
        public static TcpListener CreateTcpListener(string ip, int port)=> new TcpListener(new IPEndPoint(ToIPAddress(ip), port));      
        public static Socket CreateTcpSocket()=> new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);     
        public static Socket CreateUdpSocket()=> new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);       
        public static IPEndPoint GetLocalPoint(TcpListener tcpListener)=> (IPEndPoint)tcpListener.LocalEndpoint;
        public static string GetLocalPointIP(TcpListener tcpListener) => GetLocalPoint(tcpListener).Address.ToString();
        public static int GetLocalPointPort(TcpListener tcpListener) => GetLocalPoint(tcpListener).Port;
        public static IPEndPoint GetLocalPoint(Socket socket)=> (IPEndPoint)socket.LocalEndPoint;
        public static string GetLocalPointIP(Socket socket)=>GetLocalPoint(socket).Address.ToString();
        public static int GetLocalPointPort(Socket socket) => GetLocalPoint(socket).Port;
        public static void BindEndPoint(Socket socket, IPEndPoint endPoint)
        {
            if (!socket.IsBound)
            {
                socket.Bind(endPoint);
            }
        }
        public static void BindEndPoint(Socket socket, string ip, int port)
        {
            IPEndPoint endPoint = CreateIPEndPoint(ip, port);

            if (!socket.IsBound)
            {
                socket.Bind(endPoint);
            }
        }
        public static void StartListen(Socket socket, int port,int backLog=100)
        {
            IPEndPoint localPoint = CreateIPEndPoint(LocalHostName, port);
            BindEndPoint(socket, localPoint);
            socket.Listen(backLog);
        }
        public static void StartListen(Socket socket, string ip, int port, int maxConnection)
        {
            BindEndPoint(socket, ip, port);
            socket.Listen(maxConnection);
        }
        public static bool Connect(Socket socket, string ip, int port)
        {
            try
            {
                socket.Connect(ip, port);
                return socket.Poll(-1, SelectMode.SelectWrite);
            }
            catch (SocketException ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static void SendMsg(Socket socket, byte[] msg)=> socket.Send(msg, msg.Length, SocketFlags.None);
        public static void SendMsg(Socket socket, string msg)
        {
            byte[] buffer = Encoding.Default.GetBytes(msg);
            socket.Send(buffer, buffer.Length, SocketFlags.None);
        }
        public static void ReceiveMsg(Socket socket, byte[] buffer)=> socket.Receive(buffer);
        public static string ReceiveMsg(Socket socket)
        {
            byte[] buffer = new byte[8192];
            int receiveCount = socket.Receive(buffer);

            byte[] tempBuffer = new byte[receiveCount];
            Buffer.BlockCopy(buffer, 0, tempBuffer, 0, receiveCount);
            return DataTypeHelper.DataTypeExtMethods.Base64EncodeBytes(tempBuffer);
        }
        public static void Close(Socket socket)=> socket.Shutdown(SocketShutdown.Both);        
    }
}
