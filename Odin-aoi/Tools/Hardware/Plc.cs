using power_aoi.Enums;
using power_aoi.PopupForm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace power_aoi.Tools.Hardware
{
    class Plc
    {
        //拍摄点位X和Y方向间隔，当前都为14mm
        public static float capturePointIntervalXInMM = 13.0f;
        public static float capturePointIntervalYInMM = 13.0f;
        //单张拍摄图片对应的物理宽度，当前为17mm
        public static float singleCaptureWidthInMM = 17.0f;
        //最终用于显示的大图每1mm对应的像素个数
        public static float pixelNumPerMM = 30.0f;



        static PlcMessagePopupForm tip;
        static PlcErrorMessagePopupForm errorTip;
        private static int D2000 = 0;

        private static int D2002 = 0;
        private static int D2004 = 0;
        public static System.Timers.Timer plcTimerCheck = new System.Timers.Timer();
        private static PLCController CheckConnection()
        {
            try
            {
                if (PLCController.Instance.IsConnected)
                    return PLCController.Instance;

                string ip = INIHelper.Read("PLC", "ip", Application.StartupPath + "/config.ini");
                int port = INIHelper.ReadInteger("PLC", "port", 502, Application.StartupPath + "/config.ini");
                if (PLCController.Instance.Connection(ip, port))
                {
                    LogHelper.WriteLog("连接成功");
                    return PLCController.Instance;
                }
                else
                {
                    LogHelper.WriteLog("PLC连接失败，请检查配置或PLC状态");
                    if (tip == null)
                    {
                        tip = new PlcMessagePopupForm();
                        DialogResult dialogResult = tip.ShowDialog();
                        if (dialogResult == DialogResult.Cancel)
                        {
                            tip = null;
                        }
                    }
                    //MessageBox.Show("PLC连接失败，请检查配置或PLC状态？", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
            catch(Exception er)
            {
                LogHelper.WriteLog("PLC连接失败", er);
                return null;
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public static void Ini()
        {
            CheckConnection();
            plcTimerCheck.Interval = 200;
            plcTimerCheck.Elapsed += PlcTimerCheck_Elapsed;
            plcTimerCheck.Start();
            if (CheckPcbReady()) PcbOut(); // 当有板子的时候，发送出板信息
            SetSpeed();
            RestValue();
        }

        private static void PlcTimerCheck_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                var plc = CheckConnection();
                if (plc != null)
                {
                    byte[] newreceiveData = new byte[255]; //{ 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };
                    int num = PLCController.Instance.ReadData(1001, 1, newreceiveData);
                    int value = newreceiveData[9] * 256 + newreceiveData[10];
                    string strValue = Convert.ToString(value, 2).PadLeft(16, '0');
                    string numvalue = strValue.Substring(11, 1);
                    if (numvalue == "0")
                    {
                        Electrify();
                        LogHelper.WriteLog("重新写了使能");
                    }
                    DisplayMonitorAlarm();
                }
            }
            catch(Exception er)
            {
                LogHelper.WriteLog("PLC心跳检测失败", er);
            }
        }

        private static void DisplayMonitorAlarm()
        {
            try
            {
                List<double> listValue = new List<double>();
                int length = 2;
                byte[] receiveData = new byte[255]; //{ 1, 2, 3, 4 };
                int num = PLCController.Instance.ReadData(1147, length, receiveData);
                if (num != length * 2 + 9)
                {
                    return;
                }
                if (receiveData[8] != length * 2)
                {
                    return;
                }
                for (int i = 0; i < length; i++)
                {
                    int value = receiveData[i * 2 + 9] * 256 + receiveData[i * 2 + 10];
                    string strValue = Convert.ToString(value, 2).PadLeft(16, '0');
                    for (int j = 15; j >= 0; j--)
                    {
                        listValue.Add(double.Parse(strValue.Substring(j, 1)));
                    }
                }
                for (int k = 0; k < listValue.Count; k++)
                {
                    if (listValue[k] == 1)
                    {
                        string name = Enum.GetName(typeof(HardwareEnum.Alarm), k);

                        if (errorTip == null)
                        {
                            errorTip = new PlcErrorMessagePopupForm();
                            errorTip.showTip(name + "出现报警了，请赶快查看");
                            DialogResult dialogResult = errorTip.ShowDialog();
                            if (dialogResult == DialogResult.Cancel)
                            {
                                errorTip = null;
                            }
                        }
                    }
                }
            }
            catch (Exception exp)
            {
            }
        }

        private static void Electrify()
        {
            try
            {

                int[] registerBitall = { 10, 11, 12, 13, 14, 15 };
                foreach (int i in registerBitall)
                {
                    int registerAddress = 2000;
                    int registerBit = i;
                    int value = 1 << registerBit;

                    int currentValue = 0;

                    byte[] receiveData = new byte[255];
                    if (registerAddress == 2000)
                    {
                        D2000 = value;
                        currentValue = D2000;
                        SendValueToRegister(2000, D2000, receiveData);
                    }
                }
            }
            catch
            {
            }
        }

        private static void SendValueToRegister(int registerAddress, int value, byte[] receiveData)
        {
            try
            {
                byte[] writeValue = new byte[2] { (byte)(value / 256), (byte)(value % 256) };
                //byte[] receiveData = new byte[255];
                var plc = CheckConnection();
                if (plc != null)
                {
                    PLCController.Instance.WriteData(registerAddress, 1, writeValue, receiveData);
                }
            }
            catch (Exception exp)
            {

            }
        }
        // 软重置
        private static void RestValue()
        {
            int[] registerBitall = { 9,10 };
            foreach (int i in registerBitall)
            {
                int registerAddress = 2004;
                int registerBit = i;
                int value = 1 << registerBit;

                int currentValue = 0;

                byte[] receiveData = new byte[255];

                if (registerAddress == 2004)
                {
                    D2004 = value;
                    currentValue = D2004;
                    SendValueToRegister(2004, D2004, receiveData);
                }
            }
            Thread.Sleep(500);
            foreach (int i in registerBitall)
            {
                int registerAddress = 2004;
                int registerBit = i;
                int value = 0 << registerBit;

                int currentValue = 0;

                byte[] receiveData = new byte[255];

                if (registerAddress == 2004)
                {
                    D2004 = value;
                    currentValue = D2004;
                    SendValueToRegister(2004, D2004, receiveData);
                }
            }
        }

        /// <summary>
        /// 设置速度
        /// </summary>
        private static void SetSpeed()
        {
            int[] address = new int[] { 2050, 2052, 2054 };
            for (int i = 0; i < address.Length; i++)
            {
                double value;
                if(i==0||i==1) value = INIHelper.ReadInteger("Runspeed", address[i].ToString(), 50000, Application.StartupPath + "/config.ini");
                else value = INIHelper.ReadInteger("Runspeed", address[i].ToString(), 20000, Application.StartupPath + "/config.ini");
                int registerAddress = address[i];
                byte[] receiveData = new byte[255];
                if (value > 0xfffffff)
                {
                    MessageBox.Show("PLC设置速度超出范围");
                    return;
                }
                byte[] writeValue = new byte[4];
                writeValue = DoubleToByte(value);
                var plc = CheckConnection();
                if (plc != null)
                {
                    PLCController.Instance.WriteData(registerAddress, 2, writeValue, receiveData);
                }
            }
            address = new int[] { 2056 };
            for (int i = 0; i < address.Length; i++)
            {
                int value = INIHelper.ReadInteger("Runspeed", address[i].ToString(), 30000, Application.StartupPath + "/config.ini");
                int registerAddress = address[i];
                byte[] receiveData = new byte[255];
                if (value > 0xffff)
                {
                    MessageBox.Show("PLC设置速度超出范围");
                    return;
                }
                byte[] writeValue = new byte[2];
                writeValue[0] = (byte)(value / Math.Pow(256, 1));
                writeValue[1] = (byte)((value / Math.Pow(256, 0)) % 256);
                var plc = CheckConnection();
                if (plc != null)
                {
                    PLCController.Instance.WriteData(registerAddress, 1, writeValue, receiveData);
                }
            }
        }


        public static void WriteData(int dataAddress, int dataLength, byte[] writeValue, byte[] receiveData)
        {
            var plc = CheckConnection();
            if (plc != null) plc.WriteData(dataAddress, dataLength, writeValue, receiveData);
            Thread.Sleep(10);
        }

        public static int ReadData(int dataStartAddress, int dataLength, byte[] receiveData)
        {
            var plc = CheckConnection();
            if (plc != null) return plc.ReadData(dataStartAddress, dataLength, receiveData);
            return 0;
        }

        /// <summary>
        /// 检测板子到位情况
        /// </summary>
        /// <returns></returns>
        public static bool CheckPcbReady()
        {
            try
            {
                var plc = CheckConnection();
                if (plc != null)
                {
                    byte[] newreceiveData = new byte[255]; //{ 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };
                    int num = PLCController.Instance.ReadData(1131, 2, newreceiveData);
                    //检测板子到位信号
                    double newvalue = newreceiveData[11] * Math.Pow(256, 3) + newreceiveData[12] * Math.Pow(256, 2) + newreceiveData[9] * Math.Pow(256, 1) + newreceiveData[10];
                    if (Utils.DoubleEquals(newvalue, 0.00))
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                return false;
            }
            catch(Exception er)
            {
                return false;
            }
        }

        //轨道宽度设置
        public static void SetTrackWidth(double value)
        {
            try
            {
                //double value = kwidth + Convert.ToDouble(tbWidth.Text) * 1562.5;
                int registerAddress = 2124;
            int wordBit = 32;
            byte[] receiveData = new byte[255];
            if (wordBit == 32)
            {
                if (value > 0xffffffff)
                {
                    MessageBox.Show("超出设置范围");
                    return;
                }
                byte[] writeValue = new byte[4];
                writeValue = DoubleToByte(value);
                if (PLCController.Instance.IsConnected)
                    PLCController.Instance.WriteData(registerAddress, 2, writeValue, receiveData);
            }

                int[] registerBitall = { 5 };
                foreach (int i in registerBitall)
                {
                    registerAddress = 2004;
                    int registerBit = i;
                    int newvalue = 1 << registerBit;
                    int currentValue = 0;
                    receiveData = new byte[255];
                    if (registerAddress == 2004)
                    {
                        D2004 = newvalue;
                        currentValue = D2004;
                        SendValueToRegister(2004, D2004, receiveData);
                    }
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 关闭PLC
        /// </summary>
        public static void Close()
        {
            var plc = CheckConnection();
            if (plc != null) plc.CloseConnection();
        }

        /// <summary>
        /// A面出板
        /// </summary>
        public static void APcbOut()
        {
            byte[] receiveData = new byte[255];
            double thenewvalue = 1.00;
            byte[] thenewwriteValue = new byte[2];
            thenewwriteValue[0] = (byte)(thenewvalue / Math.Pow(256, 1));
            thenewwriteValue[1] = (byte)((thenewvalue / Math.Pow(256, 0)) % 256);
            var plc = CheckConnection();
            if (plc != null)
                plc.WriteData(2145, 1, thenewwriteValue, receiveData);
            Console.Write("A面出板信号");
        }

        /// <summary>
        /// B面出板
        /// </summary>
        public static void BPcbOut()
        {
            byte[] receiveData = new byte[255];
            double thenewvalue = 1.00;
            byte[] thenewwriteValue = new byte[2];
            thenewwriteValue[0] = (byte)(thenewvalue / Math.Pow(256, 1));
            thenewwriteValue[1] = (byte)((thenewvalue / Math.Pow(256, 0)) % 256);
            var plc = CheckConnection();
            if (plc != null)
                plc.WriteData(2147, 1, thenewwriteValue, receiveData);
            Console.Write("B面出板信号");
        }

        /// <summary>
        /// 总出板
        /// </summary>
        public static void PcbOut()
        {
            APcbOut();
            BPcbOut();
        }

        /// <summary>
        /// 转Double 转字节
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] DoubleToByte(double value)
        {
            byte[] obuf = new byte[4];

            if (value < 0)
            {
                value = Math.Abs(value);
                obuf[0] = (byte)~(byte)((value % Math.Pow(256, 2)) / 256);
                obuf[1] = (byte)~(byte)((value % Math.Pow(256, 2)) % 256);
                obuf[2] = (byte)~(byte)(value / Math.Pow(256, 3));
                obuf[3] = (byte)~(byte)((value / Math.Pow(256, 2)) % 256);

                obuf[1] = (byte)((byte)~(byte)((value % Math.Pow(256, 2)) % 256) + 1);
                // obuf[2] = (byte)((byte)~(byte)(value / Math.Pow(256, 3)) + 128);
            }
            else
            {
                obuf[0] = (byte)((value % Math.Pow(256, 2)) / 256);
                obuf[1] = (byte)((value % Math.Pow(256, 2)) % 256);
                obuf[2] = (byte)(value / Math.Pow(256, 3));
                obuf[3] = (byte)((value / Math.Pow(256, 2)) % 256);
            }
            return obuf;
        }
    }
}
