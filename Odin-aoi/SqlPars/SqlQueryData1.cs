using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace power_aoi.SqlPars
{
    // 加载datagridview必须要设置类和所有成员为public，否则EntityFramework查询出的无法绑定datagridview
    public class SqlQueryData1
    {
        public int AllPcbNums { get; set; }
        //int PassPcbNums = "(select count(*) from pcbs where is_misjudge = 0 and is_error = 0)";
        public int ErrorPcbNums { get; set; }
        public int MisreportPcbNums { get; set; }
        public float MisreportRate { get; set; }
        public float PassRate { get; set; }
    }
}
