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
    [Table(name: "pcbs")]
    public class Pcb
    {
        [Description("主键")]
        [Key]
        [Column(name:"id",TypeName = "varchar")]
        [StringLength(50)]
        public string Id { get; set; }

        [Column(name: "pcb_number")]
        [StringLength(250)]
        [Description("板号")]
        public string PcbNumber { get; set; }

        [Column(name: "pcb_name")]
        [StringLength(250)]
        [Description("PCB名称")]
        public string PcbName { get; set; }

        [Column(name: "carrier_width")]
        [Description("载板长")]
        public int CarrierLength { get; set; }

        [Column(name: "carrier_height")]
        [Description("载板宽")]
        public int CarrierWidth { get; set; }

        [Column(name: "pcb_width")]
        [Description("PCB名称")]
        public int PcbLength { get; set; }

        [Column(name: "pcb_height")]
        [Description("PCB名称")]
        public int PcbWidth { get; set; }

        [Column(name: "pcb_childen_number")]
        [Description("PCB名称")]
        public int PcbChildenNumber { get; set; }

        [Column(name: "surface_number")]
        [Description("检测的面数")]
        public int SurfaceNumber { get; set; }
        

        [Column(name: "pcb_path")]
        [StringLength(250)]
        [Description("对应的FTP保存的地址")]
        public string PcbPath { get; set; }

        [DefaultValue(0)]
        [Column(name: "is_error")]
        [Description("是否报错, 0否 1是")]
        public int IsError { get; set; }

        [DefaultValue(0)]
        [Column(name: "is_misjudge")]
        [Description("是否误判, 0否 1是, 只要results下有一个误判，那该值就改为1")]
        public int IsMisjudge { get; set; }

        [Column(name: "create_time")]
        [Description("创建时间")]
        public DateTime CreateTime { get; set; }

        public List<Result> results { get; set; }

        [NotMapped]
        public OneStitchSidePcb FrontPcb { get; set; }
        [NotMapped]
        public OneStitchSidePcb BackPcb { get; set; }
    }
}
