using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.Net.IO
{
    class PacketReader : BinaryReader
    {
        NetworkStream _networkStream;

        public PacketReader(NetworkStream networkStream) : base(networkStream)
        {
            _networkStream = networkStream;
        }

        public string ReadMessage()
        {
            byte[] messageBuffer;

            int length = ReadInt32();

            messageBuffer = new byte[length];

            _networkStream.Read(messageBuffer, 0, length);

            var message = Encoding.ASCII.GetString(messageBuffer);

            return message;
        }
    }
}
