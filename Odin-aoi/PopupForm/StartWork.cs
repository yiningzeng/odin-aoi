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
using power_aoi.Model;
using power_aoi.Tools;
using System.IO;
using power_aoi.Tools.Hardware;

namespace power_aoi.PopupForm
{
    public partial class StartWork : Form
    {
        public StartWork()
        {
            InitializeComponent();
            Ini();
        }

        private void Ini()
        {
            this.pMain.MouseDown += StartWork_MouseDown;
            this.pTitle.MouseDown += StartWork_MouseDown;
            this.pbClose.Click += PbClose_Click;
            tbCarrierLength.KeyPress += tb_KeyPress;
            tbCarrierWidth.KeyPress += tb_KeyPress;
            tbPcbLength.KeyPress += tb_KeyPress;
            tbPcbWidth.KeyPress += tb_KeyPress;
            btnStart.Click += BtnStart_Click;


            comBoxType.SelectedIndex = 0;
            tbCarrierLength.Text = "220";
            tbCarrierWidth.Text = "90";
            tbPcbLength.Text = "190";
            tbPcbWidth.Text = "50";
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            int carrierLength = int.Parse(tbCarrierLength.Text.Trim());
            int carrierWidth = int.Parse(tbCarrierWidth.Text.Trim());
            int pcbLength = int.Parse(tbPcbLength.Text.Trim());
            int pcbWidth = int.Parse(tbPcbWidth.Text.Trim());
            this.Tag = Pcb.CreatePcb(carrierLength, carrierWidth, pcbLength, pcbWidth, comBoxType.SelectedIndex);
            if (this.Tag == null) return;
            this.DialogResult = DialogResult.Yes;
            this.Close();
        }

        private void tb_KeyPress(object sender, KeyPressEventArgs e)
        {
            //如果输入的不是退格和数字，则屏蔽输入
            if (!(e.KeyChar == '\b' || (e.KeyChar >= '0' && e.KeyChar <= '9')))
            {
                e.Handled = true;
            }
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
    }
}
