using System;

namespace Common
{
    public interface IConnection
    {
        bool IsOpen { get; }
        ConnectionState State { get; }
        bool Open();
        void Close();
        bool SendCSAFECommand(uint[] cmdData, int cmdDataCount, uint[] rspData, ref int rspDataCount);
    }
}
