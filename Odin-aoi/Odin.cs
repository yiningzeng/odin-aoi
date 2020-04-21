using Emgu.CV;
using Emgu.CV.Structure;
using Tools;
using power_aoi.DockerPanelOdin;
using power_aoi.Model;
using power_aoi.PopupForm;
using power_aoi.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using System.Threading;
using System.Runtime.InteropServices;
using System.Diagnostics;
using power_aoi.Tools.Hardware;
using ImageProcessor;
using Amib.Threading;
using PylonC.NETSupportLibrary;
using System.Drawing.Imaging;
using Newtonsoft.Json;
using Odin_aoi.Tools;
using static power_aoi.Model.OneStitchSidePcb;

namespace power_aoi
{
    public partial class Odin : DockContent
    {
        #region Instance Fields
        Stopwatch sw = new Stopwatch();
        public delegate int StitchCallBack(bool isEnd, OneStitchSidePcb oneStitchSidePcb, BitmapInfo bitmapInfo, RectangleF scaleRect);
        public StitchCallBack onStitchCallBack;

        public delegate void RabbitmqMessageCallback(string message);
        public RabbitmqMessageCallback onRabbitmqMessageCallback;

        public delegate void RabbitmqConnectCallback(string message);
        public RabbitmqConnectCallback onRabbitmqConnectCallback;

        private DeserializeDockContent m_deserializeDockContent;
        ToolBarForm toolBarForm;
        FrontWorkingForm frontWorkingForm;
        BackWorkingForm backWorkingForm;

        public System.Timers.Timer plcCheckReadyTimer = new System.Timers.Timer(); // plc监听板子到位信息

        Pcb nowPcb;
        int frontCameraNum = 0; // 用于记录正面的单个数量
        int backCameraNum = 0; // 用于记录反面面的单个数量
        int allNum = 0;
        bool isIdle = true;
        private object ALock = new object();
        private object BLock = new object();
        private string cameraAid = INIHelper.Read("CameraA", "SerialNumber", Application.StartupPath + "/config.ini");
        private string cameraBid = INIHelper.Read("CameraB", "SerialNumber", Application.StartupPath + "/config.ini");

        public double kwidth = double.Parse(INIHelper.Read("Kwidth", "Kwidth", Application.StartupPath + "/config.ini"));
        public int fuck = 0;
        public int enterFuck = 0;
        public int errorFuck = 0;

        #endregion

        public Odin()
        {
            InitializeComponent();
            //this.Icon = Properties.Resources.aa;
            CreateStandardControls();
            m_deserializeDockContent = new DeserializeDockContent(GetContentFromPersistString);

            string configFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "DockPanel.config");
            if (File.Exists(configFile))
                dockPanel1.LoadFromXml(configFile, m_deserializeDockContent);
            Ini();
        }

        public void Ini()
        {
            #region 初始化委托
            onStitchCallBack = new StitchCallBack(doStitchCallBack);
            #endregion

            #region 初始化事件
            this.FormClosing += Odin_FormClosing;
            #endregion
            #region 初始化控件

            plcCheckReadyTimer.Interval = 100;
            plcCheckReadyTimer.Elapsed += plcCheckReadyTimer_Elapsed;

            for (int i = 0; i < imageListToolBar.Images.Count; i++)
            {
                Button button = new Button();
                button.Size = imageListToolBar.ImageSize;
                button.BackColor = Color.Transparent;
                button.BackgroundImage = imageListToolBar.Images[i];
                button.BackgroundImageLayout = ImageLayout.Zoom;
                button.FlatStyle = FlatStyle.Flat;
                button.FlatAppearance.BorderColor = Color.DarkGray;
                button.FlatAppearance.BorderSize = 1;
                button.FlatAppearance.MouseDownBackColor = Color.DimGray;
                button.FlatAppearance.MouseOverBackColor = Color.Silver;
                button.Location = new Point(7, 7 + i * (15 + 35));
                button.Click += Button_Click;
                button.Name = imageListToolBar.Images.Keys[i].ToString();
                if (button.Name.Contains("(debug)")) button.Visible = false;
                toolTip.SetToolTip(button, button.Name);
                panelToolBar.Controls.Add(button);
            }
            #endregion
            #region 初始化各页面窗体实例
            toolBarForm.IniForm(this, frontWorkingForm);
            this.SizeChanged += Odin_SizeChanged;
            menuStripControl.MouseDown += MenuStripControl_MouseDown;
            tsmClose.Click += TsmClose_Click;
            tsmMin.Click += TsmMin_Click;
            tsmSquare.Click += TsmSquare_Click;
            #endregion
            dockPanel1.AllowEndUserDocking = false; // 锁定整个布局

        }

        #region Initialization
        //所有相机初始化
        private void Camerasinitialization()
        {
            #region cameraA
            m_imageProvider.GrabErrorEvent += new ImageProvider.GrabErrorEventHandler(OnGrabErrorEventCallback);
            m_imageProvider.DeviceRemovedEvent += new ImageProvider.DeviceRemovedEventHandler(OnDeviceRemovedEventCallback);
            m_imageProvider.DeviceOpenedEvent += new ImageProvider.DeviceOpenedEventHandler(OnDeviceOpenedEventCallback);
            m_imageProvider.DeviceClosedEvent += new ImageProvider.DeviceClosedEventHandler(OnDeviceClosedEventCallback);
            m_imageProvider.GrabbingStartedEvent += new ImageProvider.GrabbingStartedEventHandler(OnGrabbingStartedEventCallback);
            m_imageProvider.ImageReadyEvent += new ImageProvider.ImageReadyEventHandler(OnImageReadyEventCallback);
            m_imageProvider.GrabbingStoppedEvent += new ImageProvider.GrabbingStoppedEventHandler(OnGrabbingStoppedEventCallback);
            m_imageProvider.CameraId = cameraAid;
            uint id = m_imageProvider.GetDevice(cameraAid);
            if (id == 99)
            {
                //MessageBox.Show("未连接相机");
            }
            else
            {
                m_imageProvider.Open(id);
                m_imageProvider.ContinuousShot();
                LogHelper.WriteLog("连接相机A:" + id.ToString());
            }
            #endregion

            #region cameraB
            m_imageProviderB.GrabErrorEvent += new ImageProvider.GrabErrorEventHandler(OnGrabErrorEventCallback);
            m_imageProviderB.DeviceRemovedEvent += new ImageProvider.DeviceRemovedEventHandler(OnDeviceRemovedEventCallbackB);
            m_imageProviderB.DeviceOpenedEvent += new ImageProvider.DeviceOpenedEventHandler(OnDeviceOpenedEventCallback);
            m_imageProviderB.DeviceClosedEvent += new ImageProvider.DeviceClosedEventHandler(OnDeviceClosedEventCallback);
            m_imageProviderB.GrabbingStartedEvent += new ImageProvider.GrabbingStartedEventHandler(OnGrabbingStartedEventCallback);
            m_imageProviderB.ImageReadyEvent += new ImageProvider.ImageReadyEventHandler(OnImageReadyEventCallbackB);
            m_imageProviderB.GrabbingStoppedEvent += new ImageProvider.GrabbingStoppedEventHandler(OnGrabbingStoppedEventCallback);
            m_imageProviderB.CameraId = cameraBid;
            id = m_imageProviderB.GetDevice(cameraBid);
            if (id == 99)
            {
                MessageBox.Show("未连接相机");
            }
            else
            {
                m_imageProviderB.Open(id);
                m_imageProviderB.ContinuousShot();
                LogHelper.WriteLog("连接相机B:" + id.ToString());
            }
            #endregion
        }
        #region 相机使用模块 初始化两个相机
        private ImageProvider m_imageProvider = new ImageProvider(); /* Create one image provider. */
        private ImageProvider m_imageProviderB = new ImageProvider(); /* Create one image provider. */
        /* Stops the image provider and handles exceptions. */
        public void Stop()
        {
            /* Stop the grabbing. */
            try
            {
                m_imageProvider.Stop();
                m_imageProviderB.Stop();
            }
            catch (Exception e) { LogHelper.WriteLog(m_imageProvider.GetLastErrorMessage(), e); }
        }
        public void Stop(string cameraid)
        {
            /* Stop the grabbing. */
            try
            {
                if (cameraid == cameraAid)
                {
                    m_imageProvider.Stop();
                }
                else
                {
                    m_imageProviderB.Stop();
                }

            }
            catch (Exception e)
            {
                LogHelper.WriteLog(m_imageProvider.GetLastErrorMessage(), e);
            }
        }

