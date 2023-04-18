using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.Net.IO
{
    class PacketBuilder
    {

        MemoryStream _memory;
        public PacketBuilder()
        {
            _memory = new MemoryStream();
        }

        public void WriteOpCode(byte opcode)
        {
            _memory.WriteByte(opcode);
        }

        public void WriteString(string message)
        {
            int messageLength = message.Length;
            _memory.Write(BitConverter.GetBytes(messageLength));
            _memory.Write(Encoding.ASCII.GetBytes(message));    
        }

        public byte[] GetPacketBytes()
        {
            return _memory.ToArray();
        }
    }
}
