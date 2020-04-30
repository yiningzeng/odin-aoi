using Odin_aoi.Model;
using power_aoi.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace power_aoi.DockerPanelOdin
{
    public partial class FrontWorkingForm : DockContent
    {
        //Dictionary<long, AiDetectRect> _dictPcbRect;
        Odin odin;
        AttributeForm attributeForm;

        ChildrenPcbMarkerInfo nowWorkingObj;
        PcbAlgorithmsInfo pcbAlgorithmsInfo = new PcbAlgorithmsInfo();

        public FrontWorkingForm()
        {
            InitializeComponent();
            imgBoxWorking.SelectionMoved += ImgBoxWorking_SelectionMoved;
            imgBoxWorking.SelectionResized += ImgBoxWorking_SelectionResized;
            imgBoxWorking.Selected += ImgBoxWorking_Selected;
            imgBoxWorking.Paint += ImgBoxWorking_Paint;
            Ini();
        }

        private void ImgBoxWorking_Paint(object sender, PaintEventArgs e)
        {
            Graphics g;
            GraphicsState originalState;
            Size scaledSize;
            Size originalSize;
            Size drawSize;
            bool scaleAdornmentSize;

            scaleAdornmentSize = true;

            g = e.Graphics;

            originalState = g.Save();

            // Work out the size of the marker graphic according to the current zoom level

            foreach (var item in pcbAlgorithmsInfo.childrenPcbMarkerInfos)
            {
                originalSize = item.Value.MarkerRect.Size;
                scaledSize = imgBoxWorking.GetScaledSize(originalSize);
                drawSize = scaleAdornmentSize ? scaledSize : originalSize;

                Rectangle location;

                // Work out the location of the marker graphic according to the current zoom level and scroll offset
                location = imgBoxWorking.GetOffsetRectangle(item.Value.MarkerRect);
                if (imgBoxWorking.IsPointInImage(location.Location))
                {
                    g.InterpolationMode = InterpolationMode.NearestNeighbor;



                    g.SetClip(imgBoxWorking.GetInsideViewPort(true));
                    //using (Brush brush = new SolidBrush(Color.FromArgb(128, Color.Blue)))
                    //{
                    //    g.FillRectangle(brush, location);
                    //}

                    using (Pen pen = new Pen(Color.FromArgb(135, 206, 250)))
                    {
                        g.DrawRectangle(pen, new Rectangle(location.Location, drawSize));
                    }

                    g.ResetClip();

                }
                // adjust the location so that the image is displayed above the location and centered to it
                //location.Y -= drawSize.Height;
                //location.X -= drawSize.Width >> 1;

                // Draw the marker
                //g.DrawImage(_markerImage, new Rectangle(location.Location, drawSize), new Rectangle(Point.Empty, originalSize), GraphicsUnit.Pixel);
            }

            g.Restore(originalState);
        }


        private void ImgBoxWorking_Selected(object sender, EventArgs e)
        {
            Rectangle temp = Utils.RectangleF2Rectangle(imgBoxWorking.SelectionRegion);
            if (temp.Width == 0 || temp.Height == 0) return;
            nowWorkingObj = new ChildrenPcbMarkerInfo() { MarkerRect = temp, Name = "Marker" + (pcbAlgorithmsInfo.childrenPcbMarkerInfos.Count + 1) };
            pcbAlgorithmsInfo.childrenPcbMarkerInfos.Add(nowWorkingObj.Name, nowWorkingObj);
            Console.WriteLine("ImgBoxWorking_Selected");
            ShowConfig();
        }

        private void ImgBoxWorking_SelectionResized(object sender, EventArgs e)
        {
            Console.WriteLine("ImgBoxWorking_SelectionResized");
            ShowConfig();
        }

        private void ImgBoxWorking_SelectionMoved(object sender, EventArgs e)
        {
            Console.WriteLine("ImgBoxWorking_SelectionMoved");
        }

        public void IniForm(Odin o, AttributeForm a)
        {
            odin = o;
            attributeForm = a;
        }

        public void Ini(bool isEdit = false)
        {
            imgBoxWorking.Text = "";
            
            imgBoxWorking.ScaleText = true;
            imgBoxWorking.TextBackColor = Color.LightSeaGreen;
            imgBoxWorking.TextDisplayMode = Cyotek.Windows.Forms.ImageBoxGridDisplayMode.Client;
            imgBoxWorking.ForeColor = Color.Red;
            imgBoxWorking.SelectionRegion = new RectangleF();
            if (isEdit)
            {
                imgBoxWorking.SelectionMode = Cyotek.Windows.Forms.ImageBoxSelectionMode.None;
                imgBoxWorking.Enabled = true;
                imgBoxWorking.DragHandleSize = 8;
            }
            else
            {
                imgBoxWorking.SelectionMode = Cyotek.Windows.Forms.ImageBoxSelectionMode.Rectangle;
                imgBoxWorking.Enabled = false;
                imgBoxWorking.DragHandleSize = 0;
            }
        }

        #region Public Members
        public void ShowConfig()
        {
            attributeForm.ShowConfig("编辑>正", nowWorkingObj);
        }

        public void ShowDefaultImage(string imagePath = "default")
        {
            this.BeginInvoke((Action)(()=> {
                if (imagePath == "default")
                {
                    imgBoxWorking.Image = Image.FromFile(Application.StartupPath + "/DefaultImage/Front.jpg");
                }
                else
                {
                    imgBoxWorking.Image = Image.FromFile(imagePath);
                }
                imgBoxWorking.ZoomToFit();
                imgBoxWorking.Invalidate();
            }));
        }
        #endregion
    }
}
