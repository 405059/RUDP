using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;
using System.Linq;

namespace UDP_TEST_SERVER
{
    /// <summary>
    /// 服务器端
    /// </summary>
    class Program
    {
        static Socket udpServer = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        static Dictionary<EndPoint, RUDP> ClientList = new Dictionary<EndPoint, RUDP>();
        static void Main(string[] args)
        {           
            udpServer.Bind(new IPEndPoint(IPAddress.Parse("192.168.43.62"), 8888));
            new Thread(Receive) { IsBackground = true }.Start();
            Console.ReadKey();
        }
        static void Receive()
        {
            while (true)
            {
                byte[] dataBuff = new byte[1024];
                EndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
                int len = udpServer.ReceiveFrom(dataBuff, ref remoteEndPoint);
                RUDP client;
                if (!ClientList.ContainsKey(remoteEndPoint))
                {
                    if (true)//此处加入对远端合法的判断
                    {
                        client = new RUDP(udpServer, remoteEndPoint);
                        ClientList.Add(remoteEndPoint, client);
                    }
                }
                else
                {
                    client = ClientList[remoteEndPoint];
                }
                client.ReceiveFrom(dataBuff, len);
            }
        }
    }
    
}
