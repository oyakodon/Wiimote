namespace Wiimote_PPT
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
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.終了QToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.boxStatus = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.boxLog = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.ファイルFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.終了時に切断ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.終了QToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.ディスプレイToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon
            // 
            this.notifyIcon.ContextMenuStrip = this.contextMenuStrip;
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "Wiimote PPT";
            this.notifyIcon.Visible = true;
            this.notifyIcon.Click += new System.EventHandler(this.notifyIcon_Click);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.終了QToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(116, 26);
            // 
            // 終了QToolStripMenuItem
            // 
            this.終了QToolStripMenuItem.Name = "終了QToolStripMenuItem";
            this.終了QToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
            this.終了QToolStripMenuItem.Text = "終了(&Q)";
            this.終了QToolStripMenuItem.Click += new System.EventHandler(this.終了QToolStripMenuItem_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.boxStatus);
            this.groupBox1.Location = new System.Drawing.Point(12, 29);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(190, 110);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Status";
            // 
            // boxStatus
            // 
            this.boxStatus.Location = new System.Drawing.Point(6, 18);
            this.boxStatus.Multiline = true;
            this.boxStatus.Name = "boxStatus";
            this.boxStatus.ReadOnly = true;
            this.boxStatus.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.boxStatus.Size = new System.Drawing.Size(178, 86);
            this.boxStatus.TabIndex = 2;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.boxLog);
            this.groupBox2.Location = new System.Drawing.Point(207, 29);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(190, 110);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Log";
            // 
            // boxLog
            // 
            this.boxLog.Location = new System.Drawing.Point(6, 18);
            this.boxLog.Multiline = true;
            this.boxLog.Name = "boxLog";
            this.boxLog.ReadOnly = true;
            this.boxLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.boxLog.Size = new System.Drawing.Size(178, 86);
            this.boxLog.TabIndex = 3;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ファイルFToolStripMenuItem,
            this.ディスプレイToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(409, 24);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // ファイルFToolStripMenuItem
            // 
            this.ファイルFToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.終了時に切断ToolStripMenuItem,
            this.終了QToolStripMenuItem1});
            this.ファイルFToolStripMenuItem.Name = "ファイルFToolStripMenuItem";
            this.ファイルFToolStripMenuItem.Size = new System.Drawing.Size(67, 20);
            this.ファイルFToolStripMenuItem.Text = "ファイル(&F)";
            // 
            // 終了時に切断ToolStripMenuItem
            // 
            this.終了時に切断ToolStripMenuItem.CheckOnClick = true;
            this.終了時に切断ToolStripMenuItem.Name = "終了時に切断ToolStripMenuItem";
            this.終了時に切断ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.終了時に切断ToolStripMenuItem.Text = "終了時に切断";
            // 
            // 終了QToolStripMenuItem1
            // 
            this.終了QToolStripMenuItem1.Name = "終了QToolStripMenuItem1";
            this.終了QToolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.終了QToolStripMenuItem1.Text = "終了(&Q)";
            this.終了QToolStripMenuItem1.Click += new System.EventHandler(this.終了QToolStripMenuItem_Click);
            // 
            // ディスプレイToolStripMenuItem
            // 
            this.ディスプレイToolStripMenuItem.Name = "ディスプレイToolStripMenuItem";
            this.ディスプレイToolStripMenuItem.Size = new System.Drawing.Size(71, 20);
            this.ディスプレイToolStripMenuItem.Text = "ディスプレイ";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(409, 151);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "Wiimote PPT - Status";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.contextMenuStrip.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem 終了QToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox boxStatus;
        private System.Windows.Forms.TextBox boxLog;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ファイルFToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 終了QToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 終了時に切断ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ディスプレイToolStripMenuItem;
    }
}

