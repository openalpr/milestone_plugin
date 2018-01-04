using RookArchive.Archiving;
using SegStor.Background;
using SegStor.Forms;
using SegStor.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using VideoOS.Platform;
using VideoOS.Platform.Admin;
using VideoOS.Platform.Client;
using VideoOS.Platform.Data;
using VideoOS.Platform.UI;

namespace SegStor.Client
{
    public partial class ExportControl : ViewItemUserControl
    {
        public event EventHandler RightClickEvent; //Signals that the form is right clicked

        internal event EventHandler<EventArgs> ControlCanceled;
        internal event EventHandler<JobSettings> NewBookmarkJob;

        //private SegStorViewItemManager _viewItemManager;
        private string password;
        private JobSettings jobSettings;
        private IList<string> jobsName;
        private object[] references;

        public ExportControl()//bool edit, IList<string> jobsName, SegStorViewItemManager viewItemManager, JobSettings job = null)
        {
            //_viewItemManager = viewItemManager;
            InitializeComponent();

            //if (edit)
            //    txtJobName.ReadOnly = true;

            //this.jobsName = jobsName;
            ClientControl.Instance.RegisterUIControlForAutoTheming(this);

            Initialize();
            //jobSettings = job;
        }

        public override void Init()
        {
            base.Init();
            ClientControl.Instance.RegisterUIControlForAutoTheming(this);
        }

        private void Initialize()
        {
            var fullDateTimePattern = CultureInfo.CurrentCulture.DateTimeFormat.FullDateTimePattern;


            cboPasswordFormat.Items.AddRange(new string[] { "56-bit DES", "128-bit AES", "192-bit AES", "256-bit AES" });
            cboPasswordFormat.SelectedIndex = 1;

            cboContent.Items.AddRange(new string[] { "Video only", "Audio only", "Video and Audio" });
            cboContent.SelectedIndex = 2;

            cboCodec.Items.AddRange(new string[] { "Microsoft YUV", "Intel IYUV codec", "Microsoft RLE", "Microsoft Video 1" });
            cboCodec.SelectedIndex = 1;

        }