        /* Closes the image provider and handles exceptions. */
        private void CloseTheImageProvider()
        {
            /* Close the image provider. */
            try
            {
                m_imageProvider.Close();
                m_imageProviderB.Close();
            }
            catch (Exception e)
            {
                LogHelper.WriteLog(m_imageProvider.GetLastErrorMessage(), e);
            }
        }
        /* Closes the image provider and handles exceptions. */
        private void CloseTheImageProvider(string cameraid)
        {
            /* Close the image provider. */
            try
            {
                if (cameraid == cameraAid)
                {
                    m_imageProvider.Close();
                }
                else
                {
                    m_imageProviderB.Close();
                }

            }
            catch (Exception e)
            {
                LogHelper.WriteLog(m_imageProvider.GetLastErrorMessage(), e);
            }
        }

        /* Handles the event related to the occurrence of an error while grabbing proceeds. */
        private void OnGrabErrorEventCallback(Exception grabException, string additionalErrorMessage)
        {
            if (InvokeRequired)
            {
                /* If called from a different thread, we must use the Invoke method to marshal the call to the proper thread. */
                BeginInvoke(new ImageProvider.GrabErrorEventHandler(OnGrabErrorEventCallback), grabException, additionalErrorMessage);
                return;
            }

            LogHelper.WriteLog(additionalErrorMessage, grabException);
        }

        /* Handles the event related to the removal of a currently open device. */
        //A相机方法
        private void OnDeviceRemovedEventCallback()
        {
            if (InvokeRequired)
            {
                /* If called from a different thread, we must use the Invoke method to marshal the call to the proper thread. */
                BeginInvoke(new ImageProvider.DeviceRemovedEventHandler(OnDeviceRemovedEventCallback));
                return;
            }
            /* Stops the grabbing of images. */
            Stop(cameraAid);
            /* Close the image provider. */
            CloseTheImageProvider(cameraAid);
        }
        //B相机方法
        private void OnDeviceRemovedEventCallbackB()
        {
            if (InvokeRequired)
            {
                /* If called from a different thread, we must use the Invoke method to marshal the call to the proper thread. */
                BeginInvoke(new ImageProvider.DeviceRemovedEventHandler(OnDeviceRemovedEventCallbackB));
                return;
            }
            /* Stops the grabbing of images. */
            Stop(cameraBid);
            /* Close the image provider. */
            CloseTheImageProvider(cameraBid);
        }

        /* Handles the event related to a device being open. */
        private void OnDeviceOpenedEventCallback()
        {
            if (InvokeRequired)
            {
                /* If called from a different thread, we must use the Invoke method to marshal the call to the proper thread. */
                BeginInvoke(new ImageProvider.DeviceOpenedEventHandler(OnDeviceOpenedEventCallback));
                return;
            }
        }

        /* Handles the event related to a device being closed. */
        private void OnDeviceClosedEventCallback()
        {
            if (InvokeRequired)
            {
                /* If called from a different thread, we must use the Invoke method to marshal the call to the proper thread. */
                BeginInvoke(new ImageProvider.DeviceClosedEventHandler(OnDeviceClosedEventCallback));
                return;
            }
        }

        /* Handles the event related to the image provider executing grabbing. */
        private void OnGrabbingStartedEventCallback()
        {
            if (InvokeRequired)
            {
                /* If called from a different thread, we must use the Invoke method to marshal the call to the proper thread. */
                BeginInvoke(new ImageProvider.GrabbingStartedEventHandler(OnGrabbingStartedEventCallback));
                return;
            }

        }

        /* Handles the event related to an image having been taken and waiting for processing. A面拍照 */
        private void OnImageReadyEventCallback()
        {
            //if (InvokeRequired)
            //{
            //    /* If called from a different thread, we must use the Invoke method to marshal the call to the proper thread. */
            //    BeginInvoke(new ImageProvider.ImageReadyEventHandler(OnImageReadyEventCallback));
            //    return;
            //}

            try
            {
                /* Acquire the image from the image provider. Only show the latest image. The camera may acquire images faster than images can be displayed*/
                ImageProvider.Image image = m_imageProvider.GetLatestImage();

                /* Check if the image has been removed in the meantime. */
                if (image != null)
                {
                    //String directory = savepath + captureCount.ToString();
                    //if (!Directory.Exists(directory))
                    //{
                    //    Directory.CreateDirectory(directory);
                    //}
                    /* Check if the image is compatible with the currently used bitmap. */
                    Bitmap m_bitmap = null;

                    if (BitmapFactory.IsCompatible(m_bitmap, image.Width, image.Height, image.Color))
                    {
                        /* Update the bitmap with the image data. */
                        BitmapFactory.UpdateBitmap(m_bitmap, image.Buffer, image.Width, image.Height, image.Color);
                        /* To show the new image, request the display control to update itself. */
                    }
                    else /* A new bitmap is required. */
                    {
                        BitmapFactory.CreateBitmap(out m_bitmap, image.Width, image.Height, image.Color);
                        BitmapFactory.UpdateBitmap(m_bitmap, image.Buffer, image.Width, image.Height, image.Color);
                        /* We have to dispose the bitmap after assigning the new one to the display control. */
                        /* Provide the display control with the new bitmap. This action automatically updates the display. */
                    }
                    /* The processing of the image is done. Release the image buffer. */
                    // 
                    m_imageProvider.ReleaseImage();
                    /* The buffer can be used for the next image grabs. */
                    nowPcb.FrontPcb.bitmaps.Enqueue(new BitmapInfo() { name = "F" + frontCameraNum + ".jpg", bitmap = m_bitmap });
                    string path = Path.Combine(nowPcb.FrontPcb.savePath, "camera");
                    if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                    MySmartThreadPool.Instance().QueueWorkItem((bitmap, pa) =>
                    {
                        try
                        {
                            bitmap.Save(pa, ImageFormat.Jpeg);
                        }
                        catch (Exception er)
                        {
                            LogHelper.WriteLog("保存图片出错", er);
                        }
                        lock (nowPcb.FrontPcb)
                        {
                            Aoi.StitchMain(nowPcb.FrontPcb, onStitchCallBack);
                        }
                    }, m_bitmap, Path.Combine(path, "F" + frontCameraNum + ".jpg"));
                    frontCameraNum++;
                }
            }
            catch (Exception e)
            {
                LogHelper.WriteLog(m_imageProvider.GetLastErrorMessage(), e);
            }
        }


