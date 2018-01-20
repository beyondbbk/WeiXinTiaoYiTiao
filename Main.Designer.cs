namespace ShowAndroidModel
{
    partial class Main
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.MsgList = new System.Windows.Forms.ListBox();
            this.btnUp = new System.Windows.Forms.Button();
            this.labelShow = new System.Windows.Forms.Label();
            this.btnDown = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(388, 622);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // MsgList
            // 
            this.MsgList.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.MsgList.FormattingEnabled = true;
            this.MsgList.ItemHeight = 17;
            this.MsgList.Location = new System.Drawing.Point(0, 634);
            this.MsgList.Name = "MsgList";
            this.MsgList.Size = new System.Drawing.Size(388, 157);
            this.MsgList.TabIndex = 3;
            // 
            // btnUp
            // 
            this.btnUp.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnUp.Location = new System.Drawing.Point(0, 797);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(96, 24);
            this.btnUp.TabIndex = 5;
            this.btnUp.Text = "增大距离";
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // labelShow
            // 
            this.labelShow.AutoSize = true;
            this.labelShow.Location = new System.Drawing.Point(158, 803);
            this.labelShow.Name = "labelShow";
            this.labelShow.Size = new System.Drawing.Size(71, 12);
            this.labelShow.TabIndex = 7;
            this.labelShow.Text = "当前倍率1.1";
            this.labelShow.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnDown
            // 
            this.btnDown.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnDown.Location = new System.Drawing.Point(292, 797);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(96, 24);
            this.btnDown.TabIndex = 8;
            this.btnDown.Text = "减小距离";
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // Main
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(389, 826);
            this.Controls.Add(this.btnDown);
            this.Controls.Add(this.labelShow);
            this.Controls.Add(this.btnUp);
            this.Controls.Add(this.MsgList);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Android - By 勇敢的心bbk";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ListBox MsgList;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.Label labelShow;
        private System.Windows.Forms.Button btnDown;
    }
}

