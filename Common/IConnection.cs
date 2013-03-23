using System;

namespace Common
{
    public interface IConnection
    {
        bool IsOpen { get; }
        bool Open();
        void Close();
        bool SendCSAFECommand(uint[] cmdData, int cmdDataCount, uint[] rspData, ref int rspDataCount);
    }
}