        /* Handles the event related to an image having been taken and waiting for processing. B面拍照 */
        private void OnImageReadyEventCallbackB()
        {
            //if (InvokeRequired)
            //{
            //    /* If called from a different thread, we must use the Invoke method to marshal the call to the proper thread. */
            //    BeginInvoke(new ImageProvider.ImageReadyEventHandler(OnImageReadyEventCallbackB));
            //    return;
            //}

            try
            {
                /* Acquire the image from the image provider. Only show the latest image. The camera may acquire images faster than images can be displayed*/
                ImageProvider.Image image = m_imageProviderB.GetLatestImage();

                /* Check if the image has been removed in the meantime. */
                if (image != null)
                {
                    Bitmap m_bitmap = null;
                    /* Check if the image is compatible with the currently used bitmap. */
                    if (BitmapFactory.IsCompatible(m_bitmap, image.Width, image.Height, image.Color))
                    {
                        /* Update the bitmap with the image data. */
                        BitmapFactory.UpdateBitmap(m_bitmap, image.Buffer, image.Width, image.Height, image.Color);
                        /* To show the new image, request the display control to update itself. */
                    }
                    else /* A new bitmap is required. */
                    {
                        BitmapFactory.CreateBitmap(out m_bitmap, image.Width, image.Height, image.Color);
                        BitmapFactory.UpdateBitmap(m_bitmap, image.Buffer, image.Width, image.Height, image.Color);
                    }
                    /* The processing of the image is done. Release the image buffer. */
                    // 
                    m_imageProviderB.ReleaseImage();
                    /* The buffer can be used for the next image grabs. */

                    nowPcb.BackPcb.bitmaps.Enqueue(new BitmapInfo() { name = "B" + frontCameraNum + ".jpg", bitmap = m_bitmap });
                    string path = Path.Combine(nowPcb.FrontPcb.savePath, "camera");
                    if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                    MySmartThreadPool.Instance().QueueWorkItem((bitmap, pa) =>
                    {
                        try
                        {
                            bitmap.Save(pa, ImageFormat.Jpeg);
                        }
                        catch (Exception er)
                        {
                            LogHelper.WriteLog("保存图片出错", er);
                        }
                        lock (nowPcb.BackPcb)
                        {
                            Aoi.StitchMain(nowPcb.BackPcb, onStitchCallBack);
                        }
                    }, m_bitmap, Path.Combine(path, "B" + frontCameraNum + ".jpg"));
                    backCameraNum++;
                }
            }
            catch (Exception e)
            {
                LogHelper.WriteLog(m_imageProviderB.GetLastErrorMessage(), e);
            }
        }

        /* Handles the event related to the image provider having stopped grabbing. */
        private void OnGrabbingStoppedEventCallback()
        {
            if (InvokeRequired)
            {
                /* If called from a different thread, we must use the Invoke method to marshal the call to the proper thread. */
                BeginInvoke(new ImageProvider.GrabbingStoppedEventHandler(OnGrabbingStoppedEventCallback));
                return;
            }

        }

        #endregion

