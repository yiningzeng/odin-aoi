using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace power_aoi.Enums
{
    class HardwareEnum
    {
        public class EnumHelper
        {
            public static string GetDescription(Enum obj)
            {
                string objName = obj.ToString();
                Type t = obj.GetType();
                System.Reflection.FieldInfo fi = t.GetField(objName);
                System.ComponentModel.DescriptionAttribute[] arrDesc = (System.ComponentModel.DescriptionAttribute[])fi.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);

                return arrDesc[0].Description;
            }
            public static void GetEnum<T>(string a, ref T t)
            {
                foreach (T b in Enum.GetValues(typeof(T)))
                {
                    if (GetDescription(b as Enum) == a)
                        t = b;
                }
            }

            public static T GetEnum<T>(string a)
            {
                T target = Activator.CreateInstance<T>();
                foreach (T b in Enum.GetValues(typeof(T)))
                {
                    if (GetDescription(b as Enum) == a)
                        target = b;
                }

                return target;
            }

            public static List<string> EnumList<T>()
            {
                List<string> listName = new List<string>();
                foreach (T item in Enum.GetValues(typeof(T)))
                {
                    string str = item.ToString();
                    Type type = item.GetType();
                    string name = Enum.GetName(type, item);

                    FieldInfo field = type.GetField(Enum.GetName(type, item));
                    DescriptionAttribute descAttr = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;

                    listName.Add(descAttr.Description);
                }
                return listName;
            }
        }

        /// <summary>
        /// IO监控
        /// </summary>
        public enum Monitor_IO
        {
            [Description("1000,0,A面X轴左极限,0")]
            X0_0,
            [Description("1000,1,A面X轴右极限,0")]
            X0_1,
            [Description("1000,2,A面X轴信号原点,0")]
            X0_2,
            [Description("1000,3,A面Y轴左极限,0")]
            X0_3,
            [Description("1000,4,A面Y轴右极限,0")]
            X0_4,
            [Description("1000,5,A面Y轴信号原点,0")]
            X0_5,
            [Description("1000,6,B面X轴左极限,0")]
            X0_6,
            [Description("1000,7,B面X轴右极限,0")]
            X0_7,
            [Description("1000,8,B面X轴信号原点,0")]
            X0_8,
            [Description("1000,9,B面Y轴左极限,0")]
            X0_9,
            [Description("1000,10,B面Y轴右极限,0")]
            X0_10,
            [Description("1000,11,B面Y轴信号原点,0")]
            X0_11,
            [Description("1000,12,K轴左极限,0")]
            X0_12,
            [Description("1000,13,K轴右极限,0")]
            X0_13,
            [Description("1000,14,K轴信号原点,0")]
            X0_14,
            [Description("1000,15,U轴信号原点,0")]
            X0_15,
            [Description("1001,0,急   停,0")]
            X1_0,
            [Description("1001,1,启   动,0")]
            X1_1,
            [Description("1001,2,暂   停,0")]
            X1_2,
            [Description("1001,3,复   位,0")]
            X1_3,
            [Description("1001,4,模式切换,0")]
            X1_4,
            [Description("1001,5,开门信号,0")]
            X1_5,
            [Description("1001,6,流入产品检测信号,0")]
            X1_6,
            [Description("1001,7,流出产品检测信号,0")]
            X1_7,
            [Description("1001,8,U轴近点信号,0")]
            X1_8,
            [Description("1001,9,A面X轴伺服报警,0")]
            X1_9,
            [Description("1001,10,A面Y轴伺服报警,0")]
            X1_10,
            [Description("1001,11,B面X轴伺服报警,0")]
            X1_11,
            [Description("1001,12,B面Y轴伺服报警,0")]
            X1_12,
            [Description("1001,13,K轴伺服报警,0")]
            X1_13,
            [Description("1001,14,U轴伺服报警,0")]
            X1_14,
            [Description("1001,15,备   用,0")]
            X1_15,
            [Description("1002,0,前站-输入给板信号,0")]
            X2_0,
            [Description("1002,1,前站-输入产品OK信号,0")]
            X2_1,
            [Description("1002,2,前站-输入产品NG信号,0")]
            X2_2,
            [Description("1002,3,后站-输入请求要板信号,0")]
            X2_3,
            [Description("1002,4,后站-输入备用信号,0")]
            X2_4,
            [Description("1002,5,后站-输入备用信号,0")]
            X2_5,
            [Description("1002,6,产线-输入整线请求启动,0")]
            X2_6,
            [Description("1002,7,产线-输入整线请求暂停,0")]
            X2_7,
            [Description("1002,8,产线-输入整线请求复位,0")]
            X2_8,
            [Description("1002,9,产线-输入整线模式切换,0")]
            X2_9,
            [Description("1002,10,产线-输入整线模式急停,0")]
            X2_10,
            [Description("1002,11,备   用,0")]
            X2_11,
            [Description("1002,12,备   用,0")]
            X2_12,
            [Description("1002,13,备   用,0")]
            X2_13,
            [Description("1002,14,备   用,0")]
            X2_14,
            [Description("1002,15,备   用,0")]
            X2_15,
            [Description("1003,0,A面X轴脉冲,0")]
            Y0_0,
            [Description("1003,1,A面X轴方向,0")]
            Y0_1,
            [Description("1003,2,A面Y轴脉冲,0")]
            Y0_2,
            [Description("1003,3,A面Y轴方向,0")]
            Y0_3,
            [Description("1003,4,B面X轴脉冲,0")]
            Y0_4,
            [Description("1003,5,B面X轴方向,0")]
            Y0_5,
            [Description("1003,6,B面Y轴脉冲,0")]
            Y0_6,
            [Description("1003,7,B面Y轴方向,0")]
            Y0_7,
            [Description("1003,8,K轴脉冲,0")]
            Y0_8,
            [Description("1003,9,K轴方向,0")]
            Y0_9,
            [Description("1003,10,U轴脉冲,0")]
            Y0_10,
            [Description("1003,11,U轴方向,0")]
            Y0_11,
            [Description("1003,12,备   用,0")]
            Y0_12,
            [Description("1003,13,备   用,0")]
            Y0_13,
            [Description("1003,14,备   用,0")]
            Y0_14,
            [Description("1003,15,备   用,0")]
            Y0_15,
            [Description("1004,0,备   用,0")]
            Y1_0,
            [Description("1004,1,A面相机,0")]
            Y1_1,
            [Description("1004,2,备   用,0")]
            Y1_2,
            [Description("1004,3,B面相机,0")]
            Y1_3,
            [Description("1004,4,吹   气,0")]
            Y1_4,
            [Description("1004,5,红灯警示,0")]
            Y1_5,
            [Description("1004,6,黄灯警示,0")]
            Y1_6,
            [Description("1004,7,绿灯警示,0")]
            Y1_7,
            [Description("1004,8,蓝灯警示,0")]
            Y1_8,
            [Description("1004,9,蜂鸣器,0")]
            Y1_9,
            [Description("1004,10,A面X轴使能,0")]
            Y1_10,
            [Description("1004,11,A面Y轴使能,0")]
            Y1_11,
            [Description("1004,12,B面X轴使能,0")]
            Y1_12,
            [Description("1004,13,B面Y轴使能,0")]
            Y1_13,
            [Description("1004,14,K轴使能,0")]
            Y1_14,
            [Description("1004,15,U轴使能,0")]
            Y1_15,
            [Description("1005,0,前站-输出请求要板标志位,0")]
            Y2_0,
            [Description("1005,1,前站-输出备用输出,0")]
            Y2_1,
            [Description("1005,2,前站-输出备用输出,0")]
            Y2_2,
            [Description("1005,3,后站-输出给板信号,0")]
            Y2_3,
            [Description("1005,4,后站-输出本产品OK信号,0")]
            Y2_4,
            [Description("1005,5,后站-输出本产品NG信号,0")]
            Y2_5,
            [Description("1005,6,产线-输出整线启动状态,0")]
            Y2_6,
            [Description("1005,7,产线-输出整线暂停状态,0")]
            Y2_7,
            [Description("1005,8,产线-输出整线复位状态,0")]
            Y2_8,
            [Description("1005,9,产线-输出整线模式状态,0")]
            Y2_9,
            [Description("1005,10,产线-输出整线急停状态,0")]
            Y2_10,
            [Description("1005,11,6轴报警复位,0")]
            Y2_11,
            [Description("1005,12,备   用,0")]
            Y2_12,
            [Description("1005,13,备   用,0")]
            Y2_13,
            [Description("1005,14,备   用,0")]
            Y2_14,
            [Description("1005,15,备   用,0")]
            Y2_15,
            [Description("1006,0,A面光源CH1,0")]
            Y3_0,
            [Description("1006,1,A面光源CH2,0")]
            Y3_1,
            [Description("1006,2,A面光源CH3,0")]
            Y3_2,
            [Description("1006,3,A面光源CH4,0")]
            Y3_3,
            [Description("1006,4,A面光源CH5,0")]
            Y3_4,
            [Description("1006,5,B面光源CH9,0")]
            Y3_5,
            [Description("1006,6,B面光源CH10,0")]
            Y3_6,
            [Description("1006,7,B面光源CH11,0")]
            Y3_7,
            [Description("1006,8,B面光源CH12,0")]
            Y3_8,
            [Description("1006,9,B面光源CH13,0")]
            Y3_9,
            [Description("1006,10,备   用,0")]
            Y3_10,
            [Description("1006,11,备   用,0")]
            Y3_11,
            [Description("1006,12,备   用,0")]
            Y3_12,
            [Description("1006,13,备   用,0")]
            Y3_13,
            [Description("1006,14,备   用,0")]
            Y3_14,
            [Description("1006,15,备   用,0")]
            Y3_15,
        }

        /// <summary>
        /// 私服监控
        /// </summary>
        public enum Servo
        {
            [Description("1007,A面X轴实时位置,0")]
            A_XPTIN,
            [Description("1009,A面Y轴实时位置,0")]
            A_YPTIN,
            [Description("1011,B面X轴实时位置,0")]
            B_XPTIN,
            [Description("1013,B面Y轴实时位置,0")]
            B_YPTIN,
            [Description("1015,K轴实时位置,0")]
            K_PTIN,
            [Description("1017,U轴实时位置,0")]
            U_PTIN,
            [Description("1019,A面X轴偏移位置,0")]
            A_XOFIN,
            [Description("1021,A面Y轴偏移位置,0")]
            A_YOFIN,
            [Description("1023,B面X轴偏移位置,0")]
            B_XOFIN,
            [Description("1025,B面Y轴偏移位置,0")]
            B_YOFIN,
            [Description("1027,K轴偏移位置,0")]
            K_OFIN,
            [Description("1029,A面X轴正极限位置,0")]
            SR580IN,
            [Description("1031,A面X轴反极限位置,0")]
            SR582IN,
            [Description("1033,A面Y轴正极限位置,0")]
            SR584IN,
            [Description("1035,A面Y轴负极限位置,0")]
            SR586IN,
            [Description("1037,B面X轴正极限位置,0")]
            SR588IN,
            [Description("1039,B面X轴负极限位置,0")]
            SR590IN,
            [Description("1041,B面Y轴正极限位置,0")]
            SR592IN,
            [Description("1043,B面Y轴负极限位置,0")]
            SR594IN,
            [Description("1045,K轴正极限位置,0")]
            SR596IN,
            [Description("1047,K轴负极限位置,0")]
            SR598IN,
            [Description("1049,A面X轴运行速度,0")]
            A_XSPIN,
            [Description("1050,A面Y轴运行速度,0")]
            A_YSPIN,
            [Description("1051,B面X轴运行速度,0")]
            B_XSPIN,
            [Description("1052,B面Y轴运行速度,0")]
            B_YSPIN,
            [Description("1053,K轴运行速度,0")]
            K_SPIN,
            [Description("1054,U轴运行速度,0")]
            U_SPIN,
            [Description("1055,A面X轴斜坡速度,0")]
            A_XPSPIN,
            [Description("1056,A面Y轴斜坡速度,0")]
            A_YPSPIN,
            [Description("1057,B面X轴斜坡速度,0")]
            B_XPSPIN,
            [Description("1058,B面Y轴斜坡速度,0")]
            B_YPSPIN,
            [Description("1059,K轴斜坡速度,0")]
            K_PSPIN,
            [Description("1060,U轴斜坡速度,0")]
            U_PSPIN,
            [Description("1061,A面X轴JOG速度,0")]
            A_XJOGSPIN,
            [Description("1062,A面Y轴JOG速度,0")]
            A_YJOGSPIN,
            [Description("1063,B面X轴JOG速度,0")]
            B_XJOGSPIN,
            [Description("1064,B面Y轴JOG速度,0")]
            B_YJOGSPIN,
            [Description("1065,K轴JOG速度,0")]
            K_JOGSPIN,
            [Description("1066,U轴JOG速度,0")]
            U_JOGSPIN,
            [Description("1067,A面X轴归零高速,0")]
            A_XGSIN,
            [Description("1068,A面Y轴归零高速,0")]
            A_YGSIN,
            [Description("1069,B面X轴归零高速,0")]
            B_XGSIN,
            [Description("1070,B面Y轴归零高速,0")]
            B_YGSIN,
            [Description("1071,K轴归零高速,0")]
            K_GSIN,
            [Description("1072,U轴归零高速,0")]
            U_GSIN,
            [Description("1073,A面X轴归零低速,0")]
            A_XDSIN,
            [Description("1074,A面Y轴归零低速,0")]
            A_YDSIN,
            [Description("1075,B面X轴归零低速,0")]
            B_XDSIN,
            [Description("1076,B面Y轴归零低速,0")]
            B_YDSIN,
            [Description("1077,K轴归零低速,0")]
            K_DSIN,
            [Description("1078,U轴归零低速,0")]
            U_DSIN,
            [Description("1079,A面X轴斜坡加速时间,0")]
            A_XJSPIN,
            [Description("1080,A面Y轴斜坡加速时间,0")]
            A_YJSPIN,
            [Description("1081,B面X轴斜坡加速时间,0")]
            B_XJSPIN,
            [Description("1082,B面Y轴斜坡加速时间,0")]
            B_YJSPIN,
            [Description("1083,K轴斜坡加速时间,0")]
            K_JSPIN,
            [Description("1084,U轴斜坡加速时间,0")]
            U_JSPIN,
            [Description("1085,A面X轴斜坡减速时间,0")]
            A_XGSPIN,
            [Description("1086,A面Y轴斜坡减速时间,0")]
            A_YGSPIN,
            [Description("1087,B面X轴斜坡减速时间,0")]
            B_XGSPIN,
            [Description("1088,B面Y轴斜坡减速时间,0")]
            B_YGSPIN,
            [Description("1089,K轴斜坡减速时间,0")]
            K_GSPIN,
            [Description("1090,U轴斜坡减速时间,0")]
            U_GSPIN,

        }

        public enum Run
        {
            [Description("1115,产品运行周期,0")]
            CT,
            [Description("1117,产品检测数量,0")]
            TN,
            [Description("1119,产品OK数量,0")]
            ON,
            [Description("1121,产品NG数量,0")]
            NN,
            [Description("1123,相机拍摄总次数,0")]
            CN,
            [Description("1125,设备调试时间,0")]
            TT,
            [Description("1127,设备投产时间,0")]
            IT,
            [Description("1129,设备运行时间,0")]
            RT,
            [Description("1131,定位结束信号,0")]
            SendRunEnd,
            [Description("1133,A拍摄结束发完毕结束信号,0")]
            ASendFinishEnd,
            [Description("1135,B拍摄结束发完毕结束信号,0")]
            BSendFinishEnd,
        }

        /// <summary>
        /// 报警监控
        /// </summary>
        public enum Alarm
        {
            [Description("1147,0, ,0")]
            AL0001A面X轴伺服综合报警,
            [Description("1147,1, A面Y轴伺服综合报警,0")]
            AL0002A面Y轴伺服综合报警,
            [Description("1147,2, B面X轴伺服综合报警,0")]
            AL0003B面X轴伺服综合报警,
            [Description("1147,3, B面Y轴伺服综合报警,0")]
            AL0004B面Y轴伺服综合报警,
            [Description("1147,4, K轴伺服综合报警,0")]
            AL0005K轴伺服综合报警,
            [Description("1147,5, U轴伺服综合报警,0")]
            AL0006U轴伺服综合报警,
            [Description("1147,6, A面X轴左极限到达报警,0")]
            AL0007A面X轴左极限到达报警,
            [Description("1147,7, A面X轴右极限到达报警,0")]
            AL0008A面X轴右极限到达报警,
            [Description("1147,8, A面Y轴左极限到达报警,0")]
            AL0009A面Y轴左极限到达报警,
            [Description("1147,9, A面Y轴右极限到达报警,0")]
            AL0010A面Y轴右极限到达报警,
            [Description("1147,10, B面X轴左极限到达报警,0")]
            AL0011B面X轴左极限到达报警,
            [Description("1147,11, B面X轴右极限到达报警,0")]
            AL0012B面X轴右极限到达报警,
            [Description("1147,12, B面Y轴左极限到达报警,0")]
            AL0013B面Y轴左极限到达报警,
            [Description("1147,13, B面Y轴右极限到达报警,0")]
            AL0014B面Y轴右极限到达报警,
            [Description("1147,14, K轴左极限到达报警,0")]
            AL0015K轴左极限到达报警,
            [Description("1147,15, K轴右极限到达报警,0")]
            AL0016K轴右极限到达报警,
            [Description("1148,0, 本机急停报警,0")]
            AL0017本机急停报警,
            [Description("1148,1, 外部急停报警,0")]
            AL0018外部急停报警,
            [Description("1148,2, 安全门打开报警,0")]
            AL0019安全门打开报警,
            [Description("1148,3, 下开门异常,0")]
            AL0020心跳交互异常报警,
            [Description("1148,4, 进板异常,0")]
            AL0021输入信号检测异常,
            [Description("1148,5, 备   用,0")]
            AL0022备用,
            [Description("1148,6, 备   用,0")]
            AL0023备用,
            [Description("1148,7, 备   用,0")]
            AL0024备用,
            [Description("1148,8, 备   用,0")]
            AL0025备用,
            [Description("1148,9, 备   用,0")]
            AL0026备用,
            [Description("1148,10, 备   用,0")]
            AL0027备用,
            [Description("1148,11, 备   用,0")]
            AL0028备用,
            [Description("1148,12, 备   用,0")]
            AL0029备用,
            [Description("1148,13, 备   用,0")]
            AL0030备用,
            [Description("1148,14, 备   用,0")]
            AL0031备用,
            [Description("1148,15, 备   用,0")]
            AL0032备用,
        }

        /// <summary>
        /// 手动按钮
        /// </summary>
        public enum Manual
        {
            [Description("2000,0,备   用,true")]
            Y1_0,
            [Description("2000,1,A面相机,true")]
            Y1_1,
            [Description("2000,2,备   用,true")]
            Y1_2,
            [Description("2000,3,B面相机,true")]
            Y1_3,
            [Description("2000,4,吹气,true")]
            Y1_4,
            [Description("2000,5,红灯警示,true")]
            Y1_5,
            [Description("2000,6,黄灯警示,true")]
            Y1_6,
            [Description("2000,7,绿灯警示,true")]
            Y1_7,
            [Description("2000,8,备   用,true")]
            Y1_8,
            [Description("2000,9,蜂鸣器,true")]
            Y1_9,
            [Description("2000,10,A面X轴使能,true")]
            Y1_10,
            [Description("2000,11,A面Y轴使能,true")]
            Y1_11,
            [Description("2000,12,B面X轴使能,true")]
            Y1_12,
            [Description("2000,13,B面Y轴使能,true")]
            Y1_13,
            [Description("2000,14,K轴使能,true")]
            Y1_14,
            [Description("2000,15,U轴使能,true")]
            Y1_15,
            [Description("2001,0,前站-输出请求要板标志位,true")]
            Y2_0,
            [Description("2001,1,前站-输出备用输出,true")]
            Y2_1,
            [Description("2001,2,前站-输出备用输出,true")]
            Y2_2,
            [Description("2001,3,后站-输出给板信号,true")]
            Y2_3,
            [Description("2001,4,后站-输出本产品OK信号,true")]
            Y2_4,
            [Description("2001,5,后站-输出本产品NG信号,true")]
            Y2_5,
            [Description("2001,6,产线-输出整线启动状态,true")]
            Y2_6,
            [Description("2001,7,产线-输出整线暂停状态,true")]
            Y2_7,
            [Description("2001,8,产线-输出整线复位状态,true")]
            Y2_8,
            [Description("2001,9,产线-输出整线模式状态,true")]
            Y2_9,
            [Description("2001,10,产线-输出整线急停状态,true")]
            Y2_10,
            [Description("2001,11,6轴报警复位,true")]
            Y2_11,
            [Description("2001,12,6轴报警复位,true")]
            Y2_12,
            [Description("2001,13,6轴报警复位,true")]
            Y2_13,
            [Description("2001,14,6轴报警复位,true")]
            Y2_14,
            [Description("2001,15,6轴报警复位,true")]
            Y2_15,
            [Description("2002,0,A面X轴原点复归触发,false")]
            A_XRST,
            [Description("2002,1,A面Y轴原点复归触发,false")]
            A_YRST,
            [Description("2002,2,B面X轴原点复归触发,false")]
            B_XRST,
            [Description("2002,3,B面Y轴原点复归触发,false")]
            B_YRST,
            [Description("2002,4,K轴原点复归触发,false")]
            K_RST,
            [Description("2002,5,伺服5轴一键复归触发,false")]
            AB_RST,
            [Description("2002,6,A面X轴JOG+触发,false")]
            A_XJOG_Plus,
            [Description("2002,7,A面X轴JOG-触发,false")]
            A_XJOG_Sub,
            [Description("2002,8,A面Y轴JOG+触发,false")]
            A_YJOG_Plus,
            [Description("2002,9,A面Y轴JOG-触发,false")]
            A_YJOG_Sub,
            [Description("2002,10,B面X轴JOG+触发,false")]
            B_XJOG_Plus,
            [Description("2002,11,B面X轴JOG-触发,false")]
            B_XJOG_Sub,
            [Description("2002,12,B面Y轴JOG+触发,false")]
            B_YJOG_Plus,
            [Description("2002,13,B面Y轴JOG-触发,false")]
            B_YJOG_Sub,
            [Description("2002,14,K轴JOG+触发,false")]
            K_JOG_Plus,
            [Description("2002,15,K轴JOG-触发,false")]
            K_JOG_Sub,
            [Description("2003,0,A面光源CH1,true")]
            Y3_0,
            [Description("2003,1,A面光源CH2,true")]
            Y3_1,
            [Description("2003,2,A面光源CH3,true")]
            Y3_2,
            [Description("2003,3,A面光源CH4,true")]
            Y3_3,
            [Description("2003,4,A面光源CH5,true")]
            Y3_4,
            [Description("2003,5,B面光源CH9,true")]
            Y3_5,
            [Description("2003,6,B面光源CH10,true")]
            Y3_6,
            [Description("2003,7,B面光源CH11,true")]
            Y3_7,
            [Description("2003,8,B面光源CH12,true")]
            Y3_8,
            [Description("2003,9,B面光源CH13,true")]
            Y3_9,
            [Description("2003,10,备   用,true")]
            Y3_10,
            [Description("2003,11,备   用,true")]
            Y3_11,
            [Description("2003,12,备   用,true")]
            Y3_12,
            [Description("2003,13,备   用,true")]
            Y3_13,
            [Description("2003,14,备   用,true")]
            Y3_14,
            [Description("2003,15,备   用,true")]
            Y3_15,
            [Description("2004,0,开门屏蔽报警按钮,true")]
            RST_AL,
            [Description("2004,1,A面X轴绝对位置触发,false")]
            AX_ABS,
            [Description("2004,2,A面Y轴绝对位置触发,false")]
            AY_ABS,
            [Description("2004,3,B面X轴绝对位置触发,false")]
            BX_ABS,
            [Description("2004,4,B面Y轴绝对位置触发,false")]
            BY_ABS,
            [Description("2004,5,K轴绝对位置触发,false")]
            K_ABS,
            [Description("2004,6,U轴绝对位置触发,false")]
            U_ABS,
        }

        public enum Motor
        {

        }
        public enum Servo_Config
        {
            [Description("2020,32,1000,A面X轴偏移位置(mm),0")]
            A_XOF,
            [Description("2022,32,1000,A面Y轴偏移位置(mm),0")]
            A_YOF,
            [Description("2024,32,1000,B面X轴偏移位置(mm),0")]
            B_XOF,
            [Description("2026,32,1000,B面Y轴偏移位置(mm),0")]
            B_YOF,
            [Description("2028,32,1000,K轴偏移位置(mm),0")]
            K_OF,
            [Description("2030,32,1000,A面X轴正极限位置(mm),0")]
            SR580,
            [Description("2032,32,1000,A面X轴反极限位置(mm),0")]
            SR582,
            [Description("2034,32,1000,A面Y轴正极限位置(mm),0")]
            SR584,
            [Description("2036,32,1000,A面Y轴负极限位置(mm),0")]
            SR586,
            [Description("2038,32,1000,B面X轴正极限位置(mm),0")]
            SR588,
            [Description("2040,32,1000,B面X轴负极限位置(mm),0")]
            SR590,
            [Description("2042,32,1000,B面Y轴正极限位置(mm),0")]
            SR592,
            [Description("2044,32,1000,B面Y轴负极限位置(mm),0")]
            SR594,
            [Description("2046,32,1000,K轴正极限位置(mm),0")]
            SR596,
            [Description("2048,32,1000,K轴负极限位置(mm),0")]
            SR598,
            [Description("2050,16,1,A面X轴运行速度(mm/s),0")]
            A_XSP,
            [Description("2051,16,1,A面Y轴运行速度(mm/s),0")]
            A_YSP,
            [Description("2052,16,1,B面X轴运行速度(mm/s),0")]
            B_XSP,
            [Description("2053,16,1,B面Y轴运行速度(mm/s),0")]
            B_YSP,
            [Description("2054,16,1,K轴运行速度(mm/s),0")]
            K_SP,
            [Description("2055,16,1,U轴运行速度(mm/s),0")]
            U_SP,
            [Description("2056,16,1,A面X轴斜坡速度(mm/s),0")]
            A_XPSP,
            [Description("2057,16,1,A面Y轴斜坡速度(mm/s),0")]
            A_YPSP,
            [Description("2058,16,1,B面X轴斜坡速度(mm/s),0")]
            B_XPSP,
            [Description("2059,16,1,B面Y轴斜坡速度(mm/s),0")]
            B_YPSP,
            [Description("2060,16,1,K轴斜坡速度(mm/s),0")]
            K_PSP,
            [Description("2061,16,1,U轴斜坡速度(mm/s),0")]
            U_PSP,
            [Description("2062,16,1,A面X轴JOG速度(mm/s),0")]
            A_XJOGSP,
            [Description("2063,16,1,A面Y轴JOG速度(mm/s),0")]
            A_YJOGSP,
            [Description("2064,16,1,B面X轴JOG速度(mm/s),0")]
            B_XJOGSP,
            [Description("2065,16,1,B面Y轴JOG速度(mm/s),0")]
            B_YJOGSP,
            [Description("2066,16,1,K轴JOG速度(mm/s),0")]
            K_JOGSP,
            [Description("2067,16,1,U轴JOG速度(mm/s),0")]
            U_JOGSP,
            [Description("2068,16,1,A面X轴归零高速(mm/s),0")]
            A_XGS,
            [Description("2069,16,1,A面Y轴归零高速(mm/s),0")]
            A_YGS,
            [Description("2070,16,1,B面X轴归零高速(mm/s),0")]
            B_XGS,
            [Description("2071,16,1,B面Y轴归零高速(mm/s),0")]
            B_YGS,
            [Description("2072,16,1,K轴归零高速(mm/s),0")]
            K_GS,
            [Description("2073,16,1,U轴归零高速(mm/s),0")]
            U_GS,
            [Description("2074,16,1,A面X轴归零低速(mm/s),0")]
            A_XDS,
            [Description("2075,16,1,A面Y轴归零低速(mm/s),0")]
            A_YDS,
            [Description("2076,16,1,B面X轴归零低速(mm/s),0")]
            B_XDS,
            [Description("2077,16,1,B面Y轴归零低速(mm/s),0")]
            B_YDS,
            [Description("2078,16,1,K轴归零低速(mm/s),0")]
            K_DS,
            [Description("2079,16,1,U轴归零低速(mm/s),0")]
            U_DS,
            [Description("2080,16,1,A面X轴斜坡加速时间(ms),0")]
            A_XJSP,
            [Description("2081,16,1,A面Y轴斜坡加速时间(ms),0")]
            A_YJSP,
            [Description("2082,16,1,B面X轴斜坡加速时间(ms),0")]
            B_XJSP,
            [Description("2083,16,1,B面Y轴斜坡加速时间(ms),0")]
            B_YJSP,
            [Description("2084,16,1,K轴斜坡加速时间(ms),0")]
            K_JSP,
            [Description("2085,16,1,U轴斜坡加速时间(ms),0")]
            U_JSP,
            [Description("2086,16,1,A面X轴斜坡减速时间(ms),0")]
            A_XGSP,
            [Description("2087,16,1,A面Y轴斜坡减速时间(ms),0")]
            A_YGSP,
            [Description("2088,16,1,B面X轴斜坡减速时间(ms),0")]
            B_XGSP,
            [Description("2089,16,1,B面Y轴斜坡减速时间(ms),0")]
            B_YGSP,
            [Description("2090,16,1,K轴斜坡减速时间(ms),0")]
            K_GSP,
            [Description("2091,16,1,U轴斜坡减速时间(ms),0")]
            U_GSP,

        }
        public enum Run_Config
        {
            [Description("2116,32,1000,A面X轴运行坐标位置(mm),0")]
            A_XPOS,
            [Description("2118,32,1000,A面Y轴运行坐标位置(mm),0")]
            A_YPOS,
            [Description("2120,32,1000,B面X轴运行坐标位置(mm),0")]
            B_XPOS,
            [Description("2122,32,1000,B面Y轴运行坐标位置(mm),0")]
            B_YPOS,
            [Description("2124,32,1000,K轴轨道宽度位置(mm),0")]
            K_XPOS,
            [Description("2126,32,1000,A面X轴相机拍摄距离(mm),0")]
            A_XDIS,
            [Description("2128,32,1000,A面Y轴相机拍摄距离(mm),0")]
            A_YDIS,
            [Description("2130,32,1000,B面X轴相机拍摄距离(mm),0")]
            B_XDIS,
            [Description("2132,32,1000,B面Y轴相机拍摄距离(mm),0")]
            B_YDIS,
            [Description("2134,16,1,A面X轴运行拍摄数量(PCS),0")]
            A_XAM,
            [Description("2135,16,1,A面Y轴运行拍摄数量(PCS),0")]
            A_YAM,
            [Description("2136,16,1,B面X轴运行拍摄数量(PCS),0")]
            B_XAM,
            [Description("2137,16,1,B面Y轴运行拍摄数量(PCS),0")]
            B_YAM,
            [Description("2138,16,1,A面相机拍摄脉宽(ms),0")]
            A_PUL1,
            [Description("2139,16,1,A面光源拍摄脉宽(ms),0")]
            A_PUL2,
            [Description("2140,16,1,B面相机拍摄脉宽(ms),0")]
            B_PUL1,
            [Description("2141,16,1,B面光源拍摄脉宽(ms),0")]
            B_PUL2,
            [Description("2142,16,1,A面拍摄缓停时间(ms),0")]
            A_STPT,
            [Description("2143,16,1,B面拍摄缓停时间(ms),0")]
            B_STPT,
            [Description("2144,16,1,客户端发运行标志,0")]
            A_PC_RUN,
            [Description("2145,16,1,客户端发结束标志,0")]
            A_PC_OVE,
            [Description("2146,16,1,B面客户端发运行标志,0")]
            B_PC_RUN,
            [Description("2147,16,1,B面客户端发结束标志,0")]
            B_PC_OVE,
        }
        public enum A_ABS
        {
            [Description("A_XY轴绝对位置1(mm),3000,0,3200,0")]
            A_XYP1,
            [Description("A_XY轴绝对位置2(mm),3002,0,3202,0")]
            A_XYP2,
            [Description("A_XY轴绝对位置3(mm),3004,0,3204,0")]
            A_XYP3,
            [Description("A_XY轴绝对位置4(mm),3006,0,3206,0")]
            A_XYP4,
            [Description("A_XY轴绝对位置5(mm),3008,0,3208,0")]
            A_XYP5,
            [Description("A_XY轴绝对位置6(mm),3010,0,3210,0")]
            A_XYP6,
            [Description("A_XY轴绝对位置7(mm),3012,0,3212,0")]
            A_XYP7,
            [Description("A_XY轴绝对位置8(mm),3014,0,3214,0")]
            A_XYP8,
            [Description("A_XY轴绝对位置9(mm),3016,0,3216,0")]
            A_XYP9,
            [Description("A_XY轴绝对位置10(mm),3018,0,3218,0")]
            A_XYP10,
            [Description("A_XY轴绝对位置11(mm),3020,0,3220,0")]
            A_XYP11,
            [Description("A_XY轴绝对位置12(mm),3022,0,3222,0")]
            A_XYP12,
            [Description("A_XY轴绝对位置13(mm),3024,0,3224,0")]
            A_XYP13,
            [Description("A_XY轴绝对位置14(mm),3026,0,3226,0")]
            A_XYP14,
            [Description("A_XY轴绝对位置15(mm),3028,0,3228,0")]
            A_XYP15,
            [Description("A_XY轴绝对位置16(mm),3030,0,3230,0")]
            A_XYP16,
            [Description("A_XY轴绝对位置17(mm),3032,0,3232,0")]
            A_XYP17,
            [Description("A_XY轴绝对位置18(mm),3034,0,3234,0")]
            A_XYP18,
            [Description("A_XY轴绝对位置19(mm),3036,0,3236,0")]
            A_XYP19,
            [Description("A_XY轴绝对位置20(mm),3038,0,3238,0")]
            A_XYP20,
            [Description("A_XY轴绝对位置21(mm),3040,0,3240,0")]
            A_XYP21,
            [Description("A_XY轴绝对位置22(mm),3042,0,3242,0")]
            A_XYP22,
            [Description("A_XY轴绝对位置23(mm),3044,0,3244,0")]
            A_XYP23,
            [Description("A_XY轴绝对位置24(mm),3046,0,3246,0")]
            A_XYP24,
            [Description("A_XY轴绝对位置25(mm),3048,0,3248,0")]
            A_XYP25,
            [Description("A_XY轴绝对位置26(mm),3050,0,3250,0")]
            A_XYP26,
            [Description("A_XY轴绝对位置27(mm),3052,0,3252,0")]
            A_XYP27,
            [Description("A_XY轴绝对位置28(mm),3054,0,3254,0")]
            A_XYP28,
            [Description("A_XY轴绝对位置29(mm),3056,0,3256,0")]
            A_XYP29,
            [Description("A_XY轴绝对位置30(mm),3058,0,3258,0")]
            A_XYP30,
            [Description("A_XY轴绝对位置31(mm),3060,0,3260,0")]
            A_XYP31,
            [Description("A_XY轴绝对位置32(mm),3062,0,3262,0")]
            A_XYP32,
            [Description("A_XY轴绝对位置33(mm),3064,0,3264,0")]
            A_XYP33,
            [Description("A_XY轴绝对位置34(mm),3066,0,3266,0")]
            A_XYP34,
            [Description("A_XY轴绝对位置35(mm),3068,0,3268,0")]
            A_XYP35,
            [Description("A_XY轴绝对位置36(mm),3070,0,3270,0")]
            A_XYP36,
            [Description("A_XY轴绝对位置37(mm),3072,0,3272,0")]
            A_XYP37,
            [Description("A_XY轴绝对位置38(mm),3074,0,3274,0")]
            A_XYP38,
            [Description("A_XY轴绝对位置39(mm),3076,0,3276,0")]
            A_XYP39,
            [Description("A_XY轴绝对位置40(mm),3078,0,3278,0")]
            A_XYP40,
            [Description("A_XY轴绝对位置41(mm),3080,0,3280,0")]
            A_XYP41,
            [Description("A_XY轴绝对位置42(mm),3082,0,3282,0")]
            A_XYP42,
            [Description("A_XY轴绝对位置43(mm),3084,0,3284,0")]
            A_XYP43,
            [Description("A_XY轴绝对位置44(mm),3086,0,3286,0")]
            A_XYP44,
            [Description("A_XY轴绝对位置45(mm),3088,0,3288,0")]
            A_XYP45,
            [Description("A_XY轴绝对位置46(mm),3090,0,3290,0")]
            A_XYP46,
            [Description("A_XY轴绝对位置47(mm),3092,0,3292,0")]
            A_XYP47,
            [Description("A_XY轴绝对位置48(mm),3094,0,3294,0")]
            A_XYP48,
            [Description("A_XY轴绝对位置49(mm),3096,0,3296,0")]
            A_XYP49,
            [Description("A_XY轴绝对位置50(mm),3098,0,3298,0")]
            A_XYP50,
            [Description("拍摄数量,5000,0,5002,0")]
            ZBS,
        }

        public enum B_ABS
        {
            [Description("B_XY轴绝对位置1(mm),3400,0,3600,0")]
            B_XYP1,
            [Description("B_XY轴绝对位置2(mm),3402,0,3602,0")]
            B_XYP2,
            [Description("B_XY轴绝对位置3(mm),3404,0,3604,0")]
            B_XYP3,
            [Description("B_XY轴绝对位置4(mm),3406,0,3606,0")]
            B_XYP4,
            [Description("B_XY轴绝对位置5(mm),3408,0,3608,0")]
            B_XYP5,
            [Description("B_XY轴绝对位置6(mm),3410,0,3610,0")]
            B_XYP6,
            [Description("B_XY轴绝对位置7(mm),3412,0,3612,0")]
            B_XYP7,
            [Description("B_XY轴绝对位置8(mm),3414,0,3614,0")]
            B_XYP8,
            [Description("B_XY轴绝对位置9(mm),3416,0,3616,0")]
            B_XYP9,
            [Description("B_XY轴绝对位置10(mm),3418,0,3618,0")]
            B_XYP10,
            [Description("B_XY轴绝对位置11(mm),3420,0,3620,0")]
            B_XYP11,
            [Description("B_XY轴绝对位置12(mm),3422,0,3622,0")]
            B_XYP12,
            [Description("B_XY轴绝对位置13(mm),3424,0,3624,0")]
            B_XYP13,
            [Description("B_XY轴绝对位置14(mm),3426,0,3626,0")]
            B_XYP14,
            [Description("B_XY轴绝对位置15(mm),3428,0,3628,0")]
            B_XYP15,
            [Description("B_XY轴绝对位置16(mm),3430,0,3630,0")]
            B_XYP16,
            [Description("B_XY轴绝对位置17(mm),3432,0,3632,0")]
            B_XYP17,
            [Description("B_XY轴绝对位置18(mm),3434,0,3634,0")]
            B_XYP18,
            [Description("B_XY轴绝对位置19(mm),3436,0,3636,0")]
            B_XYP19,
            [Description("B_XY轴绝对位置20(mm),3438,0,3638,0")]
            B_XYP20,
            [Description("B_XY轴绝对位置21(mm),3440,0,3640,0")]
            B_XYP21,
            [Description("B_XY轴绝对位置22(mm),3442,0,3642,0")]
            B_XYP22,
            [Description("B_XY轴绝对位置23(mm),3444,0,3644,0")]
            B_XYP23,
            [Description("B_XY轴绝对位置24(mm),3446,0,3646,0")]
            B_XYP24,
            [Description("B_XY轴绝对位置25(mm),3448,0,3648,0")]
            B_XYP25,
            [Description("B_XY轴绝对位置26(mm),3450,0,3650,0")]
            B_XYP26,
            [Description("B_XY轴绝对位置27(mm),3452,0,3652,0")]
            B_XYP27,
            [Description("B_XY轴绝对位置28(mm),3454,0,3654,0")]
            B_XYP28,
            [Description("B_XY轴绝对位置29(mm),3456,0,3656,0")]
            B_XYP29,
            [Description("B_XY轴绝对位置30(mm),3458,0,3658,0")]
            B_XYP30,
            [Description("B_XY轴绝对位置31(mm),3460,0,3660,0")]
            B_XYP31,
            [Description("B_XY轴绝对位置32(mm),3462,0,3662,0")]
            B_XYP32,
            [Description("B_XY轴绝对位置33(mm),3464,0,3664,0")]
            B_XYP33,
            [Description("B_XY轴绝对位置34(mm),3466,0,3666,0")]
            B_XYP34,
            [Description("B_XY轴绝对位置35(mm),3468,0,3668,0")]
            B_XYP35,
            [Description("B_XY轴绝对位置36(mm),3470,0,3670,0")]
            B_XYP36,
            [Description("B_XY轴绝对位置37(mm),3472,0,3672,0")]
            B_XYP37,
            [Description("B_XY轴绝对位置38(mm),3474,0,3674,0")]
            B_XYP38,
            [Description("B_XY轴绝对位置39(mm),3476,0,3676,0")]
            B_XYP39,
            [Description("B_XY轴绝对位置40(mm),3478,0,3678,0")]
            B_XYP40,
            [Description("B_XY轴绝对位置41(mm),3480,0,3680,0")]
            B_XYP41,
            [Description("B_XY轴绝对位置42(mm),3482,0,3682,0")]
            B_XYP42,
            [Description("B_XY轴绝对位置43(mm),3484,0,3684,0")]
            B_XYP43,
            [Description("B_XY轴绝对位置44(mm),3486,0,3686,0")]
            B_XYP44,
            [Description("B_XY轴绝对位置45(mm),3488,0,3688,0")]
            B_XYP45,
            [Description("B_XY轴绝对位置46(mm),3490,0,3690,0")]
            B_XYP46,
            [Description("B_XY轴绝对位置47(mm),3492,0,3692,0")]
            B_XYP47,
            [Description("B_XY轴绝对位置48(mm),3494,0,3694,0")]
            B_XYP48,
            [Description("B_XY轴绝对位置49(mm),3496,0,3696,0")]
            B_XYP49,
            [Description("B_XY轴绝对位置50(mm),3498,0,3698,0")]
            B_XYP50,
            [Description("拍摄数量,5000,0,5002,0")]
            ZBS,
        }

        public enum A_L
        {
            [Description("A面区色系配比1,6000,0")]
            A_XCOLP1,
            [Description("A面区色系配比2,6002,0")]
            A_XCOLP2,
            [Description("A面区色系配比3,6004,0")]
            A_XCOLP3,
            [Description("A面区色系配比4,6006,0")]
            A_XCOLP4,
            [Description("A面区色系配比5,6008,0")]
            A_XCOLP5,
            [Description("A面区色系配比6,6010,0")]
            A_XCOLP6,
            [Description("A面区色系配比7,6012,0")]
            A_XCOLP7,
            [Description("A面区色系配比8,6014,0")]
            A_XCOLP8,
            [Description("A面区色系配比9,6016,0")]
            A_XCOLP9,
            [Description("A面区色系配比10,6018,0")]
            A_XCOLP10,
            [Description("A面区色系配比11,6020,0")]
            A_XCOLP11,
            [Description("A面区色系配比12,6022,0")]
            A_XCOLP12,
            [Description("A面区色系配比13,6024,0")]
            A_XCOLP13,
            [Description("A面区色系配比14,6026,0")]
            A_XCOLP14,
            [Description("A面区色系配比15,6028,0")]
            A_XCOLP15,
            [Description("A面区色系配比16,6030,0")]
            A_XCOLP16,
            [Description("A面区色系配比17,6032,0")]
            A_XCOLP17,
            [Description("A面区色系配比18,6034,0")]
            A_XCOLP18,
            [Description("A面区色系配比19,6036,0")]
            A_XCOLP19,
            [Description("A面区色系配比20,6038,0")]
            A_XCOLP20,
            [Description("A面区色系配比21,6040,0")]
            A_XCOLP21,
            [Description("A面区色系配比22,6042,0")]
            A_XCOLP22,
            [Description("A面区色系配比23,6044,0")]
            A_XCOLP23,
            [Description("A面区色系配比24,6046,0")]
            A_XCOLP24,
            [Description("A面区色系配比25,6048,0")]
            A_XCOLP25,
            [Description("A面区色系配比26,6050,0")]
            A_XCOLP26,
            [Description("A面区色系配比27,6052,0")]
            A_XCOLP27,
            [Description("A面区色系配比28,6054,0")]
            A_XCOLP28,
            [Description("A面区色系配比29,6056,0")]
            A_XCOLP29,
            [Description("A面区色系配比60,6058,0")]
            A_XCOLP30,
            [Description("A面区色系配比31,6060,0")]
            A_XCOLP31,
            [Description("A面区色系配比32,6062,0")]
            A_XCOLP32,
            [Description("A面区色系配比33,6064,0")]
            A_XCOLP33,
            [Description("A面区色系配比34,6066,0")]
            A_XCOLP34,
            [Description("A面区色系配比35,6068,0")]
            A_XCOLP35,
            [Description("A面区色系配比36,6070,0")]
            A_XCOLP36,
            [Description("A面区色系配比37,6072,0")]
            A_XCOLP37,
            [Description("A面区色系配比38,6074,0")]
            A_XCOLP38,
            [Description("A面区色系配比39,6076,0")]
            A_XCOLP39,
            [Description("A面区色系配比40,6078,0")]
            A_XCOLP40,
            [Description("A面区色系配比41,6080,0")]
            A_XCOLP41,
            [Description("A面区色系配比42,6082,0")]
            A_XCOLP42,
            [Description("A面区色系配比43,6084,0")]
            A_XCOLP43,
            [Description("A面区色系配比44,6086,0")]
            A_XCOLP44,
            [Description("A面区色系配比45,6088,0")]
            A_XCOLP45,
            [Description("A面区色系配比46,6090,0")]
            A_XCOLP46,
            [Description("A面区色系配比47,6092,0")]
            A_XCOLP47,
            [Description("A面区色系配比48,6094,0")]
            A_XCOLP48,
            [Description("A面区色系配比49,6096,0")]
            A_XCOLP49,
            [Description("A面区色系配比50,6098,0")]
            A_XCOLP50,
        }
        public enum B_L
        {
            [Description("B面区色系配比1,6400,0")]
            B_XCOLP1,
            [Description("B面区色系配比2,6402,0")]
            B_XCOLP2,
            [Description("B面区色系配比3,6404,0")]
            B_XCOLP3,
            [Description("B面区色系配比4,6406,0")]
            B_XCOLP4,
            [Description("B面区色系配比5,6408,0")]
            B_XCOLP5,
            [Description("B面区色系配比6,6410,0")]
            B_XCOLP6,
            [Description("B面区色系配比7,6412,0")]
            B_XCOLP7,
            [Description("B面区色系配比8,6414,0")]
            B_XCOLP8,
            [Description("B面区色系配比9,6416,0")]
            B_XCOLP9,
            [Description("B面区色系配比10,6418,0")]
            B_XCOLP10,
            [Description("B面区色系配比11,6420,0")]
            B_XCOLP11,
            [Description("B面区色系配比12,6422,0")]
            B_XCOLP12,
            [Description("B面区色系配比13,6424,0")]
            B_XCOLP13,
            [Description("B面区色系配比14,6426,0")]
            B_XCOLP14,
            [Description("B面区色系配比15,6428,0")]
            B_XCOLP15,
            [Description("B面区色系配比16,6430,0")]
            B_XCOLP16,
            [Description("B面区色系配比17,6432,0")]
            B_XCOLP17,
            [Description("B面区色系配比18,6434,0")]
            B_XCOLP18,
            [Description("B面区色系配比19,6436,0")]
            B_XCOLP19,
            [Description("B面区色系配比20,6438,0")]
            B_XCOLP20,
            [Description("B面区色系配比21,6440,0")]
            B_XCOLP21,
            [Description("B面区色系配比22,6442,0")]
            B_XCOLP22,
            [Description("B面区色系配比23,6444,0")]
            B_XCOLP23,
            [Description("B面区色系配比24,6446,0")]
            B_XCOLP24,
            [Description("B面区色系配比25,6448,0")]
            B_XCOLP25,
            [Description("B面区色系配比26,6450,0")]
            B_XCOLP26,
            [Description("B面区色系配比27,6452,0")]
            B_XCOLP27,
            [Description("B面区色系配比28,6454,0")]
            B_XCOLP28,
            [Description("B面区色系配比29,6456,0")]
            B_XCOLP29,
            [Description("B面区色系配比30,6458,0")]
            B_XCOLP30,
            [Description("B面区色系配比31,6460,0")]
            B_XCOLP31,
            [Description("B面区色系配比32,6462,0")]
            B_XCOLP32,
            [Description("B面区色系配比33,6464,0")]
            B_XCOLP33,
            [Description("B面区色系配比34,6466,0")]
            B_XCOLP34,
            [Description("B面区色系配比35,6468,0")]
            B_XCOLP35,
            [Description("B面区色系配比36,6470,0")]
            B_XCOLP36,
            [Description("B面区色系配比37,6472,0")]
            B_XCOLP37,
            [Description("B面区色系配比38,6474,0")]
            B_XCOLP38,
            [Description("B面区色系配比39,6476,0")]
            B_XCOLP39,
            [Description("B面区色系配比40,6478,0")]
            B_XCOLP40,
            [Description("B面区色系配比41,6480,0")]
            B_XCOLP41,
            [Description("B面区色系配比42,6482,0")]
            B_XCOLP42,
            [Description("B面区色系配比43,6484,0")]
            B_XCOLP43,
            [Description("B面区色系配比44,6486,0")]
            B_XCOLP44,
            [Description("B面区色系配比45,6488,0")]
            B_XCOLP45,
            [Description("B面区色系配比46,6490,0")]
            B_XCOLP46,
            [Description("B面区色系配比47,6492,0")]
            B_XCOLP47,
            [Description("B面区色系配比48,6494,0")]
            B_XCOLP48,
            [Description("B面区色系配比49,6496,0")]
            B_XCOLP49,
            [Description("B面区色系配比50,6498,0")]
            B_XCOLP50,

        }

        public enum Light
        {
            [Description("1,0")]
            A_White1,
            [Description("2,0")]
            A_Red,
            [Description("3,0")]
            A_Green,
            [Description("4,0")]
            A_Blue,
            [Description("5,0")]
            A_White2,
            [Description("9,0")]
            B_White1,
            [Description("10,0")]
            B_Red,
            [Description("11,0")]
            B_Green,
            [Description("12,0")]
            B_Blue,
            [Description("13,0")]
            B_White2,
        }
    }
}
