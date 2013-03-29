using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpConnection.Protocol
{
    class ConnectedMessage : Message
    {
        public ConnectedMessage() :
            base(MessageType.Connected)
        {
        }

        public override void Serialise(BinaryWriter writer)
        {
        }

        public override bool Serialise(BinaryReader reader)
        {
            return true;
        }
    }
}
