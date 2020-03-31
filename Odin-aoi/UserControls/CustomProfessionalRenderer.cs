using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace power_aoi.UserControls
{
    /// <summary>
    /// 自定义MenuStrip 控件菜单样式类
    /// </summary>
    public class CustomProfessionalRenderer : ToolStripProfessionalRenderer
    {
        private Color _startColor = Color.White;//选中的渐变开始颜色
        private Color _endCoolor = Color.Blue;//选中的渐变颜色结束值
        public CustomProfessionalRenderer()
            : base()
        {
        }
        ////public CustomProfessionalRenderer(Color startColor, Color endColor)
        ////    : base()
        ////{
        ////    _startColor = startColor;
        ////    _endCoolor = endColor;
        ////}

        public Color StartColor
        {
            get { return _startColor; }
            set
            {
                _startColor = value;
            }
        }

        public Color EndColor
        {
            get { return _endCoolor; }
            set
            {
                _endCoolor = value;
            }
        }

        /// <summary>
        /// 获取圆角矩形区域  radius=直径
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public static GraphicsPath GetRoundedRectPath(Rectangle rect, int radius)
        {
            int diameter = radius;
            Rectangle arcRect = new Rectangle(rect.Location, new Size(diameter, diameter));
            GraphicsPath path = new GraphicsPath();

            // 左上角
            path.AddArc(arcRect, 180, 90);

            // 右上角
            arcRect.X = rect.Right - diameter;
            path.AddArc(arcRect, 270, 90);

            // 右下角
            arcRect.Y = rect.Bottom - diameter;
            path.AddArc(arcRect, 0, 90);

            // 左下角
            arcRect.X = rect.Left;
            path.AddArc(arcRect, 90, 90);
            path.CloseFigure();
            return path;
        }

        /// <summary>
        /// 渲染背景 包括menustrip背景 toolstripDropDown背景
        /// </summary>
        /// <param name="e"></param>
        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {
            ToolStrip toolStrip = e.ToolStrip;
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;//抗锯齿
            Rectangle bounds = e.AffectedBounds;
            LinearGradientBrush lgbrush = new LinearGradientBrush(new Point(0, 0), new Point(0, toolStrip.Height), Color.FromArgb(255, _startColor), Color.FromArgb(200, _endCoolor));
            if (toolStrip is MenuStrip)
            {
                //由menuStrip的Paint方法定义 这里不做操作

            }
            else if (toolStrip is ToolStripDropDown)
            {
                int diameter = 1;//直径
                GraphicsPath path = new GraphicsPath();
                Rectangle rect = new Rectangle(Point.Empty, toolStrip.Size);
                Rectangle arcRect = new Rectangle(rect.Location, new Size(diameter, diameter));

                path.AddLine(0, 0, 10, 0);
                // 右上角
                arcRect.X = rect.Right - diameter;
                path.AddArc(arcRect, 270, 90);

                // 右下角
                arcRect.Y = rect.Bottom - diameter;
                path.AddArc(arcRect, 0, 90);

                // 左下角
                arcRect.X = rect.Left;
                path.AddArc(arcRect, 90, 90);
                path.CloseFigure();
                toolStrip.Region = new Region(path);
                g.FillPath(lgbrush, path);
            }
            else
            {
                base.OnRenderToolStripBackground(e);
            }
        }

        //渲染边框 不绘制边框
        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
            //不调用基类的方法 屏蔽掉该方法 去掉边框
        }

        //渲染箭头 更改箭头颜色
        protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
        {
            e.ArrowColor = _endCoolor;
            base.OnRenderArrow(e);
        }

        /// <summary>
        /// 渲染项 不调用基类同名方法
        /// </summary>
        /// <param name="e"></param>
        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            Graphics g = e.Graphics;
            ToolStripItem item = e.Item;
            ToolStrip toolstrip = e.ToolStrip;

            //渲染顶级项
            if (toolstrip is MenuStrip)
            {
                LinearGradientBrush lgbrush = new LinearGradientBrush(new Point(0, 0), new Point(0, item.Height), Color.FromArgb(255, _endCoolor), Color.FromArgb(255, _startColor));
                if (e.Item.Selected)//选中
                {
                    GraphicsPath gp = GetRoundedRectPath(new Rectangle(new Point(0, 0), item.Size), 1);
                    g.FillPath(lgbrush, gp);
                }
                if (item.Pressed)//按下
                {
                    //创建上面左右2圆角的矩形路径
                    GraphicsPath path = new GraphicsPath();
                    int diameter = 1;
                    Rectangle rect = new Rectangle(Point.Empty, item.Size);
                    Rectangle arcRect = new Rectangle(rect.Location, new Size(diameter, diameter));
                    // 左上角
                    path.AddArc(arcRect, 180, 90);
                    // 右上角
                    arcRect.X = rect.Right - diameter;
                    path.AddArc(arcRect, 270, 90);
                    path.AddLine(new Point(rect.Width, rect.Height), new Point(0, rect.Height));
                    path.CloseFigure();
                    //填充路径
                    g.FillPath(lgbrush, path);
                    //g.FillRectangle(Brushes.White, new Rectangle(Point.Empty, item.Size));
                }
            }
            else if (toolstrip is ToolStripDropDown)//渲染下拉项
            {
                g.SmoothingMode = SmoothingMode.HighQuality;
                LinearGradientBrush lgbrush = new LinearGradientBrush(new Point(0, 0), new Point(item.Width, 0), Color.FromArgb(255, _startColor), Color.FromArgb(255, _endCoolor));
                if (item.Selected)
                {
                    GraphicsPath gp = GetRoundedRectPath(new Rectangle(0, 0, item.Width, item.Height), 1);
                    g.FillPath(lgbrush, gp);
                }
            }
            else
            {
                base.OnRenderMenuItemBackground(e);
            }
        }

        /// <summary>
        /// 渲染分界线
        /// </summary>
        /// <param name="e"></param>
        protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
        {
            Graphics g = e.Graphics;
            LinearGradientBrush lgbrush = new LinearGradientBrush(new Point(0, 0), new Point(e.Item.Width, 0), _startColor, Color.FromArgb(0, _endCoolor));
            g.FillRectangle(lgbrush, new Rectangle(3, e.Item.Height / 2, e.Item.Width, 1));
            //base.OnRenderSeparator(e);
        }

        /// <summary>
        /// 渲染图片区域 下拉菜单左边的图片区域
        /// </summary>
        /// <param name="e"></param>
        protected override void OnRenderImageMargin(ToolStripRenderEventArgs e)
        {
            // base.OnRenderImageMargin(e);
            //屏蔽掉左边图片竖条
        }

    }
}
