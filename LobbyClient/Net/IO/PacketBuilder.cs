using System;
using System.IO;
using System.Text;

namespace LobbyClient
{
    public class PacketBuilder
    {
        MemoryStream _ms;
        public PacketBuilder()
        {
            _ms = new MemoryStream();

        }
        public void WriteOptCode(byte opcode)
        {
            _ms.WriteByte(opcode);
        }
        public void WriteMessage(string msg)
        {
            var smgLenght = msg.Length;
            _ms.Write(BitConverter.GetBytes(msg.Length));
            _ms.Write(Encoding.UTF8.GetBytes(msg));

        }

        public byte[] GetPacketbytes()
        {
            return _ms.ToArray();
        }
    }
}
