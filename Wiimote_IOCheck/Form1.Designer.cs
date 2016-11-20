namespace Wiimote_IOCheck
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.clbButtons = new System.Windows.Forms.CheckedListBox();
            this.chkLED1 = new System.Windows.Forms.CheckBox();
            this.chkLED2 = new System.Windows.Forms.CheckBox();
            this.chkLED3 = new System.Windows.Forms.CheckBox();
            this.chkLED4 = new System.Windows.Forms.CheckBox();
            this.chkRumble = new System.Windows.Forms.CheckBox();
            this.lblAccel = new System.Windows.Forms.Label();
            this.chkC = new System.Windows.Forms.CheckBox();
            this.chkZ = new System.Windows.Forms.CheckBox();
            this.lblChuk = new System.Windows.Forms.Label();
            this.lblChukJoy = new System.Windows.Forms.Label();
            this.gb_Chuk = new System.Windows.Forms.GroupBox();
            this.chkNGraph = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblCOG = new System.Windows.Forms.Label();
            this.lblBBTotal = new System.Windows.Forms.Label();
            this.lblBBBR = new System.Windows.Forms.Label();
            this.lblBBTR = new System.Windows.Forms.Label();
            this.chkLbs = new System.Windows.Forms.CheckBox();
            this.lblBBBL = new System.Windows.Forms.Label();
            this.lblBBTL = new System.Windows.Forms.Label();
            this.pbBattery = new System.Windows.Forms.ProgressBar();
            this.lblBattery = new System.Windows.Forms.Label();
            this.lblDevicePath = new System.Windows.Forms.Label();
            this.chkExtension = new System.Windows.Forms.CheckBox();
            this.ch_x_axis = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.ch_y_axis = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.ch_z_axis = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chkGraph = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.gb_Chuk.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ch_x_axis)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ch_y_axis)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ch_z_axis)).BeginInit();
            this.SuspendLayout();
            // 
            // clbButtons
            // 
            this.clbButtons.FormattingEnabled = true;
            this.clbButtons.Items.AddRange(new object[] {
            "A",
            "B",
            "-",
            "Home",
            "+",
            "1",
            "2",
            "UP",
            "DOWN",
            "LEFT",
            "RIGHT"});
            this.clbButtons.Location = new System.Drawing.Point(12, 12);
            this.clbButtons.Name = "clbButtons";
            this.clbButtons.Size = new System.Drawing.Size(120, 172);
            this.clbButtons.TabIndex = 0;
            // 
            // chkLED1
            // 
            this.chkLED1.AutoSize = true;
            this.chkLED1.Location = new System.Drawing.Point(12, 290);
            this.chkLED1.Name = "chkLED1";
            this.chkLED1.Size = new System.Drawing.Size(51, 16);
            this.chkLED1.TabIndex = 1;
            this.chkLED1.Text = "LED1";
            this.chkLED1.UseVisualStyleBackColor = true;
            this.chkLED1.CheckedChanged += new System.EventHandler(this.chkLED_CheckedChanged);
            // 
            // chkLED2
            // 
            this.chkLED2.AutoSize = true;
            this.chkLED2.Location = new System.Drawing.Point(69, 290);
            this.chkLED2.Name = "chkLED2";
            this.chkLED2.Size = new System.Drawing.Size(51, 16);
            this.chkLED2.TabIndex = 2;
            this.chkLED2.Text = "LED2";
            this.chkLED2.UseVisualStyleBackColor = true;
            this.chkLED2.CheckedChanged += new System.EventHandler(this.chkLED_CheckedChanged);
            // 
            // chkLED3
            // 
            this.chkLED3.AutoSize = true;
            this.chkLED3.Location = new System.Drawing.Point(126, 290);
            this.chkLED3.Name = "chkLED3";
            this.chkLED3.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.chkLED3.Size = new System.Drawing.Size(51, 16);
            this.chkLED3.TabIndex = 3;
            this.chkLED3.Text = "LED3";
            this.chkLED3.UseVisualStyleBackColor = true;
            this.chkLED3.CheckedChanged += new System.EventHandler(this.chkLED_CheckedChanged);
            // 
            // chkLED4
            // 
            this.chkLED4.AutoSize = true;
            this.chkLED4.Location = new System.Drawing.Point(183, 290);
            this.chkLED4.Name = "chkLED4";
            this.chkLED4.Size = new System.Drawing.Size(51, 16);
            this.chkLED4.TabIndex = 4;
            this.chkLED4.Text = "LED4";
            this.chkLED4.UseVisualStyleBackColor = true;
            this.chkLED4.CheckedChanged += new System.EventHandler(this.chkLED_CheckedChanged);
            // 
            // chkRumble
            // 
            this.chkRumble.AutoSize = true;
            this.chkRumble.Location = new System.Drawing.Point(261, 290);
            this.chkRumble.Name = "chkRumble";
            this.chkRumble.Size = new System.Drawing.Size(62, 16);
            this.chkRumble.TabIndex = 5;
            this.chkRumble.Text = "Rumble";
            this.chkRumble.UseVisualStyleBackColor = true;
            this.chkRumble.CheckedChanged += new System.EventHandler(this.chkRumble_CheckedChanged);
            // 
            // lblAccel
            // 
            this.lblAccel.AutoSize = true;
            this.lblAccel.Location = new System.Drawing.Point(12, 253);
            this.lblAccel.Name = "lblAccel";
            this.lblAccel.Size = new System.Drawing.Size(25, 12);
            this.lblAccel.TabIndex = 6;
            this.lblAccel.Text = "Acc";
            // 
            // chkC
            // 
            this.chkC.AutoSize = true;
            this.chkC.Location = new System.Drawing.Point(6, 18);
            this.chkC.Name = "chkC";
            this.chkC.Size = new System.Drawing.Size(32, 16);
            this.chkC.TabIndex = 7;
            this.chkC.Text = "C";
            this.chkC.UseVisualStyleBackColor = true;
            // 
            // chkZ
            // 
            this.chkZ.AutoSize = true;
            this.chkZ.Location = new System.Drawing.Point(6, 40);
            this.chkZ.Name = "chkZ";
            this.chkZ.Size = new System.Drawing.Size(31, 16);
            this.chkZ.TabIndex = 8;
            this.chkZ.Text = "Z";
            this.chkZ.UseVisualStyleBackColor = true;
            // 
            // lblChuk
            // 
            this.lblChuk.AutoSize = true;
            this.lblChuk.Location = new System.Drawing.Point(4, 72);
            this.lblChuk.Name = "lblChuk";
            this.lblChuk.Size = new System.Drawing.Size(35, 12);
            this.lblChuk.TabIndex = 9;
            this.lblChuk.Text = "label1";
            // 
            // lblChukJoy
            // 
            this.lblChukJoy.AutoSize = true;
            this.lblChukJoy.Location = new System.Drawing.Point(4, 98);
            this.lblChukJoy.Name = "lblChukJoy";
            this.lblChukJoy.Size = new System.Drawing.Size(35, 12);
            this.lblChukJoy.TabIndex = 10;
            this.lblChukJoy.Text = "label2";
            // 
            // gb_Chuk
            // 
            this.gb_Chuk.Controls.Add(this.chkNGraph);
            this.gb_Chuk.Controls.Add(this.chkC);
            this.gb_Chuk.Controls.Add(this.lblChukJoy);
            this.gb_Chuk.Controls.Add(this.chkZ);
            this.gb_Chuk.Controls.Add(this.lblChuk);
            this.gb_Chuk.Location = new System.Drawing.Point(281, 168);
            this.gb_Chuk.Name = "gb_Chuk";
            this.gb_Chuk.Size = new System.Drawing.Size(250, 114);
            this.gb_Chuk.TabIndex = 11;
            this.gb_Chuk.TabStop = false;
            this.gb_Chuk.Text = "Nunchuk";
            // 
            // chkNGraph
            // 
            this.chkNGraph.AutoSize = true;
            this.chkNGraph.Location = new System.Drawing.Point(177, 18);
            this.chkNGraph.Name = "chkNGraph";
            this.chkNGraph.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkNGraph.Size = new System.Drawing.Size(54, 16);
            this.chkNGraph.TabIndex = 20;
            this.chkNGraph.Text = "Graph";
            this.chkNGraph.UseVisualStyleBackColor = true;
            this.chkNGraph.CheckedChanged += new System.EventHandler(this.chkNGraph_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblCOG);
            this.groupBox1.Controls.Add(this.lblBBTotal);
            this.groupBox1.Controls.Add(this.lblBBBR);
            this.groupBox1.Controls.Add(this.lblBBTR);
            this.groupBox1.Controls.Add(this.chkLbs);
            this.groupBox1.Controls.Add(this.lblBBBL);
            this.groupBox1.Controls.Add(this.lblBBTL);
            this.groupBox1.Location = new System.Drawing.Point(183, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(348, 148);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "BalanceBoard";
            // 
            // lblCOG
            // 
            this.lblCOG.AutoSize = true;
            this.lblCOG.Location = new System.Drawing.Point(145, 120);
            this.lblCOG.Name = "lblCOG";
            this.lblCOG.Size = new System.Drawing.Size(35, 12);
            this.lblCOG.TabIndex = 14;
            this.lblCOG.Text = "label2";
            // 
            // lblBBTotal
            // 
            this.lblBBTotal.AutoSize = true;
            this.lblBBTotal.Location = new System.Drawing.Point(145, 64);
            this.lblBBTotal.Name = "lblBBTotal";
            this.lblBBTotal.Size = new System.Drawing.Size(35, 12);
            this.lblBBTotal.TabIndex = 13;
            this.lblBBTotal.Text = "label2";
            // 
            // lblBBBR
            // 
            this.lblBBBR.AutoSize = true;
            this.lblBBBR.Location = new System.Drawing.Point(273, 98);
            this.lblBBBR.Name = "lblBBBR";
            this.lblBBBR.Size = new System.Drawing.Size(35, 12);
            this.lblBBBR.TabIndex = 12;
            this.lblBBBR.Text = "label1";
            // 
            // lblBBTR
            // 
            this.lblBBTR.AutoSize = true;
            this.lblBBTR.Location = new System.Drawing.Point(273, 44);
            this.lblBBTR.Name = "lblBBTR";
            this.lblBBTR.Size = new System.Drawing.Size(35, 12);
            this.lblBBTR.TabIndex = 11;
            this.lblBBTR.Text = "label1";
            // 
            // chkLbs
            // 
            this.chkLbs.AutoSize = true;
            this.chkLbs.Location = new System.Drawing.Point(6, 18);
            this.chkLbs.Name = "chkLbs";
            this.chkLbs.Size = new System.Drawing.Size(42, 16);
            this.chkLbs.TabIndex = 7;
            this.chkLbs.Text = "Lbs";
            this.chkLbs.UseVisualStyleBackColor = true;
            // 
            // lblBBBL
            // 
            this.lblBBBL.AutoSize = true;
            this.lblBBBL.Location = new System.Drawing.Point(33, 98);
            this.lblBBBL.Name = "lblBBBL";
            this.lblBBBL.Size = new System.Drawing.Size(35, 12);
            this.lblBBBL.TabIndex = 10;
            this.lblBBBL.Text = "label2";
            // 
            // lblBBTL
            // 
            this.lblBBTL.AutoSize = true;
            this.lblBBTL.Location = new System.Drawing.Point(33, 44);
            this.lblBBTL.Name = "lblBBTL";
            this.lblBBTL.Size = new System.Drawing.Size(35, 12);
            this.lblBBTL.TabIndex = 9;
            this.lblBBTL.Text = "label1";
            // 
            // pbBattery
            // 
            this.pbBattery.Location = new System.Drawing.Point(12, 208);
            this.pbBattery.Name = "pbBattery";
            this.pbBattery.Size = new System.Drawing.Size(100, 23);
            this.pbBattery.TabIndex = 13;
            // 
            // lblBattery
            // 
            this.lblBattery.AutoSize = true;
            this.lblBattery.Location = new System.Drawing.Point(119, 215);
            this.lblBattery.Name = "lblBattery";
            this.lblBattery.Size = new System.Drawing.Size(100, 12);
            this.lblBattery.TabIndex = 11;
            this.lblBattery.Text = "Battery Remaining";
            // 
            // lblDevicePath
            // 
            this.lblDevicePath.AutoEllipsis = true;
            this.lblDevicePath.Location = new System.Drawing.Point(10, 313);
            this.lblDevicePath.Name = "lblDevicePath";
            this.lblDevicePath.Size = new System.Drawing.Size(437, 62);
            this.lblDevicePath.TabIndex = 14;
            this.lblDevicePath.Text = "label1";
            // 
            // chkExtension
            // 
            this.chkExtension.AutoSize = true;
            this.chkExtension.Location = new System.Drawing.Point(138, 168);
            this.chkExtension.Name = "chkExtension";
            this.chkExtension.Size = new System.Drawing.Size(50, 16);
            this.chkExtension.TabIndex = 15;
            this.chkExtension.Text = "None";
            this.chkExtension.UseVisualStyleBackColor = true;
            // 
            // ch_x_axis
            // 
            this.ch_x_axis.AntiAliasing = System.Windows.Forms.DataVisualization.Charting.AntiAliasingStyles.None;
            chartArea1.AxisX.Maximum = 200D;
            chartArea1.AxisY.Maximum = 150D;
            chartArea1.AxisY.Minimum = -150D;
            chartArea1.Name = "ChartArea1";
            this.ch_x_axis.ChartAreas.Add(chartArea1);
            this.ch_x_axis.Location = new System.Drawing.Point(571, 12);
            this.ch_x_axis.Name = "ch_x_axis";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series1.Name = "Series1";
            this.ch_x_axis.Series.Add(series1);
            this.ch_x_axis.Size = new System.Drawing.Size(469, 356);
            this.ch_x_axis.TabIndex = 16;
            this.ch_x_axis.Text = "chart1";
            // 
            // ch_y_axis
            // 
            this.ch_y_axis.AntiAliasing = System.Windows.Forms.DataVisualization.Charting.AntiAliasingStyles.None;
            chartArea2.AxisX.Maximum = 200D;
            chartArea2.AxisY.Maximum = 150D;
            chartArea2.AxisY.Minimum = -150D;
            chartArea2.Name = "ChartArea1";
            this.ch_y_axis.ChartAreas.Add(chartArea2);
            this.ch_y_axis.Location = new System.Drawing.Point(4, 378);
            this.ch_y_axis.Name = "ch_y_axis";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series2.Name = "Series1";
            this.ch_y_axis.Series.Add(series2);
            this.ch_y_axis.Size = new System.Drawing.Size(469, 356);
            this.ch_y_axis.TabIndex = 17;
            this.ch_y_axis.Text = "chart1";
            // 
            // ch_z_axis
            // 
            this.ch_z_axis.AntiAliasing = System.Windows.Forms.DataVisualization.Charting.AntiAliasingStyles.None;
            chartArea3.AxisX.Maximum = 200D;
            chartArea3.AxisY.Maximum = 150D;
            chartArea3.AxisY.Minimum = -150D;
            chartArea3.Name = "ChartArea1";
            this.ch_z_axis.ChartAreas.Add(chartArea3);
            this.ch_z_axis.Location = new System.Drawing.Point(571, 378);
            this.ch_z_axis.Name = "ch_z_axis";
            series3.ChartArea = "ChartArea1";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series3.Name = "Series1";
            this.ch_z_axis.Series.Add(series3);
            this.ch_z_axis.Size = new System.Drawing.Size(469, 356);
            this.ch_z_axis.TabIndex = 18;
            this.ch_z_axis.Text = "chart1";
            // 
            // chkGraph
            // 
            this.chkGraph.AutoSize = true;
            this.chkGraph.Location = new System.Drawing.Point(458, 290);
            this.chkGraph.Name = "chkGraph";
            this.chkGraph.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkGraph.Size = new System.Drawing.Size(54, 16);
            this.chkGraph.TabIndex = 19;
            this.chkGraph.Text = "Graph";
            this.chkGraph.UseVisualStyleBackColor = true;
            this.chkGraph.CheckedChanged += new System.EventHandler(this.chkGraph_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 375);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 12);
            this.label1.TabIndex = 20;
            this.label1.Text = "Y:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(569, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(14, 12);
            this.label2.TabIndex = 21;
            this.label2.Text = "X:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(569, 375);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(14, 12);
            this.label3.TabIndex = 22;
            this.label3.Text = "Z:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1074, 734);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chkGraph);
            this.Controls.Add(this.ch_z_axis);
            this.Controls.Add(this.ch_y_axis);
            this.Controls.Add(this.ch_x_axis);
            this.Controls.Add(this.chkExtension);
            this.Controls.Add(this.lblDevicePath);
            this.Controls.Add(this.lblBattery);
            this.Controls.Add(this.pbBattery);
            this.Controls.Add(this.chkRumble);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.gb_Chuk);
            this.Controls.Add(this.lblAccel);
            this.Controls.Add(this.chkLED4);
            this.Controls.Add(this.chkLED3);
            this.Controls.Add(this.chkLED2);
            this.Controls.Add(this.chkLED1);
            this.Controls.Add(this.clbButtons);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "IOCheck";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.gb_Chuk.ResumeLayout(false);
            this.gb_Chuk.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ch_x_axis)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ch_y_axis)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ch_z_axis)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox clbButtons;
        private System.Windows.Forms.CheckBox chkLED1;
        private System.Windows.Forms.CheckBox chkLED2;
        private System.Windows.Forms.CheckBox chkLED3;
        private System.Windows.Forms.CheckBox chkLED4;
        private System.Windows.Forms.CheckBox chkRumble;
        private System.Windows.Forms.Label lblAccel;
        private System.Windows.Forms.CheckBox chkC;
        private System.Windows.Forms.CheckBox chkZ;
        private System.Windows.Forms.Label lblChuk;
        private System.Windows.Forms.Label lblChukJoy;
        private System.Windows.Forms.GroupBox gb_Chuk;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkLbs;
        private System.Windows.Forms.Label lblBBBL;
        private System.Windows.Forms.Label lblBBTL;
        private System.Windows.Forms.Label lblBBTotal;
        private System.Windows.Forms.Label lblBBBR;
        private System.Windows.Forms.Label lblBBTR;
        private System.Windows.Forms.Label lblCOG;
        private System.Windows.Forms.ProgressBar pbBattery;
        private System.Windows.Forms.Label lblBattery;
        private System.Windows.Forms.Label lblDevicePath;
        private System.Windows.Forms.CheckBox chkExtension;
        private System.Windows.Forms.DataVisualization.Charting.Chart ch_x_axis;
        private System.Windows.Forms.DataVisualization.Charting.Chart ch_y_axis;
        private System.Windows.Forms.DataVisualization.Charting.Chart ch_z_axis;
        private System.Windows.Forms.CheckBox chkNGraph;
        private System.Windows.Forms.CheckBox chkGraph;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}

