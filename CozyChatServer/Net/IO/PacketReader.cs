using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CozyChatServer.Net.IO
{
    internal class PacketReader : BinaryReader
    {
        private NetworkStream _ns;

        public PacketReader(NetworkStream ns) : base(ns)
        {
            _ns = ns;
        }

        public string ReadMessage()
        {
            byte[] msgBuffer; // temporary buffer
            var lenght = ReadInt32(); // size of the actual lenght that is recieved from the server
            msgBuffer = new byte[lenght]; // the length of the byte array is going to be whatever is recieved from the server
            _ns.Read(msgBuffer, 0, lenght); // read into the msg buffer with offset of 0 and length of the actual packet.

            var msg = Encoding.ASCII.GetString(msgBuffer);

            return msg;

            // basically all this read the upload of the packet
        }

    }
}
