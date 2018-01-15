using System.Windows.Forms;
using OpenALPRPlugin.Utility;

namespace OpenALPRPlugin.Client
{
    partial class WorkSpaceControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnSearch = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lsvBookmarks = new System.Windows.Forms.ListView();
            this.hdrNumber = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.hdrTimeBegin = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.hdrTimeEnd = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.hdrHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.hdrDescription = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.datStartTime = new System.Windows.Forms.DateTimePicker();
            this.lblMessage = new System.Windows.Forms.Label();
            this.txtCameraName = new System.Windows.Forms.TextBox();
            this.btnCameraSelect = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.datEndTime = new System.Windows.Forms.DateTimePicker();
            this.txtSearchFor = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.chkMyBookmarksOnly = new System.Windows.Forms.CheckBox();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnMapCameras = new System.Windows.Forms.Button();
            this.btnHelp = new System.Windows.Forms.Button();
            this.btnOpenLogLocation = new System.Windows.Forms.Button();
            this.picOpenALPR = new System.Windows.Forms.PictureBox();
            this.btnBlackList = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picOpenALPR)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(560, 801);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(110, 36);
            this.btnSearch.TabIndex = 11;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.Search_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 107);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Start searching from";
            // 
            // lsvBookmarks
            // 
            this.lsvBookmarks.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.hdrNumber,
            this.hdrTimeBegin,
            this.hdrTimeEnd,
            this.hdrHeader,
            this.hdrDescription});
            this.lsvBookmarks.FullRowSelect = true;
            this.lsvBookmarks.Location = new System.Drawing.Point(13, 185);
            this.lsvBookmarks.MultiSelect = false;
            this.lsvBookmarks.Name = "lsvBookmarks";
            this.lsvBookmarks.Size = new System.Drawing.Size(935, 585);
            this.lsvBookmarks.TabIndex = 9;
            this.lsvBookmarks.UseCompatibleStateImageBehavior = false;
            this.lsvBookmarks.View = System.Windows.Forms.View.Details;
            this.lsvBookmarks.MouseUp += new System.Windows.Forms.MouseEventHandler(this.LsvBookmarks_MouseUp);
            // 
            // hdrNumber
            // 
            this.hdrNumber.Text = "#";
            this.hdrNumber.Width = 30;
            // 
            // hdrTimeBegin
            // 
            this.hdrTimeBegin.Text = "Time Begin";
            this.hdrTimeBegin.Width = 130;
            // 
            // hdrTimeEnd
            // 
            this.hdrTimeEnd.Text = "Time End";
            this.hdrTimeEnd.Width = 130;
            // 
            // hdrHeader
            // 
            this.hdrHeader.Text = "Header";
            this.hdrHeader.Width = 120;
            // 
            // hdrDescription
            // 
            this.hdrDescription.Text = "Description";
            this.hdrDescription.Width = 470;
            // 
            // datStartTime
            // 
            this.datStartTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.datStartTime.Location = new System.Drawing.Point(115, 101);
            this.datStartTime.Name = "datStartTime";
            this.datStartTime.Size = new System.Drawing.Size(258, 20);
            this.datStartTime.TabIndex = 1;
            // 
            // lblMessage
            // 
            this.lblMessage.Location = new System.Drawing.Point(13, 778);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(934, 13);
            this.lblMessage.TabIndex = 10;
            // 
            // txtCameraName
            // 
            this.txtCameraName.Location = new System.Drawing.Point(524, 104);
            this.txtCameraName.Name = "txtCameraName";
            this.txtCameraName.ReadOnly = true;
            this.txtCameraName.Size = new System.Drawing.Size(422, 20);
            this.txtCameraName.TabIndex = 5;
            this.txtCameraName.TabStop = false;
            // 
            // btnCameraSelect
            // 
            this.btnCameraSelect.Location = new System.Drawing.Point(429, 102);
            this.btnCameraSelect.Name = "btnCameraSelect";
            this.btnCameraSelect.Size = new System.Drawing.Size(89, 23);
            this.btnCameraSelect.TabIndex = 4;
            this.btnCameraSelect.Text = "Camera";
            this.btnCameraSelect.UseVisualStyleBackColor = true;
            this.btnCameraSelect.Click += new System.EventHandler(this.ButtonCameraSelect_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 135);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Search up until";
            // 
            // datEndTime
            // 
            this.datEndTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.datEndTime.Location = new System.Drawing.Point(115, 129);
            this.datEndTime.Name = "datEndTime";
            this.datEndTime.Size = new System.Drawing.Size(258, 20);
            this.datEndTime.TabIndex = 3;
            // 
            // txtSearchFor
            // 
            this.txtSearchFor.Location = new System.Drawing.Point(524, 155);
            this.txtSearchFor.Name = "txtSearchFor";
            this.txtSearchFor.Size = new System.Drawing.Size(422, 20);
            this.txtSearchFor.TabIndex = 8;
            this.txtSearchFor.TabStop = false;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(521, 136);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(425, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Text appears in either of the fields Reference, Header or Description";
            // 
            // chkMyBookmarksOnly
            // 
            this.chkMyBookmarksOnly.AutoSize = true;
            this.chkMyBookmarksOnly.Location = new System.Drawing.Point(115, 155);
            this.chkMyBookmarksOnly.Name = "chkMyBookmarksOnly";
            this.chkMyBookmarksOnly.Size = new System.Drawing.Size(118, 17);
            this.chkMyBookmarksOnly.TabIndex = 6;
            this.chkMyBookmarksOnly.Text = "My Bookmarks only";
            this.chkMyBookmarksOnly.UseVisualStyleBackColor = true;
            this.chkMyBookmarksOnly.Visible = false;
            // 
            // btnNext
            // 
            this.btnNext.Enabled = false;
            this.btnNext.Location = new System.Drawing.Point(698, 801);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(110, 36);
            this.btnNext.TabIndex = 12;
            this.btnNext.Text = "Next";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.BtnNext_Click);
            // 
            // btnMapCameras
            // 
            this.btnMapCameras.Location = new System.Drawing.Point(13, 801);
            this.btnMapCameras.Name = "btnMapCameras";
            this.btnMapCameras.Size = new System.Drawing.Size(110, 36);
            this.btnMapCameras.TabIndex = 13;
            this.btnMapCameras.Text = "Camera Mapping";
            this.btnMapCameras.UseVisualStyleBackColor = true;
            this.btnMapCameras.Click += new System.EventHandler(this.BtnMapCameras_Click);
            // 
            // btnHelp
            // 
            this.btnHelp.Location = new System.Drawing.Point(836, 801);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(110, 36);
            this.btnHelp.TabIndex = 14;
            this.btnHelp.Text = "Help";
            this.btnHelp.UseVisualStyleBackColor = true;
            this.btnHelp.Click += new System.EventHandler(this.BtnHelp_Click);
            // 
            // btnOpenLogLocation
            // 
            this.btnOpenLogLocation.Location = new System.Drawing.Point(287, 801);
            this.btnOpenLogLocation.Name = "btnOpenLogLocation";
            this.btnOpenLogLocation.Size = new System.Drawing.Size(110, 36);
            this.btnOpenLogLocation.TabIndex = 15;
            this.btnOpenLogLocation.Text = "Open log";
            this.btnOpenLogLocation.UseVisualStyleBackColor = true;
            this.btnOpenLogLocation.Click += new System.EventHandler(this.BtnOpenLogLocation_Click);
            // 
            // picOpenALPR
            // 
            this.picOpenALPR.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.picOpenALPR.Image = global::OpenALPRPlugin.Properties.Resources.logo_bluegray;
            this.picOpenALPR.InitialImage = global::OpenALPRPlugin.Properties.Resources.logo_bluegray;
            this.picOpenALPR.Location = new System.Drawing.Point(429, 3);
            this.picOpenALPR.Name = "picOpenALPR";
            this.picOpenALPR.Size = new System.Drawing.Size(519, 93);
            this.picOpenALPR.TabIndex = 16;
            this.picOpenALPR.TabStop = false;
            // 
            // btnBlackList
            // 
            this.btnBlackList.Location = new System.Drawing.Point(150, 801);
            this.btnBlackList.Name = "btnBlackList";
            this.btnBlackList.Size = new System.Drawing.Size(110, 36);
            this.btnBlackList.TabIndex = 17;
            this.btnBlackList.Text = "Black List";
            this.btnBlackList.UseVisualStyleBackColor = true;
            this.btnBlackList.Click += new System.EventHandler(this.BtnBlackList_Click);
            // 
            // WorkSpaceControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.btnBlackList);
            this.Controls.Add(this.picOpenALPR);
            this.Controls.Add(this.btnOpenLogLocation);
            this.Controls.Add(this.btnHelp);
            this.Controls.Add(this.btnMapCameras);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.chkMyBookmarksOnly);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtSearchFor);
            this.Controls.Add(this.datEndTime);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtCameraName);
            this.Controls.Add(this.btnCameraSelect);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.datStartTime);
            this.Controls.Add(this.lsvBookmarks);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSearch);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "WorkSpaceControl";
            this.Size = new System.Drawing.Size(963, 845);
            this.Load += new System.EventHandler(this.BookmarkViewItemManager_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picOpenALPR)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private ColumnHeader columnHeader5;
        private Button btnSearch;
        private Label label1;
        private ListView lsvBookmarks;
        private DateTimePicker datStartTime;
        private Label lblMessage;
        private TextBox txtCameraName;
        private Button btnCameraSelect;
        private Label label2;
        private DateTimePicker datEndTime;
        private TextBox txtSearchFor;
        private Label label3;
        private CheckBox chkMyBookmarksOnly;
        private ColumnHeader hdrNumber;
        private ColumnHeader hdrTimeBegin;
        private ColumnHeader hdrTimeEnd;
        private ColumnHeader hdrHeader;
        private ColumnHeader hdrDescription;
        private Button btnNext;
        private Button btnMapCameras;
        private Button btnHelp;
        private Button btnOpenLogLocation;
        private PictureBox picOpenALPR;
        private Button btnBlackList;
    }
}
