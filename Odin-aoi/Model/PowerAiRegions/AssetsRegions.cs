using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin_aoi.Model.PowerAiRegions
{
    public class AssetsRegions
    {
        public PAsset asset { get; set; }
        public List<PRegion> regions { get; set; }
        public string version { get; set; }
    }
}
