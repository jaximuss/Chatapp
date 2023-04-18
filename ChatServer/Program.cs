using ChatServer;
using ChatServer.Net.IO;
using System.Net;
using System.Net.Sockets;

class Program
{
    static TcpListener _listener;

    static List<Client> _users;

    static void Main(string[] args)
    {
        _users = new List<Client>();
        _listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 7891);
        _listener.Start();

        //we want it to be an infinite loop so while its true always accept and create a new user

        while (true)
        {
            var client = new Client(_listener.AcceptTcpClient());
            _users.Add(client);

            //broacast the connection to everyone on the server

        }

        static void BroadcastConnection()
        {
            foreach (var user in _users)
            {
                foreach (var usr in _users)
                {
                    var broadcastPacket = new PacketBuilder();
                    broadcastPacket.WriteOpCode(1); 
                    broadcastPacket.WriteMessage(broadcastPacket.ToString());
                }
               
            }
        }   
    }

}