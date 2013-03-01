using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PM3Wrapper
{
    public class PM3Exception : Exception
    {
        internal PM3Exception(string message)
            : base(message)
        {
        }
    }

    public class CsafeException : PM3Exception
    {
        static internal void Validate(ushort error)
        {
            if (error != 0)
            {
                StringBuilder name = new StringBuilder(20);
                PM3Csafe.tkcmdsetCSAFE_get_error_name(error, name, (ushort)name.Capacity);

                StringBuilder text = new StringBuilder(400);
                PM3Csafe.tkcmdsetCSAFE_get_error_text(error, text, (ushort)text.Capacity);

                throw new CsafeException(string.Format("{0} : {1}", name, text));
            }
        }

        private CsafeException(string message)
            : base(message)
        {
        }
    }

    public class DDIException : PM3Exception
    {
        static internal void Validate(ushort error)
        {
            if (error != 0)
            {
                StringBuilder name = new StringBuilder(20);
                PM3DDI.tkcmdsetDDI_get_error_name(error, name, (ushort)name.Capacity);

                StringBuilder text = new StringBuilder(400);
                PM3DDI.tkcmdsetDDI_get_error_text(error, text, (ushort)text.Capacity);

                throw new DDIException(string.Format("{0} : {1}", name, text));
            }
        }

        private DDIException(string message)
            : base(message)
        {
        }
    }

    public class USBException : PM3Exception
    {
        static internal void Validate(ushort error)
        {
            if (error != 0)
            {
                StringBuilder name = new StringBuilder(20);
                PM3USB.tkcmdsetUSB_get_error_name(error, name, (ushort)name.Capacity);

                StringBuilder text = new StringBuilder(400);
                PM3USB.tkcmdsetUSB_get_error_text(error, text, (ushort)text.Capacity);

                throw new USBException(string.Format("{0} : {1}", name, text));
            }
        }

        private USBException(string message)
            : base(message)
        {
        }
    }
}
