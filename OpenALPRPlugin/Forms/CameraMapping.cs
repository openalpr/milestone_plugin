using OpenALPRPlugin.Background;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using VideoOS.Platform;
using System.Linq;
using OpenALPRPlugin.Client;

namespace OpenALPRPlugin.Forms
{
    public partial class CameraMapping : Form
    {
        internal IDictionary<string, OpenALPRCameraName> dictionary;
        internal bool Saved;

        public CameraMapping()
        {
            InitializeComponent();
            ClientControl.Instance.RegisterUIControlForAutoTheming(this);
            dictionary = new Dictionary<string, OpenALPRCameraName>();
        }

        private void CameraMapping_Load(object sender, EventArgs e)
        {
            var cameraItems = new List<Item>();
            OpenALPRBackgroundPlugin.FindAllCameras(Configuration.Instance.GetItemsByKind(Kind.Camera), cameraItems);
            for (int i = 0; i < cameraItems.Count; i++)
            {
                var cameraPairControl = new CameraPairControl();
                cameraPairControl.TxtMilestoneCameraName.Text = cameraItems[i].Name;
                var mapping = dictionary.FirstOrDefault(m => m.Key == cameraItems[i].Name);
                if (!string.IsNullOrEmpty(mapping.Key))
                {
                    cameraPairControl.TxtALPRCameraName.Text = mapping.Value.OpenALPRname;
                    cameraPairControl.TxtALPRCameraId.Text = mapping.Value.OpenALPRid;
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
                    var currentALPRCameraName = cameraPairControl.TxtALPRCameraName.Text;
                    var currentALPRCameraId = cameraPairControl.TxtALPRCameraId.Text;

                    if (!string.IsNullOrEmpty(currentMilestoneCameraName))
                    {
                        var mapping = dictionary.FirstOrDefault(m => m.Key == currentMilestoneCameraName);
                        if (string.IsNullOrEmpty(mapping.Key))
                            dictionary.Add(currentMilestoneCameraName, new OpenALPRCameraName { OpenALPRname = currentALPRCameraName, OpenALPRid = currentALPRCameraId });
                        else
                            dictionary[currentMilestoneCameraName] = new OpenALPRCameraName { OpenALPRname = currentALPRCameraName, OpenALPRid = currentALPRCameraId };
                    }
                }
            }
            Saved = true;
            Hide();
        }
    }
}