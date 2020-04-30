using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin_aoi.Model
{
    public class ChildrenPcbMarkerInfo
    {
        [DisplayName("名称"), CategoryAttribute("基本属性"), DescriptionAttribute("名称")]
        public string Name { get; set; }

        [DisplayName("是否是反面"), CategoryAttribute("基本属性"), DescriptionAttribute("是否是反面的"), ReadOnly(true)]
        public bool IsBack { get; set; } = false;

        [DisplayName("区域"), CategoryAttribute("数据"), DescriptionAttribute("对应的Marker点的区域")]
        public Rectangle MarkerRect { get; set; }

        [DisplayName("子板区域"), CategoryAttribute("数据"), DescriptionAttribute("Marker点对应的板子的区域")]
        public Rectangle ChildrenPcbRect { get; set; }
    }

    public class PcbAlgorithmsInfo
    {
        public Dictionary<string, ChildrenPcbMarkerInfo> childrenPcbMarkerInfos { get; set; } = new Dictionary<string, ChildrenPcbMarkerInfo>(); //new List<ChildrenPcbMarkerInfo>();
    }
}
