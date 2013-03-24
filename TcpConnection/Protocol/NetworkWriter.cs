using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TcpConnection.Protocol
{
    class NetworkWriter
    {
        public NetworkWriter()
        {
            m_Buffer = new byte[255];
            m_Writer = new BinaryWriter(new MemoryStream(m_Buffer));
        }

        public NetworkStream Stream
        {
            get { return m_NetworkStream; }
            set { m_NetworkStream = value; }
        }

        public bool Send(Message msg)
        {
            if (m_NetworkStream != null)
            {
                // Prepare the message
                m_Writer.BaseStream.Position = 0;
                msg.Serialise(m_Writer);
                int size = (int)m_Writer.BaseStream.Position;

                // Send the message
                try
                {
                    m_NetworkStream.WriteByte((byte)msg.Type);
                    m_NetworkStream.WriteByte((byte)size);
                    m_NetworkStream.Write(m_Buffer, 0, size);

                    return true;
                }
                catch (SocketException e)
                {
                    Debug.WriteLine("[NetworkWriter.Send] " + e.Message);
                }
            }

            return false;
        }

        private NetworkStream m_NetworkStream;
        private byte[] m_Buffer;
        private BinaryWriter m_Writer;
    }
}
