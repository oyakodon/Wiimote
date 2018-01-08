using System;
using System.Drawing;
using System.Windows.Forms;
using DxLibDLL;
using WiimoteLib;
using System.Collections.Generic;

namespace Wiimote_DXLib
{
    public partial class Form1 : Form
    {
        private Wiimote mWiimote;
        private delegate void UpdateWiimoteStateDelegate(WiimoteChangedEventArgs args);
        private delegate void UpdateExtensionChangedDelegate(WiimoteExtensionChangedEventArgs args);
        private WiimoteLib.ButtonState previousBtns; // 直前のボタンの状態
        private bool wmActivated = false; // 移動有効/無効
        private float speed = 4.0f; // 視点移動速度
        private static int hDebug_font; // デバッグ用フォントのハンドル

        // MMDファイル(pmx,pmd...)のパス (exeからの)
        // private const string mmdPath = "Pronama/プロ生ちゃん.pmx";
        private const string mmdPath = "サーバルちゃんver1.01/サーバルちゃんver1.01.pmx";

        private　DX.VECTOR Model; // モデルの位置
        private DX.VECTOR V_point; // カメラの視点の方向
        private DX.VECTOR Camera; // カメラの位置

        public Form1()
        {
            InitializeComponent();
            
            // DXライブラリ と Windowsフォームの連携
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
            // Wiimote接続
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

            if (previousBtns.A && !ws.ButtonState.A)
            {
                wmActivated = !wmActivated;
            }

            if (previousBtns.Minus && !ws.ButtonState.Minus)
            {
                // 移動速度減少
                speed -= 0.1f;
                if (speed < 0.1f) speed = 0.1f;
            }

            if (previousBtns.Plus && !ws.ButtonState.Plus)
            {
                // 移動速度増加
                speed += 0.1f;
            }

            if (wmActivated)
            {
                // 移動有効なら、リモコンの加速度で視点変更
                V_point.x += ws.AccelState.Values.X / 10 * speed;
                V_point.y -= ws.AccelState.Values.Y / 10 * speed;
            }

            // 1ボタンと2ボタンで終了
            if (ws.ButtonState.One && ws.ButtonState.Two)
            {
                this.Close();
            }

            // カメラ・視点・スピード　のリセット
            if (previousBtns.Home && ws.ButtonState.Home)
            {
                setVec(ref Model, 0.0f);
                setVec(ref Camera, 0.0f, 20.0f, -30.0f);
                setVec(ref V_point, 0.0f, 10.0f, 0.0f);

                speed = 4.0f;
            }

            switch (ws.ExtensionType)
            {
                case ExtensionType.Nunchuk:
                    var joy = ws.NunchukState.Joystick;

                    // スティックでカメラの位置の変更
                    const float d_cam = 0.1f;
                    if (joy.X > 0.3) Camera.x += d_cam;
                    if (joy.X < -0.3) Camera.x -= d_cam;

                    if (joy.Y > 0.3) Camera.z += d_cam;
                    if (joy.Y < -0.3) Camera.z -= d_cam;

                    if (ws.NunchukState.C) Camera.y += d_cam;
                    if (ws.NunchukState.Z) Camera.y -= d_cam;

                    break;
            }

            previousBtns = ws.ButtonState;
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

        private void setVec (ref DX.VECTOR vec, float x, float y, float z)
        {
            vec.x = x;
            vec.y = y;
            vec.z = z;
        }

        private void setVec(ref DX.VECTOR vec, float v)
        {
            vec.x = v;
            vec.y = v;
            vec.z = v;
        }

        private void drawDebugStr (int X, int Y, params string[] strs)
        {
            int x = X, y = Y;
            uint col = DX.GetColor(255, 255, 255);
            int height = 20;

            foreach (var s in strs)
            {
                DX.DrawStringToHandle(x, y, s, col, hDebug_font);
                y += height;
            }
        }

        public void DXUpdate()
        {
            int mHandle = DX.MV1LoadModel(mmdPath);
            var keys = new byte[256];
            hDebug_font = DX.CreateFontToHandle("Meiryo UI", 20, 3);
            setVec(ref Model, 0.0f);
            setVec(ref Camera, 0.0f, 20.0f, -30.0f);
            setVec(ref V_point, 0.0f, 10.0f, 0.0f);

            DX.SetCameraNearFar(0.1f, 1000.0f);

            Action<int, int, uint> drawFloor = (w, h, col) =>
            {
                DX.DrawTriangle3D(
                    DX.VGet(-w / 2, 0.0f, -h / 2), DX.VGet(-w / 2, 0.0f, h / 2), DX.VGet(w / 2, 0.0f, h / 2), col, DX.TRUE);
                DX.DrawTriangle3D(
                    DX.VGet(-w / 2, 0.0f, -h / 2), DX.VGet(w / 2, 0.0f, -h / 2), DX.VGet(w / 2, 0.0f, h / 2), col, DX.TRUE);
            };

            while (DX.ProcessMessage() == 0 && this.Created)
            {
                // 画面の初期化
                DX.ClearDrawScreen();

                // 床の描画
                drawFloor(160, 80, DX.GetColor(0, 0, 255));
                if (Camera.y >= 0.0f)
                {
                    // カメラが床より上の時
                    drawFloor(20, 10, DX.GetColor(255, 255, 255));
                }

                // 軸の描画
                DX.DrawLine3D(DX.VGet(-320.0f, 0.0f, 0.0f), DX.VGet(320.0f, 0.0f, 0.0f), DX.GetColor(255, 0, 0));
                DX.DrawLine3D(DX.VGet(0.0f, -240.0f, 0.0f), DX.VGet(0.0f, 240.0f, 0.0f), DX.GetColor(255, 0, 0));
                DX.DrawLine3D(DX.VGet(0.0f, 0.0f, -320.0f), DX.VGet(0.0f, 0.0f, 320.0f), DX.GetColor(255, 0, 0));

                #region DXLib_キー入力
                DX.GetHitKeyStateAll(out keys[0]);
                if (keys[DX.KEY_INPUT_ESCAPE] != 0) break;

                if (keys[DX.KEY_INPUT_RIGHT] != 0) Camera.x -= 0.1f;
                if (keys[DX.KEY_INPUT_LEFT] != 0) Camera.x += 0.1f;

                if (keys[DX.KEY_INPUT_UP] != 0) Camera.z -= 0.1f;
                if (keys[DX.KEY_INPUT_DOWN] != 0) Camera.z += 0.1f;

                if (keys[DX.KEY_INPUT_SPACE] != 0) Camera.y -= 0.1f;
                if (keys[DX.KEY_INPUT_LSHIFT] != 0) Camera.y += 0.1f;
                #endregion

                // デバッグ
                drawDebugStr(0, 0,
                    "Camera.x : " + Camera.x,
                    "Camera.y : " + Camera.y,
                    "Camera.z : " + Camera.z,
                    "V_point.x : " + V_point.x,
                    "V_point.y : " + V_point.y,
                    "V_point.z : " + V_point.z,
                    "Speed : "  + speed
                );

                DX.MV1SetPosition(mHandle, Model);
                DX.MV1DrawModel(mHandle);

                if (Camera.y < 0.0f)
                {
                    // カメラが床より下の時
                    drawFloor(20, 10, DX.GetColor(255, 255, 255));
                }

                DX.SetCameraPositionAndTarget_UpVecY(Camera, V_point);

                DX.ScreenFlip();
                Application.DoEvents();
            }
        }
    }
}
