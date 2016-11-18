using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DxLibDLL;

namespace Wiimote_DXLib
{
    public partial class Form1 : Form
    {
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
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            DX.DxLib_End();
        }

        public void DXUpdate()
        {
            DX.VECTOR Model;
            Model.x = Model.y = Model.z = 0.0f;

            int mHandle = DX.MV1LoadModel("Yukari/結月ゆかり_純_ver1.0.pmd");
            var keys = new byte[256];

            DX.SetCameraNearFar(0.1f, 1000.0f);

            while (DX.ProcessMessage() == 0 && this.Created)
            {
                DX.ClearDrawScreen();

                DX.GetHitKeyStateAll(out keys[0]);
                
                if (keys[DX.KEY_INPUT_ESCAPE] != 0)
                {
                    break;
                }

                DX.SetCameraPositionAndTarget_UpVecY(DX.VGet(0, 10, -20), DX.VGet(0.0f, 10.0f, 0.0f));

                if (keys[DX.KEY_INPUT_RIGHT] != 0) Model.x += 0.1f;
                if (keys[DX.KEY_INPUT_LEFT] != 0) Model.x -= 0.1f;

                if (keys[DX.KEY_INPUT_UP] != 0) Model.z -= 0.1f;
                if (keys[DX.KEY_INPUT_DOWN] != 0) Model.z += 0.1f;

                if (keys[DX.KEY_INPUT_SPACE] != 0) Model.y -= 0.1f;
                if (keys[DX.KEY_INPUT_LSHIFT] != 0) Model.y += 0.1f;

                DX.MV1SetPosition(mHandle, Model);
                DX.MV1DrawModel(mHandle);

                DX.ScreenFlip();
                Application.DoEvents();
            }
        }

    }
}
