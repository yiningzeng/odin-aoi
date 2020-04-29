using power_aoi.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace power_aoi.DockerPanelOdin
{
    public partial class AttributeForm : DockContent
    {
        Odin odin;
        FrontWorkingForm workingFrom;
        public AttributeForm()
        {
            InitializeComponent();
        }

        public void ShowConfig(OneStitchSidePcb oneStitchSidePcb)
        {
            propertyGrid.SelectedObject = oneStitchSidePcb;
        }

        public OneStitchSidePcb GetConfig()
        {
            return propertyGrid.SelectedObject as OneStitchSidePcb;
        }

        public void IniForm(Odin o, FrontWorkingForm w)
        {
            odin = o;
            workingFrom = w;
        }
    }
}
