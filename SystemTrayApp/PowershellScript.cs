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
            this.KeyDown += (sender, args) => {
                if (args.KeyCode == Keys.Enter)
                {
                    btnLogin.PerformClick();
                }
            };
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            using (ViewManager manager = new ViewManager(serviceManager))
            {
                Helper.AddUpdateAppSettings("MilestoneUserName", (chkNetworkService.Checked) ? "" : txtLogin.Text, "OpenALPRQueueMilestone.exe");
                Helper.AddUpdateAppSettings("MilestonePassword", (chkNetworkService.Checked) ? "" : txtPassword.Text, "OpenALPRQueueMilestone.exe");
                manager.RunPowershellScrip((chkNetworkService.Checked) ? "Network Service" : txtLogin.Text, txtPassword.Text);
                this.Close();
            }
        }

        private void lblNetworkService_Click(object sender, EventArgs e)
        {
            chkNetworkService.Checked = !chkNetworkService.Checked;
        }

        private void lblUse_Click(object sender, EventArgs e)
        {
            chkNetworkService.Checked = !chkNetworkService.Checked;
        }

        private void chkNetworkService_CheckedChanged(object sender, EventArgs e)
        {
            txtLogin.Enabled = !chkNetworkService.Checked;
            txtPassword.Enabled = !chkNetworkService.Checked;
        }

        private void PowershellScript_Activated(object sender, EventArgs e)
        {
            txtLogin.Text = Helper.ReadConfigKey("MilestoneUserName", "OpenALPRQueueMilestone.exe");
            txtPassword.Text = Helper.ReadConfigKey("MilestonePassword", "OpenALPRQueueMilestone.exe");
            chkNetworkService.Checked = (string.IsNullOrEmpty(txtLogin.Text) || string.IsNullOrWhiteSpace(txtLogin.Text)) &&
                (string.IsNullOrEmpty(txtPassword.Text) || string.IsNullOrWhiteSpace(txtPassword.Text));
            txtLogin.Enabled = !chkNetworkService.Checked;
            txtPassword.Enabled = !chkNetworkService.Checked;

            btnLogin.Focus();
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLogin.PerformClick();
            }
        }

        private void txtPassword_KeyUp(object sender, KeyEventArgs e)
        {
            //string password = (sender as TextBox).Text;
            //btnLogin.Enabled = (!string.IsNullOrEmpty(password) && !string.IsNullOrWhiteSpace(password));
        }
    }
}
