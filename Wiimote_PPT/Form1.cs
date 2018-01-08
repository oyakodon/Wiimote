using System;
using System.Linq;
using System.Diagnostics;
using System.Windows.Forms;

using WiimoteLib;
using System.Collections.Generic;

namespace Wiimote_PPT
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // スクリーンの大きさ取得
            var menuDisplays = new List<ToolStripMenuItem>();
            for(var i = 0; i < Screen.AllScreens.Length; i++)
            {
                var scr = Screen.AllScreens[i];
                screenSizes.Add(scr.Bounds);
                var menuDisp = new ToolStripMenuItem();
                menuDisplays.Add(menuDisp);
                menuDisp.Text = "ディスプレイ" + (i + 1);
                int num = i;

                menuDisp.Click += (s2, e2) =>
                {
                    foreach (var item in menuDisplays)
                    {
                        item.CheckState = object.ReferenceEquals(item, s2) ? CheckState.Indeterminate : CheckState.Unchecked;
                    }

                    selectedDisp = num;
                };

                ディスプレイToolStripMenuItem.DropDownItems.Add(menuDisp);
            }

            menuDisplays[0].PerformClick();

            ConnectWiimote();
            AddLog("起動しました。 " + DateTime.Now.ToString());
        }

        private Wiimote mWiimote { get; set; }
        private delegate void UpdateWiimoteStateDelegate(WiimoteChangedEventArgs args);
        private delegate void UpdateExtensionChangedDelegate(WiimoteExtensionChangedEventArgs args);
        private WiimoteLib.ButtonState previousBtns = new WiimoteLib.ButtonState(); // 直前のボタンの状態

        private List<System.Drawing.Rectangle> screenSizes = new List<System.Drawing.Rectangle>(); // ディスプレイの大きさ
        private int selectedDisp; // 選択されているディスプレイ
        private bool irDetected = false; // 赤外線を検出しているか

        private void ConnectWiimote()
        {
            // Bluetooth 認識
            var btutil = new BTUtil();
            btutil.ShowDialog();
            if (btutil.userClosed)
            {
                Debug.WriteLine("Exit -> BTUtil User Closed.");

                notifyIcon.Visible = false;
                Environment.Exit(0);
            }

            // Wiimote接続
            mWiimote = new Wiimote();

            try
            {
                mWiimote.WiimoteChanged += (s, args) => UpdateState(args);
                mWiimote.WiimoteExtensionChanged += (s, args) => UpdateExtension(args);
                mWiimote.Connect();
                mWiimote.SetReportType(InputReport.IRExtensionAccel, true);
                mWiimote.SetLEDs(true, false, false, false);

                mWiimote.SetRumble(true);
                System.Threading.Thread.Sleep(200);
                mWiimote.SetRumble(false);

            }
            catch (Exception ex)
            {
                string msg;

                switch (ex)
                {
                    case WiimoteNotFoundException _:
                        msg = "Wiiリモコンを認識できませんでした。";
                        break;
                    case WiimoteException _:
                        msg = "Wiiリモコンに何らかのエラーが発生しました。";
                        break;
                    default:
                        msg = "予期せぬエラーが発生しました。";
                        break;
                }

                MessageBox.Show(
                    msg,
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                Environment.Exit(1);
            }
        }

        private void AddLog(string _log) => boxLog.Text = _log + Environment.NewLine + boxLog.Text;

        public void UpdateState(WiimoteChangedEventArgs args)
        {
            BeginInvoke(new UpdateWiimoteStateDelegate(UpdateWiimoteChanged), args);
        }

        public void UpdateExtension(WiimoteExtensionChangedEventArgs args)
        {
            BeginInvoke(new UpdateExtensionChangedDelegate(UpdateExtensionChanged), args);
        }

        private void UpdateExtensionChanged(WiimoteExtensionChangedEventArgs args)
        {
            if (args.Inserted)
                mWiimote.SetReportType(InputReport.IRExtensionAccel, true);
            else
                mWiimote.SetReportType(InputReport.IRAccel, true);
        }

        private void UpdateWiimoteChanged(WiimoteChangedEventArgs args)
        {
            var ws = args.WiimoteState;
            CheckAction(ws);
            previousBtns = ws.ButtonState;
        }

        private void CheckAction(WiimoteState ws)
        {
            // 赤外線
            var ir = new List<KeyValuePair<int, PointF>>();

            for (int i = 0; i < 4; i++)
            {
                if (ws.IRState.IRSensors[i].Found)
                {
                    ir.Add(new KeyValuePair<int, PointF>(ws.IRState.IRSensors[i].Size, ws.IRState.IRSensors[i].Position));
                }
            }

            if (ir.Count >= 2)
            {
                irDetected = true;

                var q = ir.OrderByDescending(x => x.Key).Take(2).ToList();

                var mouse_p = new System.Drawing.Point();
                mouse_p.X = (int)((1.5 - (q[0].Value.X + q[1].Value.X)) * screenSizes[selectedDisp].Width + screenSizes[selectedDisp].X);
                mouse_p.Y = (int)((q[0].Value.Y + q[1].Value.Y - 0.5) * screenSizes[selectedDisp].Height + screenSizes[selectedDisp].Y);
                Cursor.Position = mouse_p;
            }
            else
            {
                irDetected = false;
            }

            // Aボタン : 次のスライドへ (→)
            if (!ws.ButtonState.B && !previousBtns.A && ws.ButtonState.A)
            {
                SendKeys.Send("{RIGHT}");
            }

            // Aボタンリリース (レーザーポインタ用)
            if (previousBtns.A && !ws.ButtonState.A)
            {
                WinAPI.keybd_event(0x11, 0, 2, (UIntPtr)0);
                WinAPI.mouse_event(WinAPI.MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
            }

            // 十字→ボタン : →
            if (!previousBtns.Right && ws.ButtonState.Right)
            {
                SendKeys.Send("{RIGHT}");
            }

            // 十字←ボタン : ←
            if (!previousBtns.Left && ws.ButtonState.Left)
            {
                SendKeys.Send("{LEFT}");
            }

            // 1ボタン : F5 (スライドショー開始 最初から)
            if (!previousBtns.One && ws.ButtonState.One)
            {
                SetActivePPT();
                SendKeys.Send("{F5}");
            }

            // 2ボタン : Shift + F5 (スライドショー開始 最初から)
            if (!previousBtns.Two && ws.ButtonState.Two)
            {
                SetActivePPT();
                SendKeys.Send("+{F5}");
            }

            // Homeボタン : カーソルのディスプレイの中心に移動
            if (previousBtns.Home && !ws.ButtonState.Home)
            {
                var s = Screen.FromPoint(Cursor.Position);
                var Pt = new System.Drawing.Point(s.Bounds.X + s.Bounds.Width / 2, s.Bounds.Y + s.Bounds.Height / 2);
                Cursor.Position = Pt;
            }

            // B特殊キー
            if (ws.ButtonState.B)
            {
                // B + Homeボタン : Esc (スライドショーの終了)
                if (!previousBtns.Home && ws.ButtonState.Home)
                {
                    SendKeys.Send("{ESC}");
                }

                // B + ←ボタン : ディスプレイ切り替え
                if (!previousBtns.Left && ws.ButtonState.Left)
                {
                    selectedDisp = Math.Max(0, selectedDisp - 1);
                }

                // B + →ボタン : ディスプレイ切り替え
                if (!previousBtns.Right && ws.ButtonState.Right)
                {
                    selectedDisp = Math.Min(screenSizes.Count - 1, selectedDisp + 1);
                }

                // B + -ボタン : 左クリック
                if (!previousBtns.Minus && ws.ButtonState.Minus)
                {
                    WinAPI.mouse_event(WinAPI.MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
                    WinAPI.mouse_event(WinAPI.MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
                }

                // B + +ボタン : 右クリック
                if (!previousBtns.Plus && ws.ButtonState.Plus)
                {
                    WinAPI.mouse_event(WinAPI.MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, 0);
                    WinAPI.mouse_event(WinAPI.MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
                }

                // B + Aボタン : レーザーポインタ (押している間)
                if (!previousBtns.A && ws.ButtonState.A)
                {
                    // Ctrl + 左クリックの送信
                    WinAPI.keybd_event(0x11, 0, 0, (UIntPtr)0);
                    WinAPI.mouse_event(WinAPI.MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
                }
            }

            // 十字↓ボタン : ステータス表示
            if (!previousBtns.Down && ws.ButtonState.Down)
            {
                if (this.Visible)
                    this.Hide();
                else
                    this.Show();
            }

            // 状態を表示
            if (this.Visible)
            {
                var battery = (ws.Battery > 0xc8 ? 0xc8 : (int)ws.Battery) / 200.0 * 100;
                boxStatus.Text = $"Battery : {battery:f1} %" + Environment.NewLine;
                boxStatus.Text += "IR : " + (irDetected ? "Found" : "Lost") + Environment.NewLine;
            }

        }

        private void SetActivePPT()
        {
            foreach (var p in Process.GetProcesses())
            {
                if (p.MainWindowTitle.Contains("PowerPoint"))
                {
                    //ウィンドウをアクティブにする
                    WinAPI.ShowWindow(p.MainWindowHandle, 9); // Restore
                    WinAPI.SetForegroundWindow(p.MainWindowHandle); // Foreground
                    break;
                }
            }
        }

        private void 終了QToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mWiimote.SetLEDs(false, false, false, false);

            if (終了時に切断ToolStripMenuItem.Checked)
            {
                this.Text = "Wiimote PPT - 切断中...";
                mWiimote.Disconnect();
                var count = 0;
                while (count >= 30)
                {
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(100);
                }
                var btutil = new BTUtil();
                btutil.disconnectWiimote();
            }

            notifyIcon.Visible = false;
            Environment.Exit(0);
        }

        private void notifyIcon_Click(object sender, EventArgs e)
        {
            var p = Cursor.Position;
            contextMenuStrip.Show(p);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            notifyIcon.Visible = true;
        }

    }
}
