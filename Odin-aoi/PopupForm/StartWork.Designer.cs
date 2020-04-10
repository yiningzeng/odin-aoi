namespace power_aoi.PopupForm
{
    partial class StartWork
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
            this.pMain = new System.Windows.Forms.Panel();
            this.btnStart = new System.Windows.Forms.Button();
            this.tbPcbWidth = new System.Windows.Forms.TextBox();
            this.tbPcbLength = new System.Windows.Forms.TextBox();
            this.tbCarrierWidth = new System.Windows.Forms.TextBox();
            this.tbCarrierLength = new System.Windows.Forms.TextBox();
            this.comBoxType = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pTitle = new System.Windows.Forms.Panel();
            this.pbClose = new System.Windows.Forms.PictureBox();
            this.lbTitle = new System.Windows.Forms.Label();
            this.pMain.SuspendLayout();
            this.pTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbClose)).BeginInit();
            this.SuspendLayout();
            // 
            // pMain
            // 
            this.pMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(55)))));
            this.pMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pMain.Controls.Add(this.btnStart);
            this.pMain.Controls.Add(this.tbPcbWidth);
            this.pMain.Controls.Add(this.tbPcbLength);
            this.pMain.Controls.Add(this.tbCarrierWidth);
            this.pMain.Controls.Add(this.tbCarrierLength);
            this.pMain.Controls.Add(this.comBoxType);
            this.pMain.Controls.Add(this.label5);
            this.pMain.Controls.Add(this.label4);
            this.pMain.Controls.Add(this.label3);
            this.pMain.Controls.Add(this.label2);
            this.pMain.Controls.Add(this.label1);
            this.pMain.Controls.Add(this.pTitle);
            this.pMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pMain.Location = new System.Drawing.Point(0, 0);
            this.pMain.Name = "pMain";
            this.pMain.Size = new System.Drawing.Size(334, 255);
            this.pMain.TabIndex = 0;
            // 
            // btnStart
            // 
            this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStart.BackColor = System.Drawing.Color.White;
            this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStart.Location = new System.Drawing.Point(69, 217);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(249, 23);
            this.btnStart.TabIndex = 11;
            this.btnStart.Text = "开始采集";
            this.btnStart.UseVisualStyleBackColor = false;
            // 
            // tbPcbWidth
            // 
            this.tbPcbWidth.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbPcbWidth.Location = new System.Drawing.Point(69, 176);
            this.tbPcbWidth.Name = "tbPcbWidth";
            this.tbPcbWidth.Size = new System.Drawing.Size(249, 21);
            this.tbPcbWidth.TabIndex = 10;
            // 
            // tbPcbLength
            // 
            this.tbPcbLength.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbPcbLength.Location = new System.Drawing.Point(69, 149);
            this.tbPcbLength.Name = "tbPcbLength";
            this.tbPcbLength.Size = new System.Drawing.Size(249, 21);
            this.tbPcbLength.TabIndex = 9;
            // 
            // tbCarrierWidth
            // 
            this.tbCarrierWidth.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbCarrierWidth.Location = new System.Drawing.Point(69, 122);
            this.tbCarrierWidth.Name = "tbCarrierWidth";
            this.tbCarrierWidth.Size = new System.Drawing.Size(249, 21);
            this.tbCarrierWidth.TabIndex = 8;
            // 
            // tbCarrierLength
            // 
            this.tbCarrierLength.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbCarrierLength.Location = new System.Drawing.Point(69, 95);
            this.tbCarrierLength.Name = "tbCarrierLength";
            this.tbCarrierLength.Size = new System.Drawing.Size(249, 21);
            this.tbCarrierLength.TabIndex = 7;
            // 
            // comBoxType
            // 
            this.comBoxType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comBoxType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comBoxType.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.comBoxType.FormattingEnabled = true;
            this.comBoxType.Items.AddRange(new object[] {
            "双面",
            "正面",
            "反面"});
            this.comBoxType.Location = new System.Drawing.Point(69, 69);
            this.comBoxType.Name = "comBoxType";
            this.comBoxType.Size = new System.Drawing.Size(249, 20);
            this.comBoxType.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(16, 179);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 12);
            this.label5.TabIndex = 5;
            this.label5.Text = "PCB板宽";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(16, 152);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 12);
            this.label4.TabIndex = 4;
            this.label4.Text = "PCB板长";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(10, 125);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "采集板宽";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(10, 98);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "采集板长";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(11, 72);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "采集板面";
            // 
            // pTitle
            // 
            this.pTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(45)))));
            this.pTitle.Controls.Add(this.pbClose);
            this.pTitle.Controls.Add(this.lbTitle);
            this.pTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.pTitle.Location = new System.Drawing.Point(0, 0);
            this.pTitle.Name = "pTitle";
            this.pTitle.Size = new System.Drawing.Size(332, 30);
            this.pTitle.TabIndex = 0;
            // 
            // pbClose
            // 
            this.pbClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbClose.Image = global::Odin_aoi.Properties.Resources.close;
            this.pbClose.Location = new System.Drawing.Point(300, 0);
            this.pbClose.Name = "pbClose";
            this.pbClose.Size = new System.Drawing.Size(27, 27);
            this.pbClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbClose.TabIndex = 1;
            this.pbClose.TabStop = false;
            // 
            // lbTitle
            // 
            this.lbTitle.AutoSize = true;
            this.lbTitle.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbTitle.ForeColor = System.Drawing.Color.White;
            this.lbTitle.Location = new System.Drawing.Point(3, 3);
            this.lbTitle.Name = "lbTitle";
            this.lbTitle.Size = new System.Drawing.Size(42, 21);
            this.lbTitle.TabIndex = 0;
            this.lbTitle.Text = "采集";
            // 
            // StartWork
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 255);
            this.Controls.Add(this.pMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "StartWork";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.pMain.ResumeLayout(false);
            this.pMain.PerformLayout();
            this.pTitle.ResumeLayout(false);
            this.pTitle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbClose)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pMain;
        private System.Windows.Forms.Panel pTitle;
        private System.Windows.Forms.Label lbTitle;
        private System.Windows.Forms.PictureBox pbClose;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comBoxType;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.TextBox tbPcbWidth;
        private System.Windows.Forms.TextBox tbPcbLength;
        private System.Windows.Forms.TextBox tbCarrierWidth;
        private System.Windows.Forms.TextBox tbCarrierLength;
    }
}