        #region 队列
        public void RabbitmqConnected(string message)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new RabbitmqConnectCallback(RabbitmqConnected), message);
                return;
            }
            LogHelper.WriteLog(message);
        }

        public void doWork(string message)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new RabbitmqMessageCallback(doWork), message);
                return;
            }
            LogHelper.WriteLog("接收到数据\n" + message);
        }
        #endregion

        #region DockPanel
        private IDockContent GetContentFromPersistString(string persistString)
        {
            if (persistString == typeof(FrontWorkingForm).ToString())
            {
                frontWorkingForm = new FrontWorkingForm() { CloseButton = false, CloseButtonVisible = false };
                return frontWorkingForm;
            }
            else if (persistString == typeof(BackWorkingForm).ToString())
            {
                backWorkingForm = new BackWorkingForm() { CloseButton = false, CloseButtonVisible = false };
                return backWorkingForm;
            }
            else if (persistString == typeof(ToolBarForm).ToString())
            {
                toolBarForm = new ToolBarForm() { TabText = "工具箱", CloseButton = false, CloseButtonVisible = false };
                return toolBarForm;
            }
            else return null;
        }
        private void CreateStandardControls()
        {
            frontWorkingForm = new FrontWorkingForm() { TabText = "正面", CloseButton = false, CloseButtonVisible = false };
            toolBarForm = new ToolBarForm() { TabText = "工具箱", CloseButton = false, CloseButtonVisible = false };
            backWorkingForm = new BackWorkingForm() { TabText = "反面", CloseButton = false, CloseButtonVisible = false };
        }
        #endregion
        #endregion

        #region User Event
        /// <summary>
        /// 设置点位和开始拍照
        /// </summary>
        /// <param name="isFront"></param>
        private void run(OneStitchSidePcb oneStitchSidePcb)
        {
            byte[] receiveData = new byte[255];
            byte[] writeValueX = new byte[oneStitchSidePcb.y.Count * 4];
            byte[] writeValueY = new byte[oneStitchSidePcb.y.Count * 4];
            //循环写入点位
            try
            {
                for (int i = 0; i < oneStitchSidePcb.x.Count; i++)
                {
                    if (i % 2 == 1)
                    {
                        int k = 0;
                        for (int n = oneStitchSidePcb.y.Count - 1; n >= 0; n--)
                        {
                            byte[] ibuf = new byte[4];
                            ibuf = Plc.DoubleToByte(oneStitchSidePcb.x[i]);
                            writeValueX[k * 4] = ibuf[0];
                            writeValueX[k * 4 + 1] = ibuf[1];
                            writeValueX[k * 4 + 2] = ibuf[2];
                            writeValueX[k * 4 + 3] = ibuf[3];
                            ibuf = Plc.DoubleToByte(oneStitchSidePcb.y[n]);
                            writeValueY[k * 4] = ibuf[0];
                            writeValueY[k * 4 + 1] = ibuf[1];
                            writeValueY[k * 4 + 2] = ibuf[2];
                            writeValueY[k * 4 + 3] = ibuf[3];
                            k++;
                        }
                    }
                    else
                    {
                        for (int n = 0; n < oneStitchSidePcb.y.Count; n++)
                        {
                            byte[] ibuf = new byte[4];
                            ibuf = Plc.DoubleToByte(oneStitchSidePcb.x[i]);
                            writeValueX[n * 4] = ibuf[0];
                            writeValueX[n * 4 + 1] = ibuf[1];
                            writeValueX[n * 4 + 2] = ibuf[2];
                            writeValueX[n * 4 + 3] = ibuf[3];
                            ibuf = Plc.DoubleToByte(oneStitchSidePcb.y[n]);
                            writeValueY[n * 4] = ibuf[0];
                            writeValueY[n * 4 + 1] = ibuf[1];
                            writeValueY[n * 4 + 2] = ibuf[2];
                            writeValueY[n * 4 + 3] = ibuf[3];
                        }
                    }

                    Plc.WriteData(oneStitchSidePcb.addressX, oneStitchSidePcb.y.Count * 2, writeValueX, receiveData);
                    Plc.WriteData(oneStitchSidePcb.addressY, oneStitchSidePcb.y.Count * 2, writeValueY, receiveData);
                    Plc.WriteData(oneStitchSidePcb.addressCaptureNum, 2, Plc.DoubleToByte(oneStitchSidePcb.y.Count), receiveData); // 设置拍摄数量
                    double value = 1.00;
                    byte[] newwriteValue = new byte[2];
                    newwriteValue[0] = (byte)(value / Math.Pow(256, 1));
                    newwriteValue[1] = (byte)((value / Math.Pow(256, 0)) % 256);
                    //发送开始拍摄信号
                    Plc.WriteData(oneStitchSidePcb.addressStartCapture, 1, newwriteValue, receiveData);
                    //需要等待拍摄完成后，继续发，而不是直接发
                    bool isEnd = false;
                    while (!isEnd)
                    {
                        ////检测拍完信号
                        byte[] newreceiveData = new byte[255]; //{ 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };
                        int num = PLCController.Instance.ReadData(oneStitchSidePcb.addressEndCapture, 2, newreceiveData);
                        double newvalue = newreceiveData[11] * Math.Pow(256, 3) + newreceiveData[12] * Math.Pow(256, 2) + newreceiveData[9] * Math.Pow(256, 1) + newreceiveData[10];
                        if (Utils.DoubleEquals(newvalue, 0.00)) //拍摄完成
                        {
                            while (!isEnd)
                            {
                                ////检测拍完信号
                                newreceiveData = new byte[255]; //{ 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };
                                num = PLCController.Instance.ReadData(oneStitchSidePcb.addressEndCapture, 2, newreceiveData);
                                newvalue = newreceiveData[11] * Math.Pow(256, 3) + newreceiveData[12] * Math.Pow(256, 2) + newreceiveData[9] * Math.Pow(256, 1) + newreceiveData[10];
                                if (Utils.DoubleEquals(newvalue, 1.00)) //拍摄完成
                                {
                                    isEnd = true;
                                    break;
                                }
                                Thread.Sleep(20);
                                //Console.WriteLine("持续检测单行拍摄是否完成");
                            }
                        }
                    }
                }
            }
            catch (Exception exp) { LogHelper.WriteLog("发送位置错误", exp); }
        }

        /// <summary>
        /// 拼图回调函数，用于执行界面更新
        /// </summary>
        /// <param name="end"></param>
        /// <param name="isFront"></param>
        /// <param name="bitmap"></param>
        /// <param name="rect"></param>
        /// <param name="scale"></param>
        /// <returns></returns>
        public int doStitchCallBack(bool isEnd, OneStitchSidePcb oneStitchSidePcb, BitmapInfo bitmapInfo, RectangleF scaleRect)
        {
            //bool end, bool isFront, Bitmap bitmap, RectangleF rect
            if (InvokeRequired)
            {
                // If called from a different thread, we must use the Invoke method to marshal the call to the proper GUI thread.
                // The grab result will be disposed after the event call. Clone the event arguments for marshaling to the GUI thread.
                BeginInvoke(new StitchCallBack(doStitchCallBack), isEnd, oneStitchSidePcb, bitmapInfo, scaleRect);
                return 0;
            }
            #region 
            if (isEnd) // 拍摄结束处理
            {
                #region 直接拍完照片松开，这里会执行2次，但不会叠加松开
                Plc.SetTrackWidth(Convert.ToDouble(kwidth + nowPcb.CarrierWidth * 1562.5) + 1.1 * 1562.5);
                #endregion

                if (oneStitchSidePcb.zTrajectory)
                {
                    frontWorkingForm.BeginInvoke((Action)(() =>
                    {
                        frontWorkingForm.imgBoxWorking.Text += "拍摄完成，已发送出板信息";
                        frontWorkingForm.imgBoxWorking.SelectionRegion = new RectangleF();
                    }));

                    LogHelper.WriteLog("正面拍摄完成,发送出板信息");
                    Plc.APcbOut();
                }
                else
                {
                    backWorkingForm.BeginInvoke((Action)(() =>
                    {
                        backWorkingForm.imgBoxWorking.Text += "拍摄完成，已发送出板信息";
                        backWorkingForm.imgBoxWorking.SelectionRegion = new RectangleF();
                    }));
                    LogHelper.WriteLog("反面拍摄完成,发送出板信息");
                    Plc.BPcbOut();
                }
                this.BeginInvoke((Action)(() =>
                {
                    #region 松开
                    Plc.SetTrackWidth(Convert.ToDouble(kwidth + nowPcb.CarrierWidth * 1562.5) + 1.1 * 1562.5);
                    #endregion
                }));
                return 999;
            }
            else
            {
                Rectangle scaleR = new Rectangle(Convert.ToInt32(scaleRect.X),
                        Convert.ToInt32(scaleRect.Y),
                        Convert.ToInt32(scaleRect.Width),
                        Convert.ToInt32(scaleRect.Height));
                if (oneStitchSidePcb.detectMultiScale)
                {
                    aiDetectWithCrop(oneStitchSidePcb.zTrajectory,
                        bitmapInfo,
                        scaleR,
                        oneStitchSidePcb.scale,
                        oneStitchSidePcb.confidence,
                        oneStitchSidePcb.savePath,
                        oneStitchSidePcb.equalDivision,
                        oneStitchSidePcb.overlap,
                        oneStitchSidePcb.saveCropImg);
                    aiDetect(oneStitchSidePcb.zTrajectory,
                        bitmapInfo,
                        scaleR,
                        oneStitchSidePcb.scale,
                        oneStitchSidePcb.confidence,
                        oneStitchSidePcb.savePath);
                }
                else
                {
                    if (oneStitchSidePcb.equalDivision > 1)
                    {
                        aiDetectWithCrop(oneStitchSidePcb.zTrajectory,
                            bitmapInfo,
                            scaleR,
                            oneStitchSidePcb.scale,
                            oneStitchSidePcb.confidence,
                            oneStitchSidePcb.savePath,
                            oneStitchSidePcb.equalDivision,
                            oneStitchSidePcb.overlap,
                            oneStitchSidePcb.saveCropImg);
                    }
                    else
                    {
                        aiDetect(oneStitchSidePcb.zTrajectory,
                            bitmapInfo,
                            scaleR,
                            oneStitchSidePcb.scale,
                            oneStitchSidePcb.confidence,
                            oneStitchSidePcb.savePath);
                    }
                }

                #region 实时更新界面
                if (oneStitchSidePcb.zTrajectory)
                {
                    frontWorkingForm.BeginInvoke((Action<RectangleF>)((r) =>
                    {
                        frontWorkingForm.imgBoxWorking.SelectionRegion = r;
                        frontWorkingForm.imgBoxWorking.Invalidate();
                    }), scaleRect);
                }
                else
                {
                    backWorkingForm.BeginInvoke((Action<RectangleF>)((r) =>
                    {
                        backWorkingForm.imgBoxWorking.SelectionRegion = r;
                        backWorkingForm.imgBoxWorking.Invalidate();
                    }), scaleRect);
                }
                #endregion
            }
            #endregion

            return 666;
        }


        /// <summary>
        /// AI大图检测
        /// </summary>
        /// <param name="isFront">是否是正面</param>
        /// <param name="bitmapInfo">图片信息</param>
        /// <param name="scaleRect">已经缩放的矩形框在缩放大图的位置</param>
        /// <param name="scale">缩放的尺度</param>
        /// <param name="confidence">置信度</param>
        /// <param name="savePath">图像保存地址</param>
        public void aiDetect(bool isFront, BitmapInfo bitmapInfo, Rectangle scaleRect, double scale, float confidence, string savePath)
        {
            MySmartThreadPool.InstanceSmall().QueueWorkItem((name, bmp) =>
            {
                try
                {
                    bbox_t_container boxlist = new bbox_t_container();
                    using (MemoryStream stream = new MemoryStream())
                    {
                        bmp.Save(stream, ImageFormat.Jpeg);
                        byte[] byteImg = new byte[stream.Length];
                        stream.Seek(0, SeekOrigin.Begin);
                        stream.Read(byteImg, 0, Convert.ToInt32(stream.Length));

                        var ai = LoadBalance.Balance();
                        int n = ai.Detect(byteImg, byteImg.Length, ref boxlist, confidence);
                        if (n == -1) LogHelper.WriteLog("AI调用失败");
                        else
                        {
                            resultJoin(name, isFront, scale, scaleRect, boxlist, ai.names, new Point(0, 0));
                        }
                    }
                }
                catch (Exception er) { LogHelper.WriteLog("检测出错", er); }
                finally
                {
                    bmp.Dispose();
                    allNum++;
                    aiDone(savePath);
                }
                //最总检测的结果还是放在这里发送吧

            }, bitmapInfo.name, (Bitmap)bitmapInfo.bitmap.Clone());
        }

        /// <summary>
        /// AI几等分识别
        /// </summary>
        /// <param name="isFront">是否是正面</param>
        /// <param name="bitmapInfo">图片信息</param>
        /// <param name="scaleRect">已经缩放的矩形框在缩放大图的位置</param>
        /// <param name="scale">缩放的尺度</param>
        /// <param name="confidence">置信度</param>
        /// <param name="savePath">图像保存地址</param>
        /// <param name="equalDivision">等分</param>
        /// <param name="overlap">重叠</param>
        /// <param name="saveCropImg">是否保存裁剪的图片</param>
        public void aiDetectWithCrop(bool isFront, BitmapInfo bitmapInfo, Rectangle scaleRect, double scale, float confidence, string savePath, int equalDivision, int overlap, bool saveCropImg) //有问题，不会处理完
        {
            MySmartThreadPool.InstanceSmall().QueueWorkItem((name, bmp, imgWidth, imgHeight) =>
            {
                Snowflake snowflake = new Snowflake(8);
                Image<Bgr, byte> Sub = new Image<Bgr, byte>(bmp);
                //Sub.Save(Path.Combine(p.savePath, enterFuck + ".jpg"));
                int subWidth, subHeight, subImageNum = equalDivision;
                subWidth = imgWidth / subImageNum + overlap; // 5是重叠像素
                subHeight = imgHeight / subImageNum + overlap;
                int num = 1;
                try
                {
                    // Load, resize, set the format and quality and save an image.
                    Point startPoint = new Point(0, 0);
                    Size partSize = new Size(subWidth, subHeight);
                    for (int i = 0; i < subImageNum; i++) // 我是横行
                    {
                        //Image<Bgr, byte> dst = new Image<Bgr, byte>(Sub.Size);
                        //CvInvoke.cvCopy(Sub, dst.Ptr, IntPtr.Zero);
                        for (int j = 0; j < subImageNum; j++) //我是纵向
                        {
                            string cropImgId = name.Replace(".jpg", "") + "-" + i + "-" + j;// +"("+ snowflake.nextId().ToString()+")";
                            if (startPoint.X + partSize.Width > imgWidth)
                            {
                                partSize = new Size(imgWidth - startPoint.X, partSize.Height);
                            }
                            if (startPoint.Y + partSize.Height > imgHeight)
                            {
                                partSize = new Size(partSize.Width, imgHeight - startPoint.Y);
                            }

                            try
                            {
                                Image<Bgr, byte> fffff = Sub.GetSubRect(new Rectangle(startPoint, partSize));
                                
                                using (MemoryStream stream = new MemoryStream())
                                {
                                    fffff.Bitmap.Save(stream, ImageFormat.Jpeg);
                                    byte[] detectByte = new byte[stream.Length];
                                    stream.Seek(0, SeekOrigin.Begin);
                                    stream.Read(detectByte, 0, Convert.ToInt32(stream.Length));

                                    try
                                    {
                                        #region AI检测
                                        bbox_t_container boxlist = new bbox_t_container();
                                        var ai = LoadBalance.Balance();
                                        int n = ai.Detect(detectByte, detectByte.Length, ref boxlist, confidence);
                                        if (n == -1) LogHelper.WriteLog("AI调用失败");
                                        else
                                        {
                                            resultJoin(cropImgId, isFront, scale, scaleRect, boxlist, ai.names, startPoint);
                                        }
                                        #endregion
                                    }
                                    catch (Exception er)
                                    {
                                        Console.WriteLine(er);
                                    }
                                }
                                if (saveCropImg)
                                {
                                    string path = Path.Combine(savePath, "crop");
                                    if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                                    MySmartThreadPool.Instance().QueueWorkItem((cvImage, fileName) => {
                                        cvImage.Save(fileName);
                                        cvImage.Dispose();
                                    }, fffff, Path.Combine(path, cropImgId + ".jpg"));
                                }
                            }
                            catch (Exception er)
                            {
                                Console.WriteLine(er);
                            }
                            num++;
                            startPoint = new Point(startPoint.X + subWidth, startPoint.Y);
                        }
                        //dst.Dispose();
                        startPoint = new Point(0, startPoint.Y + subHeight);
                    }
                    Sub.Dispose();
                }
                catch (Exception er)
                {
                    Console.WriteLine("errorFuck:" + errorFuck);
                    LogHelper.WriteLog("crop出错", er);
                }
                finally
                {
                    bmp.Dispose();
                    allNum++;
                    aiDone(savePath);
                }
                if (num >= subImageNum * subImageNum) return 1;
                return 0;
            }, bitmapInfo.name, (Bitmap)bitmapInfo.bitmap.Clone(), bitmapInfo.bitmap.Width, bitmapInfo.bitmap.Height);
        }

        public void resultJoin(string imgName, bool isFront, double scale, Rectangle scaleRect, bbox_t_container boxlist, List<string> names, Point startPoint)
        {
            Snowflake snowflake = new Snowflake(2);
            if (boxlist.bboxlist.Length > 0)
            {
                for (int i = 0; i < boxlist.bboxlist.Length; i++)
                {
                    if (boxlist.bboxlist[i].h == 0) break;
                    else
                    {
                        string id = imgName.Replace(".jpg", "") + "(" + snowflake.nextId().ToString() + ")";
                        bbox_t bbox = boxlist.bboxlist[i];
                        bbox.x = (uint)((bbox.x + startPoint.X) * scale) + (uint)scaleRect.X; // + (uint)scaleRect.X;
                        bbox.y = (uint)((bbox.y + startPoint.Y) * scale) + (uint)scaleRect.Y; // + (uint)scaleRect.Y;
                        bbox.w = (uint)(bbox.w * scale);
                        bbox.h = (uint)(bbox.h * scale);
                        Result result = new Result()
                        {
                            Id = id,
                            IsBack = Convert.ToInt32(!isFront),
                            score = bbox.prob,
                            PcbId = nowPcb.Id,
                            Area = "",
                            Region = bbox.x + "," + bbox.y + "," + bbox.w + "," + bbox.h,
                            NgType = names[(int)bbox.obj_id],
                            PartImagePath = id + ".jpg",
                            CreateTime = DateTime.Now,
                        };
                        lock (nowPcb.results)
                        {
                            nowPcb.results.Add(result);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// AI检测结束共用函数
        /// </summary>
        /// <param name="oneStitchSidePcb"></param>
        public void aiDone(string savePath)
        {
            Console.WriteLine(allNum);
            if (allNum == nowPcb.AllPhotoNum)
            {
                this.BeginInvoke((Action)(() =>
                {
                    try
                    {
                        sw.Stop();
                        //获取运行时间[毫秒]  
                        long times = sw.ElapsedMilliseconds / 1000;
                        Console.WriteLine("执行总共使用了, total :" + times + "s 秒");

                        MySmartThreadPool.InstanceSmall().WaitForIdle();
                        Console.WriteLine("InstanceSmall: " + MySmartThreadPool.InstanceSmall().InUseThreads);
                        Console.WriteLine("Instance: " + MySmartThreadPool.Instance().InUseThreads);
                        //这里可以直接发送了！！！！！！
                        //结束计时  
                        //MessageBox.Show("执行查询总共使用了, total :" + times + "s 秒");
                        try
                        {
                            JsonData<Pcb> jsonData = new JsonData<Pcb>();
                            jsonData.data = nowPcb;
                            jsonData.executionTime = times;
                            jsonData.ngNum = nowPcb.results.Count;
                            var jSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
                            string[] props = { "FrontPcb", "BackPcb" }; //排除掉，不能让前端看到的字段。为true的话就是只保留这些字段
                            jSetting.ContractResolver = new LimitPropsContractResolver(props, false);

                            string res = JsonConvert.SerializeObject(jsonData, jSetting);
                            RabbitMQClientHandler.GetInstance(onRabbitmqMessageCallback, onRabbitmqConnectCallback)
                            .TopicExchangePublishMessageToServerAndWaitConfirm("", "work", "work", res);

                            File.WriteAllText(Path.Combine(savePath, "result.json"), Utils.ConvertJsonString(res));
                        }
                        catch (Exception er)
                        {
                            LogHelper.WriteLog("连接队列失败!!!", er);
                        }
                    }
                    catch (Exception er)
                    {

                    }
                    finally
                    {
                        #region 松开
                        Plc.SetTrackWidth(Convert.ToDouble(kwidth + nowPcb.CarrierWidth * 1562.5) + 1.1 * 1562.5);
                        #endregion
                        isIdle = true;
                        frontCameraNum = 0;
                        backCameraNum = 0;
                        allNum = 0;
                    }
                }));
            }
        }

        /// <summary>
        /// 程序退出函数
        /// </summary>
        private void CloseApplication()
        {
            try
            {
                if(Plc.CheckPcbReady())Plc.PcbOut();
                #region 先关闭相机
                this.Stop();
                CloseTheImageProvider();
                #endregion
                LoadBalance.Dispose();
                frontWorkingForm.imgBoxWorking.Dispose();
                backWorkingForm.imgBoxWorking.Dispose();
            }
            catch (Exception er) { }
            finally { Environment.Exit(0); }
        }
        #endregion

        #region Controls Event Handlers
        /// <summary>
        /// 监控板子到位信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void plcCheckReadyTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (Plc.CheckPcbReady() && isIdle) //只有在板子到位了并且空闲的时候进行
            {
                isIdle = false;
                byte[] receiveData = new byte[255];
                LogHelper.WriteLog("-kick off-");
                
                //重置当前pcb的信息
                nowPcb = Pcb.CreatePcb(nowPcb.CarrierLength, nowPcb.CarrierWidth, nowPcb.PcbLength, nowPcb.PcbWidth, nowPcb.SideIndex);

                sw.Restart(); // 重置计时

                #region 轨道设置加紧
                Plc.SetTrackWidth(Convert.ToDouble(kwidth + nowPcb.CarrierWidth * 1562.5) - 1.1 * 1562.5);
                #endregion

                Ftp.MakeDir(Ftp.ftpPath, nowPcb.Id); // Ftp生成当前板子目录

                #region 界面初始化
                frontWorkingForm.BeginInvoke((Action)(() =>
                {
                    frontWorkingForm.Ini();
                    frontWorkingForm.ShowDefaultImage();
                }));
                backWorkingForm.BeginInvoke((Action)(() =>
                {
                    backWorkingForm.Ini();
                    backWorkingForm.ShowDefaultImage();
                }));
                #endregion


                if (nowPcb.FrontPcb == null) Plc.WriteData(5000, 2, Plc.DoubleToByte(0), receiveData); // 设置A面拍摄数量为0
                else
                {
                    string path = Path.Combine(Path.Combine(nowPcb.FrontPcb.savePath, DateTime.Now.ToString("yyyy-MM-dd")), nowPcb.Id);
                    nowPcb.FrontPcb.savePath = path;
                    if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                    MySmartThreadPool.Instance().QueueWorkItem((one) => run(one), nowPcb.FrontPcb);
                }

                if (nowPcb.BackPcb == null) Plc.WriteData(5002, 2, Plc.DoubleToByte(0), receiveData); // 设置A面拍摄数量为0
                else
                {
                    string path = Path.Combine(Path.Combine(nowPcb.BackPcb.savePath, DateTime.Now.ToString("yyyy-MM-dd")), nowPcb.Id);
                    nowPcb.BackPcb.savePath = path;
                    if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                    MySmartThreadPool.Instance().QueueWorkItem((one) => run(one), nowPcb.BackPcb);
                }
            }
        }

        /// <summary>
        /// 整合左侧功能性按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            switch (btn.Name.Replace(".png", ""))
            {
                case "采集":
                    #region 采集程序运行主
                    StartWork startWork = new StartWork();
                    DialogResult dialogResult = startWork.ShowDialog();
                    if (dialogResult == DialogResult.Yes)
                    {
                        isIdle = true;
                        plcCheckReadyTimer.Start();//开启板子到位检测
                        allNum = 0;
                        sw.Restart();
                        nowPcb = startWork.Tag as Pcb;
                        #region 重置轨道宽度
                        Plc.SetTrackWidth(Convert.ToDouble(kwidth + nowPcb.CarrierWidth * 1562.5));
                        #endregion
                        MySmartThreadPool.Instance().QueueWorkItem((p, frontForm, backForm) =>
                        {
                            frontWorkingForm.BeginInvoke((Action)(() =>
                            {
                                frontWorkingForm.Ini();
                                frontWorkingForm.ShowDefaultImage();
                            }));
                            backWorkingForm.BeginInvoke((Action)(() =>
                            {
                                backWorkingForm.Ini();
                                backWorkingForm.ShowDefaultImage();
                            }));
                        }, nowPcb, frontWorkingForm, backWorkingForm);
                    }
                    #endregion
                    break;
                case "编程":
                    #region 编程操作
                    //MessageBox.Show("还在开发中", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                    if (isIdle == false)
                    {
                        MessageBox.Show("程序运行中！请等空闲的时候操作！", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                        return;
                    }
                    MySmartThreadPool.Instance().QueueWorkItem(() =>
                    {
                        frontWorkingForm.BeginInvoke((Action)(() =>
                        {
                            frontWorkingForm.Ini();
                            frontWorkingForm.ShowDefaultImage();
                        }));
                        backWorkingForm.BeginInvoke((Action)(() =>
                        {
                            backWorkingForm.Ini();
                            backWorkingForm.ShowDefaultImage();
                        }));
                    });
                    #endregion
                    break;
                case "手动出板":
                    #region 手动出板
                    DialogResult dr = MessageBox.Show("你确定要手动出板么？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (dr == DialogResult.OK) Plc.PcbOut();
                    #endregion
                    break;
                case "手动松开":
                    #region 松开
                    Plc.SetTrackWidth(Convert.ToDouble(kwidth + nowPcb.CarrierWidth * 1562.5) + 1.1 * 1562.5);
                    #endregion
                    break;
                case "设备复位停止工作":
                    DialogResult drr = MessageBox.Show("你确定要设备复位停止工作？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (drr == DialogResult.OK) Plc.Ini();
                    break;
                case "开发测试按钮(debug)":
                    #region 开发测试，随意添加任何代码来测试
                    //Ftp.MakeDir(Ftp.ftpPath, "asdsdsd");
                    //Ftp.UpLoadFile(@"D:\AoiAssets\2020-04-09\25965986241528832\Back.jpg", Ftp.ftpPath + "asdsdsd/Back.jpg");
                    //Ftp.UploadDirectory(@"D:\AoiAssets\", "2020-04-10");
                    startWork = new StartWork();
                    dialogResult = startWork.ShowDialog();
                    if (dialogResult == DialogResult.Yes)
                    {
                        #region 界面初始化
                        frontWorkingForm.BeginInvoke((Action)(() =>
                        {
                            frontWorkingForm.Ini();
                            frontWorkingForm.ShowDefaultImage();
                        }));
                        backWorkingForm.BeginInvoke((Action)(() =>
                        {
                            backWorkingForm.Ini();
                            backWorkingForm.ShowDefaultImage();
                        }));
                        #endregion

                        //plcCheckReadyTimer.Start();//开启板子到位检测
                        allNum = 0;
                        sw.Restart();
                        nowPcb = startWork.Tag as Pcb;
                        string path = Path.Combine(Path.Combine(nowPcb.FrontPcb.savePath, DateTime.Now.ToString("yyyy-MM-dd")), nowPcb.Id);
                        nowPcb.FrontPcb.savePath = path;
                        Ftp.MakeDir(Ftp.ftpPath, nowPcb.Id); // Ftp生成当前板子目录
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        //JsonData<Pcb> jsonData = new JsonData<Pcb>();
                        //jsonData.data = nowPcb;
                        //jsonData.executionTime = 26;
                        //jsonData.ngNum = nowPcb.results.Count;
                        //var jSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
                        //string[] props = { "FrontPcb", "BackPcb" }; //排除掉，不能让前端看到的字段。为true的话就是只保留这些字段
                        //jSetting.ContractResolver = new LimitPropsContractResolver(props, false);

                        //string res = JsonConvert.SerializeObject(jsonData, jSetting);

                        //RabbitMQClientHandler.GetInstance(onRabbitmqMessageCallback, onRabbitmqConnectCallback)
                        //.TopicExchangePublishMessageToServerAndWaitConfirm("", "work", "work", res);

                        //File.WriteAllText(Path.Combine(nowPcb.FrontPcb.savePath, "result.json"), Utils.ConvertJsonString(res));

                        //MySmartThreadPool.Instance().QueueWorkItem((p, frontForm, backForm) =>
                        //{
                        //    frontWorkingForm.BeginInvoke((Action)(() =>
                        //    {
                        //        frontWorkingForm.Ini();
                        //        frontWorkingForm.ShowDefaultImage();
                        //    }));
                        //    backWorkingForm.BeginInvoke((Action)(() =>
                        //    {
                        //        backWorkingForm.Ini();
                        //        backWorkingForm.ShowDefaultImage();
                        //    }));

                        //    string path = Path.Combine(Path.Combine(nowPcb.FrontPcb.savePath, DateTime.Now.ToString("yyyy-MM-dd")), nowPcb.Id);

                        //    if (!Directory.Exists(path))
                        //    {
                        //        Directory.CreateDirectory(path);
                        //    }
                        for (int i = 0; i <= 59; i++)
                        {
                            Bitmap fBitmap = new Bitmap(Path.Combine(@"D:\Power-Ftp\test", "F" + i + ".jpg"));
                            Bitmap bBitmap = new Bitmap(Path.Combine(@"D:\Power-Ftp\test", "B" + i + ".jpg"));
                            nowPcb.FrontPcb.bitmaps.Enqueue(new BitmapInfo() { name = "F" + i + ".jpg", bitmap = fBitmap });
                            nowPcb.FrontPcb.savePath = path;


                            string path2 = Path.Combine(nowPcb.FrontPcb.savePath, "camera");
                            if (!Directory.Exists(path2)) Directory.CreateDirectory(path2);

                            MySmartThreadPool.Instance().QueueWorkItem((bitmap, pa) =>
                            {
                                bitmap.Save(pa, ImageFormat.Jpeg);
                                lock (nowPcb.FrontPcb)
                                {
                                    Aoi.StitchMain(nowPcb.FrontPcb, onStitchCallBack);
                                }
                            }, fBitmap, Path.Combine(path2, "F" + i + ".jpg"));

                            nowPcb.BackPcb.bitmaps.Enqueue(new BitmapInfo() { name = "B" + i + ".jpg", bitmap = bBitmap });
                            nowPcb.BackPcb.savePath = path;


                            string path23 = Path.Combine(nowPcb.BackPcb.savePath, "camera");
                            if (!Directory.Exists(path23)) Directory.CreateDirectory(path23);

                            MySmartThreadPool.Instance().QueueWorkItem((bitmap, pa) =>
                            {
                                bitmap.Save(pa, ImageFormat.Jpeg);
                                lock (nowPcb.BackPcb)
                                {
                                    Aoi.StitchMain(nowPcb.BackPcb, onStitchCallBack);
                                }
                            }, bBitmap, Path.Combine(path23, "B" + i + ".jpg"));
                        }
                        isIdle = false;
                        //}, nowPcb, frontWorkingForm, backWorkingForm);
                        //开始执行拍照
                    }
                    #endregion
                    break;
                case "模拟一块板子":
                    MySmartThreadPool.InstanceTest().QueueWorkItem(() =>
                    {
                        while (true)
                        {
                            if (isIdle == true)
                            {
                                fuck = 0;
                                MessageBox.Show("板子到位");
                                isIdle = false;
                                //重置当前pcb的信息
                                nowPcb = Pcb.CreatePcb(nowPcb.CarrierLength, nowPcb.CarrierWidth, nowPcb.PcbLength, nowPcb.PcbWidth, nowPcb.SideIndex);
                                Ftp.MakeDir(Ftp.ftpPath, nowPcb.Id); // Ftp生成当前板子目录
                                sw.Restart(); // 重置计时
                                #region 界面初始化
                                frontWorkingForm.BeginInvoke((Action)(() =>
                                {
                                    frontWorkingForm.Ini();
                                    frontWorkingForm.ShowDefaultImage();
                                }));
                                backWorkingForm.BeginInvoke((Action)(() =>
                                {
                                    backWorkingForm.Ini();
                                    backWorkingForm.ShowDefaultImage();
                                }));
                                #endregion

                                string path = Path.Combine(Path.Combine(nowPcb.FrontPcb.savePath, DateTime.Now.ToString("yyyy-MM-dd")), nowPcb.Id);
                                nowPcb.FrontPcb.savePath = path;
                                if (!Directory.Exists(path))
                                {
                                    Directory.CreateDirectory(path);
                                }
                                for (int i = 0; i <= 59; i++)
                                {
                                    Bitmap fBitmap = new Bitmap(Path.Combine(@"D:\Power-Ftp\test", "F" + i + ".jpg"));
                                    Bitmap bBitmap = new Bitmap(Path.Combine(@"D:\Power-Ftp\test", "B" + i + ".jpg"));
                                    nowPcb.FrontPcb.bitmaps.Enqueue(new BitmapInfo() { name = "F" + i + ".jpg", bitmap = fBitmap });
                                    MySmartThreadPool.Instance().QueueWorkItem((bitmap, pa) =>
                                    {
                                        bitmap.Save(pa, ImageFormat.Jpeg);
                                        lock (nowPcb.FrontPcb)
                                        {
                                            Aoi.StitchMain(nowPcb.FrontPcb, onStitchCallBack);
                                        }
                                    }, fBitmap, Path.Combine(Path.Combine(path, "F" + i + ".jpg")));

                                    Thread.Sleep(100);
                                    nowPcb.BackPcb.bitmaps.Enqueue(new BitmapInfo() { name = "B" + i + ".jpg", bitmap = bBitmap });
                                    //nowPcb.BackPcb.savePath = path;

                                    MySmartThreadPool.Instance().QueueWorkItem((bitmap, pa) =>
                                    {
                                        bitmap.Save(pa, ImageFormat.Jpeg);
                                        lock (nowPcb.BackPcb)
                                        {
                                            Aoi.StitchMain(nowPcb.BackPcb, onStitchCallBack);
                                        }
                                    }, bBitmap, Path.Combine(Path.Combine(path, "B" + i + ".jpg")));

                                    Thread.Sleep(50);
                                }
                                break;
                            }
                            Thread.Sleep(100);
                        }
                    });
                    break;
            }
        }

        private void Odin_Load(object sender, EventArgs e)
        {
            onRabbitmqMessageCallback = new RabbitmqMessageCallback(doWork);
            onRabbitmqConnectCallback = new RabbitmqConnectCallback(RabbitmqConnected);

            #region 加载AI
            MessagePopupForm messagePopupForm = new MessagePopupForm();
            DialogResult dialogResult = messagePopupForm.ShowDialog();
            if (dialogResult == DialogResult.No)
            {
                MessageBox.Show("AI初始化失败，需要重启软件才能继续工作！", "提示", MessageBoxButtons.OK);
                CloseApplication();
            }
            #endregion

            #region 初始化PLC
            Plc.Ini();
            #endregion

            #region 初始化相机
            Camerasinitialization();
            #endregion
        }

        /// <summary>
        /// 窗体关闭执行事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Odin_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseApplication();
        }

        /// <summary>
        /// 窗体大小更改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Odin_SizeChanged(object sender, EventArgs e)
        {
            frontWorkingForm.imgBoxWorking.ZoomToFit();
            backWorkingForm.imgBoxWorking.ZoomToFit();
        }

        /// <summary>
        /// 初始化界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IniFormToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolBarForm.DockPanel = null;
            frontWorkingForm.DockPanel = null;
            backWorkingForm.DockPanel = null;
            //new ToolBarForm() { TabText = "我是项目", CloseButton = true, CloseButtonVisible = true }.Show(this.dockPanel1, DockState.Document);
            string configFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Deafult.config");
            if (File.Exists(configFile))
                dockPanel1.LoadFromXml(configFile, m_deserializeDockContent);
        }

        /// <summary>
        /// 保存界面布局
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveFormToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string configFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "DockPanel.config");
            dockPanel1.SaveAsXml(configFile);
        }

        /// <summary>
        /// 窗体放大缩小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TsmSquare_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
            }
        }

        /// <summary>
        /// 窗体最小化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TsmMin_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        /// <summary>
        /// 窗体关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TsmClose_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("程序可能正在运行中，你确定要关闭么？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                this.Close();
            }
        }

        /// <summary>
        /// 用于用户界面拖放的代码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuStripControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, 0xA1, 0x02, 0);
            }
        }

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        #endregion
    }
}
