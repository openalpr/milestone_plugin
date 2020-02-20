// Copyright OpenALPR Technology, Inc. 2018

using System;
using System.Windows.Forms;
using VideoOS.Platform;

namespace OpenALPRPlugin.Forms
{
    public partial class EditBookmark : Form
    {
        internal string BeginTime;
        internal string EndTime;
        internal string Header;
        internal string Description;
        internal string Plate;
        internal string Vehicle;
        internal string Timestamp;
        internal string SiteName;
        internal bool saved;

        public EditBookmark()
        {
            InitializeComponent();
            ClientControl.Instance.RegisterUIControlForAutoTheming(this);
        }

        private void EditBookmark_Load(object sender, EventArgs e)
        {
            txtBeginTime.Text = BeginTime;
            txtEndTime.Text = EndTime;
            txtHeader.Text = Header;
            txtDescription.Text = Description;
            txtPlate.Text = Plate;
            txtVehicle.Text = Vehicle;
            txtTimestamp.Text = Timestamp;
            txtSiteName.Text = SiteName;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            BeginTime = txtBeginTime.Text;
            EndTime = txtEndTime.Text;
            Header = txtHeader.Text;
            Description = txtDescription.Text;
            Plate = txtPlate.Text;
            Vehicle = txtVehicle.Text;
            Timestamp = txtTimestamp.Text;
            SiteName = txtSiteName.Text;

            saved = true;
            Hide();
        }
    }
}
