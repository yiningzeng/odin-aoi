namespace power_aoi.DockerPanelOdin
{
    partial class ToolBarForm
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
            this.menuStripControl = new power_aoi.UserControls.CustomMenuStripControl();
            this.设置ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmClose = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmSquare = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmMin = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStripControl
            // 
            this.menuStripControl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.menuStripControl.EndColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.menuStripControl.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.设置ToolStripMenuItem1,
            this.tsmClose,
            this.tsmSquare,
            this.tsmMin});
            this.menuStripControl.Location = new System.Drawing.Point(0, 0);
            this.menuStripControl.Name = "menuStripControl";
            this.menuStripControl.Size = new System.Drawing.Size(1140, 25);
            this.menuStripControl.StartColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.menuStripControl.TabIndex = 7;
            this.menuStripControl.Text = " ";
            // 
            // 设置ToolStripMenuItem1
            // 
            this.设置ToolStripMenuItem1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.设置ToolStripMenuItem1.ForeColor = System.Drawing.Color.White;
            this.设置ToolStripMenuItem1.Name = "设置ToolStripMenuItem1";
            this.设置ToolStripMenuItem1.Size = new System.Drawing.Size(44, 21);
            this.设置ToolStripMenuItem1.Text = "设置";
            // 
            // tsmClose
            // 
            this.tsmClose.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsmClose.Image = global::Odin_aoi.Properties.Resources.close_no_square;
            this.tsmClose.Name = "tsmClose";
            this.tsmClose.Size = new System.Drawing.Size(28, 21);
            // 
            // tsmSquare
            // 
            this.tsmSquare.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsmSquare.Image = global::Odin_aoi.Properties.Resources.square;
            this.tsmSquare.Name = "tsmSquare";
            this.tsmSquare.Size = new System.Drawing.Size(28, 21);
            // 
            // tsmMin
            // 
            this.tsmMin.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsmMin.Image = global::Odin_aoi.Properties.Resources.min;
            this.tsmMin.Name = "tsmMin";
            this.tsmMin.Size = new System.Drawing.Size(28, 21);
            // 
            // ToolBarForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(55)))));
            this.ClientSize = new System.Drawing.Size(1140, 32);
            this.CloseButton = false;
            this.CloseButtonVisible = false;
            this.Controls.Add(this.menuStripControl);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)((((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight) 
            | WeifenLuo.WinFormsUI.Docking.DockAreas.DockTop) 
            | WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom)));
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.HideOnClose = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ToolBarForm";
            this.ShowIcon = false;
            this.TabText = "工具箱";
            this.Text = "工具箱";
            this.menuStripControl.ResumeLayout(false);
            this.menuStripControl.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public UserControls.CustomMenuStripControl menuStripControl;
        public System.Windows.Forms.ToolStripMenuItem 设置ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem tsmClose;
        private System.Windows.Forms.ToolStripMenuItem tsmSquare;
        private System.Windows.Forms.ToolStripMenuItem tsmMin;
    }
}