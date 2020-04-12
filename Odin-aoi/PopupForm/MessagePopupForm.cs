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
            MySmartThreadPool.Instance().QueueWorkItem(() =>
            {
                try
                {
                    GC.Collect();
                    GC.Collect();
                    AiSdkFront.dispose();
                    AiSdkBack.dispose();

                    AiSdkFront.init(Marshal.StringToHGlobalAnsi(INIHelper.Read("FrontAiPars", "configurationFile", Application.StartupPath + "/config.ini").Replace("\\\\", "\\").Replace("\\", "/")),
                             // 特别说明，这里的路径一定要用 '/' 不能用反斜杠
                             // 特别说明，这里的路径一定要用 '/' 不能用反斜杠
                             // 特别说明，这里的路径一定要用 '/' 不能用反斜杠 
                             Marshal.StringToHGlobalAnsi(INIHelper.Read("FrontAiPars", "weightsFile", Application.StartupPath + "/config.ini").Replace("\\\\", "\\").Replace("\\", "/")),
                             INIHelper.ReadInteger("FrontAiPars", "gpuID", 0, Application.StartupPath + "/config.ini"));
                    AiSdkFront.names = new List<string>();
                    AiSdkBack.names = new List<string>();
                    using (StreamReader sr = new StreamReader(INIHelper.Read("FrontAiPars", "names", Application.StartupPath + "/config.ini")))
                    {
                        while (!sr.EndOfStream)
                        {
                            string str = sr.ReadLine();
                            AiSdkFront.names.Add(str);
                        }
                    }
                    using (StreamReader sr = new StreamReader(INIHelper.Read("BackAiPars", "names", Application.StartupPath + "/config.ini")))
                    {
                        while (!sr.EndOfStream)
                        {
                            string str = sr.ReadLine();
                            AiSdkBack.names.Add(str);
                        }
                    }
                    GC.Collect();
                    Thread.Sleep(1000);

                    AiSdkBack.init(Marshal.StringToHGlobalAnsi(INIHelper.Read("BackAiPars", "configurationFile", Application.StartupPath + "/config.ini").Replace("\\\\", "\\").Replace("\\", "/")),
                             // 特别说明，这里的路径一定要用 '/' 不能用反斜杠
                             // 特别说明，这里的路径一定要用 '/' 不能用反斜杠
                             // 特别说明，这里的路径一定要用 '/' 不能用反斜杠
                             Marshal.StringToHGlobalAnsi(INIHelper.Read("BackAiPars", "weightsFile", Application.StartupPath + "/config.ini").Replace("\\\\", "\\").Replace("\\", "/")),
                             INIHelper.ReadInteger("BackAiPars", "gpuID", 0, Application.StartupPath + "/config.ini"));
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
