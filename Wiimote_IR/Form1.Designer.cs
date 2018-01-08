namespace Wiimote_IR
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
            this.picBox_IrWindow = new System.Windows.Forms.PictureBox();
            this.lab_Info = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picBox_IrWindow)).BeginInit();
            this.SuspendLayout();
            // 
            // picBox_IrWindow
            // 
            this.picBox_IrWindow.Location = new System.Drawing.Point(12, 27);
            this.picBox_IrWindow.Name = "picBox_IrWindow";
            this.picBox_IrWindow.Size = new System.Drawing.Size(400, 400);
            this.picBox_IrWindow.TabIndex = 0;
            this.picBox_IrWindow.TabStop = false;
            // 
            // lab_Info
            // 
            this.lab_Info.AutoSize = true;
            this.lab_Info.Location = new System.Drawing.Point(12, 9);
            this.lab_Info.Name = "lab_Info";
            this.lab_Info.Size = new System.Drawing.Size(82, 12);
            this.lab_Info.TabIndex = 1;
            this.lab_Info.Text = "INFORMATION";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(460, 460);
            this.Controls.Add(this.lab_Info);
            this.Controls.Add(this.picBox_IrWindow);
            this.Name = "Form1";
            this.Text = "Wiimote IR";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.picBox_IrWindow)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picBox_IrWindow;
        private System.Windows.Forms.Label lab_Info;
    }
}

