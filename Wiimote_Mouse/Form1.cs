using System;
using System.Windows.Forms;
using WiimoteLib;
using System.Collections.Generic;
using System.Drawing;

namespace Wiimote_Mouse
{
    public partial class Form1 : Form
    {
        private Wiimote mWiimote;
        private delegate void UpdateWiimoteStateDelegate(WiimoteChangedEventArgs args);
        private delegate void UpdateExtensionChangedDelegate(WiimoteExtensionChangedEventArgs args);

        private System.Drawing.Point MaxXY;
        private System.Drawing.Point MinXY;
        private System.Drawing.Point CursorPoint;
        private bool wmMouseActivated = false;
        private int mouseSpeed = 25;
        private WiimoteLib.ButtonState previousBtns;
        private string batteryStat = "- %";
        private List<Bitmap> lamps = new List<Bitmap>();

        public Form1()
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.Fixed3D;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // ステータスランプ
            lamps.Add(new Bitmap(pbox_lamp.Width, pbox_lamp.Height));
            lamps.Add(new Bitmap(pbox_lamp.Width, pbox_lamp.Height));

            // Disabled
            var g = Graphics.FromImage(lamps[0]);
            g.FillRectangle(Brushes.Blue, new Rectangle(0, 0, pbox_lamp.Width, pbox_lamp.Height));
            g.Dispose();

            // Enabled
            g = Graphics.FromImage(lamps[1]);
            g.FillRectangle(Brushes.Green, new Rectangle(0, 0, pbox_lamp.Width, pbox_lamp.Height));
            g.Dispose();

            pbox_lamp.Image = lamps[0];
            lblStat.Text = "Disabled";

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

            // カーソルの取得と通知テキスト設定
            CursorPoint = Cursor.Position;
            notifyIcon1.Text = "Wiimote Mouse : Disabled";

            // Wiimote初期化
            mWiimote = new Wiimote();

            try
            {
                mWiimote.WiimoteChanged += (s, args) => UpdateState(args);
                mWiimote.WiimoteExtensionChanged += (s, args) => UpdateExtension(args);
                mWiimote.Connect();
                mWiimote.SetReportType(InputReport.IRAccel, true);
                mWiimote.SetLEDs(false, false, false, false);

            }
            catch (WiimoteNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "Wiimote not found error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
            catch (WiimoteException ex)
            {
                MessageBox.Show(ex.Message, "Wiimote error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Unknown error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }

        }

        public void UpdateState(WiimoteChangedEventArgs args)
        {
            BeginInvoke(new UpdateWiimoteStateDelegate(UpdateWiimoteChanged), args);
        }

        public void UpdateExtension(WiimoteExtensionChangedEventArgs args)
        {
            BeginInvoke(new UpdateExtensionChangedDelegate(UpdateExtensionChanged), args);
        }

        private void UpdateWiimoteChanged(WiimoteChangedEventArgs args)
        {
            WiimoteState ws = args.WiimoteState;

            #region ボタン判定
            // マウスカーソル移動
            if (wmMouseActivated)
            {
                // マウスカーソルの移動
                CursorPoint.X += (int)(ws.AccelState.Values.X * mouseSpeed);
                CursorPoint.Y += (int)(ws.AccelState.Values.Y * mouseSpeed);

                // ディスプレイの端判定
                if (CursorPoint.X < MinXY.X) CursorPoint.X = MinXY.X;
                if (CursorPoint.Y < MinXY.Y) CursorPoint.Y = MinXY.Y;

                if (CursorPoint.X > MaxXY.X) CursorPoint.X = MaxXY.X;
                if (CursorPoint.Y > MaxXY.Y) CursorPoint.Y = MaxXY.Y;

                //マウスカーソルを変更
                System.Windows.Forms.Cursor.Position = new System.Drawing.Point(this.CursorPoint.X, this.CursorPoint.Y);
            }

            // Aボタン (プレス)
            if ( !previousBtns.A && ws.ButtonState.A && wmMouseActivated )
            {
                // マウス左クリック
                WinAPI.mouse_event(WinAPI.MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            }

            // Aボタン (リリース)
            if ( previousBtns.A && !ws.ButtonState.A && wmMouseActivated )
            {
                // マウス左クリック
                WinAPI.mouse_event(WinAPI.MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
            }

            // Bボタン
            if (previousBtns.B && !ws.ButtonState.B)
            {
                // マウス有効化設定
                wmMouseActivated = !wmMouseActivated;
                if (wmMouseActivated)
                {
                    mWiimote.SetLEDs(true, true, true, true);
                    notifyIcon1.Text = "Wiimote Mouse : Enabled";
                    pbox_lamp.Image = lamps[1];
                    lblStat.Text = "Enabled";
                } else
                {
                    mWiimote.SetLEDs(true, false, false, true);
                    notifyIcon1.Text = "Wiimote Mouse : Disabled";
                    pbox_lamp.Image = lamps[0];
                    lblStat.Text = "Disabled";
                }
            }

            // +ボタン
            if (previousBtns.Plus && !ws.ButtonState.Plus && wmMouseActivated)
            {
                // カーソルスピード加速
                mouseSpeed += 5;
            }

            // -ボタン
            if (previousBtns.Minus && !ws.ButtonState.Minus && wmMouseActivated)
            {
                // カーソルスピード減速
                mouseSpeed -= 5;
                if (mouseSpeed <= 0) mouseSpeed = 1;
            }

            // ↑ボタン
            if (previousBtns.Up && !ws.ButtonState.Up )
            {
                // Windowsキー
                SendKeys.Send("^{ESC}");
            }

            // ↓ボタン (プレス)
            if (!previousBtns.Down && ws.ButtonState.Down && wmMouseActivated)
            {
                // マウス右クリック
                WinAPI.mouse_event(WinAPI.MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, 0);
            }

            // ↓ボタン (リリース)
            if (previousBtns.Down && !ws.ButtonState.Down && wmMouseActivated)
            {
                // マウス右クリック
                WinAPI.mouse_event(WinAPI.MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
            }

            // Homeボタン
            if (previousBtns.Home && !ws.ButtonState.Home )
            {
                // マウスカーソルを画面の真ん中に
                var wa = Screen.GetWorkingArea(Cursor.Position);
                CursorPoint = new System.Drawing.Point(wa.Width / 2, wa.Height / 2);
                Cursor.Position = CursorPoint;

            }

            previousBtns = ws.ButtonState;
            #endregion

            pbBattery.Value = (ws.Battery > 0xc8 ? 0xc8 : (int)ws.Battery);
            batteryStat = pbBattery.Value.ToString() + "%";
            lblBattery.Text = batteryStat;

        }

        private void UpdateExtensionChanged(WiimoteExtensionChangedEventArgs args)
        {
            if (args.Inserted)
                mWiimote.SetReportType(InputReport.IRExtensionAccel, true);
            else
                mWiimote.SetReportType(InputReport.IRAccel, true);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            mWiimote.SetLEDs(false, false, false, false);
            mWiimote.Disconnect();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Visible = true;
            this.ShowInTaskbar = true;
            this.WindowState = FormWindowState.Normal;
            this.Activate();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Visible = false;
                this.ShowInTaskbar = false;
            }
        }
    }
}
