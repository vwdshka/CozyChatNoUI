using CozyChatClient.NET.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CozyChatClient.NET
{
    class Server
    {
        TcpClient _client;
        public PacketReader PacketReader;

        public event Action connectedEvent;
        public event Action msgReceivedEvent;
        public event Action userDisconnectEvent;


        PacketBuilder _packetBuilder;

        public Server()
        {
            _client = new TcpClient();
        }

        public void ConnectToServer(string username)
        {
            if (!_client.Connected)
            {
                _client.Connect("127.0.0.1", 7891); // Connect to the server
                PacketReader = new PacketReader(_client.GetStream()); // Get the stream from the client

                if (!string.IsNullOrEmpty(username))
                {
                    var connectPacket = new PacketBuilder(); // Create a new packet
                    connectPacket.WriteOpCode(0); // Write the opcode
                    connectPacket.WriteMessage(username); // Write the username
                    _client.Client.Send(connectPacket.GetPacketBytes()); // Send the packet
                }
                ReadPackets();
                
            }
        }

        private void ReadPackets() // offload this to another thread (in order not to deadlock the app but just to offload the data to another dir 
        {
            Task.Run(() => 
            { 
                while (true)
                {
                    var opcode = PacketReader.ReadByte();
                    switch (opcode)
                    {
                        case 1: connectedEvent?.Invoke(); 
                            break;

                        case 5:
                            msgReceivedEvent?.Invoke();
                            break;

                        case 10:
                            userDisconnectEvent?.Invoke();
                            break;
                        default:
                            Console.WriteLine("working fine");
                            break;
                    }
                }
            });
        }

        public void SendMessageToServer(string message)
        {
            var messagePacket = new PacketBuilder();
            messagePacket.WriteOpCode(5);
            messagePacket.WriteMessage(message);
            _client.Client.Send(messagePacket.GetPacketBytes());
        }
    }
}
