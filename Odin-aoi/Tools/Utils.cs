using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using power_aoi.Tools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace power_aoi
{
    public class Utils
    {
        public static Rectangle RectangleF2Rectangle(RectangleF rectangleF)
        {
            return new Rectangle(Convert.ToInt32(rectangleF.X),
                Convert.ToInt32(rectangleF.Y),
                Convert.ToInt32(rectangleF.Width),
                Convert.ToInt32(rectangleF.Height));
        }

        public static RectangleF Rectangle2RectangleF(Rectangle rectangle)
        {
            return new RectangleF(rectangle.X,
                rectangle.Y,
                rectangle.Width,
                rectangle.Height);
        }
        /// <summary>
        /// Json格式化输出
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ConvertJsonString(string str)
        {
            //格式化json字符串
            JsonSerializer serializer = new JsonSerializer();
            TextReader tr = new StringReader(str);
            JsonTextReader jtr = new JsonTextReader(tr);
            object obj = serializer.Deserialize(jtr);
            if (obj != null)
            {
                StringWriter textWriter = new StringWriter();
                JsonTextWriter jsonWriter = new JsonTextWriter(textWriter)
                {
                    Formatting = Formatting.Indented,
                    Indentation = 4,
                    IndentChar = ' '
                };
                serializer.Serialize(jsonWriter, obj);
                return textWriter.ToString();
            }
            else
            {
                return str;
            }
        }

        /// <summary>
        /// double比较大小
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public static bool DoubleEquals(double value1, double value2)
        {
            //双精度误差
            var DOUBLE_DELTA = 1E-6;
            return value1 == value2 || Math.Abs(value1 - value2) < DOUBLE_DELTA;
        }

        //bitmap转byte
        public static byte[] Bitmap2Byte(Bitmap bitmap)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Jpeg);
                byte[] data = new byte[stream.Length];
                stream.Seek(0, SeekOrigin.Begin);
                stream.Read(data, 0, Convert.ToInt32(stream.Length));
                return data;
            }
        }
        /// 将 Stream 转成 byte[]
        public static byte[] StreamToBytes(MemoryStream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            // 设置当前流的位置为流的开始
            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
        }
        /// <summary>
        /// 处理Double值，精确到小数点后几位
        /// </summary>
        /// <param name="_value">值</param>
        /// <param name="Length">精确到小数点后几位</param>
        /// <returns>返回值</returns>
        public static double ManagerDoubleValue(double _value, int Length)
        {
            if (Length < 0)
            {
                Length = 0;
            }
            return System.Math.Round(_value, Length);
        }

        /// <summary>
        /// 获取指定驱动器的剩余空间总大小(单位为B)
        /// </summary>
        /// <param name="str_HardDiskName">只需输入代表驱动器的字母即可 </param>
        /// <returns> </returns>
        public static long GetHardDiskFreeSpace(string str_HardDiskName)
        {
            long freeSpace = new long();
            str_HardDiskName = str_HardDiskName + ":\\";
            System.IO.DriveInfo[] drives = System.IO.DriveInfo.GetDrives();
            foreach (System.IO.DriveInfo drive in drives)
            {
                if (drive.Name == str_HardDiskName)
                {
                    freeSpace = drive.TotalFreeSpace / 1073741824;
                }
            }
            return freeSpace;
        }

        /// <summary>
        /// MD5字符串加密
        /// </summary>
        /// <param name="txt"></param>
        /// <returns>加密后字符串</returns>
        public static string GenerateMD5(string txt)
        {
            using (MD5 mi = MD5.Create())
            {
                byte[] buffer = Encoding.Default.GetBytes(txt);
                //开始加密
                byte[] newBuffer = mi.ComputeHash(buffer);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < newBuffer.Length; i++)
                {
                    sb.Append(newBuffer[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }

        /// <summary>
        /// 缺陷画框行数
        /// </summary>
        /// <param name="bitmap">原始bitmap</param>
        /// <param name="rect">Rectangle</param>
        /// <param name="ngType">缺陷类型</param>
        /// <returns></returns>
        public static Bitmap DrawRect(Bitmap bitmap, Rectangle rect, string ngType)
        {
            Graphics ghFront = Graphics.FromImage(bitmap);
            ghFront.DrawString(ngType, new Font("宋体", 10, FontStyle.Bold), Brushes.Red, rect.X, rect.Y - 15);
            ghFront.DrawRectangle(
                new Pen(Color.Red, 3),
                rect);
            return bitmap;
        }

        /// <summary>
        /// 缺陷画框行数
        /// </summary>
        /// <param name="bitmap">原始bitmap</param>
        /// <param name="rect">Rectangle</param>
        /// <param name="ngType">缺陷类型</param>
        /// <returns></returns>
        public static Bitmap DrawLine(Bitmap aa, Rectangle rect)
        {
            Bitmap bitmap = aa.Clone(new Rectangle(0, 0, aa.Width, aa.Height), aa.PixelFormat);
            Graphics ghFront = Graphics.FromImage(bitmap);
            Pen newPen = new Pen(Color.Yellow, 50);//定义一个画笔，黄色
            ghFront.DrawLine(newPen, new Point(rect.X, 0), new Point(rect.X, bitmap.Height));
            ghFront.DrawLine(newPen, new Point(0, rect.Y), new Point(bitmap.Width, rect.Y));
            return bitmap;
        }
        /// <summary>  
        /// 剪裁 -- 用GDI+   
        /// </summary>  
        /// <param name="b">原始Bitmap</param>  
        /// <returns>剪裁后的Bitmap</returns>  
        public static Bitmap BitmapCut(Bitmap b, Rectangle rect)
        {
            int StartX = rect.X, StartY = rect.Y, iWidth = rect.Width, iHeight = rect.Height;
            if (b == null)
            {
                return null;
            }
            int w = b.Width;
            int h = b.Height;
            if (StartX >= w || StartY >= h)
            {
                return null;
            }
            if (StartX + iWidth > w)
            {
                iWidth = w - StartX;
            }
            if (StartY + iHeight > h)
            {
                iHeight = h - StartY;
            }
            try
            {
                Bitmap bmpOut = new Bitmap(iWidth, iHeight, PixelFormat.Format24bppRgb);
                Graphics g = Graphics.FromImage(bmpOut);
                g.DrawImage(b, new Rectangle(0, 0, iWidth, iHeight), new Rectangle(StartX, StartY, iWidth, iHeight), GraphicsUnit.Pixel);
                g.Dispose();
                return bmpOut;
            }
            catch
            {
                return null;
            }
        }
    }
}
