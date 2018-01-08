using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;

using WiimoteLib;

namespace Wiimote_IR
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ConnectWiimote();

            // 画面サイズの取得
            ScreenSize.X = Screen.PrimaryScreen.Bounds.Width;
            ScreenSize.Y = Screen.PrimaryScreen.Bounds.Height;
            
        }

        private void Form1_Paint(object sender, PaintEventArgs e) => UpdateInfo();

        private Wiimote mWiimote { get; set; }
        private delegate void UpdateWiimoteStateDelegate(WiimoteChangedEventArgs args);
        private delegate void UpdateExtensionChangedDelegate(WiimoteExtensionChangedEventArgs args);
        private WiimoteLib.ButtonState previousBtns = new WiimoteLib.ButtonState(); // 直前のボタンの状態

        private System.Drawing.Point ScreenSize;

        private void ConnectWiimote()
        {
            // Bluetooth 認識
            var btutil = new BTUtil();
            btutil.ShowDialog();
            if (btutil.userClosed)
            {
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
                        msg = "Wiiリモコンを認識できませんでした。"; break;
                    case WiimoteException _:
                        msg = "Wiiリモコンに何らかのエラーが発生しました。"; break;
                    default:
                        msg = "予期せぬエラーが発生しました。"; break;
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

        private bool invertMode = false;
        private bool irDetected = false;
        private bool moveMouse = false;
        private List<System.Drawing.PointF> irPoints = new List<System.Drawing.PointF>();
        private System.Drawing.PointF averagedIrPoint;

        private void CheckAction(WiimoteState ws)
        {
            var count = ws.IRState.IRSensors.Count(s => s.Found);
            if (count != 0)
            {
                irDetected = true;

                irPoints.Clear();
                float sum_x = 0, sum_y = 0;
                for(var i = 0; i < count; i++)
                {
                    var p = new System.Drawing.PointF();
                    p.X = ws.IRState.IRSensors[i].Position.X;
                    p.Y = ws.IRState.IRSensors[i].Position.Y;
                    if (invertMode) p.X = 1 - p.X;
                    irPoints.Add(p);

                    sum_x += p.X;
                    sum_y += p.Y;
                }

                averagedIrPoint = new System.Drawing.PointF(
                    sum_x / count,
                    sum_y / count
                );

                if (moveMouse)
                {
                    var mouse_p = new System.Drawing.Point();
                    mouse_p.X = (int)(averagedIrPoint.X * ScreenSize.X);
                    mouse_p.Y = (int)(averagedIrPoint.Y * ScreenSize.Y);
                    System.Windows.Forms.Cursor.Position = mouse_p;
                }

                UpdateIRWindow();

            } else
            {
                irDetected = false;
            }

            // Aボタン -> マウス操作の切り替え
            if (ws.ButtonState.A && !previousBtns.A)
            {
                moveMouse = !moveMouse;
            }

            // Bボタン -> X軸の反転
            if (ws.ButtonState.B && !previousBtns.B)
            {
                invertMode = !invertMode;
            }

            UpdateInfo();

        }

        private void UpdateInfo() => lab_Info.Text = $"Inverted : {invertMode}, Detected : {irDetected}";

        private delegate void UpdateIRWindowDelegate();
        private Brush[] irColors = { Brushes.Red, Brushes.Blue, Brushes.Green, Brushes.Yellow };

        private void UpdateIRWindow()
        {
            if (InvokeRequired)
            {
                // 別スレッドから呼び出された場合
                Invoke(new UpdateIRWindowDelegate(UpdateIRWindow), irPoints);
                return;
            }

            Graphics g = picBox_IrWindow.CreateGraphics();
            g.Clear(Color.Black);

            for(var i = 0; i < irPoints.Count; i++)
            {
                g.FillEllipse(
                    irColors[i],
                    irPoints[i].X * 400,
                    irPoints[i].Y * 400, 10, 10
                );
            }

            if (irDetected)
            {
                g.FillEllipse(
                    Brushes.DeepPink,
                    averagedIrPoint.X * 400,
                    averagedIrPoint.Y * 400, 10, 10
                );
            }

            g.Dispose();
        }

    }
}
