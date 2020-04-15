using power_aoi.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Odin_aoi.Tools
{
    /// <summary>
    /// 实现简单的平滑加权轮询
    /// </summary>
    class LoadBalance
    {
        public class AiServerConfig
        {
            /// <summary>
            /// 初始权重
            /// </summary>
            public int Weight { get; set; }
            /// <summary>
            /// 当前权重
            /// </summary>
            public int Current { get; set; }
            /// <summary>
            /// 加载的ai的序号
            /// </summary>
            public int AiId { get; set; }
            /// <summary>
            /// 对应的names
            /// </summary>
            public List<string> names { get; set; }
            /// <summary>
            /// 检测函数，通过bitmap
            /// </summary>
            /// <param name="data"></param>
            /// <param name="data_length"></param>
            /// <param name="bbox_T_Container"></param>
            /// <param name="thresh"></param>
            /// <returns></returns>
            public int Detect(byte[] data, long data_length, ref bbox_t_container bbox_T_Container, float thresh = (float)0.01)
            {
                lock (this)
                {
                    switch (AiId)
                    {
                        case 1: return AiSdk1.detect_opencv_mat(data, data_length, ref bbox_T_Container, thresh);
                        case 2: return AiSdk2.detect_opencv_mat(data, data_length, ref bbox_T_Container, thresh);
                        case 3: return AiSdk3.detect_opencv_mat(data, data_length, ref bbox_T_Container, thresh);
                        case 4: return AiSdk4.detect_opencv_mat(data, data_length, ref bbox_T_Container, thresh);
                        case 5: return AiSdk5.detect_opencv_mat(data, data_length, ref bbox_T_Container, thresh);
                        case 6: return AiSdk6.detect_opencv_mat(data, data_length, ref bbox_T_Container, thresh);
                        case 7: return AiSdk7.detect_opencv_mat(data, data_length, ref bbox_T_Container, thresh);
                        case 8: return AiSdk8.detect_opencv_mat(data, data_length, ref bbox_T_Container, thresh);
                        case 9: return AiSdk9.detect_opencv_mat(data, data_length, ref bbox_T_Container, thresh);
                        case 10: return AiSdk10.detect_opencv_mat(data, data_length, ref bbox_T_Container, thresh);
                        case 11: return AiSdk11.detect_opencv_mat(data, data_length, ref bbox_T_Container, thresh);
                        case 12: return AiSdk12.detect_opencv_mat(data, data_length, ref bbox_T_Container, thresh);
                        default: return -1;
                    }
                }
            }

            /// <summary>
            /// 检测通过图片路径
            /// </summary>
            /// <param name="filename"></param>
            /// <param name="bbox_T_Container"></param>
            /// <param name="thresh"></param>
            /// <returns></returns>
            public int Detect(string filename, ref bbox_t_container bbox_T_Container, float thresh = (float)0.01)
            {
                lock (this)
                {
                    switch (AiId)
                    {
                        case 1: return AiSdk1.detect_image_path(filename, ref bbox_T_Container, thresh);
                        case 2: return AiSdk2.detect_image_path(filename, ref bbox_T_Container, thresh);
                        case 3: return AiSdk3.detect_image_path(filename, ref bbox_T_Container, thresh);
                        case 4: return AiSdk4.detect_image_path(filename, ref bbox_T_Container, thresh);
                        case 5: return AiSdk5.detect_image_path(filename, ref bbox_T_Container, thresh);
                        case 6: return AiSdk6.detect_image_path(filename, ref bbox_T_Container, thresh);
                        case 7: return AiSdk7.detect_image_path(filename, ref bbox_T_Container, thresh);
                        case 8: return AiSdk8.detect_image_path(filename, ref bbox_T_Container, thresh);
                        case 9: return AiSdk9.detect_image_path(filename, ref bbox_T_Container, thresh);
                        case 10: return AiSdk10.detect_image_path(filename, ref bbox_T_Container, thresh);
                        case 11: return AiSdk11.detect_image_path(filename, ref bbox_T_Container, thresh);
                        case 12: return AiSdk12.detect_image_path(filename, ref bbox_T_Container, thresh);
                        default: return -1;
                    }
                }
            }

            public void Dispose()
            {
                switch (AiId)
                {
                    case 1: AiSdk1.dispose(); break;
                    case 2: AiSdk2.dispose(); break;
                    case 3: AiSdk3.dispose(); break;
                    case 4: AiSdk4.dispose(); break;
                    case 5: AiSdk5.dispose(); break;
                    case 6: AiSdk6.dispose(); break;
                    case 7: AiSdk7.dispose(); break;
                    case 8: AiSdk8.dispose(); break;
                    case 9: AiSdk9.dispose(); break;
                    case 10: AiSdk10.dispose(); break;
                    case 11: AiSdk11.dispose(); break;
                    case 12: AiSdk12.dispose(); break;
                }
            }
        }

        static List<AiServerConfig> aiServerConfigs;

        /// <summary>
        /// 初始化负载
        /// </summary>
        /// <param name="num">总的加载几个</param>
        public static void Ini(int num)
        {
            aiServerConfigs = new List<AiServerConfig>();

            for (int i = 1; i <= num; i++)
            {
                GC.Collect();
                Thread.Sleep(100);
                GC.Collect();
                Thread.Sleep(100);
                IntPtr configurationFile = Marshal.StringToHGlobalAnsi(INIHelper.Read("AiPars" + i, "configurationFile", Application.StartupPath + "/config.ini").Replace("\\\\", "\\").Replace("\\", "/"));
                IntPtr weightsFile = Marshal.StringToHGlobalAnsi(INIHelper.Read("AiPars" + i, "weightsFile", Application.StartupPath + "/config.ini").Replace("\\\\", "\\").Replace("\\", "/"));
                int gpuID = INIHelper.ReadInteger("AiPars" + i, "gpuID", 0, Application.StartupPath + "/config.ini");
                string namesFile = INIHelper.Read("AiPars" + i, "names", Application.StartupPath + "/config.ini");
                GC.Collect();
                Thread.Sleep(100);
                AiServerConfig aiServerConfig = new AiServerConfig { AiId = i, Weight = 5 };
                switch (i)
                {
                    case 1:
                        AiSdk1.init(configurationFile, weightsFile, gpuID);
                        using (StreamReader sr = new StreamReader(namesFile))
                        {
                            while (!sr.EndOfStream)
                            {
                                AiSdk1.names.Add(sr.ReadLine());
                            }
                        }
                        aiServerConfig.names = AiSdk1.names;
                        break;
                    case 2:
                        AiSdk2.init(configurationFile, weightsFile, gpuID);
                        using (StreamReader sr = new StreamReader(namesFile))
                        {
                            while (!sr.EndOfStream)
                            {
                                AiSdk2.names.Add(sr.ReadLine());
                            }
                        }
                        aiServerConfig.names = AiSdk2.names;
                        break;
                    case 3:
                        AiSdk3.init(configurationFile, weightsFile, gpuID);
                        using (StreamReader sr = new StreamReader(namesFile))
                        {
                            while (!sr.EndOfStream)
                            {
                                AiSdk3.names.Add(sr.ReadLine());
                            }
                        }
                        aiServerConfig.names = AiSdk3.names;
                        break;
                    case 4:
                        AiSdk4.init(configurationFile, weightsFile, gpuID);
                        using (StreamReader sr = new StreamReader(namesFile))
                        {
                            while (!sr.EndOfStream)
                            {
                                AiSdk4.names.Add(sr.ReadLine());
                            }
                        }
                        aiServerConfig.names = AiSdk4.names;
                        break;
                    case 5:
                        AiSdk5.init(configurationFile, weightsFile, gpuID);
                        using (StreamReader sr = new StreamReader(namesFile))
                        {
                            while (!sr.EndOfStream)
                            {
                                AiSdk5.names.Add(sr.ReadLine());
                            }
                        }
                        aiServerConfig.names = AiSdk5.names;
                        break;
                    case 6:
                        AiSdk6.init(configurationFile, weightsFile, gpuID);
                        using (StreamReader sr = new StreamReader(namesFile))
                        {
                            while (!sr.EndOfStream)
                            {
                                AiSdk6.names.Add(sr.ReadLine());
                            }
                        }
                        aiServerConfig.names = AiSdk6.names;
                        break;
                    case 7:
                        AiSdk7.init(configurationFile, weightsFile, gpuID);
                        using (StreamReader sr = new StreamReader(namesFile))
                        {
                            while (!sr.EndOfStream)
                            {
                                AiSdk7.names.Add(sr.ReadLine());
                            }
                        }
                        aiServerConfig.names = AiSdk7.names;
                        break;
                    case 8:
                        AiSdk8.init(configurationFile, weightsFile, gpuID);
                        using (StreamReader sr = new StreamReader(namesFile))
                        {
                            while (!sr.EndOfStream)
                            {
                                AiSdk8.names.Add(sr.ReadLine());
                            }
                        }
                        aiServerConfig.names = AiSdk8.names;
                        break;
                    case 9:
                        AiSdk9.init(configurationFile, weightsFile, gpuID);
                        using (StreamReader sr = new StreamReader(namesFile))
                        {
                            while (!sr.EndOfStream)
                            {
                                AiSdk9.names.Add(sr.ReadLine());
                            }
                        }
                        aiServerConfig.names = AiSdk9.names;
                        break;
                    case 10:
                        AiSdk10.init(configurationFile, weightsFile, gpuID);
                        using (StreamReader sr = new StreamReader(namesFile))
                        {
                            while (!sr.EndOfStream)
                            {
                                AiSdk10.names.Add(sr.ReadLine());
                            }
                        }
                        aiServerConfig.names = AiSdk10.names;
                        break;
                    case 11:
                        AiSdk11.init(configurationFile, weightsFile, gpuID);
                        using (StreamReader sr = new StreamReader(namesFile))
                        {
                            while (!sr.EndOfStream)
                            {
                                AiSdk11.names.Add(sr.ReadLine());
                            }
                        }
                        aiServerConfig.names = AiSdk11.names;
                        break;
                    case 12:
                        AiSdk12.init(configurationFile, weightsFile, gpuID);
                        using (StreamReader sr = new StreamReader(namesFile))
                        {
                            while (!sr.EndOfStream)
                            {
                                AiSdk12.names.Add(sr.ReadLine());
                            }
                        }
                        aiServerConfig.names = AiSdk12.names;
                        break;
                }
                aiServerConfigs.Add(aiServerConfig);
            };
        }

        public static void Dispose()
        {
            foreach(var i in aiServerConfigs)
            {
                i.Dispose();
            }
        }

        public static AiServerConfig Balance()
        {
            int index = -1;
            int total = 0;
            int size = aiServerConfigs.Count();

            for (int i = 0; i < size; i++)
            {
                aiServerConfigs[i].Current += aiServerConfigs[i].Weight;
                total += aiServerConfigs[i].Weight;

                if (index == -1 || aiServerConfigs[index].Current < aiServerConfigs[i].Current)
                {
                    index = i;
                }
            }
            aiServerConfigs[index].Current -= total;
            return aiServerConfigs[index];
        }
    }
}
