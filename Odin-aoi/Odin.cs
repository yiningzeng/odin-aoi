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

namespace power_aoi
{
    public partial class Odin : DockContent
    {
        #region Instance Fields
        Stopwatch sw = new Stopwatch();
        public delegate int StitchCallBack(bool isEnd, OneStitchSidePcb oneStitchSidePcb, Bitmap bitmap,  RectangleF scaleRect);
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

        }

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
                //Thread.Sleep(100);
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
                //Thread.Sleep(100);
                m_imageProviderB.ContinuousShot();
                LogHelper.WriteLog("连接相机B:" + id.ToString());
            }
            #endregion
        }

        /// <summary>
        /// AI检测结束共用函数
        /// </summary>
        /// <param name="oneStitchSidePcb"></param>
        public void aiDone(OneStitchSidePcb oneStitchSidePcb)
        {
            Console.WriteLine(allNum);
            if (allNum == oneStitchSidePcb.AllNum * 2) // 暂时这么写！
            {
                this.BeginInvoke((Action)(() =>
                {
                    try
                    {
                        MySmartThreadPool.InstanceSmall().WaitForIdle();
                        MySmartThreadPool.Instance().WaitForIdle();
                        Console.WriteLine("InstanceSmall: " + MySmartThreadPool.InstanceSmall().InUseThreads);
                        Console.WriteLine("Instance: " + MySmartThreadPool.Instance().InUseThreads);
                        //这里可以直接发送了！！！！！！
                        //结束计时  
                        sw.Stop();
                        //获取运行时间[毫秒]  
                        long times = sw.ElapsedMilliseconds / 1000;
                        Console.WriteLine("执行总共使用了, total :" + times + "s 秒");
                        //MessageBox.Show("执行查询总共使用了, total :" + times + "s 秒");
                        MySmartThreadPool.Instance().QueueWorkItem(() =>
                        {
                            Ftp.MakeDir(Ftp.ftpPath, nowPcb.Id);
                            string localPath = "";
                            if (nowPcb.FrontPcb != null)
                            {
                                localPath = nowPcb.FrontPcb.savePath;
                            }
                            else if (nowPcb.BackPcb != null)
                            {
                                localPath = nowPcb.BackPcb.savePath;
                            }
                            Ftp.UpLoadFile(localPath + "/front.jpg", Ftp.ftpPath + nowPcb.Id + "/front.jpg");
                            Ftp.UpLoadFile(localPath + "/back.jpg", Ftp.ftpPath + nowPcb.Id + "/back.jpg");
                            try
                            {
                                JsonData<Pcb> jsonData = new JsonData<Pcb>();
                                jsonData.data = nowPcb;

                                var jSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
                                string[] props = { "FrontPcb", "BackPcb" }; //排除掉，不能让前端看到的字段。为true的话就是只保留这些字段
                                jSetting.ContractResolver = new LimitPropsContractResolver(props, false);

                                string res = JsonConvert.SerializeObject(jsonData, jSetting);
                                LogHelper.WriteLog(res);
                                RabbitMQClientHandler.GetInstance(onRabbitmqMessageCallback, onRabbitmqConnectCallback)
                                .TopicExchangePublishMessageToServerAndWaitConfirm("", "work", "work", res);
                            }
                            catch (Exception er)
                            {
                                LogHelper.WriteLog("连接队列失败!!!", er);
                            }
                        });
                    }
                    catch (Exception er)
                    {

                    }
                    finally
                    {
                        isIdle = true;
                        frontCameraNum = 0;
                        backCameraNum = 0;
                        allNum = 0;

                        nowPcb.BackPcb.currentRow = 0;
                        nowPcb.BackPcb.currentCol = 0;
                        nowPcb.BackPcb.currentRow = 0;
                        nowPcb.BackPcb.dst = null;
                        nowPcb.BackPcb.roi = new Rectangle();
                        nowPcb.BackPcb.stitchEnd = false;
                        nowPcb.BackPcb.savePath = INIHelper.Read("BaseConfig", "SavePath", Application.StartupPath + "/config.ini");

                        nowPcb.FrontPcb.currentRow = 0;
                        nowPcb.FrontPcb.currentCol = 0;
                        nowPcb.FrontPcb.currentRow = 0;
                        nowPcb.FrontPcb.dst = null;
                        nowPcb.FrontPcb.roi = new Rectangle();
                        nowPcb.FrontPcb.stitchEnd = false;
                        nowPcb.FrontPcb.savePath = INIHelper.Read("BaseConfig", "SavePath", Application.StartupPath + "/config.ini");
                    }
                }));
            }
        }

        /// <summary>
        /// 图片等分函数,并实时检测
        /// 有问题，部分检测会报错可能是截取后释放的元婴
        /// </summary>
        /// <param name="imgPath">图片路径</param>
        /// <param name="subWidth">等分的宽</param>
        /// <param name="subHeight">等分的高</param>
        /// <param name="subImageNum">宽高几等分</param>
        /// <param name="bbox_s">缺陷列表</param>
        public void aiDetectWithCrop(OneStitchSidePcb oneStitchSidePcb, Bitmap bitmap) //有问题，不会处理完
            //(bool isFront, byte[] photoBytes, int subWidth, int subHeight, int subImageNum, int parentX, int parentY)
        {
            MySmartThreadPool.InstanceSmall().QueueWorkItem((p, bmp) =>
            {
                byte[] photoBytes = Utils.Bitmap2Byte(bitmap);

                int subWidth, subHeight, subImageNum = p.equalDivision;
                subWidth = bitmap.Width / subImageNum + 5; // 5是重叠像素
                subHeight = bitmap.Height / subImageNum + 5;
                Snowflake snowflake = new Snowflake(3);
                int num = 1;
                try
                {
                    using (MemoryStream inStream = new MemoryStream(photoBytes))
                    {
                        // Initialize the ImageFactory using the overload to preserve EXIF metadata.
                        using (ImageFactory imageFactory = new ImageFactory(preserveExifData: true))
                        {
                            imageFactory.Load(inStream);
                            // Load, resize, set the format and quality and save an image.
                            Point startPoint = new Point(0, 0);
                            Size partSize = new Size(subWidth, subHeight);

                            for (int i = 0; i < subImageNum; i++) // 我是横行
                            {
                                for (int j = 0; j < subImageNum; j++) //我是纵向
                                {
                                    Console.WriteLine("i: " + i + " , j: " + j);
                                    using (MemoryStream outStream = new MemoryStream())
                                    {
                                        imageFactory.Crop(new Rectangle(startPoint, partSize))
                                                    .Save(outStream);
                                        Image.FromStream(outStream).Save(Path.Combine(@"D:\SavedPerCameraImages\crop", snowflake.nextId() + ".jpg"));

                                        #region AI检测
                                        //bbox_t_container boxlist = new bbox_t_container();
                                        ////byte[] detectByte = Utils.StreamToBytes(outStream);
                                        //byte[] detectByte = outStream.ToArray();
                                        //imageFactory.Dispose();
                                        //inStream.Dispose();
                                        //outStream.Dispose();
                                        //if (p.zTrajectory)
                                        //{
                                        //    int n = -1;
                                        //    lock (ALock)
                                        //    {
                                        //        try
                                        //        {
                                        //            n = AiSdkFront.detect_opencv_mat(detectByte, detectByte.Length, ref boxlist, p.confidence);
                                        //        }
                                        //        catch (Exception er)
                                        //        {
                                        //            LogHelper.WriteLog("正面AI调用失败", er);
                                        //        }
                                        //    }
                                        //    if (n == -1)
                                        //    {
                                        //        LogHelper.WriteLog("正面AI调用失败");
                                        //    }
                                        //}
                                        //else
                                        //{
                                        //    int n = -1;
                                        //    lock (BLock)
                                        //    {
                                        //        try
                                        //        {
                                        //            n = AiSdkBack.detect_opencv_mat(detectByte, detectByte.Length, ref boxlist, p.confidence);
                                        //        }
                                        //        catch (Exception er)
                                        //        {
                                        //            LogHelper.WriteLog("反面AI调用失败", er);
                                        //        }
                                        //    }
                                        //    if (n == -1)
                                        //    {
                                        //        LogHelper.WriteLog("反面AI调用失败");
                                        //    }
                                        //}
                                        //if (boxlist.bboxlist.Length > 0)
                                        //{

                                        //    for (int f = 0; f < boxlist.bboxlist.Length; f++)
                                        //    {
                                        //        if (boxlist.bboxlist[f].h == 0)
                                        //        {
                                        //            break;
                                        //        }
                                        //        else
                                        //        {
                                        //            string id = snowflake.nextId().ToString();
                                        //            bbox_t bbox = boxlist.bboxlist[f];
                                        //            bbox.x = bbox.x + (uint)startPoint.X + (uint)p.roi.X;
                                        //            bbox.y = bbox.y + (uint)startPoint.Y + (uint)p.roi.Y;

                                        //            lock (nowPcb.results)
                                        //            {
                                        //                nowPcb.results.Add(new Result()
                                        //                {
                                        //                    Id = id,
                                        //                    IsBack = Convert.ToInt32(!p.zTrajectory),
                                        //                    score = bbox.prob,
                                        //                    PcbId = nowPcb.Id,
                                        //                    Area = "",
                                        //                    Region = "这里面积要重新整过",
                                        //                    NgType = AiSdkFront.names[(int)bbox.obj_id],
                                        //                    PartImagePath = id + ".jpg",
                                        //                    CreateTime = DateTime.Now
                                        //                });
                                        //            }
                                        //        }
                                        //    }
                                        //}
                                        #endregion
                                    }
                                    num++;
                                    startPoint = new Point(startPoint.X + subWidth, startPoint.Y);
                                }
                                startPoint = new Point(0, startPoint.Y + subHeight);
                            }
                        }
                        photoBytes = null;
                    }
                }
                catch (Exception er)
                {
                    LogHelper.WriteLog("crop出错", er);
                }
                finally
                {
                    bmp.Dispose();
                    allNum++;
                    aiDone(oneStitchSidePcb);
                }
                if (num >= subImageNum * subImageNum) return 1;
                return 0;
            }, oneStitchSidePcb, bitmap);
        }
        
        /// <summary>
        /// AI大图检测
        /// </summary>
        /// <param name="oneStitchSidePcb"></param>
        /// <param name="bitmap"></param>
        public void aiDetect(OneStitchSidePcb oneStitchSidePcb, Bitmap bitmap)
        {
            MySmartThreadPool.InstanceSmall().QueueWorkItem((p, bmp) =>
            {
                Snowflake snowflake = new Snowflake(2);
                bbox_t_container boxlist = new bbox_t_container();
                byte[] byteImg = Utils.Bitmap2Byte(bitmap);
                if (p.zTrajectory)
                {
                    int n = -1;
                    lock (ALock)
                    {
                        n = AiSdkFront.detect_opencv_mat(byteImg, byteImg.Length, ref boxlist);
                    }
                    if (n == -1)
                    {
                        LogHelper.WriteLog("正面AI调用失败");
                    }
                }
                else
                {
                    int n = -1;
                    lock (BLock)
                    {
                        n = AiSdkBack.detect_opencv_mat(byteImg, byteImg.Length, ref boxlist);
                    }
                    if (n == -1)
                    {
                        LogHelper.WriteLog("正面AI调用失败");
                    }
                }
                if (boxlist.bboxlist.Length > 0)
                {
                    for (int i = 0; i < boxlist.bboxlist.Length; i++)
                    {
                        if (boxlist.bboxlist[i].h == 0)
                        {
                            break;
                        }
                        else
                        {
                            string id = snowflake.nextId().ToString();
                            bbox_t bbox = boxlist.bboxlist[i];
                            bbox.x = bbox.x + (uint)p.roi.X;
                            bbox.y = bbox.y + (uint)p.roi.Y;
                            lock (nowPcb.results)
                            {
                                nowPcb.results.Add(new Result()
                                {
                                    Id = id,
                                    IsBack = Convert.ToInt32(!p.zTrajectory),
                                    score = bbox.prob,
                                    PcbId = nowPcb.Id,
                                    Area = "",
                                    Region = bbox.x * oneStitchSidePcb.scale + "," + bbox.y * oneStitchSidePcb.scale + "," + bbox.w * oneStitchSidePcb.scale + "," + bbox.h * oneStitchSidePcb.scale,
                                    NgType = AiSdkFront.names[(int)bbox.obj_id],
                                    PartImagePath = id + ".jpg",
                                    CreateTime = DateTime.Now
                                });
                            }
                        }
                    }
                }
                bmp.Dispose();
                allNum++;
                aiDone(oneStitchSidePcb);
                //最总检测的结果还是放在这里发送吧

            }, oneStitchSidePcb, bitmap);
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
        public int doStitchCallBack(bool isEnd, OneStitchSidePcb oneStitchSidePcb, Bitmap bitmap, RectangleF scaleRect)
        {
            //bool end, bool isFront, Bitmap bitmap, RectangleF rect
            //if (InvokeRequired)
            //{
            //    // If called from a different thread, we must use the Invoke method to marshal the call to the proper GUI thread.
            //    // The grab result will be disposed after the event call. Clone the event arguments for marshaling to the GUI thread.
            //    BeginInvoke(new StitchCallBack(doStitchCallBack), isEnd, oneStitchSidePcb, bitmap, scaleRect);
            //    return 0;
            //}
            #region 
            if (isEnd) // 拍摄结束处理
            { 
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
                    LogHelper.WriteLog("正面拍摄完成,发送出板信息");
                    Plc.BPcbOut();
                }
                //this.BeginInvoke((Action)(() =>
                //{
                //    plcFrontCaptureStatusTimer.Start();
                //    plcBackCaptureStatusTimer.Start();
                //}));
                return 999;
            }
            else
            {
                if(oneStitchSidePcb.equalDivision > 1)
                {
                    aiDetectWithCrop(oneStitchSidePcb, bitmap);
                }
                else
                {
                    aiDetect(oneStitchSidePcb, bitmap);
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
        /// 监控板子到位信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void plcCheckReadyTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (Plc.CheckPcbReady() && isIdle) //只有在板子到位了并且空闲的时候进行
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

                isIdle = false;
                byte[] receiveData = new byte[255];
                LogHelper.WriteLog("板子到位并且空闲，开始工作");
                if (nowPcb.FrontPcb == null)
                {
                    Plc.WriteData(5000, 2, Plc.DoubleToByte(0), receiveData); // 设置A面拍摄数量为0
                }
                else
                {
                    string path = Path.Combine(Path.Combine(nowPcb.FrontPcb.savePath, DateTime.Now.ToString("yyyy-MM-dd")), nowPcb.Id);
                    if (!Directory.Exists(path))
                    {
                        nowPcb.FrontPcb.savePath = path;
                        Directory.CreateDirectory(path);
                    }
                    MySmartThreadPool.Instance().QueueWorkItem((one) =>
                    {
                        run(one);
                    }, nowPcb.FrontPcb);
                }

                if (nowPcb.BackPcb == null)
                {
                    Plc.WriteData(5002, 2, Plc.DoubleToByte(0), receiveData); // 设置A面拍摄数量为0
                }
                else
                {
                    string path = Path.Combine(Path.Combine(nowPcb.BackPcb.savePath, DateTime.Now.ToString("yyyy-MM-dd")), nowPcb.Id);
                    if (!Directory.Exists(path))
                    {
                        nowPcb.BackPcb.savePath = path;
                        Directory.CreateDirectory(path);
                    }
                    MySmartThreadPool.Instance().QueueWorkItem((one) =>
                    {
                        run(one);
                    }, nowPcb.BackPcb);
                }
            }
        }

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

                            //Thread.Sleep(50);
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
                            //Thread.Sleep(50);
                            ibuf = Plc.DoubleToByte(oneStitchSidePcb.y[n]);
                            writeValueY[n * 4] = ibuf[0];
                            writeValueY[n * 4 + 1] = ibuf[1];
                            writeValueY[n * 4 + 2] = ibuf[2];
                            writeValueY[n * 4 + 3] = ibuf[3];
                        }
                    }

                    Plc.WriteData(oneStitchSidePcb.addressX, oneStitchSidePcb.y.Count * 2, writeValueX, receiveData);
                    Thread.Sleep(10);
                    Plc.WriteData(oneStitchSidePcb.addressY, oneStitchSidePcb.y.Count * 2, writeValueY, receiveData);
                    Thread.Sleep(10);
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
                                Console.WriteLine("持续检测单行拍摄是否完成");
                            }
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                LogHelper.WriteLog("发送位置错误", exp);
            }
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
            catch (Exception e)
            {
                LogHelper.WriteLog(m_imageProvider.GetLastErrorMessage(), e);
            }
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
            if (InvokeRequired)
            {
                /* If called from a different thread, we must use the Invoke method to marshal the call to the proper thread. */
                BeginInvoke(new ImageProvider.ImageReadyEventHandler(OnImageReadyEventCallback));
                return;
            }

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
                    nowPcb.FrontPcb.bitmaps.Enqueue(m_bitmap);
                    MySmartThreadPool.Instance().QueueWorkItem((bitmap, pa) =>
                    {
                        bitmap.Save(pa, ImageFormat.Jpeg);
                        lock (nowPcb.FrontPcb)
                        {
                            Aoi.StitchMain(nowPcb.FrontPcb, onStitchCallBack);
                        }
                    }, m_bitmap, Path.Combine(Path.Combine(nowPcb.FrontPcb.savePath, "F" + frontCameraNum + ".jpg")));
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
            if (InvokeRequired)
            {
                /* If called from a different thread, we must use the Invoke method to marshal the call to the proper thread. */
                BeginInvoke(new ImageProvider.ImageReadyEventHandler(OnImageReadyEventCallbackB));
                return;
            }

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

                    nowPcb.BackPcb.bitmaps.Enqueue(m_bitmap);
                    MySmartThreadPool.Instance().QueueWorkItem((bitmap, pa) =>
                    {
                        bitmap.Save(pa, ImageFormat.Jpeg);
                        lock (nowPcb.BackPcb)
                        {
                            Aoi.StitchMain(nowPcb.BackPcb, onStitchCallBack);
                        }
                    }, m_bitmap, Path.Combine(Path.Combine(nowPcb.FrontPcb.savePath, "B" + backCameraNum + ".jpg")));
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
                // If called from a different thread, we must use the Invoke method to marshal the call to the proper GUI thread.
                // The grab result will be disposed after the event call. Clone the event arguments for marshaling to the GUI thread.
                BeginInvoke(new RabbitmqConnectCallback(RabbitmqConnected), message);
                return;
            }
            LogHelper.WriteLog(message);
        }

        public void doWork(string message)
        {
            if (InvokeRequired)
            {
                // If called from a different thread, we must use the Invoke method to marshal the call to the proper GUI thread.
                // The grab result will be disposed after the event call. Clone the event arguments for marshaling to the GUI thread.

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
            else if(persistString == typeof(ToolBarForm).ToString())
            {
                toolBarForm = new ToolBarForm() { TabText = "工具箱", CloseButton = false, CloseButtonVisible = false };
                return toolBarForm;
            }
            else return null;
        }


        private void CreateStandardControls()
        {
            frontWorkingForm = new FrontWorkingForm() { TabText="正面", CloseButton = false, CloseButtonVisible = false };
            toolBarForm = new ToolBarForm() { TabText = "工具箱", CloseButton = false, CloseButtonVisible = false };
            backWorkingForm = new BackWorkingForm() { TabText = "反面", CloseButton = false, CloseButtonVisible = false };
        }
        #endregion
        #region Event Handlers

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
            //MySmartThreadPool.Instance().QueueWorkItem(() =>
            //{

            //});
            #endregion

            #region 初始化PLC
            Plc.Ini();
            #endregion

            #region 初始化相机
            Camerasinitialization();
            #endregion
        }

        private void Button_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            switch (btn.Name.Replace(".png", ""))
            {
                case "采集":
                    StartWork startWork = new StartWork();
                    DialogResult dialogResult = startWork.ShowDialog();
                    if (dialogResult == DialogResult.Yes)
                    {
                        isIdle = true;
                        plcCheckReadyTimer.Start();//开启板子到位检测
                        allNum = 0;
                        sw.Restart();
                        nowPcb = startWork.Tag as Pcb;

                        #region 设置一下轨道宽度
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
                        //开始执行拍照
                    }
                    break;
                case "编程":
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
                    break;
                case "手动出板":
                    DialogResult dr = MessageBox.Show("你确定要手动出板么？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (dr == DialogResult.OK)
                    {
                        Plc.PcbOut();
                    }
                    break;
                case "开发测试按钮":
                    //Ftp.MakeDir(Ftp.ftpPath, "asdsdsd");
                    //Ftp.UpLoadFile(@"D:\AoiAssets\2020-04-09\25965986241528832\Back.jpg", Ftp.ftpPath + "asdsdsd/Back.jpg");
                    //Ftp.UploadDirectory(@"D:\AoiAssets\", "2020-04-10");
                    startWork = new StartWork();
                    dialogResult = startWork.ShowDialog();
                    if (dialogResult == DialogResult.Yes)
                    {
                        isIdle = true;
                        plcCheckReadyTimer.Start();//开启板子到位检测
                        allNum = 0;
                        sw.Restart();
                        nowPcb = startWork.Tag as Pcb;


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

                            string path = Path.Combine(Path.Combine(nowPcb.FrontPcb.savePath, DateTime.Now.ToString("yyyy-MM-dd")), nowPcb.Id);

                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }
                            for (int i = 0; i <= 59; i++)
                            {
                                Bitmap fBitmap = new Bitmap(Path.Combine(@"C:\Users\Administrator\Desktop\suomi-test-img\764-Ng", "F" + i + ".jpg"));
                                Bitmap bBitmap = new Bitmap(Path.Combine(@"C:\Users\Administrator\Desktop\suomi-test-img\764-Ng", "B" + i + ".jpg"));
                                nowPcb.FrontPcb.bitmaps.Enqueue(fBitmap);
                                nowPcb.BackPcb.bitmaps.Enqueue(bBitmap);
                                nowPcb.FrontPcb.savePath = path;
                                nowPcb.BackPcb.savePath = path;
                                MySmartThreadPool.Instance().QueueWorkItem((bitmap, pa) =>
                                {
                                    bitmap.Save(pa, ImageFormat.Jpeg);
                                    lock (nowPcb.FrontPcb)
                                    {
                                        Aoi.StitchMain(nowPcb.FrontPcb, onStitchCallBack);
                                    }
                                }, fBitmap, Path.Combine(Path.Combine(path, "F" + i + ".jpg")));

                                MySmartThreadPool.Instance().QueueWorkItem((bitmap, pa) =>
                                {
                                    bitmap.Save(pa, ImageFormat.Jpeg);
                                    lock (nowPcb.BackPcb)
                                    {
                                        Aoi.StitchMain(nowPcb.BackPcb, onStitchCallBack);
                                    }
                                }, bBitmap, Path.Combine(Path.Combine(path, "B" + i + ".jpg")));
                            }

                        }, nowPcb, frontWorkingForm, backWorkingForm);
                        //开始执行拍照
                    }

                    break;
            }
        }

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

        private void SaveFormToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string configFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "DockPanel.config");
            dockPanel1.SaveAsXml(configFile);
        }

        private void TsmSquare_Click(object sender, EventArgs e)
        {
            if(this.WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
            }
        }

        private void TsmMin_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void TsmClose_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("程序可能正在运行中，你确定要关闭么？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                this.Close();
            }
        }

        private void CloseApplication()
        {
            try
            {
                Plc.PcbOut();
                #region 先关闭相机
                this.Stop();
                CloseTheImageProvider();
                #endregion
                AiSdkFront.dispose();
                AiSdkBack.dispose();
                frontWorkingForm.imgBoxWorking.Dispose();
                backWorkingForm.imgBoxWorking.Dispose();
            }
            catch (Exception er)
            {

            }
            finally
            {
                Environment.Exit(0);
            }
        }

        private void Odin_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseApplication();
        }
        private void Odin_SizeChanged(object sender, EventArgs e)
        {
            frontWorkingForm.imgBoxWorking.ZoomToFit();
            backWorkingForm.imgBoxWorking.ZoomToFit();
        }
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
