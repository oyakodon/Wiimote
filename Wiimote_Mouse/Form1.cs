﻿using System;
using System.Windows.Forms;
using WiimoteLib; // Wiiリモコン
using System.Collections.Generic;
using System.Drawing;
using CoreAudio; // 音量調節
using System.Diagnostics; // Debug.WriteLine 

namespace Wiimote_Mouse
{
    public partial class Form1 : Form
    {
        #region Properties
        // ------------------------------------------------------------------------------------------
        // ------------------------------            Properties            ------------------------------
        // ------------------------------------------------------------------------------------------
        public Wiimote mWiimote;
        private delegate void UpdateWiimoteStateDelegate(WiimoteChangedEventArgs args);
        private delegate void UpdateExtensionChangedDelegate(WiimoteExtensionChangedEventArgs args);

        private MMDevice device; // オーディオデバイス

        private System.Drawing.Point MaxXY; // ディスプレイのX,Y最大値
        private System.Drawing.Point MinXY; // ディスプレイのX,Y最小値
        private System.Drawing.Point CursorPoint; // カーソル位置

        private bool wmMouseActivated = false; // マウスの移動の有効/無効
        private bool joyMouseActivated = false; // ジョイスティックでの移動の有効/無効
        private int mouseSpeed = 25; // マウス速度

        private WiimoteLib.ButtonState previousBtns; // 直前のボタンの状態
        private WiimoteLib.NunchukState previousNunchuk; // 直前のヌンチャクの状態

        public Timer pressingTimer = new Timer(); // Homeボタン長押しタイマー
        private int pressingElapsed = 0; // タイマー経過時間
        private int pressingCount = 0; // 押している時間
        private int resetCount = 0; // リセット待機時間
        private bool easterEgg = false; // 裏機能
        private bool pressingHome = false; // Homeボタン
        private Queue<string> Log = new Queue<string>(); // ボタン押下ログ
        private bool dialogShowing = false; // ダイアログ表示中

        private bool volChanged = false; // 2ボタンを押しながら音量が変更されたかどうか

        private BTUtil btu; // Bluetooth
        #endregion

        public Form1()
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.Fixed3D; // ウィンドウの大きさを変更させない

            // オーディオデバイスの取得
            var DevEnum = new MMDeviceEnumerator();
            device = DevEnum.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eMultimedia);
#if DEBUG
            statLblVol.Text = (float)(device.AudioEndpointVolume.MasterVolumeLevelScalar * 100) + "%";
#else
            statLblVol.Text = (int)(device.AudioEndpointVolume.MasterVolumeLevelScalar * 100) + "%";
#endif
            device.AudioEndpointVolume.OnVolumeNotification += new AudioEndpointVolumeNotificationDelegate(AudioEndpointVolume_OnVolumeNotification);

            // タイマー設定
            pressingTimer.Tick += PressingTimer_Tick;
            pressingTimer.Interval = 100;
            pressingTimer.Start();

