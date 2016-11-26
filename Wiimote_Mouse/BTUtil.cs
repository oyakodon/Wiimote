using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using static Wiimote_Mouse.BTApi;


namespace Wiimote_Mouse
{
    public partial class BTUtil : Form
    {
        // Constructor
        public BTUtil()
        {
            InitializeComponent();

            bgWorker_devSearch = new BackgroundWorker();
            bgWorker_devSearch.WorkerSupportsCancellation = true;
            bgWorker_devSearch.DoWork += bgWorker_devSearch_DoWork;

            radio_search();
        }

        public void disconnectWiimote()
        {
            refresh_list();
            System.Threading.Thread.Sleep(100);
            Application.DoEvents();
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

        }

        // Properties
        private BackgroundWorker bgWorker_devSearch; // 探索用バックグラウンドワーカー
        private IntPtr hradio = IntPtr.Zero;
        private BLUETOOTH_RADIO_INFO rinfo = new BLUETOOTH_RADIO_INFO();
        private List<BLUETOOTH_DEVICE_INFO> btdis = new List<BLUETOOTH_DEVICE_INFO>();
        public bool userClosed = false;
        private bool invokeClosed = false;
        private bool doPair = false;

        /// <summary>
        /// Wiiリモコンとペアリングします。
        /// </summary>
        /// <param name="btdi">BLUETOOTH DEVICE INFO</param>
        private unsafe void wii_pairing(BLUETOOTH_DEVICE_INFO btdi)
        {
            UInt32 ret = 0;
            if (btdi.fRemembered)
            {
                ret = MyBluetoothRemoveDevice(ref btdi.Address);
            }
            if (ret != 0)
            {
                throw new Exception("remove error.");
            }

            char[] pass = new char[6];
            fixed (byte* addr = rinfo.address.rgBytes)
            {
                for (int i = 0; i < 6; i++) pass[i] = (char)addr[i];
            }
            ret = MyBluetoothAuthenticateDevice(IntPtr.Zero, hradio, ref btdi, pass, 6);
            if (ret != 0)
            {
                throw new Exception("auth error. " + ret);
            }

            ret = MyBluetoothSetServiceState(hradio, ref btdi,
                ref HumanInterfaceDeviceServiceClass_UUID, BLUETOOTH_SERVICE_ENABLE);
            if (ret != 0)
            {
                throw new Exception("set service enable error");
            }
        }

        /// <summary>
        /// Wiiリモコンを切断します。
        /// </summary>
        /// <param name="btdi">BLUETOOTH DEVICE INFO</param>
        private unsafe void wii_disconnect(BLUETOOTH_DEVICE_INFO btdi)
        {
            UInt32 ret;
            ret = MyBluetoothSetServiceState(hradio, ref btdi,
                ref HumanInterfaceDeviceServiceClass_UUID, BLUETOOTH_SERVICE_DISABLE);
            if (ret != 0)
            {
                throw new Exception("set service enable error");
            }
            ret = MyBluetoothSetServiceState(hradio, ref btdi,
                ref HumanInterfaceDeviceServiceClass_UUID, BLUETOOTH_SERVICE_ENABLE);
            if (ret != 0)
            {
                throw new Exception("set service enable error");
            }
        }

        /// <summary>
        /// Wiiリモコンをコンピュータ上から削除します。(Bluetooth情報の削除)
        /// </summary>
        /// <param name="btdi">BLUETOOTH DEVICE INFO</param>
        private unsafe void wii_remove(BLUETOOTH_DEVICE_INFO btdi)
        {
            UInt32 ret = 0;
            if (btdi.fRemembered)
            {
                ret = MyBluetoothRemoveDevice(ref btdi.Address);
            }
            if (ret != 0)
            {
                throw new Exception("remove error.");
            }
        }

        /// <summary>
        /// コンピュータのBluetoothデバイスを取得します。
        /// </summary>
        private unsafe void radio_search()
        {
            IntPtr hrfind;

            var param = new BLUETOOTH_FIND_RADIO_PARAMS();
            param.dwSize = (UInt32)Marshal.SizeOf(param);
            if ((hrfind = MyBluetoothFindFirstRadio(ref param, out hradio)) != IntPtr.Zero)
            {
                rinfo.dwSize = (UInt32)Marshal.SizeOf(rinfo);
                MyBluetoothGetRadioInfo(hradio, ref rinfo);
            } else
            {
                throw new Exception("no bluetooth module");
            }
            MyBluetoothFindRadioClose(hrfind);
        }

