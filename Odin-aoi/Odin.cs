using Emgu.CV;
using Emgu.CV.Structure;
using pcbaoi.Tools;
using power_aoi.DockerPanelOdin;
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

namespace power_aoi
{
    public partial class Odin : DockContent
    {
        #region Instance Fields
        public delegate void RabbitmqMessageCallback(string message);
        public RabbitmqMessageCallback doWorkD;

        public delegate void RabbitmqConnectCallback(string message);
        public RabbitmqConnectCallback rabbitmqConnectCallback;

        private DeserializeDockContent m_deserializeDockContent;
        ToolBarForm toolBarForm;
        WorkingFrom workingFrom;

        private string cameraAid = INIHelper.Read("CameraA", "SerialNumber", Application.StartupPath + "/config.ini");
        #endregion

        public Odin()
        {
            InitializeComponent();
            this.Icon = Properties.Resources.aa;
            CreateStandardControls();
            m_deserializeDockContent = new DeserializeDockContent(GetContentFromPersistString);

            string configFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "DockPanel.config");
            if (File.Exists(configFile))
                dockPanel1.LoadFromXml(configFile, m_deserializeDockContent);
            //toolBarForm.Show(this.dockPanel1, DockState.DockRight);
            //workingFrom.Show(this.dockPanel1, DockState.Document);
            Ini();
        }

        public void Ini()
        {
            #region 初始化事件
            this.FormClosing += Odin_FormClosing;
            #endregion
            #region 添加左边工具栏
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
                button.Location = new Point(7, 7+i*(15+35));
                button.Click += Button_Click;
                button.Name = imageListToolBar.Images.Keys[i].ToString();
                panelToolBar.Controls.Add(button);
            }
            #endregion

            #region 初始化各页面窗体实例
            toolBarForm.IniForm(this, workingFrom);
            #endregion

