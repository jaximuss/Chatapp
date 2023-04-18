using ChatClient.Net.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.Net
{
    class Server
    {
        //this imports systems.clien.
        TcpClient _client;

        public Server()
        {
            _client  = new TcpClient();
        }
        public void ConnectToServer(string userName)
        {
            // we dont want to connect to server if we are already connected
            if(!_client.Connected )
            {
                _client.Connect("127.0.0.1" ,7891);

                var connectPacket = new PacketBuilder();
                connectPacket.WriteOpCode(0);
                connectPacket.WriteString(userName);
                _client.Client.Send(connectPacket.GetPacketBytes());
            }
        }
    }
}
