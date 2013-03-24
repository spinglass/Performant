using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpConnection.Protocol
{
    class HandshakeMessage : Message
    {
        public HandshakeMessage() :
            base(MessageType.Handshake)
        {
        }

        public override void Serialise(BinaryWriter writer)
        {
            writer.Write(s_Tag);
        }

        public override bool Serialise(BinaryReader reader)
        {
            char[] tag = reader.ReadChars(4);
            bool success = ( tag[0] == s_Tag[0] && tag[0] == s_Tag[0] && tag[0] == s_Tag[0] && tag[0] == s_Tag[0]);
            return success;
        }

        private static char[] s_Tag = { 'P', 'F', 'M', 'T' };
    }
}
