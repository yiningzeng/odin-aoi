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
using Odin_aoi.Tools;

namespace power_aoi.PopupForm
{
    public partial class MessagePopupForm : Form
    {
        public MessagePopupForm()
        {
            InitializeComponent();
           
            this.pMain.MouseDown += StartWork_MouseDown;
            this.pTitle.MouseDown += StartWork_MouseDown;
            this.pbClose.Click += PbClose_Click;
        }

        public MessagePopupForm(string title)
        {
            InitializeComponent();
            this.lbTitle.Text = title;
            this.pMain.MouseDown += StartWork_MouseDown;
            this.pTitle.MouseDown += StartWork_MouseDown;
            this.pbClose.Click += PbClose_Click;
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
            MySmartThreadPool.InstanceLoadModel().QueueWorkItem(() =>
            {
                try
                {
                    LoadBalance.Ini(INIHelper.ReadInteger("AiBaseConfig", "LoadModelNum", 0, Application.StartupPath + "/config.ini"));
                    this.DialogResult = DialogResult.OK;
                }
                catch (System.AccessViolationException er)
                {
                    LogHelper.WriteLog("AI初始化失败", er);
                    this.DialogResult = DialogResult.No;
                }
                finally
                {
                    this.BeginInvoke((Action)(() =>
                    {
                        this.Close();
                    }));
                }
            });
        }
    }
}
