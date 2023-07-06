using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozyChatServer.Net.IO
{
    internal class PacketBuilder
    {
        // This class is going to allow us to append data to a memory stream which will be sent to the server. (In the form of bytes of course)

        MemoryStream _ms;
        public PacketBuilder()
        {
            _ms = new MemoryStream();

        }

        public void WriteOpCode(byte opcode)
        {
            _ms.WriteByte(opcode);
        }

        public void WriteMessage(string msg)
        {
            var msgLen = msg.Length;
            _ms.Write(BitConverter.GetBytes(msgLen));
            _ms.Write(Encoding.ASCII.GetBytes(msg));
        }

        public byte[] GetPacketBytes()
        {
            return _ms.ToArray();
        }
    }
}
