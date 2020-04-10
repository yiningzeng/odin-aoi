using Emgu.CV;
using MetroFramework.Forms;
using power_aoi.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace power_aoi
{
    public partial class Login : Form
    {

        public Login()
        {
            InitializeComponent();
            this.MouseDown += Move_MouseDown;
            pTitle.MouseDown += Move_MouseDown;
            #region e
            //Mat img = new Mat(@"C:\Users\Administrator\Desktop\suomi-test-img\762-Ng\F0.jpg", Emgu.CV.CvEnum.LoadImageType.AnyColor);
            //this.Icon = Properties.Resources.aa;
            //MySmartThreadPool.Instance().QueueWorkItem((str, lim) => {
            //    try
            //    {
            //        string disk = str.Split(':')[0];
            //        long freeGb = Utils.GetHardDiskFreeSpace(disk);
            //        if (freeGb < lim)
            //        {
            //            MessageBox.Show(disk + "盘空间已经不足" + lim + "GB，请及时清理", "报警", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        LogHelper.WriteLog("磁盘空间检测", ex);
            //    }
            //}, ConfigurationManager.AppSettings["FtpPath"], Convert.ToInt32(ConfigurationManager.AppSettings["DiskRemind"]));
            //INIHelper.Write("AIConfig", "333", "0.05", Application.StartupPath + "/config.ini");
            //INIHelper.Write("AIConfig", "aokeng", "0.05", Application.StartupPath + "/config.ini");
            //INIHelper.Write("AIConfig", "huashang", "0.05", Application.StartupPath + "/config.ini");
            //INIHelper.Write("AIConfig", "jiban-duanlie", "0.05", Application.StartupPath + "/config.ini");
            //INIHelper.Write("AIConfig", "jieliu", "0.05", Application.StartupPath + "/config.ini");
            //INIHelper.Write("AIConfig", "luotong", "0.05", Application.StartupPath + "/config.ini");
            //INIHelper.Write("AIConfig", "quesun", "0.05", Application.StartupPath + "/config.ini");
            //INIHelper.Write("AIConfig", "xigao-wuran", "0.05", Application.StartupPath + "/config.ini");
            //INIHelper.Write("AIConfig", "xizhu", "0.05", Application.StartupPath + "/config.ini");
            //INIHelper.Write("AIConfig", "yanghua", "0.05", Application.StartupPath + "/config.ini");
            //INIHelper.Write("AIConfig", "yashang", "0.05", Application.StartupPath + "/config.ini");
            //INIHelper.Write("AIConfig", "yiwu", "0.05", Application.StartupPath + "/config.ini");
            //INIHelper.Write("AIConfig", "zhanxi", "0.05", Application.StartupPath + "/config.ini");
            #endregion
        }

        private void Move_MouseDown(object sender, MouseEventArgs e)
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

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // if it is a hotkey, return true; otherwise, return false
            switch (keyData)
            {
                case Keys.Enter:
                    btnLogin.Focus();
                    btnLogin.PerformClick();
                    return true;
                //......
                default:
                    break;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            lbResult.Visible = true;
            lbResult.Text = "登录中......";
            MySmartThreadPool.Instance().QueueWorkItem((username, password)=> {
                AoiModel aoiModel = DB.GetAoiModel();
                try
                {
                    string md5Pass = Utils.GenerateMD5(password);
                    User user = aoiModel.users.Where(u => u.Username == username && u.Password == md5Pass).FirstOrDefault();
                    if (user != null)
                    {
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        this.BeginInvoke((Action)(() =>
                        {
                            lbResult.Text = "用户名或密码错误";
                            lbResult.Visible = true;
                        }));
                    }
                }
                catch (Exception err)
                {
                    try
                    {
                        this.BeginInvoke((Action)(() =>
                        {
                            lbResult.Text = "连接数据库出错";
                            lbResult.Visible = true;
                        }));
                        //LogHelper.WriteLog("Login error", err);
                    }
                    catch(Exception er)
                    {
                        //LogHelper.WriteLog("Login error", err);
                    }
                }
                finally
                {
                    aoiModel.Dispose();
                }
            }, tbUsername.Text.Trim(), tbPassword.Text.Trim());
            
        }

        private void pbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
