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
    public partial class FrontWorkingForm : DockContent
    {
        public FrontWorkingForm()
        {
            InitializeComponent();
            Ini();
        }

        public void Ini()
        {
            imgBoxWorking.Text = "";
            imgBoxWorking.SelectionMode = Cyotek.Windows.Forms.ImageBoxSelectionMode.Rectangle;
            imgBoxWorking.ScaleText = true;
            imgBoxWorking.TextBackColor = Color.LightSeaGreen;
            imgBoxWorking.TextDisplayMode = Cyotek.Windows.Forms.ImageBoxGridDisplayMode.Client;
            imgBoxWorking.ForeColor = Color.Red;
        }

        #region Public Members
        public void ShowDefaultImage()
        {
            imgBoxWorking.Image = Image.FromFile(Application.StartupPath + "/DefaultImage/Front.jpg");
            imgBoxWorking.ZoomToFit();
            imgBoxWorking.Invalidate();
        }
        #endregion
    }
}
