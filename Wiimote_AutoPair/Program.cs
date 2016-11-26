using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using static Wiimote_AutoPair.BTApi;

namespace Wiimote_AutoPair
{
    class Program
    {
        private static BackgroundWorker backgroundWorker1;
        private static bool showDevices = false;
        private static bool wiimoteConnected = false;
        private static int btdiCount = 0;

        static void Main(string[] args)
        {
            backgroundWorker1 = new BackgroundWorker();
            backgroundWorker1.DoWork += backgroundWorker1_DoWork;

            radio_search();
            backgroundWorker1.RunWorkerAsync();

            char com = Console.ReadKey().KeyChar;
            while (com != 'q')
            {
                switch (com)
                {
                    case 'd':
                        refresh_list();
                        System.Threading.Thread.Sleep(100);
                        lock (btdis)
                        {
                            BLUETOOTH_DEVICE_INFO btdi;
                            for (int i = 0; i < btdis.Count; i++)
                            {
                                btdi = btdis[i];
                                if (btdi.szName.StartsWith("Nintendo"))
                                {
                                    if (btdi.fConnected) wii_disconnect(btdi);
                                }
                            }
                        }
                        wiimoteConnected = false;
                        break;
                    case 'r':
                        refresh_list();
                        System.Threading.Thread.Sleep(100);
                        lock (btdis)
                        {
                            BLUETOOTH_DEVICE_INFO btdi;
                            for (int i = 0; i < btdis.Count; i++)
                            {
                                btdi = btdis[i];
                                if (btdi.szName.Contains("RVL-CNT-01"))
                                {
                                    wii_remove(btdi);
                                }
                            }
                        }
                        wiimoteConnected = false;
                        break;
                    case 'l':
                        showDevices = true;
                        break;

                    default:
                        break;
                }
                com = Console.ReadKey().KeyChar;
            }

        }

        static IntPtr hradio = IntPtr.Zero;
        static BLUETOOTH_RADIO_INFO rinfo = new BLUETOOTH_RADIO_INFO();
        static List<BLUETOOTH_DEVICE_INFO> btdis = new List<BLUETOOTH_DEVICE_INFO>();

        private static unsafe void radio_search()
        {
            IntPtr hrfind;

            BLUETOOTH_FIND_RADIO_PARAMS param = new BLUETOOTH_FIND_RADIO_PARAMS();
            param.dwSize = (UInt32)Marshal.SizeOf(param);
            if ((hrfind = MyBluetoothFindFirstRadio(ref param, out hradio)) != IntPtr.Zero)
            {
                rinfo.dwSize = (UInt32)Marshal.SizeOf(rinfo);
                MyBluetoothGetRadioInfo(hradio, ref rinfo);
                fixed (byte* addrbyte = rinfo.address.rgBytes)
                {

                    Console.WriteLine("bluetooth host = {5:X02}:{4:X02}:{3:X02}:{2:X02}:{1:X02}:{0:X02}",
                            addrbyte[0], addrbyte[1], addrbyte[2], addrbyte[3], addrbyte[4], addrbyte[5]);
                }
            } else
            {
                Console.WriteLine("no bluetooth module");
            }
            MyBluetoothFindRadioClose(hrfind);
        }

        private static unsafe void refresh_list()
        {
            if (hradio == IntPtr.Zero) return;
            BLUETOOTH_DEVICE_INFO btdi = new BLUETOOTH_DEVICE_INFO();
            BLUETOOTH_DEVICE_SEARCH_PARAMS srch = new BLUETOOTH_DEVICE_SEARCH_PARAMS();

            btdi.dwSize = (uint)Marshal.SizeOf(btdi);
            srch.dwSize = (uint)Marshal.SizeOf(srch);

            srch.fReturnAuthenticated = true;
            srch.fReturnRemembered = true;
            srch.fReturnConnected = true;
            srch.fReturnUnknown = true;
            srch.fIssueInquiry = true;
            srch.cTimeoutMultiplier = 2;
            srch.hRadio = hradio;

            lock (btdis)
            {
                btdis.Clear();
                IntPtr hfind = MyBluetoothFindFirstDevice(ref srch, ref btdi);
                while (hfind != IntPtr.Zero)
                {
                    btdis.Add(btdi);

                    btdi = new BLUETOOTH_DEVICE_INFO();
                    btdi.dwSize = (uint)Marshal.SizeOf(btdi);

                    if (!MyBluetoothFindNextDevice(hfind, ref btdi)) break;
                }
            }
        }

