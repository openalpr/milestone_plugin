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
using Database;
using OpenALPR.SystemTrayApp;
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
                using (DB db = new DB(OpenALPRQueueMilestoneDefinition.applicationPath, "OpenALPRQueueMilestone", 3))
                {
                    db.SaveSettings("Settings", new Settings()
                    {
                        EventExpireAfterDays = Convert.ToInt32(nudEventExpireAfterDays.Value),
                        AddBookmarks = chkAddBookmarks.Checked,
                        AutoMapping = chkAutoMapping.Checked,
                        ClientSettingsProviderServiceUri = txtServiceUrl.Text,
                        EpochEndSecondsAfter = Convert.ToInt32(nudEpochEndSecondsAfter.Value),
                        EpochStartSecondsBefore = Convert.ToInt32(nudEpochStartSecondsBefore.Value),
                        MilestonePassword = txtPassword.Text,
                        MilestoneServerName = txtMilestoneServer.Text,
                        MilestoneUserName = txtLogin.Text,
                        OpenALPRServerUrl = txtOpenALPRServer.Text,
                        ServicePort = Convert.ToInt32(nudServicePort.Value)
                    });
                    tsslAlert.Text = "Please wait for the restart of the service!";
                    manager.RunPowershellScrip((chkNetworkService.Checked) ? "Network Service" : txtLogin.Text, txtPassword.Text);
                    this.Close();
                }
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
            txtLogin.Text = string.Empty;
            txtPassword.Text = string.Empty;
        }

        private void PowershellScript_Activated(object sender, EventArgs e)
        {
            using (DB db = new DB(OpenALPRQueueMilestoneDefinition.applicationPath, "OpenALPRQueueMilestone", 3))
            {
                db.CreateTable("Settings");
                Settings settings = db.GetSettings("Settings").LastOrDefault();
                if (settings == null)
                {
                    settings = db.Defaults();
                    tsslAlert.Text = "This is the first launch of the application, make changes and/or click Save.";
                    tsslAlert.ForeColor = Color.Red;
                }
                else
                {
                    tsslAlert.Text = $"Last saved settings: {settings.Created}";
                    tsslAlert.ForeColor = Color.Green;
                }

                txtLogin.Text = settings.MilestoneUserName;
                txtPassword.Text = settings.MilestonePassword;
                txtServiceUrl.Text = settings.ClientSettingsProviderServiceUri;
                txtMilestoneServer.Text = settings.MilestoneServerName;
                txtOpenALPRServer.Text = settings.OpenALPRServerUrl;
                nudEpochEndSecondsAfter.Value = settings.EpochEndSecondsAfter;
                nudEpochStartSecondsBefore.Value = settings.EpochStartSecondsBefore;
                nudEventExpireAfterDays.Value = settings.EventExpireAfterDays;
                nudServicePort.Value = settings.ServicePort;
                chkAddBookmarks.Checked = settings.AddBookmarks;
                chkAutoMapping.Checked = settings.AutoMapping;
                
                chkNetworkService.Checked = (string.IsNullOrEmpty(txtLogin.Text) || string.IsNullOrWhiteSpace(txtLogin.Text)) &&
                    (string.IsNullOrEmpty(txtPassword.Text) || string.IsNullOrWhiteSpace(txtPassword.Text));
                txtLogin.Enabled = !chkNetworkService.Checked;
                txtPassword.Enabled = !chkNetworkService.Checked;

                btnLogin.Focus();
            }
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
