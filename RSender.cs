using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace UDP_TEST_SERVER
{
    class RSender
    {
        private int sendIndex;
        private EndPoint remoteEndPoint;
        private Socket server_Socket;
        private Dictionary<int, byte[]> SendData;
        public RSender(Socket socket, EndPoint remote)
        {
            sendIndex = 0;
            server_Socket = socket;
            remoteEndPoint = remote;
            SendData = new Dictionary<int, byte[]>();
        }
        public void ReceiveMessage(int replyNumber)
        {
            SendData.Remove(replyNumber);
        }

        public void SendMessage(string s)
        {
            byte[] sendBytes = BitConverter.GetBytes(sendIndex);
            byte[] strBytes = System.Text.Encoding.UTF8.GetBytes(s);
            byte[] newBytes = sendBytes.Concat(strBytes).ToArray();
            SendData.Add(sendIndex, newBytes);
            sendIndex++;
            new Thread(SendToEnd) { IsBackground = true }.Start();
        }

        private void SendToEnd()
        {
            int times = 0;
            while (SendData.Count > 0)
            {
                times++;
                if (times > 30) //弱网断网情况，等待超过1s了，且丢包率极高
                {
                    Console.WriteLine("网络延迟过高");
                    return;
                }
                foreach(KeyValuePair<int,byte[]> k in SendData)
                {
                    server_Socket.SendTo(k.Value, remoteEndPoint);
                }
                Thread.Sleep(30);
            }
            return;
        }
    }
}

