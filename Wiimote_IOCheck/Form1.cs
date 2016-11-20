using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using WiimoteLib;

namespace Wiimote_IOCheck
{
    public partial class Form1 : Form
    {
        private Wiimote mWiimote;
        private delegate void UpdateWiimoteStateDelegate(WiimoteChangedEventArgs args);
        private delegate void UpdateExtensionChangedDelegate(WiimoteExtensionChangedEventArgs args);
        private List<Queue<float>> axis_history = new List<Queue<float>>();　// 加速度の履歴(グラフの点)
        private bool showGraph = false;　// グラフを表示するか
        private bool showNGraph = false; // ヌンチャクのグラフを表示するか

        public Form1()
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            mWiimote = new Wiimote();

            try
            {
                // Wiiリモコンへの接続
                mWiimote.WiimoteChanged += (s, args) => UpdateState(args);
                mWiimote.WiimoteExtensionChanged += (s, args) => UpdateExtension(args);
                mWiimote.Connect();
                mWiimote.SetReportType(InputReport.IRExtensionAccel, true);
                mWiimote.SetLEDs(false, true, true, false);

            }
            catch (WiimoteNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "Wiimote not found error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (WiimoteException ex)
            {
                MessageBox.Show(ex.Message, "Wiimote error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Unknown error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            //　初期化
            for(int i = 0; i < 3; i++) axis_history.Add(new Queue<float>());

            this.Size = new System.Drawing.Size(560, 400);
        }

        public void UpdateState(WiimoteChangedEventArgs args)
        {
            BeginInvoke(new UpdateWiimoteStateDelegate(UpdateWiimoteChanged), args);
        }

        public void UpdateExtension(WiimoteExtensionChangedEventArgs args)
        {
            BeginInvoke(new UpdateExtensionChangedDelegate(UpdateExtensionChanged), args);
        }

        private void chkLED_CheckedChanged(object sender, EventArgs e)
        {
            mWiimote.SetLEDs(chkLED1.Checked, chkLED2.Checked, chkLED3.Checked, chkLED4.Checked);
        }

        private void chkRumble_CheckedChanged(object sender, EventArgs e)
        {
            mWiimote.SetRumble(chkRumble.Checked);
        }

        private void UpdateWiimoteChanged(WiimoteChangedEventArgs args)
        {
            WiimoteState ws = args.WiimoteState;

            // グラフへの描画 (ヌンチャクのチェック無し)
            if (showGraph && !showNGraph)
            {
                    showChart(ch_x_axis, axis_history[0], ws.AccelState.Values.X * 100);
                    showChart(ch_y_axis, axis_history[1], ws.AccelState.Values.Y * 100);
                    showChart(ch_z_axis, axis_history[2], ws.AccelState.Values.Z * 100);
            }

            // ボタンの状態
            clbButtons.SetItemChecked(0, ws.ButtonState.A);
            clbButtons.SetItemChecked(1, ws.ButtonState.B);
            clbButtons.SetItemChecked(2, ws.ButtonState.Minus);
            clbButtons.SetItemChecked(3, ws.ButtonState.Home);
            clbButtons.SetItemChecked(4, ws.ButtonState.Plus);
            clbButtons.SetItemChecked(5, ws.ButtonState.One);
            clbButtons.SetItemChecked(6, ws.ButtonState.Two);
            clbButtons.SetItemChecked(7, ws.ButtonState.Up);
            clbButtons.SetItemChecked(8, ws.ButtonState.Down);
            clbButtons.SetItemChecked(9, ws.ButtonState.Left);
            clbButtons.SetItemChecked(10, ws.ButtonState.Right);

            // リモコンの加速度
            lblAccel.Text = ws.AccelState.Values.ToString();

            // LEDの状態
            chkLED1.Checked = ws.LEDState.LED1;
            chkLED2.Checked = ws.LEDState.LED2;
            chkLED3.Checked = ws.LEDState.LED3;
            chkLED4.Checked = ws.LEDState.LED4;

            // 接続されているアクセサリで分岐
            switch (ws.ExtensionType)
            {
                // ヌンチャク
                case ExtensionType.Nunchuk:
                    // グラフへの描画 (ヌンチャクのチェック有り)
                    if (showGraph && showNGraph)
                    {
                        showChart(ch_x_axis, axis_history[0], ws.NunchukState.AccelState.Values.X * 100);
                        showChart(ch_y_axis, axis_history[1], ws.NunchukState.AccelState.Values.Y * 100);
                        showChart(ch_z_axis, axis_history[2], ws.NunchukState.AccelState.Values.Z * 100);
                    }
                    // 加速度センサとジョイスティック
                    lblChuk.Text = ws.NunchukState.AccelState.Values.ToString();
                    lblChukJoy.Text = ws.NunchukState.Joystick.ToString();

                    // ボタン
                    chkC.Checked = ws.NunchukState.C;
                    chkZ.Checked = ws.NunchukState.Z;
                    break;

                // Wii バランスボード
                case ExtensionType.BalanceBoard:
                    // 重さ
                    if (chkLbs.Checked)
                    {
                        lblBBTL.Text = ws.BalanceBoardState.SensorValuesLb.TopLeft.ToString();
                        lblBBTR.Text = ws.BalanceBoardState.SensorValuesLb.TopRight.ToString();
                        lblBBBL.Text = ws.BalanceBoardState.SensorValuesLb.BottomLeft.ToString();
                        lblBBBR.Text = ws.BalanceBoardState.SensorValuesLb.BottomRight.ToString();
                        lblBBTotal.Text = ws.BalanceBoardState.WeightLb.ToString();
                    } else
                    {
                        lblBBTL.Text = ws.BalanceBoardState.SensorValuesKg.TopLeft.ToString();
                        lblBBTR.Text = ws.BalanceBoardState.SensorValuesKg.TopRight.ToString();
                        lblBBBL.Text = ws.BalanceBoardState.SensorValuesKg.BottomLeft.ToString();
                        lblBBBR.Text = ws.BalanceBoardState.SensorValuesKg.BottomRight.ToString();
                        lblBBTotal.Text = ws.BalanceBoardState.WeightKg.ToString();
                    }
                    // 中央の重力
                    lblCOG.Text = ws.BalanceBoardState.CenterOfGravity.ToString();
                    break;
            }

            // 電池残量
            pbBattery.Value = (ws.Battery > 0xc8 ? 0xc8 : (int)ws.Battery);
            lblBattery.Text = ws.Battery.ToString();

            // デバイスパス
            lblDevicePath.Text = "Device Path: " + mWiimote.HIDDevicePath;
        }

        private void UpdateExtensionChanged(WiimoteExtensionChangedEventArgs args)
        {
            chkExtension.Text = args.ExtensionType.ToString();
            chkExtension.Checked = args.Inserted;

            if (args.Inserted)
                mWiimote.SetReportType(InputReport.IRExtensionAccel, true);
            else
                mWiimote.SetReportType(InputReport.IRAccel, true);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            mWiimote.Disconnect();
        }

        private void showChart(Chart chart, Queue<float> q, float v)
        {
            // グラフへの描画
            q.Enqueue(v);

            // 200個以上のデータは削除
            while (q.Count > 200)
            {
                q.Dequeue();
            }

            // 表上の点を全て削除
            chart.Series[0].Points.Clear();
            foreach (int value in q)
            {
                // 点の追加
                chart.Series[0].Points.Add(new DataPoint(0, value));
            }
        }

        private void chkNGraph_CheckedChanged(object sender, EventArgs e)
        {
            if (chkNGraph.Checked) {
                // ヌンチャクがチェックされた
                chkGraph.Checked = true;
                showGraph = true;
                showNGraph = true;
            } else
            {
                // チャック外された
                showNGraph = false;

            }
        }

        private void chkGraph_CheckedChanged(object sender, EventArgs e)
        {
            showGraph = chkGraph.Checked;
            if (showGraph) this.Size = new System.Drawing.Size(1090, 773);
            else this.Size = new System.Drawing.Size(560, 400);
        }
    }
}

