using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace power_aoi.UserControls
{
    public partial class CustomMenuStripControl : MenuStrip
    {
        private Color _startColor = Color.White;//选中的渐变开始颜色
        private Color _endCoolor = Color.Blue;//选中的渐变颜色结束值
        private CustomProfessionalRenderer render;
        public CustomMenuStripControl()
        {
            InitializeComponent();
            render = new CustomProfessionalRenderer();
            this.Renderer = render;
        }

        [Category("wyl")]
        [Description("选中渐变开始的颜色")]
        public Color StartColor
        {
            get { return _startColor; }
            set
            {
                _startColor = value;
                render.StartColor = value;
                base.Invalidate();
            }
        }

        [Category("wyl")]
        [Description("选中渐变结束的颜色")]
        public Color EndColor
        {
            get { return _endCoolor; }
            set
            {
                _endCoolor = value;
                render.EndColor = value;
                base.Invalidate();
            }
        }
    }
}