            // Bluetooth
            btu = new BTUtil();
            btu.ShowDialog();
            System.Threading.Thread.Sleep(2000);
            if (btu.userClosed)
            {
                Environment.Exit(0);
            }
        }

        // ------------------------------------------------------------------------------------------
        // ------------------------------           Form Events          ------------------------------
        // ------------------------------------------------------------------------------------------
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

            // カーソルの取得と通知テキスト設定
            CursorPoint = Cursor.Position;
            notifyIcon1.Text = "Wiimote Mouse : Disabled";

            // Wiimote接続
            mWiimote = new Wiimote();

            try
            {
                mWiimote.WiimoteChanged += (s, args) => UpdateState(args);
                mWiimote.WiimoteExtensionChanged += (s, args) => UpdateExtension(args);
                mWiimote.Connect();
                mWiimote.SetReportType(InputReport.IRExtensionAccel, true);
                mWiimote.SetLEDs(true, false, false, true);

            }
            catch (WiimoteNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "Wiimote not found error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(1);
            }
            catch (WiimoteException ex)
            {
                MessageBox.Show(ex.Message, "Wiimote error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Unknown error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(1);
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            // 閉じられたら
            this.Text = "Wiimote Mouse - Closing...";
            this.notifyIcon1.Text = "Wiimote Mouse - Closing...";

            mWiimote.SetLEDs(false, false, false, false);
            mWiimote.Disconnect();
            System.Threading.Thread.Sleep(3000);
            Application.DoEvents();
            btu = new BTUtil();
            btu.disconnectWiimote();
            Application.DoEvents();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            // 最小化されたら
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Visible = false;
                this.ShowInTaskbar = false;
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // 通知アイコンがダブルクリックされたら
            this.Visible = true;
            this.ShowInTaskbar = true;
            this.WindowState = FormWindowState.Normal;
            this.Activate();
        }

        private void motionMouseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            wmMouseActivated = !wmMouseActivated;
            joyMouseActivated = !wmMouseActivated;
            motionMouseToolStripMenuItem.Checked = wmMouseActivated;
            nunchuckMouseToolStripMenuItem.Checked = !wmMouseActivated;
        }

        private void nunchuckMouseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            joyMouseActivated = !joyMouseActivated;
            wmMouseActivated = !joyMouseActivated;
            nunchuckMouseToolStripMenuItem.Checked = joyMouseActivated;
            motionMouseToolStripMenuItem.Checked = !joyMouseActivated;
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Wiimote Mouse\nCopyright (c) 2016 Oyakodon\n- WiimoteLib\n- CoreApi", "Option", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void quitQToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            Environment.Exit(0);
        }

        // ------------------------------------------------------------------------------------------
        // ------------------------------          Audio Events          ------------------------------
        // ------------------------------------------------------------------------------------------
        private void AudioEndpointVolume_OnVolumeNotification(AudioVolumeNotificationData data)
        {
            if (this.InvokeRequired)
            {
                object[] Params = new object[1];
                Params[0] = data;
                this.Invoke(new AudioEndpointVolumeNotificationDelegate(AudioEndpointVolume_OnVolumeNotification), Params);
            } else
            {
#if DEBUG
                statLblVol.Text = (float)(data.MasterVolume * 100) + "%";
#else
                statLblVol.Text = (int)(data.MasterVolume * 100) + "%";
#endif
            }
        }

        // ------------------------------------------------------------------------------------------
        // ------------------------------        Wiimote Events        ------------------------------
        // ------------------------------------------------------------------------------------------
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
            WiimoteState ws = args.WiimoteState;

            pressingHome = ws.ButtonState.Home;
            if (!easterEgg) CheckAction(ws);
            else
            {
                // 裏モード
                // ボタンログ

                var btn = BtnToString(ws.ButtonState);
                if (btn != null)
                {
                    if (btn == "Home")
                    {
                        var com = "";
                        foreach (var b in Log)
                        {
                            com += b;
                        }
                        Debug.WriteLine(com);

                        if (com.Contains("UpUpDownDownLeftRightLeftRightBA") && !dialogShowing)
                        {
                            dialogShowing = true;
                            MessageBox.Show("KONMAI", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            dialogShowing = false;
                        }

                        Log.Clear();
                    } else if (Log.Count > 12)
                    {
                        Log.Clear();
                    } else
                    {
                        Log.Enqueue(btn);
                    }
                }

                // 1ボタン + 2ボタンで終了
                if (ws.ButtonState.One && ws.ButtonState.Two)
                {
                    this.Close();
                    Environment.Exit(0);
                }
            }

            previousBtns = ws.ButtonState;

            // バッテリー残量
            statPbBattery.Value = (ws.Battery > 0xc8 ? 0xc8 : (int)ws.Battery);
            statLblBattery.Text = statPbBattery.Value.ToString() + "%";

        }

        private void MoveCursor(float dx, float dy)
        {
            // カーソルの移動 (変位)
            CursorPoint.X += (int)(dx * mouseSpeed);
            CursorPoint.Y += (int)(dy * mouseSpeed);

            // ディスプレイの端判定
            if (CursorPoint.X < MinXY.X) CursorPoint.X = MinXY.X;
            if (CursorPoint.Y < MinXY.Y) CursorPoint.Y = MinXY.Y;

            if (CursorPoint.X > MaxXY.X) CursorPoint.X = MaxXY.X;
            if (CursorPoint.Y > MaxXY.Y) CursorPoint.Y = MaxXY.Y;

            //マウスカーソルを変更
            System.Windows.Forms.Cursor.Position = new System.Drawing.Point(this.CursorPoint.X, this.CursorPoint.Y);
        }

        private void PressingTimer_Tick(object sender, EventArgs e)
        {
            // Homeボタン長押しタイマー　イベント
            if (easterEgg)
            {
                mWiimote.SetLEDs(1 << (pressingElapsed % 4));
            }

            if (pressingCount >= 30)
            {
                if (easterEgg)
                {
                    this.Text = "Wiimote Mouse";
                    easterEgg = false;
                    wmMouseActivated = joyMouseActivated = false;
                    mWiimote.SetLEDs(true, false, false, true);
                } else
                {
                    // 裏モード開始
                    Debug.WriteLine("EasterEgg Mode");
                    this.Text = "Wiimote Mouse - EasterEgg";
                    easterEgg = true;
                    wmMouseActivated = joyMouseActivated = false;
                }

                pressingCount = 0;
            }

            if (pressingHome)
            {
                resetCount = 0;
                pressingCount++;
            } else
            {
                if (pressingCount > 0)
                {
                    resetCount++;
                    if (resetCount >= 5)
                    {
                        pressingCount = 0;
                        resetCount = 0;
                    }
                }
            }

            pressingElapsed++;
        }

        private void CheckAction(WiimoteState ws)
        {
            // マウスカーソル移動
            if (wmMouseActivated && !joyMouseActivated)
            {
                // マウスカーソルの移動
                MoveCursor(ws.AccelState.Values.X, ws.AccelState.Values.Y);
            }

            // ヌンチャク
            switch (ws.ExtensionType)
            {
                case ExtensionType.Nunchuk:
                    // ヌンチャク刺さってるンゴ -> スティックでマウスカーソル移動
                    var joy = ws.NunchukState.Joystick;

                    if (previousNunchuk.C && !ws.NunchukState.C)
                    {
                        wmMouseActivated = false;
                        joyMouseActivated = !joyMouseActivated;
                        motionMouseToolStripMenuItem.Checked = wmMouseActivated;
                        nunchuckMouseToolStripMenuItem.Checked = joyMouseActivated;
                    }

                    if (joyMouseActivated && !wmMouseActivated)
                    {
                        if (ws.NunchukState.Z)
                        {
                            if (Math.Abs(joy.Y) >= 0.15)
                                WinAPI.mouse_event(WinAPI.MOUSEEVENTF_WHEEL, 0, 0, (int)(100 * joy.Y), 0);
                        } else
                        {
                            if (Math.Abs(joy.X) >= 0.15) MoveCursor(joy.X, 0);
                            if (Math.Abs(joy.Y) >= 0.15) MoveCursor(0, -joy.Y);
                        }
                    }

                    previousNunchuk = ws.NunchukState;
                    break;
            }

            // Aボタン (プレス)
            if (!previousBtns.A && ws.ButtonState.A)
            {
                // マウス左クリック
                WinAPI.mouse_event(WinAPI.MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            }

            // Aボタン (リリース)
            if (previousBtns.A && !ws.ButtonState.A)
            {
                // マウス左クリック
                WinAPI.mouse_event(WinAPI.MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
            }

            // Bボタン (プレス)
            if (!previousBtns.B && ws.ButtonState.B)
            {
                // マウス右クリック
                WinAPI.mouse_event(WinAPI.MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, 0);
            }

            // Bボタン (リリース)
            if (previousBtns.B && !ws.ButtonState.B)
            {
                // マウス右クリック
                WinAPI.mouse_event(WinAPI.MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
            }

            // +ボタン
            if (previousBtns.Plus && !ws.ButtonState.Plus && (wmMouseActivated || joyMouseActivated))
            {
                // カーソルスピード加速
                mouseSpeed += 5;
            }

            // -ボタン
            if (previousBtns.Minus && !ws.ButtonState.Minus && (wmMouseActivated || joyMouseActivated))
            {
                // カーソルスピード減速
                mouseSpeed -= 5;
                if (mouseSpeed <= 0) mouseSpeed = 1;
            }

            // ↑ボタン
            if (ws.ButtonState.Up && !ws.ButtonState.Two)
            {
                // マウスホイール　上
                WinAPI.mouse_event(WinAPI.MOUSEEVENTF_WHEEL, 0, 0, 65, 0);
            }

            if (previousBtns.Up && !ws.ButtonState.Up && ws.ButtonState.Two)
            {
                // 音量+
                volChanged = true;
                var vol = device.AudioEndpointVolume.MasterVolumeLevelScalar;
                device.AudioEndpointVolume.MasterVolumeLevelScalar += vol <= 1.0f - 0.05f ? 0.05f : 1.0f - vol;
            }

            // ↓ボタン
            if (ws.ButtonState.Down && !ws.ButtonState.Two)
            {
                // マウスホイール　下
                WinAPI.mouse_event(WinAPI.MOUSEEVENTF_WHEEL, 0, 0, -65, 0);
            }

            if (previousBtns.Down && !ws.ButtonState.Down && ws.ButtonState.Two)
            {
                // 音量-
                volChanged = true;
                var vol = device.AudioEndpointVolume.MasterVolumeLevelScalar;
                device.AudioEndpointVolume.MasterVolumeLevelScalar -= vol >= 0.05f ? 0.05f : vol;
            }

            // →ボタン
            if (previousBtns.Right && !ws.ButtonState.Right)
            {
                // マウスカーソルを画面の真ん中に
                var s = Screen.FromPoint(Cursor.Position);
                CursorPoint = new System.Drawing.Point(s.Bounds.X + s.Bounds.Width / 2, s.Bounds.Y + s.Bounds.Height / 2);
                Cursor.Position = CursorPoint;
            }

            // ←ボタン
            if (previousBtns.Left && !ws.ButtonState.Left)
            {
                // ウィンドウの表示・非表示
                if (this.Visible)
                {
                    // トレーに格納
                    this.Visible = false;
                    this.ShowInTaskbar = false;
                    this.WindowState = FormWindowState.Minimized;
                } else
                {
                    // 表示
                    this.Visible = true;
                    this.ShowInTaskbar = true;
                    this.WindowState = FormWindowState.Normal;
                    this.Activate();
                }

            }

            // 1ボタン
            if (previousBtns.One && !ws.ButtonState.One)
            {
                // Windowsキー
                SendKeys.Send("^{ESC}");
            }

            if (previousBtns.Two && !ws.ButtonState.Two)
            {
                if (!volChanged)
                {
                    // ミュート
                    device.AudioEndpointVolume.Mute = !device.AudioEndpointVolume.Mute;
                }
                volChanged = false;
            }

            // Homeボタン
            if (previousBtns.Home && !ws.ButtonState.Home)
            {
                // マウス有効化設定
                wmMouseActivated = !wmMouseActivated;
                if (wmMouseActivated)
                {
                    joyMouseActivated = false;
                    mWiimote.SetLEDs(true, true, true, true);
                    notifyIcon1.Text = "Wiimote Mouse : Enabled";
                } else
                {
                    mWiimote.SetLEDs(true, false, false, true);
                    notifyIcon1.Text = "Wiimote Mouse : Disabled";
                }

                motionMouseToolStripMenuItem.Checked = wmMouseActivated;
                nunchuckMouseToolStripMenuItem.Checked = joyMouseActivated;
            }

        }

        private string BtnToString(WiimoteLib.ButtonState btn)
        {
            if (!btn.A && previousBtns.A) return "A";
            if (!btn.B && previousBtns.B) return "B";
            if (!btn.Down && previousBtns.Down) return "Down";
            if (!btn.Home && previousBtns.Home) return "Home";
            if (!btn.Left && previousBtns.Left) return "Left";
            if (!btn.Minus && previousBtns.Minus) return "Minus";
            if (!btn.One && previousBtns.One) return "One";
            if (!btn.Plus && previousBtns.Plus) return "Plus";
            if (!btn.Right && previousBtns.Right) return "Right";
            if (!btn.Two && previousBtns.Two) return "Two";
            if (!btn.Up && previousBtns.Up) return "Up";

            return null;
        }
    }
}
