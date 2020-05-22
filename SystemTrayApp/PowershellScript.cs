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
                        MilestoneServerName = "http://localhost:80/",
                        MilestoneUserName = txtLogin.Text,
                        OpenALPRServerUrl = "http://localhost:48125/",
                        ServicePort = Convert.ToInt32(nudServicePort.Value)
                    });
                }

                Helper.AddUpdateAppSettings("MilestoneUserName", (chkNetworkService.Checked) ? "" : txtLogin.Text, "OpenALPRQueueMilestone.exe");
                Helper.AddUpdateAppSettings("MilestonePassword", (chkNetworkService.Checked) ? "" : txtPassword.Text, "OpenALPRQueueMilestone.exe");

                Helper.AddUpdateAppSettings("ClientSettingsProvider.ServiceUri", txtServiceUrl.Text, "OpenALPRQueueMilestone.exe");
                Helper.AddUpdateAppSettings("EventExpireAfterDays", nudEventExpireAfterDays.Value.ToString(), "OpenALPRQueueMilestone.exe");
                Helper.AddUpdateAppSettings("EpochStartSecondsBefore", nudEpochStartSecondsBefore.Value.ToString(), "OpenALPRQueueMilestone.exe");
                Helper.AddUpdateAppSettings("EpochEndSecondsAfter", nudEpochEndSecondsAfter.Value.ToString(), "OpenALPRQueueMilestone.exe");
                Helper.AddUpdateAppSettings("ServicePort", nudServicePort.Value.ToString(), "OpenALPRQueueMilestone.exe");
                Helper.AddUpdateAppSettings("AddBookmarks", chkAddBookmarks.Checked.ToString(), "OpenALPRQueueMilestone.exe");
                Helper.AddUpdateAppSettings("AutoMapping", chkAutoMapping.Checked.ToString(), "OpenALPRQueueMilestone.exe");

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
            txtLogin.Text = string.Empty;
            txtPassword.Text = string.Empty;
        }

        private void PowershellScript_Activated(object sender, EventArgs e)
        {
            using (DB db = new DB(OpenALPRQueueMilestoneDefinition.applicationPath, "OpenALPRQueueMilestone", 3))
            {
                db.CreateTable("Settings");
                Settings settings = db.GetSettings("Settings").LastOrDefault();
            }

            txtLogin.Text = Helper.ReadConfigKey("MilestoneUserName", "OpenALPRQueueMilestone.exe");
            txtPassword.Text = Helper.ReadConfigKey("MilestonePassword", "OpenALPRQueueMilestone.exe");
            txtServiceUrl.Text = Helper.ReadConfigKey("ClientSettingsProvider.ServiceUri", "OpenALPRQueueMilestone.exe");
            
            int.TryParse(Helper.ReadConfigKey("EventExpireAfterDays", "OpenALPRQueueMilestone.exe"), out int eventExpireAfterDays);
            nudEventExpireAfterDays.Value = eventExpireAfterDays;

            int.TryParse(Helper.ReadConfigKey("EpochStartSecondsBefore", "OpenALPRQueueMilestone.exe"), out int epochStartSecondsBefore);
            nudEpochStartSecondsBefore.Value = epochStartSecondsBefore;

            int.TryParse(Helper.ReadConfigKey("EpochEndSecondsAfter", "OpenALPRQueueMilestone.exe"), out int epochEndSecondsAfter);
            nudEpochEndSecondsAfter.Value = epochEndSecondsAfter;

            int.TryParse(Helper.ReadConfigKey("ServicePort", "OpenALPRQueueMilestone.exe"), out int servicePort);
            nudServicePort.Value = servicePort;

            bool.TryParse(Helper.ReadConfigKey("AddBookmarks", "OpenALPRQueueMilestone.exe"), out bool addBookmarks);
            chkAddBookmarks.Checked = addBookmarks;

            bool.TryParse(Helper.ReadConfigKey("AutoMapping", "OpenALPRQueueMilestone.exe"), out bool autoMapping);
            chkAutoMapping.Checked = autoMapping;

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
