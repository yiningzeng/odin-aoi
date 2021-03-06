namespace power_aoi
{
    partial class Odin
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Odin));
            this.panelToolBar = new System.Windows.Forms.Panel();
            this.imageListToolBar = new System.Windows.Forms.ImageList(this.components);
            this.dockPanel1 = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.vS2015DarkTheme1 = new WeifenLuo.WinFormsUI.Docking.VS2015DarkTheme();
            this.visualStudioToolStripExtender1 = new WeifenLuo.WinFormsUI.Docking.VisualStudioToolStripExtender(this.components);
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.menuStripControl = new power_aoi.UserControls.CustomMenuStripControl();
            this.tsmLogo = new System.Windows.Forms.ToolStripMenuItem();
            this.设置ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.初始化窗体布局ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.保存窗体布局ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmClose = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmSquare = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmMin = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelToolBar
            // 
            this.panelToolBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.panelToolBar.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelToolBar.Location = new System.Drawing.Point(0, 25);
            this.panelToolBar.Name = "panelToolBar";
            this.panelToolBar.Size = new System.Drawing.Size(44, 747);
            this.panelToolBar.TabIndex = 8;
            // 
            // imageListToolBar
            // 
            this.imageListToolBar.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListToolBar.ImageStream")));
            this.imageListToolBar.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListToolBar.Images.SetKeyName(0, "采集");
            this.imageListToolBar.Images.SetKeyName(1, "编程");
            this.imageListToolBar.Images.SetKeyName(2, "手动出板");
            this.imageListToolBar.Images.SetKeyName(3, "手动松开");
            this.imageListToolBar.Images.SetKeyName(4, "设备复位停止工作");
            this.imageListToolBar.Images.SetKeyName(5, "开发测试按钮(debug)");
            this.imageListToolBar.Images.SetKeyName(6, "模拟一块板子(debug)");
            // 
            // dockPanel1
            // 
            this.dockPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.dockPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dockPanel1.DockBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.dockPanel1.Location = new System.Drawing.Point(44, 25);
            this.dockPanel1.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.dockPanel1.Name = "dockPanel1";
            this.dockPanel1.Padding = new System.Windows.Forms.Padding(6);
            this.dockPanel1.ShowAutoHideContentOnHover = false;
            this.dockPanel1.Size = new System.Drawing.Size(1457, 747);
            this.dockPanel1.TabIndex = 10;
            this.dockPanel1.Theme = this.vS2015DarkTheme1;
            // 
            // visualStudioToolStripExtender1
            // 
            this.visualStudioToolStripExtender1.DefaultRenderer = null;
            // 
            // menuStripControl
            // 
            this.menuStripControl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.menuStripControl.EndColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.menuStripControl.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmLogo,
            this.设置ToolStripMenuItem1,
            this.tsmClose,
            this.tsmSquare,
            this.tsmMin});
            this.menuStripControl.Location = new System.Drawing.Point(0, 0);
            this.menuStripControl.Name = "menuStripControl";
            this.menuStripControl.Size = new System.Drawing.Size(1501, 25);
            this.menuStripControl.StartColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.menuStripControl.TabIndex = 6;
            this.menuStripControl.Text = " ";
            // 
            // tsmLogo
            // 
            this.tsmLogo.Image = global::Odin_aoi.Properties.Resources.logo;
            this.tsmLogo.Name = "tsmLogo";
            this.tsmLogo.Size = new System.Drawing.Size(40, 21);
            this.tsmLogo.Text = " ";
            // 
            // 设置ToolStripMenuItem1
            // 
            this.设置ToolStripMenuItem1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.设置ToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.初始化窗体布局ToolStripMenuItem,
            this.保存窗体布局ToolStripMenuItem});
            this.设置ToolStripMenuItem1.ForeColor = System.Drawing.Color.White;
            this.设置ToolStripMenuItem1.Name = "设置ToolStripMenuItem1";
            this.设置ToolStripMenuItem1.Size = new System.Drawing.Size(44, 21);
            this.设置ToolStripMenuItem1.Text = "设置";
            // 
            // 初始化窗体布局ToolStripMenuItem
            // 
            this.初始化窗体布局ToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.初始化窗体布局ToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.初始化窗体布局ToolStripMenuItem.Name = "初始化窗体布局ToolStripMenuItem";
            this.初始化窗体布局ToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.初始化窗体布局ToolStripMenuItem.Text = "初始化窗体布局";
            this.初始化窗体布局ToolStripMenuItem.Click += new System.EventHandler(this.IniFormToolStripMenuItem_Click);
            // 
            // 保存窗体布局ToolStripMenuItem
            // 
            this.保存窗体布局ToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.保存窗体布局ToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.保存窗体布局ToolStripMenuItem.Name = "保存窗体布局ToolStripMenuItem";
            this.保存窗体布局ToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.保存窗体布局ToolStripMenuItem.Text = "保存窗体布局";
            this.保存窗体布局ToolStripMenuItem.Click += new System.EventHandler(this.SaveFormToolStripMenuItem_Click);
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
            // Odin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1501, 772);
            this.Controls.Add(this.dockPanel1);
            this.Controls.Add(this.panelToolBar);
            this.Controls.Add(this.menuStripControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.IsMdiContainer = true;
            this.Name = "Odin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Odin";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Odin_Load);
            this.menuStripControl.ResumeLayout(false);
            this.menuStripControl.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Panel panelToolBar;
        public System.Windows.Forms.ToolStripMenuItem 设置ToolStripMenuItem1;
        public System.Windows.Forms.ToolStripMenuItem 初始化窗体布局ToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem 保存窗体布局ToolStripMenuItem;
        public UserControls.CustomMenuStripControl menuStripControl;
        public System.Windows.Forms.ImageList imageListToolBar;
        public WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel1;
        public WeifenLuo.WinFormsUI.Docking.VS2015DarkTheme vS2015DarkTheme1;
        public WeifenLuo.WinFormsUI.Docking.VisualStudioToolStripExtender visualStudioToolStripExtender1;
        private System.Windows.Forms.ToolStripMenuItem tsmLogo;
        private System.Windows.Forms.ToolStripMenuItem tsmClose;
        private System.Windows.Forms.ToolStripMenuItem tsmSquare;
        private System.Windows.Forms.ToolStripMenuItem tsmMin;
        private System.Windows.Forms.ToolTip toolTip;
    }
}