using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin_aoi.Model.PowerAiRegions
{
    public class PAsset
    {
        public class Size
        {
            public int width { get; set; }
            public int height { get; set; }
        }

        public string format { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string path { get; set; }
        public Size size { get; set; }
        public int state { get; set; }
        public int type { get; set; }
    }
}
