using System;
using System.Runtime.InteropServices;

namespace Wiimote_AutoPair
{
    public class BTApi
    {
        public const int BLUETOOTH_MAX_NAME_SIZE = 248;

        public const UInt32 BLUETOOTH_SERVICE_DISABLE = 0x00;
        public const UInt32 BLUETOOTH_SERVICE_ENABLE = 0x01;
        public static Guid HumanInterfaceDeviceServiceClass_UUID =
            new Guid(0x00001124, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        [StructLayout(LayoutKind.Sequential)]
        public struct BLUETOOTH_FIND_RADIO_PARAMS
        {
            public uint dwSize;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct BLUETOOTH_RADIO_INFO
        {
            public uint dwSize;
            public BLUETOOTH_ADDRESS address;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = BLUETOOTH_MAX_NAME_SIZE)]
            public string szName;
            public UInt32 ulClassofDevice;
            public UInt16 lmpSubversion;
            public UInt16 manufacturer;
        }

        [StructLayout(LayoutKind.Explicit)]
        public unsafe struct BLUETOOTH_ADDRESS
        {
            [FieldOffset(0)]
            public UInt64 ullLong;
            [FieldOffset(0)]
            public fixed byte rgBytes[6];
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct BLUETOOTH_DEVICE_SEARCH_PARAMS
        {
            public uint dwSize;
            public bool fReturnAuthenticated;
            public bool fReturnRemembered;
            public bool fReturnUnknown;
            public bool fReturnConnected;
            public bool fIssueInquiry;
            public byte cTimeoutMultiplier;
            public IntPtr hRadio;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct BLUETOOTH_DEVICE_INFO
        {
            public uint dwSize;
            public BLUETOOTH_ADDRESS Address;
            public uint ulClassofDevice;
            public bool fConnected;
            public bool fRemembered;
            public bool fAuthenticated;
            public SYSTEMTIME stLastSeen;
            public SYSTEMTIME stLastUsed;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = BLUETOOTH_MAX_NAME_SIZE)]
            public string szName;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SYSTEMTIME
        {
            public Int16 wYear;
            public Int16 wMonth;
            public Int16 wDayOfWeek;
            public Int16 wDay;
            public Int16 wHour;
            public Int16 wMinute;
            public Int16 wSecond;
            public Int16 wMilliseconds;
        }

        [DllImport("mybthprops.dll")]
        public extern static IntPtr MyBluetoothFindFirstRadio(
         ref BLUETOOTH_FIND_RADIO_PARAMS btfrp,
         out IntPtr hRadio
        );

        [DllImport("mybthprops.dll")]
        public extern static bool MyBluetoothFindNextRadio(
            IntPtr hFind,
            out IntPtr hRadio
        );

        [DllImport("mybthprops.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static UInt32 MyBluetoothGetRadioInfo(
            IntPtr hRadio,
            ref BLUETOOTH_RADIO_INFO pRadioInfo
        );

        [DllImport("mybthprops.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static UInt32 MyBluetoothFindRadioClose(IntPtr hFind);

        [DllImport("mybthprops.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static IntPtr MyBluetoothFindFirstDevice(
            ref BLUETOOTH_DEVICE_SEARCH_PARAMS btsp,
            ref BLUETOOTH_DEVICE_INFO btdi
            );

        [DllImport("mybthprops.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static bool MyBluetoothFindNextDevice(
            IntPtr hFind,
            ref BLUETOOTH_DEVICE_INFO btdi
        );

        [DllImport("mybthprops.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static bool MyBluetoothFindDeviceClose(IntPtr hFind);

        [DllImport("mybthprops.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static bool MyCloseHandle(IntPtr hObj);

        [DllImport("mybthprops.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static UInt32 MyBluetoothRemoveDevice(ref BLUETOOTH_ADDRESS address);

        [DllImport("mybthprops.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static UInt32 MyBluetoothSetServiceState(
            IntPtr hRadio,
            ref BLUETOOTH_DEVICE_INFO btdi,
            ref Guid GUIDService,
            UInt32 dwServiceFlags
        );

        [DllImport("mybthprops.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static UInt32 MyBluetoothAuthenticateDevice(
                        IntPtr hwnd, IntPtr hRadio, ref BLUETOOTH_DEVICE_INFO btdi,
                        char[] passkey, UInt32 passkeylen);

        [DllImport("mybthprops.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static UInt32 MyBluetoothEnumerateInstalledServices(
            IntPtr hRadio, ref BLUETOOTH_DEVICE_INFO btdi,
            ref UInt32 pcServices, Guid[] pGuidServices);

    }
}
