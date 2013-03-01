using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PM3Wrapper
{
    public class PM3
    {
        static public string s_ProductName = "Concept2 Performance Monitor 3 (PM3)";

        public PM3()
        {
        }

        public void Start()
        {
            ushort error;

            error = PM3DDI.tkcmdsetDDI_init();
            DDIException.Validate(error);

            error = PM3Csafe.tkcmdsetCSAFE_init_protocol(1000);
            CsafeException.Validate(error);
        }

        public void Stop()
        {
            ushort error = PM3DDI.tkcmdsetDDI_shutdown_all();
            DDIException.Validate(error);
        }

        public int DiscoverUnits()
        {
            ushort numUnits;
            ushort error = PM3DDI.tkcmdsetDDI_discover_pm3s(s_ProductName, 0, out numUnits);
            DDIException.Validate(error);
            return numUnits;
        }

        public UnitInfo GetUnitInfo(int port)
        {
            ushort error;

            StringBuilder hwVersion = new StringBuilder(20);
            error = PM3DDI.tkcmdsetDDI_hw_version((ushort)port, hwVersion, (ushort)hwVersion.Capacity);
            DDIException.Validate(error);

            StringBuilder fwVersion = new StringBuilder(20);
            error = PM3DDI.tkcmdsetDDI_fw_version((ushort)port, fwVersion, (ushort)fwVersion.Capacity);
            DDIException.Validate(error);

            StringBuilder serialNumber = new StringBuilder(16);
            error = PM3DDI.tkcmdsetDDI_serial_number((ushort)port, serialNumber, (byte)serialNumber.Capacity);
            DDIException.Validate(error);

            UnitInfo info = new UnitInfo();
            info.hwVersion = hwVersion.ToString();
            info.fwVersion = fwVersion.ToString();
            info.serialNumber = serialNumber.ToString();
            return info;
        }

        public void SendCSAFECommand(int port, uint[] cmdData, int cmdDataCount, uint[] rspData, ref int rspDataCount)
        {
            ushort tmpRspDataCount = (ushort)rspDataCount;
            ushort error = PM3Csafe.tkcmdsetCSAFE_command((ushort)port, (ushort)cmdDataCount, cmdData, ref tmpRspDataCount, rspData);
            rspDataCount = tmpRspDataCount;
            CsafeException.Validate(error);
        }
    }
}
