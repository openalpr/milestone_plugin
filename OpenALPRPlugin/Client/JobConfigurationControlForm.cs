using DevComponents.DotNetBar;
using RookArchive.Archiving;
using SegStor;
using SegStor.Background;
using SegStor.Forms;
using SegStor.Properties;
using SegStor.Utility;
using SegStorUserControls;
using StorageQuest.Sam.SamCommon.ExtendedDirectoryInfos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using VideoOS.Platform;
using VideoOS.Platform.Admin;
using VideoOS.Platform.Data;
using VideoOS.Platform.UI;
using ProxyServer = VideoOS.Platform.SDK.Proxy.Server;

namespace RookArchive.Admin
{
    public partial class JobConfigurationControlForm : Office2007Form
    {
        internal JobSettings CurrentJob;
        internal bool IsCancled = true;

        private DataDownloader dataDownloader;
        private IList<string> jobsName;
        private bool isNew;
        private bool stopSearching;

        public JobConfigurationControlForm()
        {
            InitializeComponent();

            columnHeader.Width = lsvCameras.Width - 100;

            if (EnvironmentManager.Instance.MasterSite.ServerId.ServerType != ServerId.CorporateManagementServerType)
            {
                Logger.Log.Warn("Evidence locks are not supported on this product.");
                rdoEvidenceLocks.Checked = false;
                rdoEvidenceLocks.Enabled = false;
            }

            var fullDateTimePattern = CultureInfo.InvariantCulture.DateTimeFormat.FullDateTimePattern;

            dtPickerStartSearchFrom.CustomFormat = fullDateTimePattern;
            dtPickerStartSearchTo.CustomFormat = fullDateTimePattern;

            cboUdfFormat.Items.AddRange(new string[] { "102", "150", "150_UNICODE", "200", "201", "250", "260" });
            cboUdfFormat.SelectedIndex = 1;
            cboMediaType.Items.AddRange(new string[] { "CDR", "DVDR", "DVDR-DL", "DVDR-BD", "DVDR-BD-DL", "DVDR-BD-TL" });

            var index = cboMediaType.FindStringExact("DVDR-BD");
            cboMediaType.SelectedIndex = index != -1 ? index : 0;
        }

        private void JobConfigurationControlForm_Load(object sender, EventArgs e)
        {
            ClientControl.Instance.RegisterUIControlForAutoTheming(this);

            cboMediaType.ThemeAware = true;
            cboMediaType.ForeColor = Color.Black;

            cboUdfFormat.ThemeAware = true;
            cboUdfFormat.ForeColor = Color.Black;

            if (isNew)
                txtJobName.Focus();
            else
            {
                txtJobName.ReadOnly = true;
                wizard1.Focus();
            }
        }

