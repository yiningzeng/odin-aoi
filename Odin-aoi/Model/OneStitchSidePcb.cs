using Emgu.CV;
using power_aoi.Tools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace power_aoi.Model
{
    /// <summary>
    /// 默认都是Front A面
    /// </summary>
    public class OneStitchSidePcb
    {
        public class BitmapInfo
        {
            public string name { get; set; }
            public Bitmap bitmap { get; set; }
        }
        #region AI参数
        public int equalDivision = 1; // #表示检测的图像按照边长等分的数量，=2的话就是4等分
        public int overlap = 50; // #表示等分的时候重叠的区域
        public bool saveCropImg = false; //#是否保存等分的图片
        public bool detectMultiScale = false; // #开启多尺度检测的时候要把equalDivision设置为2
        public float confidence = (float)0.01;
        #endregion

        #region Plc参数
        //拍摄点位X和Y方向间隔，当前都为14mm
        private float capturePointIntervalXInMM = 13.0f;
        private float capturePointIntervalYInMM = 13.0f;
        //单张拍摄图片对应的物理宽度，当前为17mm 不知道干嘛用的暂时放着
        private float singleCaptureWidthInMM = 17.0f;

        public int addressX = 3000; // X什么绝对位置
        public int addressY = 3200; // y绝对位置
        public int addressCaptureNum = 5000; //拍摄数量地址
        public int addressStartCapture = 2144; //开始拍摄地址
        public int addressOneSidePcbOut = 2145; // 发送单面的出板信息
        public int addressEndCapture = 1133; // 发送单面的出板信息

        //拍摄点位XY
        public List<int> x, y;
        #endregion

        #region 其他参数
        public string pcbId;
        public string savePath;
        #endregion

        #region 拼图参数
        public double or_hl = 0.23; // lower bound for horizontal overlap ratio
        public double or_hu = 0.24; // upper
        public double or_vl = 0.065; // vertical
        public double or_vu = 0.08;
        public double dr_hu = 0.01; // upper bound for horizontal drift ratio
        public double dr_vu = 0.01; //
        public int allNum; // 图片总数
        public int allRows; // 总行数
        public int allCols; // 总列数
        public int currentRow = 0; // 拼图当前行
        public int currentCol = 0; // 拼图当前列
        public bool zTrajectory = true; // 默认是Z轨迹，背面的话是S字形轨迹
        public int trajectorySide; //主要用于拼图的时候切换对其边
        public Mat dst = null; // 最终输出大图
        public Rectangle roi = new Rectangle(); // 对齐的参考的区域
        public double scale = 0.25;
        public bool stitchEnd = false; //拼图结束的标示
        //图片队列
        public Queue<BitmapInfo> bitmaps = new Queue<BitmapInfo>();
        #endregion
    }
}
