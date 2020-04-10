using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace power_aoi
{
    [Table(name:"users")]
    public class User
    {
        [Description("主键")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Column(name: "group_id")]
        [Description("组id")]
        public int GroupId { get; set; }

        [Column(name: "username")]
        [Description("用户名")]
        public string Username { get; set; }

        [Column(name: "password")]
        [Description("密码")]
        public string Password { get; set; }

        [Column(name: "type")]
        [Description("类型，0禁止登录 1可以登录")]
        public int Type { get; set; }

        [Column(name: "create_time")]
        [Description("创建时间")]
        public DateTime CreateTime { get; set; }
    }
}
