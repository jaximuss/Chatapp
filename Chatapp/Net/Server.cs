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
        public PacketReader packetReader;

        //EVENTS
        public event Action connectedEvent;
        public event Action messageRecievedEvent;
        public event Action userDisconnectedEvent;


        public Server()
        {
            _client = new TcpClient();
        }
        public void ConnectToServer(string userName)
        {
            // we dont want to connect to server if we are already connected
            if (!_client.Connected)
            {
                _client.Connect("127.0.0.1", 7891);
                packetReader = new PacketReader(_client.GetStream());

                if (!string.IsNullOrEmpty(userName))
                {
                    var connectPacket = new PacketBuilder();
                    connectPacket.WriteOpCode(0);
                    connectPacket.WriteMessaage(userName);
                    _client.Client.Send(connectPacket.GetPacketBytes());
                }
                ReadPackets();

            }
        }

        private void ReadPackets()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    var opCode = packetReader.ReadByte();
                    switch (opCode)
                    {
                        case 1:
                            connectedEvent?.Invoke();
                            break;

                        case 5:
                            messageRecievedEvent?.Invoke();
                            break;
                        case 10:
                            userDisconnectedEvent?.Invoke();
                            break;
                        default:
                            Console.WriteLine("ah.. yes");
                            break;
                    }
                }
            });
        }


        public void SendMessageToServer(string message)
        {
            var messagePacket = new PacketBuilder();
            messagePacket.WriteOpCode(5);
            messagePacket.WriteMessaage(message);

            //SEND MESSAGE
            _client.Client.Send(messagePacket.GetPacketBytes());
        }
    }
}
