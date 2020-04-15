using Amib.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace power_aoi.Tools
{
    public class MySmartThreadPool
    {
        static SmartThreadPool Pool = new SmartThreadPool() { MaxThreads = INIHelper.ReadInteger("BaseConfig", "BaseMaxThreads", 0, Application.StartupPath + "/config.ini") };
        static SmartThreadPool LimitPool = new SmartThreadPool() { MaxThreads = INIHelper.ReadInteger("BaseConfig", "AiMaxThreads", 0, Application.StartupPath + "/config.ini") };
        static SmartThreadPool TestPool = new SmartThreadPool() { MaxThreads = 2 };
        static SmartThreadPool LoadModelPool = new SmartThreadPool() { MaxThreads = 1 };
        public static SmartThreadPool Instance()
        {
            return Pool;
        }

        public static SmartThreadPool InstanceSmall()
        {
            return LimitPool;
        }
        public static SmartThreadPool InstanceTest()
        {
            return TestPool;
        }
        public static SmartThreadPool InstanceLoadModel()
        {
            return LoadModelPool;
        }
    }
}
