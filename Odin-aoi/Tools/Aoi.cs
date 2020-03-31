using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace power_aoi.Tools
{
    class Aoi
    {
        public enum side { none = 0, left = 1, up = 2, right = 4, down = 8 };
        // 图像匹配算子
        // img: 待搜索的图像
        // templ: 目标模板
        // pos: 匹配结果（输出），目标模板templ在img内的区域的左上角顶点位置
        // binarize: 是否在预处理中加二值化操作，默认不进行(false)
        // method: 匹配度指标
        // return: 匹配结果得分
        //     method使用默认时，值不大于1；得分越高说明匹配程度越高；反之，接近0可认为基本不匹配
        //     method=1时，值不小于0；得分越低说明匹配程度越高
        [DllImport("aoi-v2-release.dll", EntryPoint = "marker_match_crop", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern double marker_match_crop(IntPtr iplImage, IntPtr patch, ref Point point, ref Rectangle rectangle, bool binarize=false, int method=1, bool debug=false);

        [DllImport("aoi-v2-release.dll", EntryPoint = "marker_match", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern double marker_match(IntPtr iplImage, IntPtr patch, ref Point point);
        [DllImport("aoi-v2-release.dll", EntryPoint = "stitch_v2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void stitchv2(IntPtr img, Rectangle rectangle, IntPtr patch, ref Rectangle roi_patch, int side1, int overlap_lb1, int overlap_ub1, int drift_ub1, int side2 = 0, int overlap_lb2 = 0, int overlap_ub2 = 0, int drift_ub2 = 0);
        [DllImport("aoi-v2-release.dll", EntryPoint = "copy_to", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int copy_to(IntPtr iplImage, IntPtr patch, Rectangle rectangle);

        /// <summary>
        /// 拼图
        /// </summary>
        /// <param name="dst">原始图片</param>
        /// <param name="bitmap">小图，照片拍的图</param>
        /// <param name="n_rows">总的行数</param>
        /// <param name="n_cols">总的列数</param>
        /// <param name="row">当前行</param>
        /// <param name="col">当前列</param>
        public static void ImageStitch(ref Mat dst, Bitmap bitmap, ref Rectangle roi0, ref Rectangle roi, int n_rows, int n_cols, bool isFirstRow, bool isFirstCol)
        {
            Emgu.CV.Image<Bgr, Byte> currentFrame = new Emgu.CV.Image<Bgr, Byte>(bitmap);
            Mat img = new Mat();
            CvInvoke.BitwiseAnd(currentFrame, currentFrame, img);
            double or_hl = 0.23; // lower bound for horizontal overlap ratio
            double or_hu = 0.24; // upper
            double or_vl = 0.065; // vertical
            double or_vu = 0.08;
            double dr_hu = 0.01; // upper bound for horizontal drift ratio
            double dr_vu = 0.01; //


            if (isFirstRow)
            {
                if (isFirstCol)
                {
                    roi0 = new Rectangle(Convert.ToInt32(img.Cols * (n_cols - 1) * dr_hu), Convert.ToInt32(img.Rows * (n_rows - 1) * dr_vu), img.Cols, img.Rows);
                    dst = new Mat(Convert.ToInt32(img.Rows * (n_rows + (n_rows - 1) * (dr_vu * 2 - or_vl))), Convert.ToInt32(img.Cols * (n_cols + (n_cols - 1) * (dr_hu * 2 - or_hl))), img.Depth, 3); // 第一张图不要0,0 最好留一些像素
                    roi = roi0;
                }
                else
                {
                    stitchv2(dst.Ptr, roi, img.Ptr, ref roi, (int)side.left, Convert.ToInt32(img.Cols * or_hl), Convert.ToInt32(img.Cols * or_hu), Convert.ToInt32(img.Rows * dr_vu));
                }
                copy_to(dst.Ptr, img.Ptr, roi);
            }
            else
            {
                if (isFirstCol)
                {
                    stitchv2(dst.Ptr, roi0, img.Ptr, ref roi0, (int)side.up, Convert.ToInt32(img.Cols * or_vl), Convert.ToInt32(img.Cols * or_vu), Convert.ToInt32(img.Rows * dr_hu));
                    roi = roi0;
                }
                else
                {
                    stitchv2(dst.Ptr, roi, img.Ptr, ref roi, (int)side.left, Convert.ToInt32(img.Cols * or_hl), Convert.ToInt32(img.Cols * or_hu), Convert.ToInt32(img.Rows * dr_vu), (int)side.up, Convert.ToInt32(img.Rows * or_vl), Convert.ToInt32(img.Rows * or_vu), Convert.ToInt32(img.Cols * dr_hu));
                }
                copy_to(dst.Ptr, img.Ptr, roi);
            }
            dst.Save(@"C:\Users\Administrator\Desktop\suomi-test-img\dst"+ roi.X+ ".jpg");
        }
    }
}