        private void ExportControl_Load(object sender, EventArgs e)
        {
            //CheckServerVersionForVMDAreaMask();
            //var EnvironmentProduct = EnvironmentManager.Instance.EnvironmentProduct;// Milestone XProtect Smart Client9.0c
            //if (ClientControl.Instance.Theme.ThemeType == ThemeType.Light)

            cboContent.ThemeAware = true;
            cboPasswordFormat.ThemeAware = true;
            cboCodec.ThemeAware = true;

            cboContent.ForeColor = Color.Black;
            cboPasswordFormat.ForeColor = Color.Black;
            cboCodec.ForeColor = Color.Black;

            lblQuality.Text = hScrollBarAdv1.Value.ToString();


            (hScrollBarAdv1 as Control).BackColor = ClientControl.Instance.Theme.BackgroundColor;
            (hScrollBarAdv1 as Control).ForeColor = ClientControl.Instance.Theme.ViewItemHeaderTextColor;

            //dtPickerStartSearchFrom.BackColor = ClientControl.Instance.Theme.BackgroundColor;
            //dtPickerStartSearchFrom.ForeColor = ClientControl.Instance.Theme.ViewItemHeaderTextColor;

            //dtPickerSearchExpiryDate.BackColor = ClientControl.Instance.Theme.BackgroundColor;
            //dtPickerSearchExpiryDate.ForeColor = ClientControl.Instance.Theme.ViewItemHeaderTextColor;

            if (chkXProtectFormat.Checked)
                Expand(btnXProtectFormat);
            else if (chkMediaPlayerFormat.Checked)
                Expand(btnMediaPlayerFormat);
            else
                Expand(btnStillImages);

            if (jobSettings == null)
                jobSettings = new JobSettings();
            else
            {
                password = jobSettings.Password;


                foreach (var c in jobSettings.GetCameras())
                {
                    var camera = Configuration.Instance.GetItem(EnvironmentManager.Instance.MasterSite.ServerId, c.FQID.ObjectId, Kind.Camera);
                    if (camera != null)
                    {
                        var item = new ListViewItem { Name = camera.Name, Text = camera.Name, Tag = camera };
                    }
                }

                chkXProtectFormat.Checked = jobSettings.IsXProtectFormat;
                panelBlk.Enabled = jobSettings.IsXProtectFormat;
                chkIncludePlayer.Checked = jobSettings.IncludePlayer;
                chkEncryption.Checked = jobSettings.Encryption;
                cboPasswordFormat.Enabled = jobSettings.Encryption;
                cboPasswordFormat.SelectedIndex = (int)jobSettings.EncryptionStrength;
                chkSignExport.Checked = jobSettings.SignExport;
                chkMergeAllCameras.Checked = jobSettings.MergeAllCameras;

                // Media player
                chkMediaPlayerFormat.Checked = jobSettings.IsMediaPlayerFormat;
                panelAvi.Enabled = jobSettings.IsMediaPlayerFormat;

                cboContent.SelectedIndex = (int)jobSettings.Content;
                cboCodec.SelectedIndex = (int)jobSettings.Codec;

                chkAutoSplitExportFile.Checked = jobSettings.AutoSplitExportFile;
                intMaxAVIFileSize.Enabled = jobSettings.AutoSplitExportFile;
                intMaxAVIFileSize.Value = jobSettings.MaxAVIFileSize;
                chkIncludeTimestamps.Checked = jobSettings.Timestamp;
                intAudioSampleDepth.Value = jobSettings.AudioSampleDepth;
                intAudioSampleRate.Value = jobSettings.AudioSampleRate;
                intChannels.Value = jobSettings.Channels;
                intFrameRate.Value = jobSettings.FrameRate;
                intWidth.Value = jobSettings.Height;
                intHeight.Value = jobSettings.Width;
                chkUseFrameDefaultValues.Checked = jobSettings.UseFrameDefaultValues;

                chkMatroskaMkv.Checked = jobSettings.IsMkvFormat;

                chkStillImages.Checked = jobSettings.IsStillImages;
                panelImg.Enabled = jobSettings.IsStillImages;

                hScrollBarAdv1.Value = jobSettings.Quality;
                lblQuality.Text = hScrollBarAdv1.Value.ToString();

                numSampleImages.Value = 5000;
                if (jobSettings.StillImagesOption == 1)
                    rbAllImages.Checked = true;
                else if (jobSettings.StillImagesOption == 2)
                    rbMinImages.Checked = true;
                else
                {
                    rbSampleImages.Checked = true;
                    numSampleImages.Value = jobSettings.SampleImages;
                }

                numSampleImages.Enabled = rbSampleImages.Checked;
                intIgnoreImagesMinCount.Value = jobSettings.IgnoreImagesMinCount;
                chkAllowUpscaling.Checked = jobSettings.AllowUpscaling;
                intImageWidth.Value = jobSettings.ImageWidth;
                intImageHeight.Value = jobSettings.ImageHeight;
                chkKeepAspectRatio.Checked = jobSettings.KeepAspectRatio;
                chkFillSpace.Checked = jobSettings.FillSpace;
            }
        }

        #region Plug-in native methods

        private void ViewItemUserControlMouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                FireClickEvent();
            else if (e.Button == MouseButtons.Right)
                FireRightClickEvent(e);
        }

        /// <summary>
        /// Activates the RightClickEvent
        /// </summary>
        /// <param name="e">Event args</param>
        protected virtual void FireRightClickEvent(EventArgs e)
        {
            RightClickEvent?.Invoke(this, e);
        }

        private object ThemeChangedIndicationHandler(VideoOS.Platform.Messaging.Message message, FQID destination, FQID source)
        {
            Selected = _selected;
            return null;
        }

        /// <summary>
        /// Gets boolean indicating whether the view item can be maximized or not. <br/>
        /// The content holder should implement the click and double click events even if it is not maximizable. 
        /// </summary>
        public override bool Maximizable
        {
            get { return true; }
        }

