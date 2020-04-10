using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace power_aoi.Model
{
    [Serializable]
    [Table(name: "markers")]
    public class Marker
    {
        [Description("主键")]
        [Key]
        [StringLength(50)]
        [Column(name: "id", TypeName = "varchar")]
        public string Id { get; set; }

        [Column(name: "pcb_id", TypeName = "varchar")]
        [StringLength(50)]
        [Description("对应的pcb板id")]
        public string PcbId { get; set; }

        [Column(name: "marker_point_x", TypeName = "float")]
        [Description("对应的单个marker点的左上角x")]
        public float MarkerPointX { get; set; }

        [Column(name: "marker_point_y", TypeName = "float")]
        [Description("对应的单个marker点的左上角y")]
        public float MarkerPointY { get; set; }

        [Column(name: "create_time")]
        [Description("创建时间")]
        public DateTime CreateTime { get; set; }

    }
}
