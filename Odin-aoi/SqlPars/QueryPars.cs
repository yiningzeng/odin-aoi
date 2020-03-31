using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace power_aoi.SqlPars
{
    public class QueryPars
    {
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }

        public string pcbNumber { get; set; }
        /// <summary>
        /// 是否精确查询
        /// </summary>
        public bool searchPcbNumberAccurate { get; set; }

        public string pcbName { get; set; }

        public int nums { get; set; }

        public int pages { get; set; }
    }
}