        private static unsafe void wii_pairing(BLUETOOTH_DEVICE_INFO btdi)
        {
            UInt32 ret = 0;
            if (btdi.fRemembered)
            {
                ret = MyBluetoothRemoveDevice(ref btdi.Address);
            }
            if (ret != 0)
            {
                Console.WriteLine("remove error.");
                return;
            }

            char[] pass = new char[6];
            fixed (byte* addr = rinfo.address.rgBytes)
            {
                for (int i = 0; i < 6; i++) pass[i] = (char)addr[i];
            }
            ret = MyBluetoothAuthenticateDevice(IntPtr.Zero, hradio, ref btdi, pass, 6);
            if (ret != 0)
            {
                Console.WriteLine("auth error. " + ret);
                return;
            }

            ret = MyBluetoothSetServiceState(hradio, ref btdi,
                ref HumanInterfaceDeviceServiceClass_UUID, BLUETOOTH_SERVICE_ENABLE);
            if (ret != 0)
            {
                Console.WriteLine("set service enable error");
                return;
            }

            Console.WriteLine("successfully pair Wiimote!");
        }
        private static unsafe void wii_disconnect(BLUETOOTH_DEVICE_INFO btdi)
        {
            UInt32 ret;
            ret = MyBluetoothSetServiceState(hradio, ref btdi,
                ref HumanInterfaceDeviceServiceClass_UUID, BLUETOOTH_SERVICE_DISABLE);
            if (ret != 0)
            {
                Console.WriteLine("set service enable error");
                return;
            }
            ret = MyBluetoothSetServiceState(hradio, ref btdi,
                ref HumanInterfaceDeviceServiceClass_UUID, BLUETOOTH_SERVICE_ENABLE);
            if (ret != 0)
            {
                Console.WriteLine("set service enable error");
                return;
            }

            Console.WriteLine("successfully disconnected.");
        }

        private static unsafe void wii_remove(BLUETOOTH_DEVICE_INFO btdi)
        {
            UInt32 ret = 0;
            if (btdi.fRemembered)
            {
                ret = MyBluetoothRemoveDevice(ref btdi.Address);
            }
            if (ret != 0)
            {
                Console.WriteLine("remove error.");
                return;
            }

            Console.WriteLine("successfully removed");
        }

        private static unsafe void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                Console.WriteLine("Searching...");
                refresh_list();
                System.Threading.Thread.Sleep(100);

                var doList = showDevices;
                lock (btdis)
                {
                    BLUETOOTH_DEVICE_INFO btdi;
                    if (showDevices) btdiCount = btdis.Count;
                    for (int i = 0; i < btdis.Count; i++)
                    {
                        btdi = btdis[i];
                        string btaddr = string.Format("{0:X02}:{1:X02}:{2:X02}:{3:X02}:{4:X02}:{5:X02}",
                            btdi.Address.rgBytes[5], btdi.Address.rgBytes[4], btdi.Address.rgBytes[3],
                            btdi.Address.rgBytes[2], btdi.Address.rgBytes[1], btdi.Address.rgBytes[0]);

                        if (btdi.szName.StartsWith("Nintendo"))
                        {
                            //if (btdi.fRemembered) Console.WriteLine("知ってるで");
                            //if (btdi.fAuthenticated) Console.WriteLine("認証しとるで");
                            //if (btdi.fConnected) Console.WriteLine("もう繋がっとるで");

                            if (!wiimoteConnected && btdi.fConnected) Console.WriteLine("Wiimote Connected!");
                            if (wiimoteConnected && !btdi.fConnected) Console.WriteLine("Wiimote Disconnected(Not 'd' command).");
                            wiimoteConnected = btdi.fConnected;

                            if (!btdi.fRemembered && !btdi.fAuthenticated && !btdi.fConnected)
                            {
                                Console.WriteLine("ペアリングするで");
                                System.Threading.Thread.Sleep(1000);
                                wii_pairing(btdi);
                            }
                        }

                        if (doList) Console.WriteLine("<{0}>\nCon:{2}, Addr:{1}", btdi.szName, btaddr, btdi.fConnected);
                    }
                }
                if (doList && showDevices) showDevices = false;

                System.Threading.Thread.Sleep(2000);
            }

        }


    }
}