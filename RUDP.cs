using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace UDP_TEST_SERVER
{
    //一个客户端对应一个RUDP去处理他的连接
    class RUDP
    {
        private RReveiver rReveiver;
        private RSender rSender;
        private Socket server_Socket;
        private EndPoint remoteEndPoint;
        public RUDP(Socket server, EndPoint remote)
        {
            server_Socket = server;
            rReveiver = new RReveiver(server, remote);
            rSender = new RSender(server, remote);
            remoteEndPoint = remote;
        }

        public void ReceiveFrom(byte[] buffer, int len)
        {
            int index = BitConverter.ToInt32(buffer, 0);
            
            if (index < 0)
            {
                int replyNumber = BitConverter.ToInt32(buffer, 4);
                rSender.ReceiveMessage(replyNumber);
            }
            else
            {
                string message = System.Text.Encoding.UTF8.GetString(buffer, 4, len - 4);
                rReveiver.ReceiveMessage(index, message);
            }
        }
        public void SendTo(string message)
        {
            rSender.SendMessage(message);
        }
    }
}
