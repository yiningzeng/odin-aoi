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

namespace power_aoi.DockerPanelOdin
{
    public partial class BackWorkingForm : DockContent
    {
        public BackWorkingForm()
        {
            InitializeComponent();
            Ini(false);
        }

        public void Ini(bool isEdit = false)
        {
            imgBoxWorking.Text = "";
            imgBoxWorking.ScaleText = true;
            imgBoxWorking.TextBackColor = Color.LightSeaGreen;
            imgBoxWorking.TextDisplayMode = Cyotek.Windows.Forms.ImageBoxGridDisplayMode.Client;
            imgBoxWorking.ForeColor = Color.Red;
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
        public void ShowDefaultImage(string imagePath = "default")
        {
            this.BeginInvoke((Action)(() => {
                if (imagePath == "default")
                {
                    imgBoxWorking.Image = Image.FromFile(Application.StartupPath + "/DefaultImage/Back.jpg");
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
