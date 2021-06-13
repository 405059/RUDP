using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace UDP_TEST_SERVER
{
    class RReveiver
    {
        private int receiveIndex;
        private EndPoint remoteEndPoint;
        private Socket server_Socket;
        private Dictionary<int, string> AdvancedMessageDict;
        public RReveiver(Socket socket, EndPoint remote)
        {
            receiveIndex = 0;
            server_Socket = socket;
            remoteEndPoint = remote;
            AdvancedMessageDict = new Dictionary<int, string>();
        }
        public void ReceiveMessage(int index, string message)
        {
            if (index == receiveIndex) //以第0条消息判断一下符合逻辑
            {
                ParseMessage(message);
                receiveIndex++;
                while (AdvancedMessageDict.Count > 0)
                {
                    if (AdvancedMessageDict.ContainsKey(receiveIndex))
                    {
                        message = AdvancedMessageDict[receiveIndex];
                        ParseMessage(message);
                        AdvancedMessageDict.Remove(receiveIndex);
                        receiveIndex++;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else if (index > receiveIndex) 
            {
                if (!AdvancedMessageDict.ContainsKey(index))
                {
                    AdvancedMessageDict.Add(index, message);
                }

            }
            Reply(index);
        }

        private void ParseMessage(string msg)
        {
            Console.WriteLine("接受到一条消息" + msg);
        }

        private void Reply(int index)
        {
            byte[] confirmer = BitConverter.GetBytes(-1);
            byte[] confirmNum = BitConverter.GetBytes(index);
            byte[] data = confirmer.Concat(confirmNum).ToArray();
            server_Socket.SendTo(data, remoteEndPoint);
        }
    }
}
