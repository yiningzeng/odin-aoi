using power_aoi.Tools;
using power_aoi.Tools.Hardware;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace power_aoi.Model
{
    [Table(name: "pcbs")]
    public class Pcb
    {
        [Description("主键")]
        [Key]
        [Column(name:"id",TypeName = "varchar")]
        [StringLength(50)]
        public string Id { get; set; }

        [Column(name: "pcb_number")]
        [StringLength(250)]
        [Description("板号")]
        public string PcbNumber { get; set; }

        [Column(name: "pcb_name")]
        [StringLength(250)]
        [Description("PCB名称")]
        public string PcbName { get; set; }

        [Column(name: "carrier_width")]
        [Description("载板长")]
        public int CarrierLength { get; set; }

        [Column(name: "carrier_height")]
        [Description("载板宽")]
        public int CarrierWidth { get; set; }

        [Column(name: "pcb_width")]
        [Description("PCB名称")]
        public int PcbLength { get; set; }

        [Column(name: "pcb_height")]
        [Description("PCB名称")]
        public int PcbWidth { get; set; }

        [Column(name: "pcb_childen_number")]
        [Description("PCB名称")]
        public int PcbChildenNumber { get; set; }

        [Column(name: "surface_number")]
        [Description("检测的面数")]
        public int SurfaceNumber { get; set; }

        [Column(name: "pcb_path")]
        [StringLength(250)]
        [Description("对应的FTP保存的地址")]
        public string PcbPath { get; set; }

        [DefaultValue(0)]
        [Column(name: "is_error")]
        [Description("是否报错, 0否 1是")]
        public int IsError { get; set; }

        [DefaultValue(0)]
        [Column(name: "is_misjudge")]
        [Description("是否误判, 0否 1是, 只要results下有一个误判，那该值就改为1")]
        public int IsMisjudge { get; set; }

        [Column(name: "create_time")]
        [Description("创建时间")]
        public DateTime CreateTime { get; set; }

        public List<Result> results { get; set; }

        [NotMapped]
        [Description("总的图片数量")]
        public int AllPhotoNum { get; set; }
        [NotMapped]
        [Description("主要用于创建的新的pcb的时候的正反面标识")]
        public int SideIndex { get; set; }
        [NotMapped]
        public OneStitchSidePcb FrontPcb { get; set; }
        [NotMapped]
        public OneStitchSidePcb BackPcb { get; set; }

        /// <summary>
        /// 用于创建初始的Pcb
        /// </summary>
        /// <param name="carrierLength"></param>
        /// <param name="carrierWidth"></param>
        /// <param name="pcbLength"></param>
        /// <param name="pcbWidth"></param>
        /// <param name="side"></param>
        /// <returns></returns>
        public static Pcb CreatePcb(int carrierLength, int carrierWidth, int pcbLength, int pcbWidth, int sideIndex)
        {
            try
            {
                string sPath = INIHelper.Read("BaseConfig", "SavePath", Application.StartupPath + "/config.ini");
                if (!Directory.Exists(sPath)) Directory.CreateDirectory(sPath);
                string id = new Snowflake(1).nextId().ToString();
                if (carrierLength == 0 || carrierWidth == 0 || pcbLength == 0 || pcbWidth == 0) { MessageBox.Show("长宽不可为0"); return null; }
                Pcb pcb = new Pcb()
                {
                    Id = id,
                    CarrierLength = carrierLength,
                    CarrierWidth = carrierWidth,
                    PcbLength = pcbLength,
                    PcbWidth = pcbWidth,
                    PcbPath = id,
                    SideIndex = sideIndex,
                    results = new List<Result>(),
                };

                int xvalue = pcb.PcbWidth;
                int yvalue = pcb.PcbLength;
                int xdifferencevalue = (pcb.CarrierWidth - pcb.PcbWidth) / 2;
                int ydifferencevalue = (pcb.CarrierLength - pcb.PcbLength) / 2;

                byte[] receiveData = new byte[255];
                var frontX = Xycoordinate.axcoordinate((int)Math.Ceiling((float)xvalue / Plc.capturePointIntervalXInMM), (int)(Plc.capturePointIntervalXInMM), xdifferencevalue);
                var frontY = Xycoordinate.aycoordinate((int)Math.Ceiling((float)yvalue / Plc.capturePointIntervalYInMM), (int)(Plc.capturePointIntervalYInMM), ydifferencevalue);
                bool frontDetectMultiScale = INIHelper.ReadBoolean("FrontAiPars", "detectMultiScale", false, Application.StartupPath + "/config.ini");
                OneStitchSidePcb front = new OneStitchSidePcb()
                {
                    overlap = INIHelper.ReadInteger("FrontAiPars", "overlap", 50, Application.StartupPath + "/config.ini"),
                    saveCropImg = INIHelper.ReadBoolean("FrontAiPars", "saveCropImg", false, Application.StartupPath + "/config.ini"),
                    equalDivision = INIHelper.ReadInteger("FrontAiPars", "equalDivision", 1, Application.StartupPath + "/config.ini"),
                    detectMultiScale = frontDetectMultiScale,
                    confidence = float.Parse(INIHelper.Read("FrontAiPars", "confidence", Application.StartupPath + "/config.ini")),

                    addressX = 3000,
                    addressY = 3200,
                    addressCaptureNum = 5000,
                    addressStartCapture = 2144,
                    addressEndCapture = 1133,
                    addressOneSidePcbOut = 2145,

                    x = frontX,
                    y = frontY,

                    pcbId = id,
                    savePath = sPath,

                    allRows = frontX.Count,
                    allCols = frontY.Count,
                    allNum = frontDetectMultiScale ? frontX.Count * frontY.Count * 2 : frontX.Count * frontY.Count, // 这里多尺度是需要改变总数*2的
                    currentRow = 0,
                    currentCol = 0,
                    zTrajectory = true,
                    dst = null,
                    roi = new Rectangle(),
                    scale = 0.25,
                    stitchEnd = false,
                    bitmaps = new Queue<Bitmap>(),
                };

                var backX = Xycoordinate.bxcoordinate((int)Math.Ceiling((float)xvalue / Plc.capturePointIntervalXInMM), (int)(Plc.capturePointIntervalXInMM), xdifferencevalue);
                var backY = Xycoordinate.bycoordinate((int)Math.Ceiling((float)yvalue / Plc.capturePointIntervalYInMM), (int)(Plc.capturePointIntervalYInMM), ydifferencevalue);
                bool backDetectMultiScale = INIHelper.ReadBoolean("BackAiPars", "detectMultiScale", false, Application.StartupPath + "/config.ini");
                OneStitchSidePcb back = new OneStitchSidePcb()
                {
                    overlap = INIHelper.ReadInteger("BackAiPars", "overlap", 50, Application.StartupPath + "/config.ini"),
                    saveCropImg = INIHelper.ReadBoolean("BackAiPars", "saveCropImg", false, Application.StartupPath + "/config.ini"),
                    equalDivision = INIHelper.ReadInteger("BackAiPars", "equalDivision", 1, Application.StartupPath + "/config.ini"),
                    detectMultiScale = backDetectMultiScale,
                    confidence = float.Parse(INIHelper.Read("BackAiPars", "confidence", Application.StartupPath + "/config.ini")),

                    addressX = 3400,
                    addressY = 3600,
                    addressCaptureNum = 5002,
                    addressStartCapture = 2146,
                    addressEndCapture = 1135,
                    addressOneSidePcbOut = 2147,

                    x = backX,
                    y = backY,

                    pcbId = id,
                    savePath = sPath,

                    allRows = backX.Count,
                    allCols = backY.Count,
                    allNum = backDetectMultiScale ? backX.Count * backY.Count * 2 : backX.Count * backY.Count,
                    currentRow = 0,
                    currentCol = 0,
                    zTrajectory = false,
                    dst = null,
                    roi = new Rectangle(),
                    scale = 0.25,
                    stitchEnd = false,
                    bitmaps = new Queue<Bitmap>(),
                };

                switch (sideIndex)
                {
                    case 0:
                        pcb.SurfaceNumber = 2;
                        pcb.BackPcb = back;
                        pcb.FrontPcb = front;
                        pcb.AllPhotoNum = back.allNum + front.allNum;
                        break;
                    case 1:
                        pcb.SurfaceNumber = 1;
                        pcb.BackPcb = null;
                        pcb.FrontPcb = front;
                        pcb.AllPhotoNum = front.allNum;
                        break;
                    case 2:
                        pcb.SurfaceNumber = 1;
                        pcb.BackPcb = back;
                        pcb.FrontPcb = null;
                        pcb.AllPhotoNum = back.allNum;
                        break;
                }
                return pcb;
            }
            catch (Exception er) { return null; }
        }
    }
}