        /// <summary>
        /// Bluetoothデバイスの一覧を更新します。
        /// </summary>
        private unsafe void refresh_list()
        {
            if (hradio == IntPtr.Zero) return;
            var btdi = new BLUETOOTH_DEVICE_INFO();
            var srch = new BLUETOOTH_DEVICE_SEARCH_PARAMS();

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
                var hfind = MyBluetoothFindFirstDevice(ref srch, ref btdi);
                while (hfind != IntPtr.Zero)
                {
                    btdis.Add(btdi);

                    btdi = new BLUETOOTH_DEVICE_INFO();
                    btdi.dwSize = (uint)Marshal.SizeOf(btdi);

                    if (!MyBluetoothFindNextDevice(hfind, ref btdi)) break;
                }
            }
        }

        /// <summary>
        /// Wiiリモコンの探索を行います。
        /// </summary>
        private unsafe void bgWorker_devSearch_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = sender as BackgroundWorker;
            var broken = false;

            while (true)
            {
                // 探索開始
                refresh_list();
                System.Threading.Thread.Sleep(100);

                lock (btdis)
                {
                    BLUETOOTH_DEVICE_INFO btdi;
                    for (int i = 0; i < btdis.Count; i++)
                    {
                        btdi = btdis[i];
                        Debug.WriteLine("{0} : {1}", btdi.szName, doPair);

                        if (btdi.szName.StartsWith("Nintendo"))
                        {
                            if (worker.CancellationPending)
                            {
                                e.Cancel = true;
                                bgWorker_devSearch.CancelAsync();
                                broken = true;
                                break;
                            }

                            if (btdi.fConnected)
                            {
                                // 接続完了
                                bgClose();
                            }

                            if (doPair || (!btdi.fRemembered && !btdi.fAuthenticated && !btdi.fConnected))
                            {
                                // Wiiリモコンとのペアリング
                                Debug.WriteLine("Do Pairing.");
                                doPair = false;
                                System.Threading.Thread.Sleep(1000);
                                wii_pairing(btdi);
                                System.Threading.Thread.Sleep(1000);
                                bgClose();
                            }
                        }
                    }

                    if (broken) break;
                }

                System.Threading.Thread.Sleep(2000);
            }

        }

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lblStat.Text = "Connecting...";
            removeToolStripMenuItem.Enabled = pairToolStripMenuItem.Enabled = connectToolStripMenuItem.Enabled = false;
            btnCancel.Enabled = true;

            if (!bgWorker_devSearch.IsBusy)
                bgWorker_devSearch.RunWorkerAsync();
        }

        private void pairToolStripMenuItem_Click(object sender, EventArgs e)
        {
            doPair = true;
            lblStat.Text = "Pairing...";
            removeToolStripMenuItem.Enabled = connectToolStripMenuItem.Enabled = pairToolStripMenuItem.Enabled = false;
            btnCancel.Enabled = true;

            MessageBox.Show("Press and release the SYNC button just below the batteries on the Wii Remote.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
            if (!bgWorker_devSearch.IsBusy)
                bgWorker_devSearch.RunWorkerAsync();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            lblStat.Text = "...";
            btnCancel.Enabled = false;
            removeToolStripMenuItem.Enabled = connectToolStripMenuItem.Enabled = pairToolStripMenuItem.Enabled = true;
            bgWorker_devSearch.CancelAsync();

        }

        private void BTUtil_Shown(object sender, EventArgs e)
        {
            connectToolStripMenuItem.PerformClick();
        }

        private void BTUtil_FormClosing(object sender, FormClosingEventArgs e)
        {
            bgWorker_devSearch.CancelAsync();
        }

        private void BTUtil_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!invokeClosed)
                userClosed = true;
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnCancel.Enabled = false;
            connectToolStripMenuItem.Enabled = pairToolStripMenuItem.Enabled = true;
            removeToolStripMenuItem.Enabled = false;
            lblStat.Text = "...";
            bgWorker_devSearch.CancelAsync();

            refresh_list();
            System.Threading.Thread.Sleep(100);
            Application.DoEvents();
            lock (btdis)
            {
                BLUETOOTH_DEVICE_INFO btdi;
                for (int i = 0; i < btdis.Count; i++)
                {
                    btdi = btdis[i];
                    if (btdi.szName.StartsWith("Nintendo"))
                    {
                        if (btdi.fConnected) wii_remove(btdi);
                    }
                }
            }
            MessageBox.Show("Wiimote device successfully removed in this PC.");
        }

        delegate void bgCloseCallback();

        private void bgClose()
        {
            invokeClosed = true;

            if (this.InvokeRequired)
            {
                bgCloseCallback d = new bgCloseCallback(bgClose);
                this.Invoke(d);
            } else
            {
                this.Close();
            }
        }
    }
}
