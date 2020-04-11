
using System;
using System.Collections.Generic;

using System.Linq;

using System.Windows.Forms;

namespace power_aoi
{

    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);


            bool Exist;//定义一个bool变量，用来表示是否已经运行
                       //创建Mutex互斥对象
            System.Threading.Mutex newMutex = new System.Threading.Mutex(true, "Odin-aoi", out Exist);
            if (Exist)//如果没有运行
            {
                newMutex.ReleaseMutex();//运行新窗体
                Login login = new Login();
                DialogResult dialogResult = login.ShowDialog();
                login.Close();
                if (dialogResult == DialogResult.OK)
                {
                    Application.Run(new Odin());
                    //Application.Run(new Main());
                }
            }
            else
            {
                MessageBox.Show("本程序一次只能运行一个实例！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);//弹出提示信息
            }
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            LogHelper.WriteLog("AppDomain中遇到未处理异常：" + e.ExceptionObject.ToString());
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            LogHelper.WriteLog("Application中遇到未处理异常：" + e.Exception.Message + "\r\n" + e.Exception.StackTrace);
        }
    }
}
