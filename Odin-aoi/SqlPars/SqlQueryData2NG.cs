using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace power_aoi.SqlPars
{
    // 加载datagridview必须要设置类和所有成员为public，否则EntityFramework查询出的无法绑定datagridview
    public class SqlQueryData2NG
    {
        public string Type { get; set; }
        public int Nums { get; set; }
    }
}