        /// <summary>
        /// Tell if ViewItem is selectable
        /// </summary>
        public override bool Selectable
        {
            get { return true; }
        }

        /// <summary>
        /// Make support for Theme colors to show if this ViewItem is selected or not.
        /// </summary>
        public override bool Selected
        {
            get { return base.Selected; }
            set
            {
                base.Selected = value;
                if (value)
                {
                    //panelHeader.BackColor = ClientControl.Instance.Theme.ViewItemSelectedHeaderColor;
                    //panelHeader.ForeColor = ClientControl.Instance.Theme.ViewItemSelectedHeaderTextColor;
                }
                else
                {
                    //panelHeader.BackColor = ClientControl.Instance.Theme.ViewItemHeaderColor;
                    //panelHeader.ForeColor = ClientControl.Instance.Theme.ViewItemHeaderTextColor;
                }
            }
        }

        #endregion Plug-in native methods

        #region Camera

        private void btnSelectCamera_Click(object sender, EventArgs e)
        {
            if (Common.IsHostedbySmartClient)
            {
                using (var form = new ItemPickerForm())
                {
                    form.CategoryFilter = Category.VideoIn;
                    form.AutoAccept = true;
                    form.Init(Configuration.Instance.GetItemsByKind(Kind.Camera));
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        if (form.SelectedItem != null)
                        {
                            var item = new ListViewItem { Name = form.SelectedItem.Name, Text = form.SelectedItem.Name, Tag = form.SelectedItem };//.FQID };

                           
                        }
                    }
                }
            }
        }

       

        #endregion Camera

        private void btnHelp_Click(object sender, EventArgs e)
        {
            const string helpFile = "StorageQuest SegStor - User Manual.pdf";

            var helpFilePath = Path.Combine(Environment.CurrentDirectory, helpFile);
#if DEBUG
            helpFilePath = Path.Combine(@"C:\Program Files\VideoOS\MIPPlugins\StorageQuest SegStor\", helpFile);
#endif

            if (File.Exists(helpFilePath))
            {
                try
                {
                    using (var p = new Process())
                    {
                        p.StartInfo.FileName = helpFilePath;
                        p.Start();
                    }
                }
                catch (Exception ex)
                {
                    EnvironmentManager.Instance.ExceptionDialog("ExportControl", "btnHelp_Click", ex);
                    SegStorPluginDefinition.Logger.Log.Error(null, ex);
                }
            }
            else
            {
                MessageBox.Show("Could not find the help file.", Common.Abbreviation, MessageBoxButtons.OK);
            }
        }

        private void btnSaveOnly_Click(object sender, EventArgs e)
        {
            if (VerifyInput())
                Save(false);
        }

        private void btnStartExport_Click(object sender, EventArgs e)
        {
            if (VerifyInput())
                Save(true);
        }

        private void btnEditListAndExportAll_Click(object sender, EventArgs e)
        {
            if (VerifyInput())
            {
                if (EditList())
                    Save(true, true);
            }
        }

        private bool EditList()
        {

            try
            {
                using (var exportList = new ExportList())
                {
                    var deviceTypesGuids = new Guid[] { Kind.Camera, Kind.Microphone, Kind.Speaker };
                   // exportList.Initialize(txtJobName.Text, chkBookmarks.Checked, chkMyBookmarksOnly.Checked, chkEvidenceLocks.Checked, chkMyEvidenceLockOnly.Checked, chkSequences.Checked, dtPickerStartSearchFrom.Value, dtPickerSearchExpiryDate.Value, items, deviceTypesGuids);
                    exportList.ShowDialog(this);
                    if (!exportList.Canceld)
                    {
                        var checkedCount = exportList.CheckedCount;
                        references = exportList.References();
                        exportList.Close();
                        return checkedCount != 0;
                    }

                    exportList.Close();
                }
            }
            catch (Exception ex)
            {
                SegStorPluginDefinition.Logger.Log.Error(null, ex);
            }

            return false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SaveSettings();
            ControlCanceled?.Invoke(this, new EventArgs());
        }

        private bool Save(bool startExport, bool useReferencesList = false)
        {
            if (jobSettings == null)
                jobSettings = new JobSettings();
            else
                jobSettings.ClearCameras();

            jobSettings.StopRunning = false;
            jobSettings.StartExport = startExport;
            jobSettings.useReferencesList = useReferencesList;
            jobSettings.References = references;

            // XProtect
            jobSettings.IsXProtectFormat = chkXProtectFormat.Checked;
            jobSettings.IncludePlayer = chkIncludePlayer.Checked;
            jobSettings.Encryption = chkEncryption.Checked;
            jobSettings.EncryptionStrength = (EncryptionStrength)cboPasswordFormat.SelectedIndex;
            jobSettings.SignExport = chkSignExport.Checked;
            jobSettings.MergeAllCameras = chkMergeAllCameras.Checked;
            jobSettings.Password = password;

            jobSettings.IsMkvFormat = chkMatroskaMkv.Checked;

            // Media player
            jobSettings.IsMediaPlayerFormat = chkMediaPlayerFormat.Checked;
            jobSettings.AudioSampleDepth = (int)intAudioSampleDepth.Value;
            jobSettings.AudioSampleRate = (int)intAudioSampleRate.Value;
            jobSettings.AutoSplitExportFile = chkAutoSplitExportFile.Checked;
            jobSettings.MaxAVIFileSize = (int)intMaxAVIFileSize.Value;
            jobSettings.Channels = (int)intChannels.Value;
            jobSettings.Codec = (AVICodec)cboCodec.SelectedIndex;
            jobSettings.FrameRate = (int)intFrameRate.Value;
            jobSettings.Height = (int)intWidth.Value;
            jobSettings.Width = (int)intHeight.Value;
            jobSettings.UseFrameDefaultValues = chkUseFrameDefaultValues.Checked;
            jobSettings.Timestamp = chkIncludeTimestamps.Checked;
            jobSettings.Content = (ExportContent)cboContent.SelectedIndex;

            // Still images
            jobSettings.IsStillImages = chkStillImages.Checked;
            jobSettings.AllowUpscaling = chkAllowUpscaling.Checked;
            jobSettings.Quality = hScrollBarAdv1.Value;
            jobSettings.ImageWidth = (int)intImageWidth.Value;
            jobSettings.ImageHeight = (int)intImageHeight.Value;
            jobSettings.KeepAspectRatio = chkKeepAspectRatio.Checked;
            jobSettings.FillSpace = chkFillSpace.Checked;
            jobSettings.StillImagesOption = rbAllImages.Checked ? 1 : rbMinImages.Checked ? 2 : 3;
            jobSettings.SampleImages = rbAllImages.Checked ? int.MaxValue : (int)numSampleImages.Value;
            jobSettings.IgnoreImagesMinCount = (int)intIgnoreImagesMinCount.Value;

            //foreach (ListViewItem viewItem in lsvCameras.Items)
            //{
            //    var item = viewItem.Tag as Item;
            //    if (item != null)
            //        jobSettings.AddCamera(item);
            //}

            NewBookmarkJob?.Invoke(this, jobSettings);
            SaveSettings();

            return true;
        }

        private bool VerifyInput()
        {
            //txtJobName.Text = txtJobName.Text.Trim();

            //if (txtJobName.Text.Length == 0)
            //{
            //    MessageBox.Show("Job name can not be empty.", Common.Abbreviation, MessageBoxButtons.OK);
            //    txtJobName.Focus();
            //    return false;
            //}

            // new
            //if (!txtJobName.ReadOnly && jobsName.Contains(txtJobName.Text))
            //{
            //    MessageBox.Show("Job name already used. Please choose another name.", Common.Abbreviation, MessageBoxButtons.OK);
            //    txtJobName.Focus();
            //    return false;
            //}

            //if (dtPickerStartSearchFrom.Value > dtPickerSearchExpiryDate.Value)
            //{
            //    //EnvironmentManager.Instance.ExceptionDialog("ExportControl", "VerifyInput", null);
            //    MessageBox.Show("Search expired should exceed the start time.", Common.Abbreviation, MessageBoxButtons.OK);
            //    dtPickerSearchExpiryDate.Focus();
            //    return false;
            //}

            //if (lsvCameras.Items.Count == 0)
            //{
            //    MessageBox.Show("Please select one or more Camera.", Common.Abbreviation, MessageBoxButtons.OK);
            //    lsvCameras.Focus();
            //    return false;
            //}

            //if (!chkEvidenceLocks.Checked && !chkBookmarks.Checked && !chkSequences.Checked)
            //{
            //    MessageBox.Show("Please select at least one selection type.", Common.Abbreviation, MessageBoxButtons.OK);
            //    chkBookmarks.Focus();
            //    return false;
            //}

            if (!chkXProtectFormat.Checked && !chkMediaPlayerFormat.Checked && !chkMatroskaMkv.Checked && !chkStillImages.Checked)
            {
                MessageBox.Show("Please select at least one export format type.", Common.Abbreviation, MessageBoxButtons.OK);
                return false;
            }

            if (chkEncryption.Checked)
            {
                if (string.IsNullOrEmpty(password))
                {
                    using (var form = new Password(password))
                    {
                        form.ShowDialog();
                        if (form.DialogResult == DialogResult.OK)
                            password = form.PasswordString;
                    }
                }

                if (string.IsNullOrEmpty(password))
                    return false;
            }

            return true;
        }

        private void Expand(object sender)
        {
            const int panelStillImagesHeight = 150;
            const int panelXProtectFormatHeight = 160;
            const int panelMediaPlayerFormatHeight = 200;

            var buttton = sender as Button;
            switch (buttton.Name)
            {
                case nameof(btnXProtectFormat):
                    panelXProtectFormat.Height = panelXProtectFormatHeight;
                    panelMediaPlayerFormat.Location = new Point(panelMediaPlayerFormat.Location.X, panelXProtectFormat.Location.Y + panelXProtectFormat.Height + 6);
                    panelMediaPlayerFormat.Height = panelMediaPlayerFormatHeight - 150;
                    panelStillImages.Location = new Point(panelStillImages.Location.X, panelMediaPlayerFormat.Location.Y + panelMediaPlayerFormat.Height + 6);
                    panelStillImages.Height = panelStillImagesHeight - 95;

                    break;

                case nameof(btnMediaPlayerFormat):
                    panelXProtectFormat.Height = panelXProtectFormatHeight - 110;
                    panelMediaPlayerFormat.Location = new Point(panelMediaPlayerFormat.Location.X, panelXProtectFormat.Location.Y + panelXProtectFormat.Height);
                    panelMediaPlayerFormat.Height = panelMediaPlayerFormatHeight;
                    panelStillImages.Location = new Point(panelStillImages.Location.X, panelMediaPlayerFormat.Location.Y + panelMediaPlayerFormat.Height);
                    panelStillImages.Height = panelStillImagesHeight - 95;

                    break;

                default:
                    panelXProtectFormat.Height = panelXProtectFormatHeight - 110;
                    panelMediaPlayerFormat.Location = new Point(panelMediaPlayerFormat.Location.X, panelXProtectFormat.Location.Y + panelXProtectFormat.Height);
                    panelMediaPlayerFormat.Height = panelMediaPlayerFormatHeight - 150;
                    panelStillImages.Location = new Point(panelStillImages.Location.X, panelMediaPlayerFormat.Location.Y + panelMediaPlayerFormat.Height);
                    panelStillImages.Height = panelStillImagesHeight;

                    break;
            }
        }

        private void SaveSettings()
        {
            try
            {
                //Settings1.Default.ExportDestination = txtExportDestination.Text;
                //Settings1.Default.Save();
            }
            catch (Exception ex)
            {
                EnvironmentManager.Instance.Log(true, "Save Settings", ex.Message);
            }
        }

        #region Archive


        private void btnBrowseDestination_Click(object sender, EventArgs e)
        {
            using (var folderBrowserDialog1 = new FolderBrowserDialog())
            {
                folderBrowserDialog1.Description = "Specify the folder that you want to export to.";
                folderBrowserDialog1.ShowNewFolderButton = true;

                //if (string.IsNullOrEmpty(txtExportDestination.Text))
                //    folderBrowserDialog1.RootFolder = Environment.SpecialFolder.Desktop;
                //else
                //    folderBrowserDialog1.SelectedPath = txtExportDestination.Text;

                //if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                //{
                //    txtExportDestination.Text = folderBrowserDialog1.SelectedPath;
                //    Settings1.Default.ExportDestination = txtExportDestination.Text;
                //    Settings1.Default.Save();
                //}
            }
        }

        private void btnArchiveSettings_Click(object sender, EventArgs e)
        {
            using (var archiveSetting = new ArchiveSetting())
            {
                archiveSetting.Initialize(jobSettings);
                archiveSetting.ShowDialog(this);
                if (!archiveSetting.Cancled)
                    archiveSetting.GetValues(jobSettings);
                archiveSetting.Close();
            }
        }

        #endregion Archive

        #region XProtect format

        private void btnXProtectFormat_Click(object sender, EventArgs e)
        {
            Expand(sender);
        }

        private void chkXProtectFormat_CheckedChanged(object sender, EventArgs e)
        {
            panelBlk.Enabled = chkXProtectFormat.Checked;
        }

        private void chkEncryption_CheckedChanged(object sender, EventArgs e)
        {
            cboPasswordFormat.Enabled = chkEncryption.Checked;
        }

        #endregion XProtect format

        #region Media Player format

        private void btnMediaPlayerFormat_Click(object sender, EventArgs e)
        {
            Expand(sender);
        }

        private void chkMediaPlayerFormat_CheckedChanged(object sender, EventArgs e)
        {
            panelAvi.Enabled = chkMediaPlayerFormat.Checked;
        }

        #endregion Media Player format

        #region Still Images

        private void btnStillImages_Click(object sender, EventArgs e)
        {
            Expand(sender);
        }

        private void chkStillImages_CheckedChanged(object sender, EventArgs e)
        {
            panelImg.Enabled = chkStillImages.Enabled;
        }

        private void hScrollBarAdv1_Scroll(object sender, ScrollEventArgs e)
        {
            if (hScrollBarAdv1.Value > 100)
                hScrollBarAdv1.Value = 100;

            lblQuality.Text = hScrollBarAdv1.Value.ToString();
        }

        #endregion Still Images

        public override void Close()
        {
            base.Close();
            SaveSettings();
        }

        public override void Print()
        {
            Print("SegStor", "www.StorageQuest.com");
        }

        private void chkUseFrameDefaultValues_CheckedChanged(object sender, EventArgs e)
        {
            intFrameRate.Enabled = !chkUseFrameDefaultValues.Checked;
            intWidth.Enabled = !chkUseFrameDefaultValues.Checked;
            intHeight.Enabled = !chkUseFrameDefaultValues.Checked;
        }

        private void chkAutoSplitExportFile_CheckedChanged(object sender, EventArgs e)
        {
            intMaxAVIFileSize.Enabled = chkAutoSplitExportFile.Checked;
        }

        private void rbSampleImages_CheckedChanged(object sender, EventArgs e)
        {
            numSampleImages.Enabled = rbSampleImages.Checked;
        }
    }
}

//private void lsvItems_MouseDown(object sender, MouseEventArgs e)
//{
//    if (e.X <= 12)
//    {
//        // Get the item at the mouse pointer.
//        ListViewHitTestInfo info = this.listViewEx1.HitTest(e.X, e.Y);

//        ListViewItem.ListViewSubItem subItem = null;

//        // Get the subitem 120 pixels to the right.
//        if (info != null)
//            if (info.Item != null)
//            {
//                subItem = info.Item.GetSubItemAt(e.X + 120, e.Y);
//                listViewEx1.Items.Remove(info.Item);
//            }

//    }
//}