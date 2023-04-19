using ChatServer.Net.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer
{
    class Client
    {

        public string UserName { get; set; }
        public Guid UID { get; set; }

        public TcpClient ClientSocket { get; set; }

        PacketReader _packetReader;

        public Client(TcpClient client)
        {
            ClientSocket = client;

            //GENERATE A NEW ID FOR THE USER
            UID = Guid.NewGuid();
            _packetReader = new PacketReader(ClientSocket.GetStream());

            var opCode = _packetReader.ReadByte();

            UserName = _packetReader.ReadMessage();


            Console.WriteLine($"[{DateTime.Now}]: client as connected with the username : {UserName}");

            Task.Run(() => process());
        }

        void process()
        {
            while (true)
            {
                try
                {
                    var opCode =  _packetReader.ReadByte();

                    switch (opCode)
                    {
                        case 5:
                            var message = _packetReader.ReadMessage();
                            Console.WriteLine($"[{DateTime.Now}]: Message Recieved! {message}");
                            Program.BroadcastMessage($"[{DateTime.Now}]: {UserName} :- {message}");
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine($"[{UID.ToString()}] : Disconnected");
                    Program.BroadcastDisconnect(UID.ToString());
                    ClientSocket.Close();
                    break;
                }
            }
        }
    }
}