            #region camera
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
        }

        private void Odin_FormClosing(object sender, FormClosingEventArgs e)
        {
            //先关闭相机
            this.Stop();
            Environment.Exit(0);
        }



        #region 相机使用模块 初始化两个相机
        private ImageProvider m_imageProvider = new ImageProvider(); /* Create one image provider. */
        /* Stops the image provider and handles exceptions. */
        public void Stop()
        {
            /* Stop the grabbing. */
            try
            {
                m_imageProvider.Stop();
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
            Stop();
            /* Close the image provider. */
            CloseTheImageProvider();
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
                    ///* Check if the image is compatible with the currently used bitmap. */
                    //if (BitmapFactory.IsCompatible(m_bitmap, image.Width, image.Height, image.Color))
                    //{
                    //    /* Update the bitmap with the image data. */
                    //    BitmapFactory.UpdateBitmap(m_bitmap, image.Buffer, image.Width, image.Height, image.Color);
                    //    /* To show the new image, request the display control to update itself. */
                    //    if (m_imageProvider.CameraId == cameraAid)
                    //    {

                    //        m_bitmap.Save(directory + "\\F" + Aimagenum.ToString() + ".jpg", ImageFormat.Jpeg);
                    //        Bitmap listbitmap;
                    //        //listbitmap = (Bitmap)singleCaptureCropAndResize(m_bitmap).Clone();
                    //        listbitmap = (Bitmap)m_bitmap.Clone();
                    //        Amats.Enqueue(listbitmap);
                    //        //Abitmaps.Add(listbitmap);
                    //        //listbitmap.Dispose();
                    //        pbFrontImg.Image = m_bitmap;
                    //        if (tbFrontOrBack.Text == "正面")
                    //        {
                    //            pbMainImg.Image = m_bitmap;
                    //        }


                    //        //listbitmap.Dispose();
                    //        //m_bitmap.Save("d:\\pic\\test" + ".bmp", ImageFormat.Bmp);
                    //        //Abitmaps[Abitmaps.Count - 1].Save("d:\\pic\\test2" + ".bmp", ImageFormat.Bmp);
                    //        //;
                    //    }
                    //    Aimagenum++;
                    //    lock (objA)
                    //    {
                    //        Arun++;
                    //    }

                    //}
                    //else /* A new bitmap is required. */
                    //{
                    //    BitmapFactory.CreateBitmap(out m_bitmap, image.Width, image.Height, image.Color);
                    //    BitmapFactory.UpdateBitmap(m_bitmap, image.Buffer, image.Width, image.Height, image.Color);
                    //    /* We have to dispose the bitmap after assigning the new one to the display control. */
                    //    Bitmap bitmap = pbFrontImg.Image as Bitmap;
                    //    /* Provide the display control with the new bitmap. This action automatically updates the display. */
                    //    if (m_imageProvider.CameraId == cameraAid)
                    //    {
                    //        m_bitmap.Save(directory + "\\F" + Aimagenum.ToString() + ".jpg", ImageFormat.Jpeg);
                    //        //保存相机拍摄的原始图片
                    //        Bitmap listbitmap;
                    //        //listbitmap = (Bitmap)singleCaptureCropAndResize(m_bitmap).Clone();
                    //        listbitmap = (Bitmap)m_bitmap.Clone();
                    //        Amats.Enqueue(listbitmap);
                    //        // Abitmaps.Add(listbitmap);
                    //        pbFrontImg.Image = m_bitmap;
                    //        if (tbFrontOrBack.Text == "正面")
                    //        {
                    //            pbMainImg.Image = m_bitmap;
                    //        }
                    //        //listbitmap.Dispose();

                    //        //listbitmap.Dispose();
                    //    }
                    //    if (bitmap != null)
                    //    {
                    //        /* Dispose the bitmap. */
                    //        bitmap.Dispose();
                    //    }
                    //    Aimagenum++;
                    //    lock (objA)
                    //    {
                    //        Arun++;
                    //    }
                    //}
                    /* The processing of the image is done. Release the image buffer. */
                    // 
                    m_imageProvider.ReleaseImage();
                    /* The buffer can be used for the next image grabs. */
                }
            }
            catch (Exception e)
            {
                LogHelper.WriteLog(m_imageProvider.GetLastErrorMessage(), e);
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

        private void Odin_Load(object sender, EventArgs e)
        {
            doWorkD = new RabbitmqMessageCallback(doWork);
            rabbitmqConnectCallback = new RabbitmqConnectCallback(RabbitmqConnected);
            try
            {
                // 这里不需要当消费者！！！！！！！
                RabbitMQClientHandler.GetInstance(doWorkD, rabbitmqConnectCallback).SyncDataFromServer("work");
            }
            catch (Exception er)
            {
                MessageBox.Show("连接队列失败!!!");
                
            }
        }
        #endregion

        private IDockContent GetContentFromPersistString(string persistString)
        {
            if (persistString == typeof(WorkingFrom).ToString())
            {
                workingFrom = new WorkingFrom() { CloseButton = false, CloseButtonVisible = false };
                return workingFrom;
            }else if(persistString == typeof(ToolBarForm).ToString())
            {
                toolBarForm = new ToolBarForm() { TabText = "工具箱", CloseButton = false, CloseButtonVisible = false };
                return toolBarForm;
            }
            


            else return null;
        }

        private void CreateStandardControls()
        {
            workingFrom = new WorkingFrom() { CloseButton = false, CloseButtonVisible = false };
            toolBarForm = new ToolBarForm() { TabText = "工具箱", CloseButton = false, CloseButtonVisible = false };
        }


        #region Event Handlers

        private void Button_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            switch (btn.Name.Replace(".png", ""))
            {
                case "camera":
                    Rectangle roi0 = new Rectangle(); //上一行第一张的区域
                    Rectangle roi = new Rectangle(); // 左对齐的参考的区域
                    Mat img = null;
                    Aoi.ImageStitch(ref img, new Bitmap(@"C:\Users\Administrator\Desktop\suomi-test-img\762-Ng\F0.jpg"),
                        ref roi0,
                        ref roi,
                        1, 2, true, true);
                    Aoi.ImageStitch(ref img, new Bitmap(@"C:\Users\Administrator\Desktop\suomi-test-img\762-Ng\F1.jpg"),
                        ref roi0,
                        ref roi,
                        1, 2, true, false);
                   // Aoi.ImageStitch(ref img, new Bitmap(@"C:\Users\Administrator\Desktop\suomi-test-img\762-Ng\F2.jpg"),
                   //ref roi0,
                   //ref roi,
                   //1, 2, true, false);
                   // Aoi.ImageStitch(ref img, new Bitmap(@"C:\Users\Administrator\Desktop\suomi-test-img\762-Ng\F3.jpg"),
                   //ref roi0,
                   //ref roi,
                   //1, 2, true, false);
                   // Aoi.ImageStitch(ref img, new Bitmap(@"C:\Users\Administrator\Desktop\suomi-test-img\762-Ng\F4.jpg"),
                   //ref roi0,
                   //ref roi,
                   //1, 2, true, false);
                    img.Save(@"C:\Users\Administrator\Desktop\suomi-test-img\fu.jpg");
                    break;
                case "editor":

                    break;
            }
        }
        private void 初始化窗体布局ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolBarForm.DockPanel = null;
            workingFrom.DockPanel = null;
            //new ToolBarForm() { TabText = "我是项目", CloseButton = true, CloseButtonVisible = true }.Show(this.dockPanel1, DockState.Document);
            string configFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Deafult.config");
            if (File.Exists(configFile))
                dockPanel1.LoadFromXml(configFile, m_deserializeDockContent);
        }

        private void 保存窗体布局ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string configFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "DockPanel.config");
            dockPanel1.SaveAsXml(configFile);
        }
        #endregion

    }
}
