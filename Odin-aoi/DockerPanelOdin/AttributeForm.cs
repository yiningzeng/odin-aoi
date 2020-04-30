using Odin_aoi.Model;
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

        public void ShowConfig(string from, object obj)
        {
            switch (from)
            {
                case "编辑>正":
                case "编辑>反":
                    ChildrenPcbMarkerInfo childrenPcbMarkerInfo = obj as ChildrenPcbMarkerInfo;
                    propertyGrid.SelectedObject = childrenPcbMarkerInfo;
                    break;
            }
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
