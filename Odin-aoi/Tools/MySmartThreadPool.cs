using Amib.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace power_aoi.Tools
{
    public class MySmartThreadPool
    {
        static SmartThreadPool Pool = new SmartThreadPool() { MaxThreads = 25};
        public static SmartThreadPool Instance()
        {
            return Pool;
        }
    }
}
