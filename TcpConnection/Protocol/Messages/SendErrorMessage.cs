using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpConnection.Protocol
{
    class SendErrorMessage : Message
    {
        public SendErrorMessage() :
            base(MessageType.SendError)
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
