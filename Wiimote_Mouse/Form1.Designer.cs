namespace Wiimote_Mouse
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.statPbBattery = new System.Windows.Forms.ToolStripProgressBar();
            this.statLblBattery = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.statLblVol = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quitQToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.motionMouseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nunchuckMouseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "Wiimote";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // statusStrip1
            // 
            this.statusStrip1.AllowMerge = false;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel2,
            this.statPbBattery,
            this.statLblBattery,
            this.toolStripStatusLabel3,
            this.toolStripStatusLabel1,
            this.statLblVol});
            this.statusStrip1.Location = new System.Drawing.Point(0, 28);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(430, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 6;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(47, 17);
            this.toolStripStatusLabel2.Text = "Battery:";
            // 
            // statPbBattery
            // 
            this.statPbBattery.Name = "statPbBattery";
            this.statPbBattery.Size = new System.Drawing.Size(100, 16);
            // 
            // statLblBattery
            // 
            this.statLblBattery.Name = "statLblBattery";
            this.statLblBattery.Size = new System.Drawing.Size(22, 17);
            this.statLblBattery.Text = "-%";
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(196, 17);
            this.toolStripStatusLabel3.Spring = true;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(26, 17);
            this.toolStripStatusLabel1.Text = "Vol:";
            // 
            // statLblVol
            // 
            this.statLblVol.Name = "statLblVol";
            this.statLblVol.Size = new System.Drawing.Size(22, 17);
            this.statLblVol.Text = "-%";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileFToolStripMenuItem,
            this.toolsTToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(430, 24);
            this.menuStrip1.TabIndex = 7;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileFToolStripMenuItem
            // 
            this.fileFToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.quitQToolStripMenuItem});
            this.fileFToolStripMenuItem.Name = "fileFToolStripMenuItem";
            this.fileFToolStripMenuItem.Size = new System.Drawing.Size(51, 20);
            this.fileFToolStripMenuItem.Text = "File(&F)";
            // 
            // quitQToolStripMenuItem
            // 
            this.quitQToolStripMenuItem.Name = "quitQToolStripMenuItem";
            this.quitQToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.quitQToolStripMenuItem.Text = "Quit(&Q)";
            this.quitQToolStripMenuItem.Click += new System.EventHandler(this.quitQToolStripMenuItem_Click);
            // 
            // toolsTToolStripMenuItem
            // 
            this.toolsTToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.motionMouseToolStripMenuItem,
            this.nunchuckMouseToolStripMenuItem,
            this.optionsToolStripMenuItem});
            this.toolsTToolStripMenuItem.Name = "toolsTToolStripMenuItem";
            this.toolsTToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
            this.toolsTToolStripMenuItem.Text = "Tools(&T)";
            // 
            // motionMouseToolStripMenuItem
            // 
            this.motionMouseToolStripMenuItem.Name = "motionMouseToolStripMenuItem";
            this.motionMouseToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.motionMouseToolStripMenuItem.Text = "Motion Mouse";
            this.motionMouseToolStripMenuItem.Click += new System.EventHandler(this.motionMouseToolStripMenuItem_Click);
            // 
            // nunchuckMouseToolStripMenuItem
            // 
            this.nunchuckMouseToolStripMenuItem.Name = "nunchuckMouseToolStripMenuItem";
            this.nunchuckMouseToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.nunchuckMouseToolStripMenuItem.Text = "Nunchuck Mouse";
            this.nunchuckMouseToolStripMenuItem.Click += new System.EventHandler(this.nunchuckMouseToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.optionsToolStripMenuItem.Text = "Options...";
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(430, 50);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Wiimote Mouse";
            this.TopMost = true;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel statLblVol;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripProgressBar statPbBattery;
        private System.Windows.Forms.ToolStripStatusLabel statLblBattery;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileFToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsTToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quitQToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem motionMouseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nunchuckMouseToolStripMenuItem;
    }
}

