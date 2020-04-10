using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.IO;
using ImageProcessor;
using System.Windows.Forms;

namespace power_aoi.Tools
{
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct bbox_t
    {
        public uint x, y, w, h;       // 缺陷框坐标 定点x,y 宽高w,h
        public float prob;            // 置信度
        public uint obj_id;           // 缺陷id
        public uint track_id;         // 预留，tracking id for video (0 - untracked, 1 - inf - tracked object)
        public uint frames_counter;   // 预留，counter of frames on which the object was detected
        public float x_3d, y_3d, z_3d;// 预留 

    };
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct bbox_t_container
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1000)]
        public bbox_t[] bboxlist;
    };
    public class AiSdkFront
    {

        #region wuran
        [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct FP0
        {
            public int feature;          // 特征类型编号
            public int ks;               // 卷积核尺寸，与图像尺寸成正比
            public float f_lb;           // 特征下限
            public float f_ub;           // 特征上限
            public int a_lb;             // 面积下限
            public int a_ub;             // 面积上限
            public uint n_boxes;     // 输出的bbox个数
        };

        [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct feature_bbox_t_container
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 100)]
            public Rectangle[] bboxlist;
        };

        [DllImport(@"feature_filter_csharp.dll", EntryPoint = "feature_filter_csharp", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int feature_filter_csharp(IntPtr iplImage, ref feature_bbox_t_container f, FP0 fP0);
        #endregion

        public static List<string> names = new List<string>();
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="configurationFilename">配置文件路径</param>
        /// <param name="weightsFilename">权重文件路径，这里注意一定要使用斜杠，不能使用反斜杠</param>
        /// <param name="gpuID">gpuid，不清楚的直接填写0，如果要更改请查阅nvidia-smi</param>
        /// <returns></returns>
        [DllImport(@"ai_cpp_dll_front.dll", EntryPoint = "init", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int init(IntPtr configurationFilename, IntPtr weightsFilename, int gpuID);

        /// <summary>
        /// 通过byte[]来检测
        /// </summary>
        /// <param name="data">图片byte[]</param>
        /// <param name="data_length">长度</param>
        /// <param name="bbox_T_Container">返回结果</param>
        /// <returns>返回-1表示，调用opencv失败</returns>
        [DllImport(@"ai_cpp_dll_front.dll", EntryPoint = "detect_mat", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int detect_opencv_mat(byte[] data, long data_length, ref bbox_t_container bbox_T_Container, float thresh = (float)0.01);

        /// <summary>
        /// 通过图片路径检测
        /// </summary>
        /// <param name="filename">图片路径</param>
        /// <param name="bbox_T_Container">返回结果</param>
        /// <returns></returns>
        [DllImport(@"ai_cpp_dll_front.dll", EntryPoint = "detect_image", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int detect_image_path(string filename, ref bbox_t_container bbox_T_Container, float thresh = (float)0.01);

        /// <summary>
        /// 释放
        /// </summary>
        /// <returns></returns>
        [DllImport(@"ai_cpp_dll_front.dll", EntryPoint = "dispose", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int dispose();

    }

    public class AiSdkBack
    {
        public static List<string> names = new List<string>();
        public static float Confidence = float.Parse(INIHelper.Read("BackAiPars", "confidence", Application.StartupPath + "/config.ini"));
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="configurationFilename">配置文件路径</param>
        /// <param name="weightsFilename">权重文件路径，这里注意一定要使用斜杠，不能使用反斜杠</param>
        /// <param name="gpuID">gpuid，不清楚的直接填写0，如果要更改请查阅nvidia-smi</param>
        /// <returns></returns>
        [DllImport(@"ai_cpp_dll_back.dll", EntryPoint = "init", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        [System.Runtime.ExceptionServices.HandleProcessCorruptedStateExceptions]
        public static extern int init(IntPtr configurationFilename, IntPtr weightsFilename, int gpuID);

        /// <summary>
        /// 通过byte[]来检测
        /// </summary>
        /// <param name="data">图片byte[]</param>
        /// <param name="data_length">长度</param>
        /// <param name="bbox_T_Container">返回结果</param>
        /// <returns>返回-1表示，调用opencv失败</returns>
        [DllImport(@"ai_cpp_dll_back.dll", EntryPoint = "detect_mat", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int detect_opencv_mat(byte[] data, long data_length, ref bbox_t_container bbox_T_Container, float thresh = (float)0.1);

        /// <summary>
        /// 通过图片路径检测
        /// </summary>
        /// <param name="filename">图片路径</param>
        /// <param name="bbox_T_Container">返回结果</param>
        /// <returns></returns>
        [DllImport(@"ai_cpp_dll_back.dll", EntryPoint = "detect_image", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int detect_image_path(string filename, ref bbox_t_container bbox_T_Container, float thresh = (float)0.1);

        /// <summary>
        /// 释放
        /// </summary>
        /// <returns></returns>
        [DllImport(@"ai_cpp_dll_back.dll", EntryPoint = "dispose", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int dispose();
    }
}