        internal void FillContent(JobSettings job, IList<string> jobsName, bool isNewJob)
        {
            CurrentJob = job;
            this.jobsName = jobsName;
            isNew = isNewJob;

            if (CurrentJob == null)
                CurrentJob = new JobSettings();

            if (!isNewJob)
                Text = TitleText = $"Edit job: {CurrentJob.Name}";

            foreach (var c in CurrentJob.GetCameras())
            {
                var camera = Configuration.Instance.GetItem(EnvironmentManager.Instance.MasterSite.ServerId, c.FQID.ObjectId, Kind.Camera);
                if (camera != null)
                {
                    var item = new ListViewItem { Name = camera.Name, Text = camera.Name, Tag = camera };
                    lsvCameras.Items.Add(item);
                }
            }

            CurrentJob.StartExport = false;
            CurrentJob.UseReferencesList = false;

            txtJobName.Text = CurrentJob.Name;
            txtDescription.Text = CurrentJob.Description;

            rdoSequences.Checked = CurrentJob.IsSequences;
            chkMergerSequenceExport.Checked = CurrentJob.MergeAllSequenceExport;
            intSegmentExport.Value = CurrentJob.SegmentExport;
            rdoBookmarks.Checked = CurrentJob.IsBookmark;
            rdoEvidenceLocks.Checked = CurrentJob.IsEvidenceLock;
            chkMyBookmarksOnly.Checked = CurrentJob.MyBookmarksOnly;
            chkMyEvidenceLockOnly.Checked = CurrentJob.MyEvidenceLock;
            chkAllELCameras.Checked = CurrentJob.AllEvidenceLockCameras;
            txtSearch.Text = CurrentJob.Search;

            dtPickerStartSearchFrom.Value = CurrentJob.StartTimeUniversal.ToLocalTime();
            dtPickerStartSearchTo.Value = CurrentJob.EndTimeUniversal.ToLocalTime();

            exportTypeControl.XProtectFormat = CurrentJob.IsXProtectFormat;
            exportTypeControl.IncludePlayer = CurrentJob.IncludePlayer;
            exportTypeControl.Encryption = CurrentJob.Encryption;
            exportTypeControl.EncryptionStrength = (int)CurrentJob.EncryptionStrength;
            exportTypeControl.SignExport = CurrentJob.SignExport;
            exportTypeControl.MergeAllCameras = CurrentJob.MergeAllCameras;

            exportTypeControl.MediaPlayerFormat = CurrentJob.IsMediaPlayerFormat;
            exportTypeControl.Content = (int)CurrentJob.Content;
            exportTypeControl.Codec = (int)CurrentJob.Codec;
            exportTypeControl.AutoSplitExportFile = CurrentJob.AutoSplitExportFile;
            exportTypeControl.MaxAVIFileSize = CurrentJob.MaxAVIFileSize;
            exportTypeControl.IncludeTimestamps = CurrentJob.Timestamp;
            exportTypeControl.AudioSampleDepth = CurrentJob.AudioSampleDepth;
            exportTypeControl.AudioSampleRate = CurrentJob.AudioSampleRate;
            exportTypeControl.Channels = CurrentJob.Channels;
            exportTypeControl.FrameRate = CurrentJob.FrameRate;
            exportTypeControl.FrameWidth = CurrentJob.FrameWidth;
            exportTypeControl.FrameHeight = CurrentJob.FrameHeight;
            exportTypeControl.UseFrameDefaultValues = CurrentJob.UseFrameDefaultValues;

            exportTypeControl.MatroskaMkv = CurrentJob.IsMkvFormat;

            exportTypeControl.StillImages = CurrentJob.IsStillImages;
            exportTypeControl.StillImagesOption = CurrentJob.StillImagesOption;
            exportTypeControl.SampleImages = CurrentJob.SampleImages;
            exportTypeControl.Quality = CurrentJob.Quality;
            exportTypeControl.IgnoreImagesMinCount = CurrentJob.IgnoreImagesMinCount;
            exportTypeControl.ImageWidth = CurrentJob.ImageWidth;
            exportTypeControl.ImageHeight = CurrentJob.ImageHeight;
            exportTypeControl.KeepAspectRatio = CurrentJob.KeepAspectRatio;
            exportTypeControl.FillSpace = CurrentJob.FillSpace;
            exportTypeControl.AllowUpscaling = CurrentJob.AllowUpscaling;

            var index = cboMediaType.FindStringExact(CurrentJob.MediaType);
            if (index != -1)
                cboMediaType.SelectedIndex = index;

            index = cboUdfFormat.FindStringExact(CurrentJob.UdfFormat);
            if (index != -1)
                cboUdfFormat.SelectedIndex = index;

            txtHelperApplicationPath.Text = CurrentJob.HelperApplicationPath;
            chkDeleteAfterArchiving.Checked = CurrentJob.DeleteAfterArchiving;
            txtPrintLabel.Text = CurrentJob.BtwFile;
            txtCustomerLogo.Text = CurrentJob.LogoFile;
            txtCustomerInfo.Text = CurrentJob.CompanyInfo;
            txtExportDestination.Text = CurrentJob.ExportDestination;
            txtUserName.Text = CurrentJob.NetworkUsername;
            txtPassword.Text = CurrentJob.NetworkPassword;
            txtDomain.Text = CurrentJob.NetworkDomain;

            btnTest.Enabled = txtExportDestination.Text.Trim().Length != 0 && txtUserName.Text.Trim().Length != 0;

            if (string.IsNullOrEmpty(txtExportDestination.Text))
                txtExportDestination.Text = Settings1.Default.ExportDestination;

            chkMyBookmarksOnly.Enabled = rdoBookmarks.Checked;
            panelEvidenceLock.Enabled = rdoEvidenceLocks.Checked;
        }

        #region Wizard Events

        private void wizard1_BackButtonClick(object sender, CancelEventArgs e)
        {
            switch (wizard1.SelectedPageIndex)
            {
                case 6:
                    wizard1.HelpButtonVisible = false;
                    break;
            }
        }

        private void wizard1_NextButtonClick(object sender, CancelEventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            Application.DoEvents();

            switch (wizard1.SelectedPageIndex)
            {
                case 0: // Job name and description
                    e.Cancel = !VerifyJobName();
                    break;

                case 1: // Search filter
                    

                    e.Cancel = !VerifySearchFilter();
                    break;

                case 2: // Type selection
                    var page = wizard1.WizardPages[3];

                    if (page != null)
                    {
                        if (rdoBookmarks.Checked)
                            page.PageTitle = page.Text = "Bookmarks";
                        else if (rdoEvidenceLocks.Checked)
                            page.PageTitle = page.Text = "Evidence Locks";
                        else
                            page.PageTitle = page.Text = "Sequences";
                    }
                    break;

                case 3: //  Bookmarks or Evidence locks list
                    break;

                case 4: // 
                    e.Cancel = !VerifyExportType();
                    break;

                case 5: // Target
                    e.Cancel = !VerifyTarget();
                    break;

                case 6: // Export settings
                    e.Cancel = !VerifyArchiving();
                    if (!e.Cancel)
                    {
                        ShowReview();
                        wizard1.HelpButtonVisible = true;
                        wizard1.HelpButtonText = "Save and Export";
                        wizard1.HelpButtonWidth = wizard1.NextButtonWidth * 2;
                    }
                    break;

                case 7: // Review
                    break;
            }

            Cursor = Cursors.Default;
        }

