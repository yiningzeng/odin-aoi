using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin_aoi.Model.PowerAiRegions
{
    public class PRegion
    {
        public class BoundingBox
        {
            public float height { get; set; }
            public float width { get; set; }
            public float left { get; set; }
            public float top { get; set; }
        }
        public class PPoint
        {
            public float x { get; set; }
            public float y { get; set; }
        }
        public string id { get; set; }
        public string type { get; set; }
        public List<string> tags { get; set; }
        public BoundingBox boundingBox { get; set; }
        public List<PPoint> points { get; set; }
    }
}
