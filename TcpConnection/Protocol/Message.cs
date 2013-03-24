using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpConnection.Protocol
{
    abstract class Message
    {
        public Message(MessageType type)
        {
            m_Type = type;
        }

        public MessageType Type { get { return m_Type; } }

        public abstract void Serialise(BinaryWriter writer);
        public abstract bool Serialise(BinaryReader reader);

        private MessageType m_Type;
    }
}
