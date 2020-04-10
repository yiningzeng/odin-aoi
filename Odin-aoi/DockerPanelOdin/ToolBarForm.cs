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
    public partial class ToolBarForm : DockContent
    {
        Odin odin;
        FrontWorkingForm workingFrom;
        public ToolBarForm()
        {
            InitializeComponent();
        }

        public void IniForm(Odin o, FrontWorkingForm w)
        {
            odin = o;
            workingFrom = w;
        }
    }
}
