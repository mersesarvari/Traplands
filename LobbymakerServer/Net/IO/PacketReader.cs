using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LobbymakerServer
{
    public class PacketReader : BinaryReader
    {
        private NetworkStream _ns;
        public PacketReader(NetworkStream ns) : base(ns)
        {
            _ns = ns;
        }

        public string ReadMessage()
        {            
            byte[] msgBuffer;
            var length = ReadInt32();
            //var length = 1024;
            msgBuffer = new byte[length];
            _ns.Read(msgBuffer, 0, length);

            var message = Encoding.UTF8.GetString(msgBuffer);
            return message;
            
        }
    }
}
