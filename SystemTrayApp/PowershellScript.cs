using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
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
                manager.RunPowershellScrip((chkNetworkService.Checked) ? "Network Service" : txtLogin.Text, txtPassword.Text);
                Thread.Sleep(3000);
                #region Use SQLite
                using (DB db = new DB("OpenALPRQueueMilestone"))
                {
                    db.SaveSettings(new Settings()
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
                        ServicePort = Convert.ToInt32(nudServicePort.Value),
                        UseUTC = chkUseUTC.Checked
                    });
                    
                    tsslAlert.Text = "Please wait for the restart of the service!";
                }
                #endregion

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

        private void PowershellScript_Load(object sender, EventArgs e)
        {
            #region Use SQLite
            using (DB db = new DB("OpenALPRQueueMilestone", true))
            {
                Settings settings = db.GetSettings();

                tsslAlert.Text = $"Last saved settings: {settings.Created}";
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
                chkUseUTC.Checked = settings.UseUTC;
            }
            #endregion

            chkNetworkService.Checked = (string.IsNullOrEmpty(txtLogin.Text) || string.IsNullOrWhiteSpace(txtLogin.Text)) &&
                (string.IsNullOrEmpty(txtPassword.Text) || string.IsNullOrWhiteSpace(txtPassword.Text));
            txtLogin.Enabled = !chkNetworkService.Checked;
            txtPassword.Enabled = !chkNetworkService.Checked;

            btnLogin.Focus();
        }
    }
}
