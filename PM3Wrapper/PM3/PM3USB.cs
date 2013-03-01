using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PM3Wrapper
{
    internal class PM3USB
    {
        // PM3Event
        public delegate void PM3Event(ushort a, byte b);

        // tkcmdsetUSB_get_dll_version
        [DllImport(@"PM3\PM3USBCP.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort tkcmdsetUSB_get_dll_version();

        // tkcmdsetUSB_init
        [DllImport(@"PM3\PM3USBCP.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort tkcmdsetUSB_init();

        // tkcmdsetUSB_get_error_name
        [DllImport(@"PM3\PM3USBCP.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void tkcmdsetUSB_get_error_name(
            ushort erecoderor,
            [MarshalAs(UnmanagedType.LPStr)] StringBuilder nameptr,
            ushort namelen);

        // tkcmdsetUSB_get_error_text
        [DllImport(@"PM3\PM3USBCP.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void tkcmdsetUSB_get_error_text(
            ushort ecode,
            [MarshalAs(UnmanagedType.LPStr)] StringBuilder textptr,
            ushort textlen);

        // tkcmdsetUSB_register_events
        [DllImport(@"PM3\PM3USBCP.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort tkcmdsetUSB_register_events(PM3Event callback);

        // tkcmdsetUSB_unregister_events
        [DllImport(@"PM3\PM3USBCP.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort tkcmdsetUSB_unregister_events();
    }
}
