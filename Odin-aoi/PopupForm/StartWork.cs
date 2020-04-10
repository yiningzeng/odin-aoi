﻿using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using power_aoi.Model;
using power_aoi.Tools;
using System.IO;
using power_aoi.Tools.Hardware;

namespace power_aoi.PopupForm
{
    public partial class StartWork : Form
    {
        public StartWork()
        {
            InitializeComponent();
            Ini();
        }

        private void Ini()
        {
            this.pMain.MouseDown += StartWork_MouseDown;
            this.pTitle.MouseDown += StartWork_MouseDown;
            this.pbClose.Click += PbClose_Click;
            tbCarrierLength.KeyPress += tb_KeyPress;
            tbCarrierWidth.KeyPress += tb_KeyPress;
            tbPcbLength.KeyPress += tb_KeyPress;
            tbPcbWidth.KeyPress += tb_KeyPress;
            btnStart.Click += BtnStart_Click;


            comBoxType.SelectedIndex = 0;
            tbCarrierLength.Text = "220";
            tbCarrierWidth.Text = "90";
            tbPcbLength.Text = "190";
            tbPcbWidth.Text = "50";
        }

        Pcb CreatePcb()
        {
            try
            {
                string sPath = INIHelper.Read("BaseConfig", "SavePath", Application.StartupPath + "/config.ini");
                if (!Directory.Exists(sPath))
                {
                    Directory.CreateDirectory(sPath);
                }
                string id = new Snowflake(1).nextId().ToString();
                int carrierLength = int.Parse(tbCarrierLength.Text.Trim());
                int carrierWidth = int.Parse(tbCarrierWidth.Text.Trim());
                int pcbLength = int.Parse(tbPcbLength.Text.Trim());
                int pcbWidth = int.Parse(tbPcbWidth.Text.Trim());
                if (carrierLength == 0 || carrierWidth == 0 || pcbLength == 0 || pcbWidth == 0)
                {
                    MessageBox.Show("长宽不可为0");
                    return null;
                }
                Pcb pcb = new Pcb()
                {
                    Id = id,
                    CarrierLength = carrierLength,
                    CarrierWidth = carrierWidth,
                    PcbLength = pcbLength,
                    PcbWidth = pcbWidth,
                    PcbPath = id,
                    results = new List<Result>(),
                };


                int xvalue = pcb.PcbWidth;
                int yvalue = pcb.PcbLength;
                int xdifferencevalue = (pcb.CarrierWidth - pcb.PcbWidth) / 2;
                int ydifferencevalue = (pcb.CarrierLength - pcb.PcbLength) / 2;
          

                byte[] receiveData = new byte[255];
                //byte[] writeValueX = new byte[y.Count * 4];
                //byte[] writeValueY = new byte[y.Count * 4];
                //nowPcb.FrontPcb.allRows = x.Count;
                //nowPcb.FrontPcb.allCols = y.Count;

                OneStitchSidePcb front = new OneStitchSidePcb()
                {
                    addressX = 3000,
                    addressY = 3200,
                    addressCaptureNum = 5000,
                    addressStartCapture = 2144,
                    addressOneSidePcbOut = 2145,

                    x = Xycoordinate.axcoordinate((int)Math.Ceiling((float)xvalue / Plc.capturePointIntervalXInMM), (int)(Plc.capturePointIntervalXInMM), xdifferencevalue),
                    y = Xycoordinate.aycoordinate((int)Math.Ceiling((float)yvalue / Plc.capturePointIntervalYInMM), (int)(Plc.capturePointIntervalYInMM), ydifferencevalue),

                    savePath = sPath,
                    zTrajectory = true,
                    equalDivision = INIHelper.ReadInteger("FrontAiPars", "equalDivision", 1, Application.StartupPath + "/config.ini"),
                    confidence = float.Parse(INIHelper.Read("FrontAiPars", "confidence", Application.StartupPath + "/config.ini")),
                };

                OneStitchSidePcb back = new OneStitchSidePcb()
                {
                    addressX = 3400,
                    addressY = 3600,
                    addressCaptureNum = 5002,
                    addressStartCapture = 2146,
                    addressOneSidePcbOut = 2147,

                    x = Xycoordinate.bxcoordinate((int)Math.Ceiling((float)xvalue / Plc.capturePointIntervalXInMM), (int)(Plc.capturePointIntervalXInMM), xdifferencevalue),
                    y = Xycoordinate.bycoordinate((int)Math.Ceiling((float)yvalue / Plc.capturePointIntervalYInMM), (int)(Plc.capturePointIntervalYInMM), ydifferencevalue),

                    savePath = sPath,
                    zTrajectory = false,
                    equalDivision = INIHelper.ReadInteger("BackAiPars", "equalDivision", 1, Application.StartupPath + "/config.ini"),
                    confidence = float.Parse(INIHelper.Read("BackAiPars", "confidence", Application.StartupPath + "/config.ini")),
                };
                front.allRows = front.x.Count;
                front.allCols = front.y.Count;
                front.AllNum = front.allRows * front.allCols;
                back.allRows = back.x.Count;
                back.allCols = back.y.Count;
                back.AllNum = back.allRows * back.allCols;


                switch (comBoxType.SelectedIndex)
                {
                    case 0:
                        pcb.SurfaceNumber = 2;

                        pcb.BackPcb = back;
                        pcb.FrontPcb = front;
                        break;
                    case 1:
                        pcb.SurfaceNumber = 1;
                        pcb.BackPcb = null;
                        pcb.FrontPcb = front;
                        break;
                    case 2:
                        pcb.SurfaceNumber = 1;
                        pcb.BackPcb = back;
                        pcb.FrontPcb = null;
                        break;
                }
               
                return pcb;
            }
            catch(Exception er)
            {
                MessageBox.Show("请输入信息");
                return null;
            }
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            this.Tag = CreatePcb();
            if (this.Tag == null) return;
            this.DialogResult = DialogResult.Yes;
            this.Close();
        }

        private void tb_KeyPress(object sender, KeyPressEventArgs e)
        {
            //如果输入的不是退格和数字，则屏蔽输入
            if (!(e.KeyChar == '\b' || (e.KeyChar >= '0' && e.KeyChar <= '9')))
            {
                e.Handled = true;
            }
        }

        private void PbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void StartWork_MouseDown(object sender, MouseEventArgs e)
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
    }
}
