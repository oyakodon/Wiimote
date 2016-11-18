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
            while (DX.ProcessMessage() == 0 && this.Created)
            {


                DX.ScreenFlip();
                Application.DoEvents();
            }
        }

    }
}
