namespace power_aoi.DockerPanelOdin
{
    partial class BackWorkingForm
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
            this.imgBoxWorking = new Cyotek.Windows.Forms.Demo.ImageBoxEx();
            this.SuspendLayout();
            // 
            // imgBoxWorking
            // 
            this.imgBoxWorking.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imgBoxWorking.DragHandleSize = 0;
            this.imgBoxWorking.Font = new System.Drawing.Font("宋体", 300F);
            this.imgBoxWorking.GridCellSize = 10;
            this.imgBoxWorking.Location = new System.Drawing.Point(0, 0);
            this.imgBoxWorking.Name = "imgBoxWorking";
            this.imgBoxWorking.ScaleText = true;
            this.imgBoxWorking.SelectionMode = Cyotek.Windows.Forms.ImageBoxSelectionMode.Rectangle;
            this.imgBoxWorking.Size = new System.Drawing.Size(620, 510);
            this.imgBoxWorking.TabIndex = 0;
            this.imgBoxWorking.Zoom = 10;
            // 
            // BackWorkingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(55)))));
            this.ClientSize = new System.Drawing.Size(620, 510);
            this.CloseButton = false;
            this.CloseButtonVisible = false;
            this.ControlBox = false;
            this.Controls.Add(this.imgBoxWorking);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)(((((WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft | WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight) 
            | WeifenLuo.WinFormsUI.Docking.DockAreas.DockTop) 
            | WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom) 
            | WeifenLuo.WinFormsUI.Docking.DockAreas.Document)));
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.HelpButton = true;
            this.HideOnClose = true;
            this.Name = "BackWorkingForm";
            this.TabText = "反面";
            this.ResumeLayout(false);

        }

        #endregion

        public Cyotek.Windows.Forms.Demo.ImageBoxEx imgBoxWorking;
    }
}