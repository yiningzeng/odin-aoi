using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace power_aoi.Tools.Hardware
{
    /// <summary>
    /// 计算运行点位
    /// </summary>
    public class Xycoordinate
    {
        //a面 x 限位
        static int ax = INIHelper.ReadInteger("XYwidth", "ax", -70563, Application.StartupPath + "/config.ini");
        //b面 x 限位
        static int bx = INIHelper.ReadInteger("XYwidth", "bx", -61541, Application.StartupPath + "/config.ini"); 
        //a面 y 限位
        static int ay = INIHelper.ReadInteger("XYwidth", "ay", -81058, Application.StartupPath + "/config.ini");
        //b面 y 限位
        static int by = INIHelper.ReadInteger("XYwidth", "by", -94959, Application.StartupPath + "/config.ini");
        //电机和物理毫米对应关系
        static int motorScale = 250;
        /// <summary>
        /// a 面 x 运行点位
        /// </summary>
        /// <param name="num">运行数量</param>
        /// <param name="xIntervalInMM">x 运行距离</param>
        /// <param name="differencevalue">载板与pcb直接差值</param>
        /// <returns></returns>
        public static List<int> axcoordinate(int num, int xIntervalInMM, int differencevalue)
        {
            List<int> xcoordinatelist = new List<int>();
            for (int i = 0; i < num; i++)
            {
                xcoordinatelist.Add(ax + differencevalue * motorScale + xIntervalInMM * i * motorScale);
            }

            return xcoordinatelist;
        }
        /// <summary>
        /// b 面 x 运行点位
        /// </summary>
        /// <param name="num">运行数量</param>
        /// <param name="xIntervalInMM">x 运行距离</param>
        /// <param name="differencevalue">载板与pcb直接差值</param>
        /// <returns></returns>
        public static List<int> bxcoordinate(int num, int xIntervalInMM, int differencevalue)
        {
            List<int> xcoordinatelist = new List<int>();
            for (int i = 0; i < num; i++)
            {
                xcoordinatelist.Add(bx + differencevalue * motorScale + xIntervalInMM * i * motorScale);
            }

            return xcoordinatelist;
        }
        /// <summary>
        /// a 面 y 运行点位
        /// </summary>
        /// <param name="num">运行数量</param>
        /// <param name="yIntervalInMM">y 运行距离</param>
        /// <param name="differencevalue">载板与pcb直接差值</param>
        /// <returns></returns>
        public static List<int> aycoordinate(int num, int yIntervalInMM, int differencevalue)
        {
            List<int> ycoordinatelist = new List<int>();
            for (int i = 0; i < num; i++)
            {

                ycoordinatelist.Add(ay + differencevalue * motorScale + yIntervalInMM * i * motorScale);

            }

            return ycoordinatelist;
        }
        /// <summary>
        /// b 面 y 运行点位
        /// </summary>
        /// <param name="num">运行数量</param>
        /// <param name="yIntervalInMM">y 运行距离</param>
        /// <param name="differencevalue">载板与pcb直接差值</param>
        /// <returns></returns>
        public static List<int> bycoordinate(int num, int yIntervalInMM, int differencevalue)
        {
            List<int> ycoordinatelist = new List<int>();
            for (int i = 0; i < num; i++)
            {
                ycoordinatelist.Add(by + differencevalue * motorScale + yIntervalInMM * i * motorScale);
            }

            return ycoordinatelist;
        }
    }
}
