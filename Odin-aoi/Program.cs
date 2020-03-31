
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

            Login login = new Login();
            DialogResult dialogResult = login.ShowDialog();
            login.Close();
            if (dialogResult == DialogResult.OK)
            {
                Application.Run(new Odin());
                //Application.Run(new Main());
            }
        }
    }
}
