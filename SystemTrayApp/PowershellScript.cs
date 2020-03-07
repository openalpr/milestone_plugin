using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenALPR.SystemTrayApp.Utility;

namespace SystemTrayApp
{
    public partial class PowershellScript : Form
    {
        private ServiceManager serviceManager;

        public PowershellScript()
        {
            InitializeComponent();
            serviceManager = new ServiceManager();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {

            using (ViewManager manager = new ViewManager(serviceManager))
            {
                manager.RunPowershellScrip("admin", "test");
                this.Close();
            }
        }
    }
}
