using OpenALPRPlugin.Background;
using OpenALPRPlugin.Client;
using OpenALPRPlugin.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using VideoOS.Platform;

namespace OpenALPRPlugin.Forms
{
    public partial class CameraMapping : Form
    {
        internal IList<OpenALPRmilestoneCameraName> CameraList;
        internal bool Saved;

        public CameraMapping()
        {
            InitializeComponent();
            ClientControl.Instance.RegisterUIControlForAutoTheming(this);
            CameraList = new List<OpenALPRmilestoneCameraName> ();
        }

        private void CameraMapping_Load(object sender, EventArgs e)
        {
            var namesList = new List<KeyValuePair<string, string>>();
            OpenALPRLNameHelper.FillCameraNameList(namesList);

            if (namesList.Count != 0)
            {
                namesList = namesList.OrderBy(o => o.Key).ToList();
                namesList.Insert(0, new KeyValuePair<string, string>("-1", "No ALPR mapping"));
            }

            var cameraItems = new List<Item>();
            OpenALPRBackgroundPlugin.FindAllCameras(Configuration.Instance.GetItemsByKind(Kind.Camera), cameraItems);
            for (int i = 0; i < cameraItems.Count; i++)
            {
                var cameraPairControl = new CameraPairControl();

                cameraPairControl.cboName.DataSource = new BindingSource(namesList, null);
                cameraPairControl.cboName.DisplayMember = "Value";
                cameraPairControl.cboName.ValueMember = "Key";

                cameraPairControl.TxtMilestoneCameraName.Text = cameraItems[i].Name;
                var mapping = CameraList.FirstOrDefault(m => m.MilestoneName == cameraItems[i].Name);
                if (mapping != null)
                {
                    var index = namesList.FindIndex(a => a.Key == mapping.OpenALPRId);
                    if (index > -1)
                        cameraPairControl.cboName.SelectedIndex = index;
                }
     
                flowLayoutPanel1.Controls.Add(cameraPairControl);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < flowLayoutPanel1.Controls.Count; i++)
            {
                var cameraPairControl = flowLayoutPanel1.Controls[i] as CameraPairControl;
                if (cameraPairControl != null)
                {
                    var currentMilestoneCameraName = cameraPairControl.TxtMilestoneCameraName.Text;

                    string currentALPRCameraId = string.Empty;
                    string currentALPRCameraName = string.Empty;

                    if (cameraPairControl.cboName.SelectedItem != null)
                    {
                        currentALPRCameraId = ((KeyValuePair<string, string>)cameraPairControl.cboName.SelectedItem).Key;
                        if (currentALPRCameraId == "-1")
                            currentALPRCameraId = string.Empty;
                        else
                            currentALPRCameraName = ((KeyValuePair<string, string>)cameraPairControl.cboName.SelectedItem).Value;
                    }

                    if (!string.IsNullOrEmpty(currentMilestoneCameraName))
                    {
                        var mapping = CameraList.FirstOrDefault(m => m.MilestoneName == currentMilestoneCameraName);
                        if (mapping == null)
                            CameraList.Add(new OpenALPRmilestoneCameraName { MilestoneName = currentMilestoneCameraName, OpenALPRname = currentALPRCameraName, OpenALPRId = currentALPRCameraId });
                        else
                        {
                            mapping.OpenALPRname = currentALPRCameraName;
                            mapping.OpenALPRId = currentALPRCameraId;
                        }
                    }
                }
            }
            Saved = true;
            Hide();
        }
    }
}