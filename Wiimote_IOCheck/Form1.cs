using System;
using System.Windows.Forms;
using WiimoteLib;

namespace Wiimote_IOCheck
{
    public partial class Form1 : Form
    {
        private Wiimote mWiimote;
        private delegate void UpdateWiimoteStateDelegate(WiimoteChangedEventArgs args);
        private delegate void UpdateExtensionChangedDelegate(WiimoteExtensionChangedEventArgs args);

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            mWiimote = new Wiimote();

            try
            {
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

            // GUI IOCheck
            #region GUIIOCheck
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

            lblAccel.Text = ws.AccelState.Values.ToString();

            chkLED1.Checked = ws.LEDState.LED1;
            chkLED2.Checked = ws.LEDState.LED2;
            chkLED3.Checked = ws.LEDState.LED3;
            chkLED4.Checked = ws.LEDState.LED4;

            switch (ws.ExtensionType)
            {
                case ExtensionType.Nunchuk:
                    lblChuk.Text = ws.NunchukState.AccelState.Values.ToString();
                    lblChukJoy.Text = ws.NunchukState.Joystick.ToString();
                    chkC.Checked = ws.NunchukState.C;
                    chkZ.Checked = ws.NunchukState.Z;
                    break;

                case ExtensionType.BalanceBoard:
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
                    lblCOG.Text = ws.BalanceBoardState.CenterOfGravity.ToString();
                    break;
            }

            pbBattery.Value = (ws.Battery > 0xc8 ? 0xc8 : (int)ws.Battery);
            lblBattery.Text = ws.Battery.ToString();
            lblDevicePath.Text = "Device Path: " + mWiimote.HIDDevicePath;
#endregion

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
    }
}