        private void wizard1_HelpButtonClick(object sender, CancelEventArgs e)
        {
            if (GetPassword())
            {
                CurrentJob.StartExport = true;
                CurrentJob.UseReferencesList = true;
                Save();
                IsCancled = false;
                Hide();
            }
        }

        private bool VerifyJobName()
        {
            if (txtJobName.Text.Trim().Length == 0)
            {
                MessageBoxEx.Show(this, "Job name should not be empty.", "Empty job name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (jobsName.Contains(txtJobName.Text.Trim()) && isNew)
            {
                MessageBoxEx.Show(this, "Job name already used. Please select a different job name.", "Job name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            var pattern = new Regex("[^a-zA-Z0-9-]");
            if (pattern.IsMatch(txtJobName.Text.Trim()))
            {
                MessageBoxEx.Show(this, "Invalid job name.\nPlease use numbers, letters, or dashes only.", "Job name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private bool VerifySearchFilter()
        {
            if (dtPickerStartSearchTo.Value < dtPickerStartSearchFrom.Value)
            {
                MessageBoxEx.Show(this, "Start date should be grater than end date.", "Searching date", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (lsvCameras.Items.Count == 0)
            {
                MessageBoxEx.Show(this, "You should select 1 camera at least.", "No camera selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private bool VerifyExportType()
        {
            if (!exportTypeControl.XProtectFormat && !exportTypeControl.MediaPlayerFormat && !exportTypeControl.MatroskaMkv && !exportTypeControl.StillImages)
            {
                MessageBoxEx.Show(this, "You should select atleast one export type format.", "Export type", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private bool VerifyTarget()
        {
            if (txtExportDestination.Text.Trim().Length == 0)
            {
                MessageBoxEx.Show(this, "Export detsination path cannot be empty.", "Export detsination", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private bool VerifyArchiving()
        {
            return true;
        }

        private void wizard1_FinishButtonClick(object sender, CancelEventArgs e)
        {
            if (GetPassword())
            {
                CurrentJob.StartExport = false;
                Save();
                IsCancled = false;
                Hide();
            }
        }

        private void ShowReview()
        {
            var review = new StringBuilder();
            review.AppendLine($"Job name: {txtJobName.Text.Trim()}");
            review.AppendLine($"Description: {txtDescription.Text.Trim()}");
            review.AppendLine();

            review.AppendLine($"\tstart time: {dtPickerStartSearchFrom.Value.ToString(SegStorPluginDefinition.TimeFormat)}");
            review.AppendLine($"\tend time: {dtPickerStartSearchTo.Value.ToString(SegStorPluginDefinition.TimeFormat)}");
            if (lsvCameras.Items.Count != 0)
            {
                review.AppendLine($"\tcamera(s) selected: {lsvCameras.Items.Count.ToString()}");
                for (int i = 0; i < lsvCameras.Items.Count; i++)
                {
                    var viewItem = lsvCameras.Items[i] as ListViewItem;
                    var item = viewItem.Tag as Item;
                    if (item != null)
                        review.AppendLine($"\t\t\t{item.Name}");
                }
            }

            review.AppendLine();

            if (rdoBookmarks.Checked)
            {
                review.AppendLine($"Bookmarks slected: {lsvData.CheckedIndices.Count.ToString()}");
                if (chkMyBookmarksOnly.Checked)
                    review.AppendLine("\tmy Bookmarks only");
            }
            else if (rdoEvidenceLocks.Checked)
            {
                review.AppendLine($"Evidence locks slected: {lsvData.CheckedIndices.Count.ToString()}");
                if (chkMyEvidenceLockOnly.Checked)
                    review.AppendLine("\tmy Evidence Locks only");

                if (chkAllELCameras.Checked)
                    review.AppendLine("\texport all Camera(s) found in the Evidence Locks");
            }

            if (txtSearch.Text.Trim().Length != 0)
                review.AppendLine($"\tsearch for: {txtSearch.Text.Trim()}");

            if (rdoSequences.Checked)
            {
                review.AppendLine($"Sequences slected: {lsvData.CheckedIndices.Count.ToString()}");
                if (chkMergerSequenceExport.Checked)
                    review.AppendLine($"\tMerge all exports into one Sequence per Camera");

                review.AppendLine($"\tSegment Export: {intSegmentExport.Value.ToString()} hours");
            }

            review.AppendLine();

            if (exportTypeControl.XProtectFormat)
            {
                review.AppendLine("XProtect format selected");
                if (exportTypeControl.IncludePlayer)
                    review.AppendLine("\tinclude player selected");

                if (exportTypeControl.Encryption)
                    review.AppendLine($"\tencryption selected: {exportTypeControl.EncryptionStrengthString}");

                if (exportTypeControl.SignExport)
                    review.AppendLine("\tsign export");

                if (exportTypeControl.MergeAllCameras)
                    review.AppendLine("\tmerge all cameras");

                review.AppendLine();
            }

            if (exportTypeControl.MediaPlayerFormat)
            {
                review.AppendLine("Media player format selected");
                review.AppendLine($"\tcontent: {exportTypeControl.ContentString}");
                review.AppendLine($"\tcodec: {exportTypeControl.CodecString}");

                if (exportTypeControl.AutoSplitExportFile)
                    review.AppendLine($"\tauto split export file: {exportTypeControl.MaxAVIFileSize.ToString()} MB");

                if (exportTypeControl.IncludeTimestamps)
                    review.AppendLine("\tinclude timestamps");

                review.AppendLine($"\taudio sample depth: {exportTypeControl.AudioSampleDepth.ToString()}");
                review.AppendLine($"\tsample rate: {exportTypeControl.AudioSampleRate.ToString()}");
                review.AppendLine($"\tchannels: {exportTypeControl.Channels.ToString()}");

                if (exportTypeControl.UseFrameDefaultValues)
                    review.AppendLine("\tuse frame default values");
                else
                {
                    review.AppendLine($"\tframe rate: {exportTypeControl.FrameRate.ToString()}");
                    review.AppendLine($"\tframe width: {exportTypeControl.FrameWidth.ToString()}");
                    review.AppendLine($"\tframe height: {exportTypeControl.FrameHeight.ToString()}");
                }

                review.AppendLine();
            }

            if (exportTypeControl.MatroskaMkv)
            {
                review.AppendLine("Matroska mkv format selected");
                review.AppendLine();
            }

            if (exportTypeControl.StillImages)
            {
                review.AppendLine("Still images format selected");

                var option = exportTypeControl.StillImagesOption;
                if (option == 0)
                    review.AppendLine("\texport a still image for each from in the selected period ");
                else if (option == 1)
                    review.AppendLine("\texport minimum still image for the selected period ");
                else
                    review.AppendLine($"\texport a sample of still images from the duration of the selected period: {exportTypeControl.SampleImages.ToString()}");

                review.AppendLine($"\tquality: {exportTypeControl.Quality.ToString()}");

                if (exportTypeControl.IgnoreImagesMinCount != 0)
                    review.AppendLine($"\tignore images minimum count: {exportTypeControl.IgnoreImagesMinCount.ToString()} images");

                review.AppendLine($"\timage height: {exportTypeControl.ImageHeight.ToString()}");
                review.AppendLine($"\timage width: {exportTypeControl.ImageWidth.ToString()}");

                if (exportTypeControl.KeepAspectRatio)
                    review.AppendLine("\tkeep aspect ratio");

                if (exportTypeControl.FillSpace)
                    review.AppendLine("\tfill space");

                if (exportTypeControl.AllowUpscaling)
                    review.AppendLine("\tallow upscaling");

                review.AppendLine();
            }

            review.AppendLine("Export target:");
            if (txtExportDestination.Text.Trim().Length != 0)
                review.AppendLine($"\texport destination: {txtExportDestination.Text.Trim()}");

            if (txtUserName.Text.Trim().Length != 0)
                review.AppendLine($"\tuser name: {txtUserName.Text.Trim()}");

            if (txtDomain.Text.Trim().Length != 0)
                review.AppendLine($"\thostname/domain: {txtDomain.Text.Trim()}");

            review.AppendLine();
            review.AppendLine("Export settings:");
            review.AppendLine($"\tmedia type: {cboMediaType.SelectedItem.ToString()}");
            review.AppendLine($"\tUDf format: {cboUdfFormat.SelectedItem.ToString()}");

            if (txtHelperApplicationPath.Text.Trim().Length != 0)
                review.AppendLine($"\tinclude folder: {txtHelperApplicationPath.Text.Trim()}");

            if (chkDeleteAfterArchiving.Checked)
                review.AppendLine("\tdelete export source files after archiving");

            if (txtPrintLabel.Text.Trim().Length != 0)
                review.AppendLine($"\tprint label file path: {txtPrintLabel.Text.Trim()}");

            if (txtCustomerLogo.Text.Trim().Length != 0)
                review.AppendLine($"\tlogo file path: {txtCustomerLogo.Text.Trim()}");

            if (txtCustomerInfo.Text.Trim().Length != 0)
                review.AppendLine($"\tcustomer info: {txtCustomerInfo.Text.Trim()}");

            txtReview.Text = review.ToString();
        }

        private bool GetPassword()
        {
            if (exportTypeControl.Encryption)
            {
                if (string.IsNullOrEmpty(CurrentJob.Password))
                {
                    using (var form = new Password(CurrentJob.Password))
                    {
                        form.ShowDialog();
                        if (form.DialogResult == DialogResult.OK)
                            CurrentJob.Password = form.PasswordString;
                    }
                }

                if (string.IsNullOrEmpty(CurrentJob.Password))
                    return false;
            }

            return true;
        }

        private void Save()
        {
            CurrentJob.Name = txtJobName.Text.Trim();
            CurrentJob.Description = txtDescription.Text.Trim();

            CurrentJob.IsSequences = rdoSequences.Checked;
            CurrentJob.MergeAllSequenceExport = chkMergerSequenceExport.Checked;
            CurrentJob.SegmentExport = intSegmentExport.Value;
            CurrentJob.IsBookmark = rdoBookmarks.Checked;
            CurrentJob.IsEvidenceLock = rdoEvidenceLocks.Checked;
            CurrentJob.MyBookmarksOnly = chkMyBookmarksOnly.Checked;
            CurrentJob.MyEvidenceLock = chkMyEvidenceLockOnly.Checked;
            CurrentJob.AllEvidenceLockCameras = chkAllELCameras.Checked;
            CurrentJob.Search = txtSearch.Text;

            CurrentJob.StartTimeUniversal = dtPickerStartSearchFrom.Value.ToUniversalTime();
            CurrentJob.EndTimeUniversal = dtPickerStartSearchTo.Value.ToUniversalTime();

            CurrentJob.IsXProtectFormat = exportTypeControl.XProtectFormat;
            CurrentJob.IncludePlayer = exportTypeControl.IncludePlayer;
            CurrentJob.Encryption = exportTypeControl.Encryption;
            CurrentJob.EncryptionStrength = (EncryptionStrength)exportTypeControl.EncryptionStrength;
            CurrentJob.SignExport = exportTypeControl.SignExport;
            CurrentJob.MergeAllCameras = exportTypeControl.MergeAllCameras;

            CurrentJob.IsMediaPlayerFormat = exportTypeControl.MediaPlayerFormat;
            CurrentJob.Content = (ExportContent)exportTypeControl.Content;
            CurrentJob.Codec = (AVICodec)exportTypeControl.Codec;
            CurrentJob.AutoSplitExportFile = exportTypeControl.AutoSplitExportFile;
            CurrentJob.MaxAVIFileSize = exportTypeControl.MaxAVIFileSize;
            CurrentJob.Timestamp = exportTypeControl.IncludeTimestamps;
            CurrentJob.AudioSampleDepth = exportTypeControl.AudioSampleDepth;
            CurrentJob.AudioSampleRate = exportTypeControl.AudioSampleRate;
            CurrentJob.Channels = exportTypeControl.Channels;
            CurrentJob.FrameRate = exportTypeControl.FrameRate;
            CurrentJob.FrameWidth = exportTypeControl.FrameWidth;
            CurrentJob.FrameHeight = exportTypeControl.FrameHeight;
            CurrentJob.UseFrameDefaultValues = exportTypeControl.UseFrameDefaultValues;

            CurrentJob.IsMkvFormat = exportTypeControl.MatroskaMkv;

            CurrentJob.IsStillImages = exportTypeControl.StillImages;
            CurrentJob.StillImagesOption = exportTypeControl.StillImagesOption;
            CurrentJob.SampleImages = exportTypeControl.SampleImages;
            CurrentJob.Quality = exportTypeControl.Quality;
            CurrentJob.IgnoreImagesMinCount = exportTypeControl.IgnoreImagesMinCount;
            CurrentJob.ImageWidth = exportTypeControl.ImageWidth;
            CurrentJob.ImageHeight = exportTypeControl.ImageHeight;
            CurrentJob.KeepAspectRatio = exportTypeControl.KeepAspectRatio;
            CurrentJob.FillSpace = exportTypeControl.FillSpace;
            CurrentJob.AllowUpscaling = exportTypeControl.AllowUpscaling;

            CurrentJob.MediaType = cboMediaType.SelectedItem.ToString();
            CurrentJob.UdfFormat = cboUdfFormat.SelectedItem.ToString();

            CurrentJob.HelperApplicationPath = txtHelperApplicationPath.Text;
            CurrentJob.DeleteAfterArchiving = chkDeleteAfterArchiving.Checked;
            CurrentJob.BtwFile = txtPrintLabel.Text;
            CurrentJob.LogoFile = txtCustomerLogo.Text;
            CurrentJob.CompanyInfo = txtCustomerInfo.Text;
            CurrentJob.ExportDestination = txtExportDestination.Text;
            CurrentJob.NetworkUsername = txtUserName.Text;
            CurrentJob.NetworkPassword = txtPassword.Text;
            CurrentJob.NetworkDomain = txtDomain.Text;

            CurrentJob.ClearCameras();
            for (int i = 0; i < lsvCameras.Items.Count; i++)
            {
                var viewItem = lsvCameras.Items[i] as ListViewItem;
                var item = viewItem.Tag as Item;
                if (item != null)
                    CurrentJob.AddCamera(item);
            }

            CurrentJob.References = References();
        }

        internal object[] References()
        {
            try
            {
                var list = new object[lsvData.CheckedIndices.Count];
                for (int i = 0; i < lsvData.CheckedItems.Count; i++)
                {
                    list[i] = lsvData.Items[i].Tag;
                }

                return list;
            }
            catch (Exception ex)
            {
                Logger.Log.Error(null, ex);
            }

            return new object[0];
        }

        private void rdoBookmarks_CheckedChanged(object sender, EventArgs e)
        {
            chkMyBookmarksOnly.Enabled = rdoBookmarks.Checked;
            panelEvidenceLock.Enabled = !rdoBookmarks.Checked;
            txtSearch.Enabled = !rdoSequences.Checked;
        }

        private void rdoSequences_CheckedChanged(object sender, EventArgs e)
        {
            chkMyBookmarksOnly.Enabled = !rdoSequences.Checked;
            panelEvidenceLock.Enabled = !rdoSequences.Checked;
            txtSearch.Enabled = !rdoSequences.Checked;
            panelSequences.Enabled = rdoSequences.Checked;
        }

        #region search filter

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

                            if (!lsvCameras.Items.ContainsKey(item.Name))
                                lsvCameras.Items.Add(item);
                        }
                    }
                }
            }
            else
                lsvCameras.Items.Add(new ListViewItem { Text = Guid.NewGuid().ToString() });
        }

        private void btnRemoveChecked_Click(object sender, EventArgs e)
        {
            var list = lsvCameras.CheckedItems;
            for (int i = list.Count - 1; i >= 0; i--)
            {
                lsvCameras.Items.Remove(list[i]);
            }
        }

        private void btnRemoveAll_Click(object sender, EventArgs e)
        {
            lsvCameras.Items.Clear();
        }

        #endregion search filter

        #region Bookmarks - Evidence Locks - Sequences

        private void StartSearching<T>(Item[] items) where T : class
        {
            var type = typeof(T);
            var deviceTypesGuids = new Guid[] { Kind.Camera, Kind.Microphone, Kind.Speaker };
            bool myOwn = false;
            if (rdoBookmarks.Checked && chkMyBookmarksOnly.Checked || rdoEvidenceLocks.Checked && chkMyEvidenceLockOnly.Checked)
                myOwn = true;

            dataDownloader = new DataDownloader(items, deviceTypesGuids, myOwn, type == typeof(ProxyServer.MarkedData), txtJobName.Text, txtSearch.Text, chkMergerSequenceExport.Checked, intSegmentExport.Value);
            var chunks = dataDownloader.Start<T>(dtPickerStartSearchFrom.Value.ToUniversalTime(), dtPickerStartSearchTo.Value.ToUniversalTime());
            ListViewItem listViewItem = null;

            foreach (var chunk in chunks)
            {
                if (chunk.Length != 0)
                {
                    for (int i = 0; i < chunk.Length; i++)
                    {
                        if (type == typeof(Bookmark))
                        {
                            var data = chunk[i] as Bookmark;
                            string[] row = { data.Header, data.Description, data.TimeBegin.ToLocalTime().ToString(SegStorPluginDefinition.TimeFormat), data.TimeEnd.ToLocalTime().ToString(SegStorPluginDefinition.TimeFormat), data.GetDeviceItem().Name, data.Reference };
                            listViewItem = new ListViewItem(row);
                        }
                        else if (type == typeof(SequenceData))
                        {
                            var data = chunk[i] as SequenceData;
                            string[] row = { data.EventHeader.Message, data.EventHeader.CustomTag, data.EventSequence.StartDateTime.ToLocalTime().ToString(SegStorPluginDefinition.TimeFormat), data.EventSequence.EndDateTime.ToLocalTime().ToString(SegStorPluginDefinition.TimeFormat), data.EventHeader.Name, data.EventHeader.Class };
                            listViewItem = new ListViewItem(row);
                        }
                        else if (type == typeof(ProxyServer.MarkedData))
                        {
                            var data = chunk[i] as ProxyServer.MarkedData;
                            var camerasNames = string.Join(";", items.Where(item => data.DeviceIds.Contains(item.FQID.ObjectId)).Select(item => item.Name).OrderBy(item => item).ToArray());
                            string[] row = { data.Header, data.Description, data.StartTime.ToLocalTime().ToString(SegStorPluginDefinition.TimeFormat), data.EndTime.ToLocalTime().ToString(SegStorPluginDefinition.TimeFormat), camerasNames, data.Reference };
                            listViewItem = new ListViewItem(row);
                        }

                        listViewItem.Tag = chunk[i];
                        listViewItem.Checked = true;
                        lsvData.Items.Add(listViewItem);

                        if (SegStorBackgroundPlugin.Stop)
                            break;
                    }

                    lsvData.Refresh();
                    Application.DoEvents();
                    Thread.Sleep(200);
                }

                if (SegStorBackgroundPlugin.Stop)
                    break;
            }

            dataDownloader.Stop();
            dataDownloader.Dispose();
        }

        private void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            Check(chkAll.Checked);
        }

        private void Check(bool value)
        {
            if (lsvData.Items.Count != 0)
            {
                for (int i = 0; i < lsvData.Items.Count; i++)
                {
                    lsvData.Items[i].Checked = value;
                }

                lsvData.Refresh();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            btnSearch.Enabled = false;
            btnStop.Enabled = true;
            btnVideoReplay.Enabled = false;
            btnStop.Refresh();
            lsvData.Items.Clear();
            lblBookmarksCount.Text = 0.ToString();
            stopSearching = false;

            Cursor = Cursors.WaitCursor;
            Application.DoEvents();

            try
            {
                Search();
            }
            catch (Exception ex)
            {
                Logger.Log.Error(null, ex);
            }

            btnSearch.Enabled = true;
            btnStop.Enabled = false;
            btnVideoReplay.Enabled = true;

            Cursor = Cursors.Default;

            chkAll.Checked = lsvData.Items.Count != 0;
            lblBookmarksCount.Text = lsvData.Items.Count.ToString();

            var msg = stopSearching ? "interrupted" : "completed";
            MessageBox.Show(this, $"Search {msg}.", Common.Abbreviation, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Search()
        {
            var items = new Item[lsvCameras.Items.Count];

            for (int i = 0; i < lsvCameras.Items.Count; i++)
            {
                var viewItem = lsvCameras.Items[i] as ListViewItem;
                var item = viewItem.Tag as Item;
                if (item != null)
                    items[i] = item;
            }

            lsvData.Items.Clear();

            try
            {
                if (rdoBookmarks.Checked)
                    StartSearching<Bookmark>(items);
                else if (rdoEvidenceLocks.Checked)
                    StartSearching<ProxyServer.MarkedData>(items);
                else
                    StartSearching<SequenceData>(items);
            }
            catch (Exception ex)
            {
                Logger.Log.Error(null, ex);
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            stopSearching = true;
            if (dataDownloader != null)
            {
                try
                {
                    dataDownloader.Stop();
                }
                catch { }
            }
        }

        #endregion Bookmarks - Evidence Locks - Sequences

        #region Target

        private void rdoNetwork_CheckedChanged(object sender, EventArgs e)
        {
            panelCredentials.Enabled = rdoNetwork.Checked;
        }

        #endregion Target

        #region Archiving

        private void btnBrowseIncludeFolder_Click(object sender, EventArgs e)
        {
            using (var folderBrowserDialog1 = new FolderBrowserDialog())
            {
                folderBrowserDialog1.RootFolder = Environment.SpecialFolder.Desktop;
                folderBrowserDialog1.Description = "Select a folder to add contents to each media.";
                folderBrowserDialog1.ShowNewFolderButton = false;
                var result = folderBrowserDialog1.ShowDialog();

                if (result == DialogResult.OK)
                    txtHelperApplicationPath.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void btnPrintLabel_Click(object sender, EventArgs e)
        {
            using (var folderBrowserDialog1 = new OpenFileDialog())
            {
                try
                {
                    folderBrowserDialog1.InitialDirectory = @"\\RIMAGESYSTEM\Rimage\Labels\StorageQuest";
                }
                catch
                {
                    folderBrowserDialog1.InitialDirectory = Environment.SpecialFolder.Desktop.ToString();
                }

                folderBrowserDialog1.Title = "Select a print label file.";
                folderBrowserDialog1.Filter = "Label files (*.btw)|*.btw";
                folderBrowserDialog1.RestoreDirectory = true;
                folderBrowserDialog1.Multiselect = false;

                var result = folderBrowserDialog1.ShowDialog();

                if (result == DialogResult.OK)
                    txtPrintLabel.Text = folderBrowserDialog1.FileName;
            }
        }

        private void btnCustomerLogo_Click(object sender, EventArgs e)
        {
            using (var folderBrowserDialog1 = new OpenFileDialog())
            {
                try
                {
                    folderBrowserDialog1.InitialDirectory = @"\\RIMAGESYSTEM\Rimage\Labels\StorageQuest";
                }
                catch
                {
                    folderBrowserDialog1.InitialDirectory = Environment.SpecialFolder.Desktop.ToString();
                }

                folderBrowserDialog1.Title = "Select logo file.";
                folderBrowserDialog1.Filter = "PNG Files (*.png)|*.png|JPEG Files (*.jpeg)|*.jpeg|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif|BMP Files (*.bmp)|*.bmp";
                folderBrowserDialog1.RestoreDirectory = true;
                folderBrowserDialog1.Multiselect = false;

                var result = folderBrowserDialog1.ShowDialog();

                if (result == DialogResult.OK)
                    txtCustomerLogo.Text = folderBrowserDialog1.FileName;
            }
        }

        private void btnGetCustomerInfo_Click(object sender, EventArgs e)
        {
            using (var customerInfo = new CustomerInfo())
            {
                customerInfo.Initialize(txtCustomerInfo.Text);
                customerInfo.ShowDialog(this);
                txtCustomerInfo.Text = string.Empty;
                var info = customerInfo.GetInfo;
                if (info != "||||")
                    txtCustomerInfo.Text = customerInfo.GetInfo;
                customerInfo.Close();
            }
        }

        private void btnBrowseExportDestination_Click(object sender, EventArgs e)
        {
            using (var folderBrowserDialog1 = new FolderBrowserDialog())
            {
                folderBrowserDialog1.RootFolder = Environment.SpecialFolder.Desktop;

                folderBrowserDialog1.Description = "Specify the folder that you want to export to.";
                folderBrowserDialog1.ShowNewFolderButton = true;

                if (!string.IsNullOrEmpty(txtExportDestination.Text))
                    folderBrowserDialog1.SelectedPath = txtExportDestination.Text;

                if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                {
                    txtExportDestination.Text = folderBrowserDialog1.SelectedPath;
                    try
                    {
                        Settings1.Default.ExportDestination = txtExportDestination.Text;
                        Settings1.Default.Save();
                    }
                    catch (Exception ex)
                    {
                        Logger.Log.Error(null, ex);
                    }
                }
            }
        }

        private async void btnTest_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            Application.DoEvents();
            Tuple<bool, string> tuple = null;

            try
            {
                tuple = await TestConnection(txtUserName.Text, txtPassword.Text, txtExportDestination.Text, txtDomain.Text);
            }
            catch (Exception ex)
            {
                Logger.Log.Error(null, ex);
            }
            finally
            {
                Cursor = Cursors.Default;
            }

            var succeeded = $"Test connection to {txtExportDestination.Text} succeeded";

            if (tuple.Item1)
                MessageBoxEx.Show(this, succeeded, "Successfully connected", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
            {
                if (DirectoryInfoFactory.IsExists(txtExportDestination.Text))
                    MessageBoxEx.Show(this, succeeded, "Successfully connected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBoxEx.Show(this, $"Failed to connect to {txtExportDestination.Text}\n{tuple.Item2}", "Failed to connect", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task<Tuple<bool, string>> TestConnection(string userName, string password, string sharedPath, string domain = "")
        {
            var tuple = new Tuple<bool, string>(true, string.Empty);

            using (var unc = new UNCAccessWithCredentials())
            {
                bool netIsConnected = false;

                try
                {
                    await Task.Run(() => netIsConnected = UNCAccessWithCredentials.NetIsConnected(sharedPath));
                }
                catch (Exception ex)
                {
                    Logger.Log.Error(null, ex);
                }

                if (!netIsConnected)
                {
                    if (!unc.NetUseWithCredentials(sharedPath, userName, domain, password))
                    {
                        tuple = new Tuple<bool, string>(false, unc.LastErrorDescription);
                        Logger.Log.Warn($"Failed to connect to {sharedPath}{unc.LastErrorDescription}");
                    }
                }
                else
                    tuple = new Tuple<bool, string>(true, string.Empty);
            }

            return tuple;
        }


        #endregion Archiving

        #endregion Wizard Events

        private void btnVideoReplay_Click(object sender, EventArgs e)
        {
            VideoReplayView videoReplayView = null;
            try
            {
                videoReplayView = new VideoReplayView();
                Tuple<Item, DateTime, DateTime>[] tuples = null;

                if (lsvData.SelectedItems.Count != 0)
                {
                    var data = lsvData.SelectedItems[0].Tag;
                    if (data != null)
                    {
                        if (data is Bookmark)
                        {
                            var bm = data as Bookmark;
                            var tuple = new Tuple<Item, DateTime, DateTime>(bm.GetDeviceItem(), bm.TimeBegin, bm.TimeEnd);
                            tuples = new Tuple<Item, DateTime, DateTime>[] { tuple };

                        }
                        else if (data is SequenceData)
                        {
                            var s = data as SequenceData;
                            var cameraName = s.EventHeader.Name;

                            Item item = null;
                            for (int i = 0; i < lsvCameras.Items.Count; i++)
                            {
                                var viewItem = lsvCameras.Items[i] as ListViewItem;
                                item = viewItem.Tag as Item;
                                if (item != null && item.Name == s.EventHeader.Name)
                                    break;
                            }

                            if (item != null)
                            {
                                var tuple = new Tuple<Item, DateTime, DateTime>(item, s.EventSequence.StartDateTime, s.EventSequence.EndDateTime);
                                tuples = new Tuple<Item, DateTime, DateTime>[] { tuple };
                            }
                        }
                        else if (data is ProxyServer.MarkedData)
                        {
                            var el = data as ProxyServer.MarkedData;

                            Tuple<Item, DateTime, DateTime> tuple = null;
                            IList<Tuple<Item, DateTime, DateTime>> tupleList = new List<Tuple<Item, DateTime, DateTime>>(el.DeviceIds.Length);

                            for (int i = 0; i < el.DeviceIds.Length; i++)
                            {
                                var camera = Configuration.Instance.GetItem(EnvironmentManager.Instance.MasterSite.ServerId, el.DeviceIds[i], Kind.Camera);
                                if (camera != null)
                                {
                                    tuple = new Tuple<Item, DateTime, DateTime>(camera, el.StartTime, el.EndTime);
                                    tupleList.Add(tuple);
                                }
                            }

                            tuples = tupleList.ToArray();
                        }

                        videoReplayView.Add(tuples);
                    }
                }
                else
                {
                    if (lsvData.Items.Count != 0)
                        MessageBoxEx.Show(this, "Please select one export to replay it", "No export selected", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

                if (tuples.Length != 0)
                    videoReplayView.ShowDialog(this);
            }
            catch (Exception ex)
            {
                Logger.Log.Error(null, ex);
            }
            finally
            {
                if (videoReplayView != null)
                    videoReplayView.CloseMe();
            }
        }
    }
}
