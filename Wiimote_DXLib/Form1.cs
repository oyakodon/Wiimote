using System;
using System.Drawing;
using System.Windows.Forms;
using DxLibDLL;
using WiimoteLib;

namespace Wiimote_DXLib
{
    public partial class Form1 : Form
    {
        private Wiimote mWiimote;
        private delegate void UpdateWiimoteStateDelegate(WiimoteChangedEventArgs args);
        private delegate void UpdateExtensionChangedDelegate(WiimoteExtensionChangedEventArgs args);
        private WiimoteLib.ButtonState previousBtns;
        private WiimoteLib.NunchukState previousNunchuk;
        private bool wmActivated = false;
        private int speed = 200;

        private　DX.VECTOR Model;
        private DX.VECTOR Rotate;

        public Form1()
        {
            InitializeComponent();
            this.Text = "Wiimote MMD";
            this.ClientSize = new Size(640, 480);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            DX.SetUserWindow(this.Handle); //DxLibの親ウインドウをこのフォームウインドウにセット
            DX.SetDrawScreen(DX.DX_SCREEN_BACK);
            DX.SetAlwaysRunFlag(DX.TRUE);
            DX.DxLib_Init();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Wiimote初期化
            mWiimote = new Wiimote();

            try
            {
                mWiimote.WiimoteChanged += (s, args) => UpdateState(args);
                mWiimote.WiimoteExtensionChanged += (s, args) => UpdateExtension(args);
                mWiimote.Connect();
                mWiimote.SetReportType(InputReport.IRExtensionAccel, true);
                mWiimote.SetLEDs(true, false, false, false);

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

            // DX
            Model.x = Model.y = Model.z = 0.0f;
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

            #region 判定
            // 加速度センサによるモデル
            if (previousBtns.A && !ws.ButtonState.A)
            {
                wmActivated = !wmActivated;
            }

            if (previousBtns.Minus && !ws.ButtonState.Minus)
            {
                speed += 100;
            }

            if (previousBtns.Plus && !ws.ButtonState.Plus)
            {
                speed -= 100;
                if (speed < 200) speed = 200;
            }

            if (wmActivated)
            {
                Rotate.y -= (float)(Math.Round(ws.AccelState.Values.X * 20) / speed);
                Rotate.x += (float)(Math.Round(ws.AccelState.Values.Y * 20) / speed);
            }

            if (ws.ButtonState.Home)
            {
                this.Close();
            }

            switch (ws.ExtensionType)
            {
                case ExtensionType.Nunchuk:
                    var joy = ws.NunchukState.Joystick;
                    if (joy.X > 0.3) Model.x -= 0.25f;
                    if (joy.X < -0.3) Model.x += 0.25f;

                    if (joy.Y > 0.3) Model.z -= 0.25f;
                    if (joy.Y < -0.3) Model.z += 0.25f;

                    if (ws.NunchukState.C) Model.y -= 0.25f;
                    if (ws.NunchukState.Z) Model.y += 0.25f;

                    previousNunchuk = ws.NunchukState;
                    break;
                    
            }

            previousBtns = ws.ButtonState;
            #endregion

        }

        private void UpdateExtensionChanged(WiimoteExtensionChangedEventArgs args)
        {
            if (args.Inserted)
                mWiimote.SetReportType(InputReport.IRExtensionAccel, true);
            else
                mWiimote.SetReportType(InputReport.IRAccel, true);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            mWiimote.SetLEDs(false, false, false, false);
            mWiimote.Disconnect();

            DX.DxLib_End();
        }

        public void DXUpdate()
        {
            int mHandle = DX.MV1LoadModel("Yukari/結月ゆかり_純_ver1.0.pmd");
            var keys = new byte[256];

            DX.SetCameraNearFar(0.1f, 1000.0f);

            while (DX.ProcessMessage() == 0 && this.Created)
            {
                DX.ClearDrawScreen();

                #region DXLib_キー入力
                DX.GetHitKeyStateAll(out keys[0]);
                if (keys[DX.KEY_INPUT_ESCAPE] != 0) break;

                if (keys[DX.KEY_INPUT_RIGHT] != 0) Model.x += 0.1f;
                if (keys[DX.KEY_INPUT_LEFT] != 0) Model.x -= 0.1f;

                if (keys[DX.KEY_INPUT_UP] != 0) Model.z -= 0.1f;
                if (keys[DX.KEY_INPUT_DOWN] != 0) Model.z += 0.1f;

                if (keys[DX.KEY_INPUT_SPACE] != 0) Model.y -= 0.1f;
                if (keys[DX.KEY_INPUT_LSHIFT] != 0) Model.y += 0.1f;
                #endregion

                DX.SetCameraPositionAndTarget_UpVecY(DX.VGet(0, 10, -20), DX.VGet(0.0f, 10.0f, 0.0f));

                DX.MV1SetPosition(mHandle, Model);
                DX.MV1SetRotationXYZ(mHandle, Rotate);
                DX.MV1DrawModel(mHandle);

                DX.ScreenFlip();
                Application.DoEvents();
            }
        }
    }
}
