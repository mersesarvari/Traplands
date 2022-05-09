using System;
using System.IO;
using System.Text;

namespace Server
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
            _ms.Write(BitConverter.GetBytes(smgLenght));
            _ms.Write(Encoding.ASCII.GetBytes(msg));

        }

        public byte[] GetPacketbytes()
        {
            return _ms.ToArray();
        }
    }
}
