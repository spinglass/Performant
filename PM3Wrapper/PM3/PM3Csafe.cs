using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PM3Wrapper
{
    internal class PM3Csafe
    {
        // tkcmdsetCSAFE_get_dll_version
        [DllImport(@"PM3\PM3CsafeCP.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort tkcmdsetCSAFE_get_dll_version();

        // tkcmdsetCSAFE_get_error_name
        [DllImport(@"PM3\PM3CsafeCP.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void tkcmdsetCSAFE_get_error_name(
            [In] ushort erecoderor,
            [Out, MarshalAs(UnmanagedType.LPStr)] StringBuilder nameptr,
            [In] ushort namelen);

        // tkcmdsetCSAFE_get_error_text
        [DllImport(@"PM3\PM3CsafeCP.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void tkcmdsetCSAFE_get_error_text(
            [In] ushort ecode,
            [Out, MarshalAs(UnmanagedType.LPStr)] StringBuilder textptr,
            [In] ushort textlen);

        // tkcmdsetCSAFE_get_cmd_name
        [DllImport(@"PM3\PM3CsafeCP.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort tkcmdsetCSAFE_get_cmd_name(
            [In] byte cmd,
            [Out, MarshalAs(UnmanagedType.LPStr)] StringBuilder textptr,
            [In] ushort textlen);

        //tkcmdsetCSAFE_get_cmd_text
        [DllImport(@"PM3\PM3CsafeCP.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort tkcmdsetCSAFE_get_cmd_text(
            [In] byte cmd,
            [Out, MarshalAs(UnmanagedType.LPStr)] StringBuilder textptr,
            [In] ushort textlen);

        //tkcmdsetCSAFE_get_cmd_data_types
        [DllImport(@"PM3\PM3CsafeCP.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort tkcmdsetCSAFE_get_cmd_data_types(
            [In] byte cmd,
            [Out] out byte cmd_type,
            [In, Out] ref byte num_cmd_data_types,
            [In] byte[] cmd_data_type,
            [In, Out] ref byte num_rsp_data_types,
            [In] byte[] rsp_data_type);

        // tkcmdsetCSAFE_init_protocol
        [DllImport(@"PM3\PM3CsafeCP.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort tkcmdsetCSAFE_init_protocol(
            [In] ushort timeout);

        // tkcmdsetCSAFE_command
        [DllImport(@"PM3\PM3CsafeCP.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort tkcmdsetCSAFE_command(
           [In] ushort unit_address,
           [In] ushort cmd_data_size,
           [In] uint[] cmd_data,
           [In, Out] ref ushort rsp_data_size,
           [In] uint[] rsp_data);

        // tkcmdsetCSAFE_get_status
        [DllImport(@"PM3\PM3CsafeCP.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort tkcmdsetCSAFE_get_status();
    }
}
