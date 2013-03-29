using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpConnection.Protocol
{
    enum MessageType
    {
        Invalid = 0,
        None,

        Handshake,
        Connected,
        Disconnected,
        Command,
        SendError,
    }
}
