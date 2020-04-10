using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using power_aoi.Tools;
using System.IO;
using System.Threading;

namespace power_aoi.PopupForm
{
    public partial class PlcErrorMessagePopupForm : Form
    {
        public PlcErrorMessagePopupForm()
        {
            InitializeComponent();
           
            this.pMain.MouseDown += StartWork_MouseDown;
            this.pTitle.MouseDown += StartWork_MouseDown;
            this.pbClose.Click += PbClose_Click;
        }

        public PlcErrorMessagePopupForm(string title)
        {
            InitializeComponent();
            this.lbTitle.Text = title;
            this.pMain.MouseDown += StartWork_MouseDown;
            this.pTitle.MouseDown += StartWork_MouseDown;
            this.pbClose.Click += PbClose_Click;
        }

        public void showTip(string str)
        {
            this.label1.Text = str;
        }

        private void PbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void StartWork_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, 0xA1, 0x02, 0);
            }
        }

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        private void MessagePopupForm_Load(object sender, EventArgs e)
        {
            
        }
    }
}
