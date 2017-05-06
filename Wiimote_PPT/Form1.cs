using System;
using System.Diagnostics;
using System.Windows.Forms;

using WiimoteLib;

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
            foreach (var s in Screen.AllScreens)
            {
                if (s.Bounds.X < 0) MinXY.X += s.Bounds.X;
                if (s.Bounds.Y < 0) MinXY.Y += s.Bounds.Y;
                if (s.Bounds.X > 0) MaxXY.X += s.Bounds.X;
                if (s.Bounds.Y > 0) MaxXY.Y += s.Bounds.Y;
            }
            MaxXY.X += Screen.PrimaryScreen.Bounds.Width;
            MaxXY.Y += Screen.PrimaryScreen.Bounds.Height;

            // カーソルの取得
            Pt = Cursor.Position;

            ConnectWiimote();
            AddLog("起動しました。 " + DateTime.Now.ToString());
        }

        private Wiimote mWiimote { get; set; }
        private delegate void UpdateWiimoteStateDelegate(WiimoteChangedEventArgs args);
        private delegate void UpdateExtensionChangedDelegate(WiimoteExtensionChangedEventArgs args);
        private WiimoteLib.ButtonState previousBtns = new WiimoteLib.ButtonState(); // 直前のボタンの状態

        private System.Drawing.Point MaxXY; // ディスプレイのX,Y最大値
        private System.Drawing.Point MinXY; // ディスプレイのX,Y最小値
        private System.Drawing.Point Pt; // ポインタ位置
        private int pointSpeed = 25; // レーザーポインタの移動速度

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
                System.Threading.Thread.Sleep(250);
                mWiimote.SetRumble(false);

            }
            catch (WiimoteNotFoundException ex)
            {
                MessageBox.Show("Wiiリモコンを認識できませんでした。", "Wiimote PPT - エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Debug.WriteLine("Exit -> Wiimote Not Found.");
                Debug.WriteLine(ex.ToString());

                notifyIcon.Visible = false;
                Environment.Exit(1);
            }
            catch (WiimoteException ex)
            {
                MessageBox.Show("Wiiリモコンに何らかのエラーが発生しました。", "Wiimote PPT - エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Debug.WriteLine("Exit -> Wiimote Exception.");
                Debug.WriteLine(ex.ToString());

                notifyIcon.Visible = false;
                Environment.Exit(1);
            }
            catch (Exception ex)
            {
                MessageBox.Show("予期せぬエラーが発生しました。", "Wiimote PPT - エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Debug.WriteLine("Exit -> Unknown Error.");
                Debug.WriteLine(ex.ToString());

                notifyIcon.Visible = false;
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

        private void MoveCursor(float dx, float dy)
        {
            // カーソルの移動 (変位)
            Pt.X += (int)(dx * pointSpeed);
            Pt.Y += (int)(dy * pointSpeed);

            // ディスプレイの端判定
            if (Pt.X < MinXY.X) Pt.X = MinXY.X;
            if (Pt.Y < MinXY.Y) Pt.Y = MinXY.Y;

            if (Pt.X > MaxXY.X) Pt.X = MaxXY.X;
            if (Pt.Y > MaxXY.Y) Pt.Y = MaxXY.Y;

            //マウスカーソルを変更
            Cursor.Position = new System.Drawing.Point(this.Pt.X, this.Pt.Y);
        }

        private void CheckAction(WiimoteState ws)
        {
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

            // +ボタン : レーザーポインタの移動速度変更　加速
            if (!previousBtns.Plus && ws.ButtonState.Plus)
            {
                pointSpeed += 5;
            }

            // -ボタン : レーザーポインタの移動速度変更　減速
            if (!previousBtns.Minus && ws.ButtonState.Minus)
            {
                pointSpeed = Math.Max(1, pointSpeed - 5);
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
                Pt = new System.Drawing.Point(s.Bounds.X + s.Bounds.Width / 2, s.Bounds.Y + s.Bounds.Height / 2);
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

                // B + Aボタン : レーザーポインタ (押している間)
                if (ws.ButtonState.A)
                {
                    // Ctrl + 左クリックの送信
                    WinAPI.keybd_event(0x11, 0, 0, (UIntPtr)0);
                    WinAPI.mouse_event(WinAPI.MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
                    // ポインタの移動
                    MoveCursor(ws.AccelState.Values.X, ws.AccelState.Values.Y);
                }
            }

            // 十字↓ボタン : ステータス表示
            if (!previousBtns.Down && ws.ButtonState.Down)
            {
                if (this.Visible)
                {
                    this.Hide();
                }
                else
                {
                    var battery = (ws.Battery > 0xc8 ? 0xc8 : (int)ws.Battery) / 200.0 * 100;
                    boxStatus.Text = $"Battery : {battery:f1} %";
                    this.Show();
                }
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
