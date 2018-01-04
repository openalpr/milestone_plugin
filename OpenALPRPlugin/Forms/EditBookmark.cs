using System;
using System.Windows.Forms;
using VideoOS.Platform;

namespace OpenALPRPlugin.Forms
{
    public partial class EditBookmark : Form
    {
        internal string BeginTime;
        internal string EndTime;
        internal string Reference;
        internal string Header;
        internal string Description;
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
            txtReference.Text = Reference;
            txtHeader.Text = Header;
            txtDescription.Text = Description;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            BeginTime = txtBeginTime.Text;
            EndTime = txtEndTime.Text;
            Reference = txtReference.Text;
            Header = txtHeader.Text;
            Description = txtDescription.Text;

            saved = true;
            Hide();
        }
    }
}
