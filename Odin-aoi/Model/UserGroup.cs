using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace power_aoi.Model
{
    [Table(name: "user_groups")]
    public class UserGroup
    {
        [Description("主键")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Column(name: "group_name")]
        [Description("组名")]
        public string GroupName { get; set; }

        [Column(name: "type")]
        [Description("组类型 0禁止登录 1可以登录")]
        public int type { get; set; }

        [Column(name: "create_time")]
        [Description("创建时间")]
        public DateTime CreateTime { get; set; }
    }
}
