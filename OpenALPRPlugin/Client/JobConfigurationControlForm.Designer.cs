namespace RookArchive.Admin
{
    partial class JobConfigurationControlForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(JobConfigurationControlForm));
            this.itemSunday = new DevComponents.Editors.ComboItem();
            this.itemMonday = new DevComponents.Editors.ComboItem();
            this.itemTuesday = new DevComponents.Editors.ComboItem();
            this.itemWednesday = new DevComponents.Editors.ComboItem();
            this.itemThursday = new DevComponents.Editors.ComboItem();
            this.itemFriday = new DevComponents.Editors.ComboItem();
            this.itemSaturday = new DevComponents.Editors.ComboItem();
            this.wizard1 = new DevComponents.DotNetBar.Wizard();
            this.wpJobNameAndDescription = new DevComponents.DotNetBar.WizardPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtDescription = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtJobName = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.wpSearchFilter = new DevComponents.DotNetBar.WizardPage();
            this.panel6 = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.lsvCameras = new System.Windows.Forms.ListView();
            this.columnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnRemoveChecked = new System.Windows.Forms.Button();
            this.btnRemoveAll = new System.Windows.Forms.Button();
            this.btnSelectCamera = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.dtPickerStartSearchTo = new DevComponents.Editors.DateTimeAdv.DateTimeInput();
            this.dtPickerStartSearchFrom = new DevComponents.Editors.DateTimeAdv.DateTimeInput();
            this.wpTypeSelection = new DevComponents.DotNetBar.WizardPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panelSequences = new System.Windows.Forms.Panel();
            this.chkMergerSequenceExport = new System.Windows.Forms.CheckBox();
            this.label16 = new System.Windows.Forms.Label();
            this.intSegmentExport = new DevComponents.Editors.IntegerInput();
            this.rdoSequences = new System.Windows.Forms.RadioButton();
            this.label12 = new System.Windows.Forms.Label();
            this.txtSearch = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.panelEvidenceLock = new System.Windows.Forms.Panel();
            this.chkAllELCameras = new System.Windows.Forms.CheckBox();
            this.chkMyEvidenceLockOnly = new System.Windows.Forms.CheckBox();
            this.chkMyBookmarksOnly = new System.Windows.Forms.CheckBox();
            this.rdoEvidenceLocks = new System.Windows.Forms.RadioButton();
            this.rdoBookmarks = new System.Windows.Forms.RadioButton();
            this.wpBookmarksEvidenceLocksSequences = new DevComponents.DotNetBar.WizardPage();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnVideoReplay = new System.Windows.Forms.Button();
            this.lblBookmarksCount = new System.Windows.Forms.Label();
            this.lblBECount = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.chkAll = new System.Windows.Forms.CheckBox();
            this.lsvData = new System.Windows.Forms.ListView();
            this.Header = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Description = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.TimeBegin = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.TimeEnd = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Cameras = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Reference = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.wpExportType = new DevComponents.DotNetBar.WizardPage();
            this.panel4 = new System.Windows.Forms.Panel();
            this.exportTypeControl = new SegStorUserControls.ExportControl();
            this.wpTarget = new DevComponents.DotNetBar.WizardPage();
            this.panelTarget = new System.Windows.Forms.Panel();
            this.label17 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.txtExportDestination = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.panelCredentials = new System.Windows.Forms.Panel();
            this.txtUserName = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.label13 = new System.Windows.Forms.Label();
            this.txtPassword = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.label14 = new System.Windows.Forms.Label();
            this.txtDomain = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.btnTest = new System.Windows.Forms.Button();
            this.label15 = new System.Windows.Forms.Label();
            this.btnBrowseExportDestination = new System.Windows.Forms.Button();
            this.label18 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.rdoNetwork = new System.Windows.Forms.RadioButton();
            this.rdoLocal = new System.Windows.Forms.RadioButton();
            this.wpArchiveSettings = new DevComponents.DotNetBar.WizardPage();
            this.panel5 = new System.Windows.Forms.Panel();
            this.btnEditCustomerInfo = new System.Windows.Forms.Button();
            this.btnCustomerLogo = new System.Windows.Forms.Button();
            this.btnPrintLabel = new System.Windows.Forms.Button();
            this.btnBrowseIncludeFolder = new System.Windows.Forms.Button();
            this.txtCustomerInfo = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtPrintLabel = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtCustomerLogo = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.chkDeleteAfterArchiving = new System.Windows.Forms.CheckBox();
            this.txtHelperApplicationPath = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.label4 = new System.Windows.Forms.Label();
            this.cboUdfFormat = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.label3 = new System.Windows.Forms.Label();
            this.cboMediaType = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX6 = new System.Windows.Forms.Label();
            this.wpReview = new DevComponents.DotNetBar.WizardPage();
            this.txtReview = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.wizard1.SuspendLayout();
            this.wpJobNameAndDescription.SuspendLayout();
            this.panel1.SuspendLayout();
            this.wpSearchFilter.SuspendLayout();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtPickerStartSearchTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtPickerStartSearchFrom)).BeginInit();
            this.wpTypeSelection.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panelSequences.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.intSegmentExport)).BeginInit();
            this.panelEvidenceLock.SuspendLayout();
            this.wpBookmarksEvidenceLocksSequences.SuspendLayout();
            this.panel3.SuspendLayout();
            this.wpExportType.SuspendLayout();
            this.panel4.SuspendLayout();
            this.wpTarget.SuspendLayout();
            this.panelTarget.SuspendLayout();
            this.panelCredentials.SuspendLayout();
            this.wpArchiveSettings.SuspendLayout();
            this.panel5.SuspendLayout();
            this.wpReview.SuspendLayout();
            this.SuspendLayout();
            // 
            // itemSunday
            // 
            this.itemSunday.Text = "Sunday";
            // 
            // itemMonday
            // 
            this.itemMonday.Text = "Monday";
            // 
            // itemTuesday
            // 
            this.itemTuesday.Text = "Tuesday";
            // 
            // itemWednesday
            // 
            this.itemWednesday.Text = "Wednesday";
            // 
            // itemThursday
            // 
            this.itemThursday.Text = "Thursday";
            // 
            // itemFriday
            // 
            this.itemFriday.Text = "Friday";
            // 
            // itemSaturday
            // 
            this.itemSaturday.Text = "Saturday";
            // 
            // wizard1
            // 
            this.wizard1.BackColor = System.Drawing.Color.Transparent;
            this.wizard1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("wizard1.BackgroundImage")));
            this.wizard1.ButtonStyle = DevComponents.DotNetBar.eWizardStyle.Office2007;
            this.wizard1.CancelButtonTabStop = false;
            this.wizard1.CancelButtonWidth = 0;
            this.wizard1.Cursor = System.Windows.Forms.Cursors.Default;
            this.wizard1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizard1.FinishButtonTabIndex = 3;
            this.wizard1.FinishButtonText = "Save";
            this.wizard1.FooterHeight = 40;
            // 
            // 
            // 
            this.wizard1.FooterStyle.BackColor = System.Drawing.Color.Transparent;
            this.wizard1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.wizard1.FormCancelButton = DevComponents.DotNetBar.eWizardFormCancelButton.None;
            this.wizard1.HeaderCaptionFont = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.wizard1.HeaderDescriptionVisible = false;
            this.wizard1.HeaderHeight = 50;
            this.wizard1.HeaderImageAlignment = DevComponents.DotNetBar.eWizardTitleImageAlignment.Left;
            this.wizard1.HeaderImageSize = new System.Drawing.Size(16, 16);
            this.wizard1.HeaderImageVisible = false;
            // 
            // 
            // 
            this.wizard1.HeaderStyle.BackColor = System.Drawing.Color.Transparent;
            this.wizard1.HeaderStyle.BackColorGradientAngle = 90;
            this.wizard1.HeaderStyle.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.wizard1.HeaderStyle.BorderBottomColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(157)))), ((int)(((byte)(182)))));
            this.wizard1.HeaderStyle.BorderBottomWidth = 1;
            this.wizard1.HeaderStyle.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.wizard1.HeaderStyle.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.wizard1.HeaderTitleIndent = 30;
            this.wizard1.HelpButtonTabStop = false;
            this.wizard1.HelpButtonVisible = false;
            this.wizard1.HelpButtonWidth = 0;
            this.wizard1.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F";
            this.wizard1.Location = new System.Drawing.Point(0, 0);
            this.wizard1.Name = "wizard1";
            this.wizard1.Size = new System.Drawing.Size(946, 509);
            this.wizard1.TabIndex = 0;
            this.wizard1.WizardPages.AddRange(new DevComponents.DotNetBar.WizardPage[] {
            this.wpJobNameAndDescription,
            this.wpSearchFilter,
            this.wpTypeSelection,
            this.wpBookmarksEvidenceLocksSequences,
            this.wpExportType,
            this.wpTarget,
            this.wpArchiveSettings,
            this.wpReview});
            this.wizard1.BackButtonClick += new System.ComponentModel.CancelEventHandler(this.wizard1_BackButtonClick);
            this.wizard1.NextButtonClick += new System.ComponentModel.CancelEventHandler(this.wizard1_NextButtonClick);
            this.wizard1.FinishButtonClick += new System.ComponentModel.CancelEventHandler(this.wizard1_FinishButtonClick);
            this.wizard1.HelpButtonClick += new System.ComponentModel.CancelEventHandler(this.wizard1_HelpButtonClick);
            // 
            // wpJobNameAndDescription
            // 
            this.wpJobNameAndDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.wpJobNameAndDescription.BackColor = System.Drawing.Color.Transparent;
            this.wpJobNameAndDescription.CanvasColor = System.Drawing.Color.Transparent;
            this.wpJobNameAndDescription.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.wpJobNameAndDescription.Controls.Add(this.panel1);
            this.wpJobNameAndDescription.Location = new System.Drawing.Point(7, 62);
            this.wpJobNameAndDescription.Name = "wpJobNameAndDescription";
            this.wpJobNameAndDescription.PageDescription = "Job name and description";
            this.wpJobNameAndDescription.PageTitle = "Job name and description";
            this.wpJobNameAndDescription.Size = new System.Drawing.Size(932, 395);
            this.wpJobNameAndDescription.TabIndex = 0;
            this.wpJobNameAndDescription.TabStop = true;
            this.wpJobNameAndDescription.Text = "Job name and description";
            this.wpJobNameAndDescription.ThemeAware = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.txtDescription);
            this.panel1.Controls.Add(this.txtJobName);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(932, 395);
            this.panel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(70, 111);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Description";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label6.Location = new System.Drawing.Point(95, 85);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Name";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtDescription
            // 
            // 
            // 
            // 
            this.txtDescription.Border.Class = "TextBoxBorder";
            this.txtDescription.ForeColor = System.Drawing.SystemColors.ControlText;
            this.txtDescription.Location = new System.Drawing.Point(136, 109);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(665, 20);
            this.txtDescription.TabIndex = 3;
            this.txtDescription.WatermarkText = "Job description";
            // 
            // txtJobName
            // 
            // 
            // 
            // 
            this.txtJobName.Border.Class = "TextBoxBorder";
            this.txtJobName.ForeColor = System.Drawing.SystemColors.ControlText;
            this.txtJobName.Location = new System.Drawing.Point(136, 83);
            this.txtJobName.MaxLength = 25;
            this.txtJobName.Name = "txtJobName";
            this.txtJobName.Size = new System.Drawing.Size(188, 20);
            this.txtJobName.TabIndex = 1;
            this.txtJobName.WatermarkText = "Job name";
            // 
            // wpSearchFilter
            // 
            this.wpSearchFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.wpSearchFilter.BackColor = System.Drawing.Color.Transparent;
            this.wpSearchFilter.CanvasColor = System.Drawing.Color.Transparent;
            this.wpSearchFilter.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.wpSearchFilter.Controls.Add(this.panel6);
            this.wpSearchFilter.Location = new System.Drawing.Point(7, 62);
            this.wpSearchFilter.Name = "wpSearchFilter";
            this.wpSearchFilter.PageDescription = "Search filter";
            this.wpSearchFilter.PageTitle = "Search filter";
            this.wpSearchFilter.Size = new System.Drawing.Size(932, 395);
            this.wpSearchFilter.TabIndex = 1;
            this.wpSearchFilter.TabStop = true;
            this.wpSearchFilter.Text = "Search filter";
            this.wpSearchFilter.ThemeAware = true;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.label9);
            this.panel6.Controls.Add(this.lsvCameras);
            this.panel6.Controls.Add(this.btnRemoveChecked);
            this.panel6.Controls.Add(this.btnRemoveAll);
            this.panel6.Controls.Add(this.btnSelectCamera);
            this.panel6.Controls.Add(this.label11);
            this.panel6.Controls.Add(this.label10);
            this.panel6.Controls.Add(this.dtPickerStartSearchTo);
            this.panel6.Controls.Add(this.dtPickerStartSearchFrom);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel6.Location = new System.Drawing.Point(0, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(932, 395);
            this.panel6.TabIndex = 0;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label9.Location = new System.Drawing.Point(92, 80);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(54, 13);
            this.label9.TabIndex = 18;
            this.label9.Text = "Camera(s)";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lsvCameras
            // 
            this.lsvCameras.BackColor = System.Drawing.SystemColors.Control;
            this.lsvCameras.CheckBoxes = true;
            this.lsvCameras.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader});
            this.lsvCameras.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lsvCameras.Location = new System.Drawing.Point(152, 80);
            this.lsvCameras.Name = "lsvCameras";
            this.lsvCameras.Size = new System.Drawing.Size(740, 269);
            this.lsvCameras.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lsvCameras.TabIndex = 14;
            this.lsvCameras.UseCompatibleStateImageBehavior = false;
            this.lsvCameras.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader
            // 
            this.columnHeader.Text = "Name";
            this.columnHeader.Width = 700;
            // 
            // btnRemoveChecked
            // 
            this.btnRemoveChecked.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnRemoveChecked.BackColor = System.Drawing.SystemColors.Control;
            this.btnRemoveChecked.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnRemoveChecked.Location = new System.Drawing.Point(326, 355);
            this.btnRemoveChecked.Name = "btnRemoveChecked";
            this.btnRemoveChecked.Size = new System.Drawing.Size(117, 23);
            this.btnRemoveChecked.TabIndex = 16;
            this.btnRemoveChecked.Text = "Remove Checked";
            this.btnRemoveChecked.UseVisualStyleBackColor = false;
            this.btnRemoveChecked.Click += new System.EventHandler(this.btnRemoveChecked_Click);
            // 
            // btnRemoveAll
            // 
            this.btnRemoveAll.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnRemoveAll.BackColor = System.Drawing.SystemColors.Control;
            this.btnRemoveAll.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnRemoveAll.Location = new System.Drawing.Point(483, 355);
            this.btnRemoveAll.Name = "btnRemoveAll";
            this.btnRemoveAll.Size = new System.Drawing.Size(117, 23);
            this.btnRemoveAll.TabIndex = 17;
            this.btnRemoveAll.Text = "Remove All";
            this.btnRemoveAll.UseVisualStyleBackColor = false;
            this.btnRemoveAll.Click += new System.EventHandler(this.btnRemoveAll_Click);
            // 
            // btnSelectCamera
            // 
            this.btnSelectCamera.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSelectCamera.BackColor = System.Drawing.SystemColors.Control;
            this.btnSelectCamera.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnSelectCamera.Location = new System.Drawing.Point(152, 355);
            this.btnSelectCamera.Name = "btnSelectCamera";
            this.btnSelectCamera.Size = new System.Drawing.Size(134, 23);
            this.btnSelectCamera.TabIndex = 15;
            this.btnSelectCamera.Text = "Select Camera ...";
            this.btnSelectCamera.UseVisualStyleBackColor = false;
            this.btnSelectCamera.Click += new System.EventHandler(this.btnSelectCamera_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label11.Location = new System.Drawing.Point(68, 46);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(78, 13);
            this.label11.TabIndex = 13;
            this.label11.Text = "Search up until";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label10.Location = new System.Drawing.Point(40, 20);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(106, 13);
            this.label10.TabIndex = 12;
            this.label10.Text = "Start Searching From";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtPickerStartSearchTo
            // 
            // 
            // 
            // 
            this.dtPickerStartSearchTo.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dtPickerStartSearchTo.ButtonClear.Text = "today";
            this.dtPickerStartSearchTo.ButtonDropDown.Shortcut = DevComponents.DotNetBar.eShortcut.AltDown;
            this.dtPickerStartSearchTo.ButtonDropDown.Visible = true;
            this.dtPickerStartSearchTo.ForeColor = System.Drawing.SystemColors.ControlText;
            this.dtPickerStartSearchTo.Format = DevComponents.Editors.eDateTimePickerFormat.Custom;
            this.dtPickerStartSearchTo.Location = new System.Drawing.Point(152, 42);
            // 
            // 
            // 
            this.dtPickerStartSearchTo.MonthCalendar.AnnuallyMarkedDates = new System.DateTime[0];
            // 
            // 
            // 
            this.dtPickerStartSearchTo.MonthCalendar.BackgroundStyle.BackColor = System.Drawing.SystemColors.Window;
            this.dtPickerStartSearchTo.MonthCalendar.ClearButtonVisible = true;
            // 
            // 
            // 
            this.dtPickerStartSearchTo.MonthCalendar.CommandsBackgroundStyle.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground2;
            this.dtPickerStartSearchTo.MonthCalendar.CommandsBackgroundStyle.BackColorGradientAngle = 90;
            this.dtPickerStartSearchTo.MonthCalendar.CommandsBackgroundStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this.dtPickerStartSearchTo.MonthCalendar.CommandsBackgroundStyle.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.dtPickerStartSearchTo.MonthCalendar.CommandsBackgroundStyle.BorderTopColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarDockedBorder;
            this.dtPickerStartSearchTo.MonthCalendar.CommandsBackgroundStyle.BorderTopWidth = 1;
            this.dtPickerStartSearchTo.MonthCalendar.DisplayMonth = new System.DateTime(2015, 8, 1, 0, 0, 0, 0);
            this.dtPickerStartSearchTo.MonthCalendar.MarkedDates = new System.DateTime[0];
            this.dtPickerStartSearchTo.MonthCalendar.MonthlyMarkedDates = new System.DateTime[0];
            // 
            // 
            // 
            this.dtPickerStartSearchTo.MonthCalendar.NavigationBackgroundStyle.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.dtPickerStartSearchTo.MonthCalendar.NavigationBackgroundStyle.BackColorGradientAngle = 90;
            this.dtPickerStartSearchTo.MonthCalendar.NavigationBackgroundStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.dtPickerStartSearchTo.MonthCalendar.TodayButtonVisible = true;
            this.dtPickerStartSearchTo.MonthCalendar.WeeklyMarkedDays = new System.DayOfWeek[0];
            this.dtPickerStartSearchTo.Name = "dtPickerStartSearchTo";
            this.dtPickerStartSearchTo.ShowUpDown = true;
            this.dtPickerStartSearchTo.Size = new System.Drawing.Size(279, 20);
            this.dtPickerStartSearchTo.TabIndex = 11;
            this.dtPickerStartSearchTo.Value = new System.DateTime(2015, 8, 21, 0, 0, 0, 0);
            // 
            // dtPickerStartSearchFrom
            // 
            // 
            // 
            // 
            this.dtPickerStartSearchFrom.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dtPickerStartSearchFrom.ButtonClear.Text = "today";
            this.dtPickerStartSearchFrom.ButtonDropDown.Shortcut = DevComponents.DotNetBar.eShortcut.AltDown;
            this.dtPickerStartSearchFrom.ButtonDropDown.Visible = true;
            this.dtPickerStartSearchFrom.ForeColor = System.Drawing.SystemColors.ControlText;
            this.dtPickerStartSearchFrom.Format = DevComponents.Editors.eDateTimePickerFormat.Custom;
            this.dtPickerStartSearchFrom.Location = new System.Drawing.Point(152, 16);
            // 
            // 
            // 
            this.dtPickerStartSearchFrom.MonthCalendar.AnnuallyMarkedDates = new System.DateTime[0];
            // 
            // 
            // 
            this.dtPickerStartSearchFrom.MonthCalendar.BackgroundStyle.BackColor = System.Drawing.SystemColors.Window;
            this.dtPickerStartSearchFrom.MonthCalendar.ClearButtonVisible = true;
            // 
            // 
            // 
            this.dtPickerStartSearchFrom.MonthCalendar.CommandsBackgroundStyle.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground2;
            this.dtPickerStartSearchFrom.MonthCalendar.CommandsBackgroundStyle.BackColorGradientAngle = 90;
            this.dtPickerStartSearchFrom.MonthCalendar.CommandsBackgroundStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this.dtPickerStartSearchFrom.MonthCalendar.CommandsBackgroundStyle.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.dtPickerStartSearchFrom.MonthCalendar.CommandsBackgroundStyle.BorderTopColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarDockedBorder;
            this.dtPickerStartSearchFrom.MonthCalendar.CommandsBackgroundStyle.BorderTopWidth = 1;
            this.dtPickerStartSearchFrom.MonthCalendar.DisplayMonth = new System.DateTime(2015, 8, 1, 0, 0, 0, 0);
            this.dtPickerStartSearchFrom.MonthCalendar.MarkedDates = new System.DateTime[0];
            this.dtPickerStartSearchFrom.MonthCalendar.MonthlyMarkedDates = new System.DateTime[0];
            // 
            // 
            // 
            this.dtPickerStartSearchFrom.MonthCalendar.NavigationBackgroundStyle.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.dtPickerStartSearchFrom.MonthCalendar.NavigationBackgroundStyle.BackColorGradientAngle = 90;
            this.dtPickerStartSearchFrom.MonthCalendar.NavigationBackgroundStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.dtPickerStartSearchFrom.MonthCalendar.TodayButtonVisible = true;
            this.dtPickerStartSearchFrom.MonthCalendar.WeeklyMarkedDays = new System.DayOfWeek[0];
            this.dtPickerStartSearchFrom.Name = "dtPickerStartSearchFrom";
            this.dtPickerStartSearchFrom.ShowUpDown = true;
            this.dtPickerStartSearchFrom.Size = new System.Drawing.Size(279, 20);
            this.dtPickerStartSearchFrom.TabIndex = 9;
            this.dtPickerStartSearchFrom.Value = new System.DateTime(2015, 8, 21, 0, 0, 0, 0);
            // 
            // wpTypeSelection
            // 
            this.wpTypeSelection.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.wpTypeSelection.BackColor = System.Drawing.Color.Transparent;
            this.wpTypeSelection.CanvasColor = System.Drawing.Color.Transparent;
            this.wpTypeSelection.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.wpTypeSelection.Controls.Add(this.panel2);
            this.wpTypeSelection.Location = new System.Drawing.Point(7, 62);
            this.wpTypeSelection.Name = "wpTypeSelection";
            this.wpTypeSelection.PageDescription = "Type selection";
            this.wpTypeSelection.PageTitle = "Type selection";
            this.wpTypeSelection.Size = new System.Drawing.Size(932, 395);
            this.wpTypeSelection.TabIndex = 2;
            this.wpTypeSelection.TabStop = true;
            this.wpTypeSelection.Text = "Type selection";
            this.wpTypeSelection.ThemeAware = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panelSequences);
            this.panel2.Controls.Add(this.rdoSequences);
            this.panel2.Controls.Add(this.label12);
            this.panel2.Controls.Add(this.txtSearch);
            this.panel2.Controls.Add(this.panelEvidenceLock);
            this.panel2.Controls.Add(this.chkMyBookmarksOnly);
            this.panel2.Controls.Add(this.rdoEvidenceLocks);
            this.panel2.Controls.Add(this.rdoBookmarks);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(932, 395);
            this.panel2.TabIndex = 0;
            // 
            // panelSequences
            // 
            this.panelSequences.Controls.Add(this.chkMergerSequenceExport);
            this.panelSequences.Controls.Add(this.label16);
            this.panelSequences.Controls.Add(this.intSegmentExport);
            this.panelSequences.Enabled = false;
            this.panelSequences.Location = new System.Drawing.Point(180, 318);
            this.panelSequences.Name = "panelSequences";
            this.panelSequences.Size = new System.Drawing.Size(383, 53);
            this.panelSequences.TabIndex = 16;
            // 
            // chkMergerSequenceExport
            // 
            this.chkMergerSequenceExport.AutoSize = true;
            this.chkMergerSequenceExport.BackColor = System.Drawing.Color.Transparent;
            this.chkMergerSequenceExport.Checked = true;
            this.chkMergerSequenceExport.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkMergerSequenceExport.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkMergerSequenceExport.Location = new System.Drawing.Point(18, 3);
            this.chkMergerSequenceExport.Name = "chkMergerSequenceExport";
            this.chkMergerSequenceExport.Size = new System.Drawing.Size(256, 17);
            this.chkMergerSequenceExport.TabIndex = 13;
            this.chkMergerSequenceExport.Text = "Merge all exports into one Sequence per Camera";
            this.chkMergerSequenceExport.UseVisualStyleBackColor = false;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.BackColor = System.Drawing.Color.Transparent;
            this.label16.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label16.Location = new System.Drawing.Point(78, 29);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(121, 13);
            this.label16.TabIndex = 15;
            this.label16.Text = "Segment export in hours";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // intSegmentExport
            // 
            // 
            // 
            // 
            this.intSegmentExport.BackgroundStyle.Class = "DateTimeInputBackground";
            this.intSegmentExport.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.intSegmentExport.ForeColor = System.Drawing.SystemColors.ControlText;
            this.intSegmentExport.Location = new System.Drawing.Point(18, 26);
            this.intSegmentExport.MaxValue = 240;
            this.intSegmentExport.MinValue = 1;
            this.intSegmentExport.Name = "intSegmentExport";
            this.intSegmentExport.ShowUpDown = true;
            this.intSegmentExport.Size = new System.Drawing.Size(54, 20);
            this.intSegmentExport.TabIndex = 14;
            this.intSegmentExport.Value = 24;
            // 
            // rdoSequences
            // 
            this.rdoSequences.AutoSize = true;
            this.rdoSequences.ForeColor = System.Drawing.SystemColors.ControlText;
            this.rdoSequences.Location = new System.Drawing.Point(136, 295);
            this.rdoSequences.Name = "rdoSequences";
            this.rdoSequences.Size = new System.Drawing.Size(79, 17);
            this.rdoSequences.TabIndex = 12;
            this.rdoSequences.Text = "Sequences";
            this.rdoSequences.UseVisualStyleBackColor = true;
            this.rdoSequences.CheckedChanged += new System.EventHandler(this.rdoSequences_CheckedChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label12.Location = new System.Drawing.Point(133, 235);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(41, 13);
            this.label12.TabIndex = 10;
            this.label12.Text = "Search";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtSearch
            // 
            // 
            // 
            // 
            this.txtSearch.Border.Class = "TextBoxBorder";
            this.txtSearch.ForeColor = System.Drawing.SystemColors.ControlText;
            this.txtSearch.Location = new System.Drawing.Point(180, 233);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(387, 20);
            this.txtSearch.TabIndex = 11;
            this.txtSearch.WatermarkText = "Text appears in either of the fields Reference, Header or Description";
            // 
            // panelEvidenceLock
            // 
            this.panelEvidenceLock.Controls.Add(this.chkAllELCameras);
            this.panelEvidenceLock.Controls.Add(this.chkMyEvidenceLockOnly);
            this.panelEvidenceLock.Enabled = false;
            this.panelEvidenceLock.Location = new System.Drawing.Point(184, 144);
            this.panelEvidenceLock.Name = "panelEvidenceLock";
            this.panelEvidenceLock.Size = new System.Drawing.Size(383, 47);
            this.panelEvidenceLock.TabIndex = 9;
            // 
            // chkAllELCameras
            // 
            this.chkAllELCameras.AutoSize = true;
            this.chkAllELCameras.BackColor = System.Drawing.Color.Transparent;
            this.chkAllELCameras.Checked = true;
            this.chkAllELCameras.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAllELCameras.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkAllELCameras.Location = new System.Drawing.Point(26, 27);
            this.chkAllELCameras.Name = "chkAllELCameras";
            this.chkAllELCameras.Size = new System.Drawing.Size(247, 17);
            this.chkAllELCameras.TabIndex = 4;
            this.chkAllELCameras.Text = "Export all Cameras found in the Evidence Lock";
            this.chkAllELCameras.UseVisualStyleBackColor = false;
            // 
            // chkMyEvidenceLockOnly
            // 
            this.chkMyEvidenceLockOnly.AutoSize = true;
            this.chkMyEvidenceLockOnly.BackColor = System.Drawing.Color.Transparent;
            this.chkMyEvidenceLockOnly.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkMyEvidenceLockOnly.Location = new System.Drawing.Point(26, 5);
            this.chkMyEvidenceLockOnly.Name = "chkMyEvidenceLockOnly";
            this.chkMyEvidenceLockOnly.Size = new System.Drawing.Size(137, 17);
            this.chkMyEvidenceLockOnly.TabIndex = 3;
            this.chkMyEvidenceLockOnly.Text = "My Evidence Lock only";
            this.chkMyEvidenceLockOnly.UseVisualStyleBackColor = false;
            // 
            // chkMyBookmarksOnly
            // 
            this.chkMyBookmarksOnly.AutoSize = true;
            this.chkMyBookmarksOnly.BackColor = System.Drawing.Color.Transparent;
            this.chkMyBookmarksOnly.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkMyBookmarksOnly.Location = new System.Drawing.Point(212, 59);
            this.chkMyBookmarksOnly.Name = "chkMyBookmarksOnly";
            this.chkMyBookmarksOnly.Size = new System.Drawing.Size(113, 17);
            this.chkMyBookmarksOnly.TabIndex = 8;
            this.chkMyBookmarksOnly.Text = "My Bookmark only";
            this.chkMyBookmarksOnly.UseVisualStyleBackColor = false;
            // 
            // rdoEvidenceLocks
            // 
            this.rdoEvidenceLocks.AutoSize = true;
            this.rdoEvidenceLocks.ForeColor = System.Drawing.SystemColors.ControlText;
            this.rdoEvidenceLocks.Location = new System.Drawing.Point(136, 121);
            this.rdoEvidenceLocks.Name = "rdoEvidenceLocks";
            this.rdoEvidenceLocks.Size = new System.Drawing.Size(102, 17);
            this.rdoEvidenceLocks.TabIndex = 1;
            this.rdoEvidenceLocks.Text = "Evidence Locks";
            this.rdoEvidenceLocks.UseVisualStyleBackColor = true;
            // 
            // rdoBookmarks
            // 
            this.rdoBookmarks.AutoSize = true;
            this.rdoBookmarks.Checked = true;
            this.rdoBookmarks.ForeColor = System.Drawing.SystemColors.ControlText;
            this.rdoBookmarks.Location = new System.Drawing.Point(136, 36);
            this.rdoBookmarks.Name = "rdoBookmarks";
            this.rdoBookmarks.Size = new System.Drawing.Size(78, 17);
            this.rdoBookmarks.TabIndex = 0;
            this.rdoBookmarks.TabStop = true;
            this.rdoBookmarks.Text = "Bookmarks";
            this.rdoBookmarks.UseVisualStyleBackColor = true;
            this.rdoBookmarks.CheckedChanged += new System.EventHandler(this.rdoBookmarks_CheckedChanged);
            // 
            // wpBookmarksEvidenceLocksSequences
            // 
            this.wpBookmarksEvidenceLocksSequences.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.wpBookmarksEvidenceLocksSequences.BackColor = System.Drawing.Color.Transparent;
            this.wpBookmarksEvidenceLocksSequences.CanvasColor = System.Drawing.Color.Transparent;
            this.wpBookmarksEvidenceLocksSequences.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.wpBookmarksEvidenceLocksSequences.Controls.Add(this.panel3);
            this.wpBookmarksEvidenceLocksSequences.Location = new System.Drawing.Point(7, 62);
            this.wpBookmarksEvidenceLocksSequences.Name = "wpBookmarksEvidenceLocksSequences";
            this.wpBookmarksEvidenceLocksSequences.PageDescription = "Bookmarks";
            this.wpBookmarksEvidenceLocksSequences.PageTitle = "Bookmarks";
            this.wpBookmarksEvidenceLocksSequences.Size = new System.Drawing.Size(932, 395);
            this.wpBookmarksEvidenceLocksSequences.TabIndex = 3;
            this.wpBookmarksEvidenceLocksSequences.TabStop = true;
            this.wpBookmarksEvidenceLocksSequences.Text = "Bookmarks";
            this.wpBookmarksEvidenceLocksSequences.ThemeAware = true;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnVideoReplay);
            this.panel3.Controls.Add(this.lblBookmarksCount);
            this.panel3.Controls.Add(this.lblBECount);
            this.panel3.Controls.Add(this.btnSearch);
            this.panel3.Controls.Add(this.btnStop);
            this.panel3.Controls.Add(this.chkAll);
            this.panel3.Controls.Add(this.lsvData);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(932, 395);
            this.panel3.TabIndex = 2;
            // 
            // btnVideoReplay
            // 
            this.btnVideoReplay.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnVideoReplay.BackColor = System.Drawing.SystemColors.Control;
            this.btnVideoReplay.Enabled = false;
            this.btnVideoReplay.Location = new System.Drawing.Point(445, 365);
            this.btnVideoReplay.Name = "btnVideoReplay";
            this.btnVideoReplay.Size = new System.Drawing.Size(90, 23);
            this.btnVideoReplay.TabIndex = 23;
            this.btnVideoReplay.Text = "Play";
            this.btnVideoReplay.UseVisualStyleBackColor = false;
            this.btnVideoReplay.Click += new System.EventHandler(this.btnVideoReplay_Click);
            // 
            // lblBookmarksCount
            // 
            this.lblBookmarksCount.BackColor = System.Drawing.SystemColors.Control;
            this.lblBookmarksCount.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblBookmarksCount.Location = new System.Drawing.Point(171, 368);
            this.lblBookmarksCount.Name = "lblBookmarksCount";
            this.lblBookmarksCount.Size = new System.Drawing.Size(97, 18);
            this.lblBookmarksCount.TabIndex = 22;
            this.lblBookmarksCount.Text = "0";
            this.lblBookmarksCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblBECount
            // 
            this.lblBECount.AutoSize = true;
            this.lblBECount.BackColor = System.Drawing.SystemColors.Control;
            this.lblBECount.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblBECount.Location = new System.Drawing.Point(105, 370);
            this.lblBECount.Name = "lblBECount";
            this.lblBECount.Size = new System.Drawing.Size(60, 13);
            this.lblBECount.TabIndex = 21;
            this.lblBECount.Text = "Data found";
            this.lblBECount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnSearch
            // 
            this.btnSearch.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSearch.BackColor = System.Drawing.SystemColors.Control;
            this.btnSearch.Location = new System.Drawing.Point(288, 365);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(61, 23);
            this.btnSearch.TabIndex = 20;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnStop
            // 
            this.btnStop.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnStop.BackColor = System.Drawing.SystemColors.Control;
            this.btnStop.Enabled = false;
            this.btnStop.Location = new System.Drawing.Point(367, 365);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(61, 23);
            this.btnStop.TabIndex = 19;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = false;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // chkAll
            // 
            this.chkAll.AutoSize = true;
            this.chkAll.BackColor = System.Drawing.Color.Transparent;
            this.chkAll.Location = new System.Drawing.Point(5, 369);
            this.chkAll.Name = "chkAll";
            this.chkAll.Size = new System.Drawing.Size(37, 17);
            this.chkAll.TabIndex = 4;
            this.chkAll.Text = "All";
            this.chkAll.UseVisualStyleBackColor = false;
            this.chkAll.CheckedChanged += new System.EventHandler(this.chkAll_CheckedChanged);
            // 
            // lsvData
            // 
            this.lsvData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lsvData.BackColor = System.Drawing.SystemColors.Control;
            this.lsvData.CheckBoxes = true;
            this.lsvData.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Header,
            this.Description,
            this.TimeBegin,
            this.TimeEnd,
            this.Cameras,
            this.Reference});
            this.lsvData.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lsvData.Location = new System.Drawing.Point(0, 0);
            this.lsvData.Name = "lsvData";
            this.lsvData.Size = new System.Drawing.Size(932, 359);
            this.lsvData.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lsvData.TabIndex = 0;
            this.lsvData.UseCompatibleStateImageBehavior = false;
            this.lsvData.View = System.Windows.Forms.View.Details;
            // 
            // Header
            // 
            this.Header.Text = "Header";
            this.Header.Width = 180;
            // 
            // Description
            // 
            this.Description.Text = "Description";
            this.Description.Width = 140;
            // 
            // TimeBegin
            // 
            this.TimeBegin.Text = "Time begin";
            this.TimeBegin.Width = 135;
            // 
            // TimeEnd
            // 
            this.TimeEnd.Text = "Time end";
            this.TimeEnd.Width = 135;
            // 
            // Cameras
            // 
            this.Cameras.Text = "Camera";
            this.Cameras.Width = 160;
            // 
            // Reference
            // 
            this.Reference.Text = "Reference";
            this.Reference.Width = 135;
            // 
            // wpExportType
            // 
            this.wpExportType.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.wpExportType.BackColor = System.Drawing.Color.Transparent;
            this.wpExportType.CanvasColor = System.Drawing.Color.Transparent;
            this.wpExportType.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.wpExportType.Controls.Add(this.panel4);
            this.wpExportType.Location = new System.Drawing.Point(7, 62);
            this.wpExportType.Name = "wpExportType";
            this.wpExportType.PageDescription = "Export type";
            this.wpExportType.PageTitle = "Export type";
            this.wpExportType.Size = new System.Drawing.Size(932, 395);
            this.wpExportType.TabIndex = 4;
            this.wpExportType.TabStop = true;
            this.wpExportType.Text = "Export type";
            this.wpExportType.ThemeAware = true;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.exportTypeControl);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(932, 395);
            this.panel4.TabIndex = 1;
            // 
            // exportTypeControl
            // 
            this.exportTypeControl.AllowUpscaling = false;
            this.exportTypeControl.AudioSampleDepth = 16;
            this.exportTypeControl.AudioSampleRate = 8000;
            this.exportTypeControl.AutoSplitExportFile = false;
            this.exportTypeControl.BackColor = System.Drawing.Color.Transparent;
            this.exportTypeControl.Channels = 1;
            this.exportTypeControl.Codec = 1;
            this.exportTypeControl.Content = 2;
            this.exportTypeControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.exportTypeControl.Encryption = false;
            this.exportTypeControl.EncryptionStrength = 1;
            this.exportTypeControl.FillSpace = false;
            this.exportTypeControl.ForeColor = System.Drawing.SystemColors.ControlText;
            this.exportTypeControl.FrameHeight = 240;
            this.exportTypeControl.FrameRate = 8;
            this.exportTypeControl.FrameWidth = 320;
            this.exportTypeControl.IgnoreImagesMinCount = 0;
            this.exportTypeControl.ImageHeight = 480;
            this.exportTypeControl.ImageWidth = 640;
            this.exportTypeControl.IncludePlayer = false;
            this.exportTypeControl.IncludeTimestamps = false;
            this.exportTypeControl.KeepAspectRatio = true;
            this.exportTypeControl.Location = new System.Drawing.Point(0, 0);
            this.exportTypeControl.MatroskaMkv = false;
            this.exportTypeControl.MaxAVIFileSize = 512;
            this.exportTypeControl.MediaPlayerFormat = false;
            this.exportTypeControl.MergeAllCameras = false;
            this.exportTypeControl.Name = "exportTypeControl";
            this.exportTypeControl.Quality = 75;
            this.exportTypeControl.SampleImages = 5000;
            this.exportTypeControl.SignExport = false;
            this.exportTypeControl.Size = new System.Drawing.Size(932, 395);
            this.exportTypeControl.StillImages = false;
            this.exportTypeControl.StillImagesOption = 0;
            this.exportTypeControl.TabIndex = 0;
            this.exportTypeControl.UseFrameDefaultValues = true;
            this.exportTypeControl.XProtectFormat = true;
            // 
            // wpTarget
            // 
            this.wpTarget.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.wpTarget.BackColor = System.Drawing.Color.Transparent;
            this.wpTarget.CanvasColor = System.Drawing.Color.Transparent;
            this.wpTarget.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.wpTarget.Controls.Add(this.panelTarget);
            this.wpTarget.Location = new System.Drawing.Point(7, 62);
            this.wpTarget.Name = "wpTarget";
            this.wpTarget.PageDescription = "Target";
            this.wpTarget.PageTitle = "Target";
            this.wpTarget.Size = new System.Drawing.Size(932, 395);
            this.wpTarget.TabIndex = 5;
            this.wpTarget.TabStop = true;
            this.wpTarget.Text = "Target";
            this.wpTarget.ThemeAware = true;
            // 
            // panelTarget
            // 
            this.panelTarget.Controls.Add(this.label17);
            this.panelTarget.Controls.Add(this.label19);
            this.panelTarget.Controls.Add(this.txtExportDestination);
            this.panelTarget.Controls.Add(this.panelCredentials);
            this.panelTarget.Controls.Add(this.btnBrowseExportDestination);
            this.panelTarget.Controls.Add(this.label18);
            this.panelTarget.Controls.Add(this.label2);
            this.panelTarget.Controls.Add(this.rdoNetwork);
            this.panelTarget.Controls.Add(this.rdoLocal);
            this.panelTarget.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTarget.ForeColor = System.Drawing.SystemColors.ControlText;
            this.panelTarget.Location = new System.Drawing.Point(0, 0);
            this.panelTarget.Name = "panelTarget";
            this.panelTarget.Size = new System.Drawing.Size(932, 395);
            this.panelTarget.TabIndex = 43;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.BackColor = System.Drawing.Color.Transparent;
            this.label17.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label17.Location = new System.Drawing.Point(37, 27);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(76, 13);
            this.label17.TabIndex = 38;
            this.label17.Text = "Folder location";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.BackColor = System.Drawing.Color.Transparent;
            this.label19.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label19.Location = new System.Drawing.Point(52, 145);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(59, 13);
            this.label19.TabIndex = 42;
            this.label19.Text = "Credentials";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtExportDestination
            // 
            // 
            // 
            // 
            this.txtExportDestination.Border.Class = "TextBoxBorder";
            this.txtExportDestination.ForeColor = System.Drawing.SystemColors.ControlText;
            this.txtExportDestination.Location = new System.Drawing.Point(119, 69);
            this.txtExportDestination.MaxLength = 32000;
            this.txtExportDestination.Name = "txtExportDestination";
            this.txtExportDestination.Size = new System.Drawing.Size(632, 20);
            this.txtExportDestination.TabIndex = 28;
            this.txtExportDestination.WatermarkText = "Export destination";
            // 
            // panelCredentials
            // 
            this.panelCredentials.Controls.Add(this.txtUserName);
            this.panelCredentials.Controls.Add(this.label13);
            this.panelCredentials.Controls.Add(this.txtPassword);
            this.panelCredentials.Controls.Add(this.label14);
            this.panelCredentials.Controls.Add(this.txtDomain);
            this.panelCredentials.Controls.Add(this.btnTest);
            this.panelCredentials.Controls.Add(this.label15);
            this.panelCredentials.Enabled = false;
            this.panelCredentials.Location = new System.Drawing.Point(119, 167);
            this.panelCredentials.Name = "panelCredentials";
            this.panelCredentials.Size = new System.Drawing.Size(548, 61);
            this.panelCredentials.TabIndex = 41;
            // 
            // txtUserName
            // 
            // 
            // 
            // 
            this.txtUserName.Border.Class = "TextBoxBorder";
            this.txtUserName.ForeColor = System.Drawing.SystemColors.ControlText;
            this.txtUserName.Location = new System.Drawing.Point(107, 7);
            this.txtUserName.MaxLength = 32000;
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(126, 20);
            this.txtUserName.TabIndex = 31;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.BackColor = System.Drawing.Color.Transparent;
            this.label13.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label13.Location = new System.Drawing.Point(43, 9);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(58, 13);
            this.label13.TabIndex = 30;
            this.label13.Text = "User name";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPassword
            // 
            // 
            // 
            // 
            this.txtPassword.Border.Class = "TextBoxBorder";
            this.txtPassword.ForeColor = System.Drawing.SystemColors.ControlText;
            this.txtPassword.Location = new System.Drawing.Point(379, 7);
            this.txtPassword.MaxLength = 32000;
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(126, 20);
            this.txtPassword.TabIndex = 33;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.BackColor = System.Drawing.Color.Transparent;
            this.label14.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label14.Location = new System.Drawing.Point(320, 9);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(53, 13);
            this.label14.TabIndex = 32;
            this.label14.Text = "Password";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtDomain
            // 
            // 
            // 
            // 
            this.txtDomain.Border.Class = "TextBoxBorder";
            this.txtDomain.ForeColor = System.Drawing.SystemColors.ControlText;
            this.txtDomain.Location = new System.Drawing.Point(107, 33);
            this.txtDomain.MaxLength = 32000;
            this.txtDomain.Name = "txtDomain";
            this.txtDomain.Size = new System.Drawing.Size(126, 20);
            this.txtDomain.TabIndex = 35;
            // 
            // btnTest
            // 
            this.btnTest.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnTest.BackColor = System.Drawing.SystemColors.Control;
            this.btnTest.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnTest.Location = new System.Drawing.Point(404, 30);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(61, 23);
            this.btnTest.TabIndex = 36;
            this.btnTest.Text = "Test";
            this.btnTest.UseVisualStyleBackColor = false;
            this.btnTest.Visible = false;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.BackColor = System.Drawing.Color.Transparent;
            this.label15.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label15.Location = new System.Drawing.Point(5, 35);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(96, 13);
            this.label15.TabIndex = 34;
            this.label15.Text = "Hostname/Domain";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnBrowseExportDestination
            // 
            this.btnBrowseExportDestination.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnBrowseExportDestination.BackColor = System.Drawing.SystemColors.Control;
            this.btnBrowseExportDestination.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnBrowseExportDestination.Location = new System.Drawing.Point(757, 66);
            this.btnBrowseExportDestination.Name = "btnBrowseExportDestination";
            this.btnBrowseExportDestination.Size = new System.Drawing.Size(61, 23);
            this.btnBrowseExportDestination.TabIndex = 29;
            this.btnBrowseExportDestination.Text = "...";
            this.btnBrowseExportDestination.UseVisualStyleBackColor = false;
            this.btnBrowseExportDestination.Click += new System.EventHandler(this.btnBrowseExportDestination_Click);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.BackColor = System.Drawing.Color.Transparent;
            this.label18.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label18.Location = new System.Drawing.Point(124, 92);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(410, 13);
            this.label18.TabIndex = 40;
            this.label18.Text = "Note: if Archiving will be selected, this path should be accessible by Rimage Pub" +
    "lisher";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label2.Location = new System.Drawing.Point(52, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 27;
            this.label2.Text = "Export path";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // rdoNetwork
            // 
            this.rdoNetwork.AutoSize = true;
            this.rdoNetwork.ForeColor = System.Drawing.SystemColors.ControlText;
            this.rdoNetwork.Location = new System.Drawing.Point(300, 27);
            this.rdoNetwork.Name = "rdoNetwork";
            this.rdoNetwork.Size = new System.Drawing.Size(65, 17);
            this.rdoNetwork.TabIndex = 39;
            this.rdoNetwork.Text = "Network";
            this.rdoNetwork.UseVisualStyleBackColor = true;
            this.rdoNetwork.CheckedChanged += new System.EventHandler(this.rdoNetwork_CheckedChanged);
            // 
            // rdoLocal
            // 
            this.rdoLocal.AutoSize = true;
            this.rdoLocal.Checked = true;
            this.rdoLocal.ForeColor = System.Drawing.SystemColors.ControlText;
            this.rdoLocal.Location = new System.Drawing.Point(119, 27);
            this.rdoLocal.Name = "rdoLocal";
            this.rdoLocal.Size = new System.Drawing.Size(123, 17);
            this.rdoLocal.TabIndex = 37;
            this.rdoLocal.TabStop = true;
            this.rdoLocal.Text = "Local / shared folder";
            this.rdoLocal.UseVisualStyleBackColor = true;
            // 
            // wpArchiveSettings
            // 
            this.wpArchiveSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.wpArchiveSettings.BackColor = System.Drawing.Color.Transparent;
            this.wpArchiveSettings.CanvasColor = System.Drawing.Color.Transparent;
            this.wpArchiveSettings.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.wpArchiveSettings.Controls.Add(this.panel5);
            this.wpArchiveSettings.Location = new System.Drawing.Point(7, 62);
            this.wpArchiveSettings.Name = "wpArchiveSettings";
            this.wpArchiveSettings.PageDescription = "Export settings";
            this.wpArchiveSettings.PageTitle = "Export settings";
            this.wpArchiveSettings.Size = new System.Drawing.Size(932, 395);
            this.wpArchiveSettings.TabIndex = 6;
            this.wpArchiveSettings.TabStop = true;
            this.wpArchiveSettings.Text = "Export settings";
            this.wpArchiveSettings.ThemeAware = true;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.btnEditCustomerInfo);
            this.panel5.Controls.Add(this.btnCustomerLogo);
            this.panel5.Controls.Add(this.btnPrintLabel);
            this.panel5.Controls.Add(this.btnBrowseIncludeFolder);
            this.panel5.Controls.Add(this.txtCustomerInfo);
            this.panel5.Controls.Add(this.txtPrintLabel);
            this.panel5.Controls.Add(this.txtCustomerLogo);
            this.panel5.Controls.Add(this.label8);
            this.panel5.Controls.Add(this.label7);
            this.panel5.Controls.Add(this.label5);
            this.panel5.Controls.Add(this.chkDeleteAfterArchiving);
            this.panel5.Controls.Add(this.txtHelperApplicationPath);
            this.panel5.Controls.Add(this.label4);
            this.panel5.Controls.Add(this.cboUdfFormat);
            this.panel5.Controls.Add(this.label3);
            this.panel5.Controls.Add(this.cboMediaType);
            this.panel5.Controls.Add(this.labelX6);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(932, 395);
            this.panel5.TabIndex = 0;
            // 
            // btnEditCustomerInfo
            // 
            this.btnEditCustomerInfo.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnEditCustomerInfo.BackColor = System.Drawing.SystemColors.Control;
            this.btnEditCustomerInfo.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnEditCustomerInfo.Location = new System.Drawing.Point(793, 227);
            this.btnEditCustomerInfo.Name = "btnEditCustomerInfo";
            this.btnEditCustomerInfo.Size = new System.Drawing.Size(61, 23);
            this.btnEditCustomerInfo.TabIndex = 16;
            this.btnEditCustomerInfo.Text = "Edit";
            this.btnEditCustomerInfo.UseVisualStyleBackColor = false;
            this.btnEditCustomerInfo.Click += new System.EventHandler(this.btnGetCustomerInfo_Click);
            // 
            // btnCustomerLogo
            // 
            this.btnCustomerLogo.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCustomerLogo.BackColor = System.Drawing.SystemColors.Control;
            this.btnCustomerLogo.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnCustomerLogo.Location = new System.Drawing.Point(793, 194);
            this.btnCustomerLogo.Name = "btnCustomerLogo";
            this.btnCustomerLogo.Size = new System.Drawing.Size(61, 23);
            this.btnCustomerLogo.TabIndex = 13;
            this.btnCustomerLogo.Text = "...";
            this.btnCustomerLogo.UseVisualStyleBackColor = false;
            this.btnCustomerLogo.Click += new System.EventHandler(this.btnCustomerLogo_Click);
            // 
            // btnPrintLabel
            // 
            this.btnPrintLabel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnPrintLabel.BackColor = System.Drawing.SystemColors.Control;
            this.btnPrintLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnPrintLabel.Location = new System.Drawing.Point(793, 161);
            this.btnPrintLabel.Name = "btnPrintLabel";
            this.btnPrintLabel.Size = new System.Drawing.Size(61, 23);
            this.btnPrintLabel.TabIndex = 10;
            this.btnPrintLabel.Text = "...";
            this.btnPrintLabel.UseVisualStyleBackColor = false;
            this.btnPrintLabel.Click += new System.EventHandler(this.btnPrintLabel_Click);
            // 
            // btnBrowseIncludeFolder
            // 
            this.btnBrowseIncludeFolder.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnBrowseIncludeFolder.BackColor = System.Drawing.SystemColors.Control;
            this.btnBrowseIncludeFolder.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnBrowseIncludeFolder.Location = new System.Drawing.Point(793, 101);
            this.btnBrowseIncludeFolder.Name = "btnBrowseIncludeFolder";
            this.btnBrowseIncludeFolder.Size = new System.Drawing.Size(61, 23);
            this.btnBrowseIncludeFolder.TabIndex = 6;
            this.btnBrowseIncludeFolder.Text = "...";
            this.btnBrowseIncludeFolder.UseVisualStyleBackColor = false;
            this.btnBrowseIncludeFolder.Click += new System.EventHandler(this.btnBrowseIncludeFolder_Click);
            // 
            // txtCustomerInfo
            // 
            // 
            // 
            // 
            this.txtCustomerInfo.Border.Class = "TextBoxBorder";
            this.txtCustomerInfo.ForeColor = System.Drawing.SystemColors.ControlText;
            this.txtCustomerInfo.Location = new System.Drawing.Point(155, 230);
            this.txtCustomerInfo.MaxLength = 32000;
            this.txtCustomerInfo.Name = "txtCustomerInfo";
            this.txtCustomerInfo.ReadOnly = true;
            this.txtCustomerInfo.Size = new System.Drawing.Size(632, 20);
            this.txtCustomerInfo.TabIndex = 15;
            // 
            // txtPrintLabel
            // 
            // 
            // 
            // 
            this.txtPrintLabel.Border.Class = "TextBoxBorder";
            this.txtPrintLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.txtPrintLabel.Location = new System.Drawing.Point(155, 164);
            this.txtPrintLabel.MaxLength = 32000;
            this.txtPrintLabel.Name = "txtPrintLabel";
            this.txtPrintLabel.Size = new System.Drawing.Size(632, 20);
            this.txtPrintLabel.TabIndex = 9;
            this.txtPrintLabel.WatermarkText = "\\\\RIMAGESYSTEM\\Rimage\\Labels\\StorageQuest";
            // 
            // txtCustomerLogo
            // 
            // 
            // 
            // 
            this.txtCustomerLogo.Border.Class = "TextBoxBorder";
            this.txtCustomerLogo.ForeColor = System.Drawing.SystemColors.ControlText;
            this.txtCustomerLogo.Location = new System.Drawing.Point(155, 197);
            this.txtCustomerLogo.MaxLength = 32000;
            this.txtCustomerLogo.Name = "txtCustomerLogo";
            this.txtCustomerLogo.Size = new System.Drawing.Size(632, 20);
            this.txtCustomerLogo.TabIndex = 12;
            this.txtCustomerLogo.WatermarkText = "\\\\RIMAGESYSTEM\\Rimage\\Labels\\StorageQuest";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label8.Location = new System.Drawing.Point(77, 232);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(72, 13);
            this.label8.TabIndex = 14;
            this.label8.Text = "Customer Info";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label7.Location = new System.Drawing.Point(102, 199);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(47, 13);
            this.label7.TabIndex = 11;
            this.label7.Text = "Logo file";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label5.Location = new System.Drawing.Point(67, 166);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(82, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Print label (.btw)";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chkDeleteAfterArchiving
            // 
            this.chkDeleteAfterArchiving.AutoSize = true;
            this.chkDeleteAfterArchiving.BackColor = System.Drawing.Color.Transparent;
            this.chkDeleteAfterArchiving.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkDeleteAfterArchiving.Location = new System.Drawing.Point(155, 134);
            this.chkDeleteAfterArchiving.Name = "chkDeleteAfterArchiving";
            this.chkDeleteAfterArchiving.Size = new System.Drawing.Size(192, 17);
            this.chkDeleteAfterArchiving.TabIndex = 7;
            this.chkDeleteAfterArchiving.Text = "Delete exported files after archiving";
            this.chkDeleteAfterArchiving.UseVisualStyleBackColor = false;
            // 
            // txtHelperApplicationPath
            // 
            // 
            // 
            // 
            this.txtHelperApplicationPath.Border.Class = "TextBoxBorder";
            this.txtHelperApplicationPath.ForeColor = System.Drawing.SystemColors.ControlText;
            this.txtHelperApplicationPath.Location = new System.Drawing.Point(155, 101);
            this.txtHelperApplicationPath.MaxLength = 32000;
            this.txtHelperApplicationPath.Name = "txtHelperApplicationPath";
            this.txtHelperApplicationPath.Size = new System.Drawing.Size(632, 20);
            this.txtHelperApplicationPath.TabIndex = 5;
            this.txtHelperApplicationPath.WatermarkText = "Contents are added to each media in the job.";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label4.Location = new System.Drawing.Point(75, 103);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Include Folder";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboUdfFormat
            // 
            this.cboUdfFormat.DisplayMember = "Text";
            this.cboUdfFormat.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboUdfFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboUdfFormat.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cboUdfFormat.FormattingEnabled = true;
            this.cboUdfFormat.ItemHeight = 14;
            this.cboUdfFormat.Location = new System.Drawing.Point(155, 68);
            this.cboUdfFormat.Name = "cboUdfFormat";
            this.cboUdfFormat.Size = new System.Drawing.Size(151, 20);
            this.cboUdfFormat.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.cboUdfFormat.TabIndex = 3;
            this.cboUdfFormat.WatermarkText = "Select UDF format";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label3.Location = new System.Drawing.Point(93, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Udf format";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboMediaType
            // 
            this.cboMediaType.DisplayMember = "Text";
            this.cboMediaType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboMediaType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMediaType.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.cboMediaType.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cboMediaType.FormattingEnabled = true;
            this.cboMediaType.ItemHeight = 14;
            this.cboMediaType.Location = new System.Drawing.Point(155, 35);
            this.cboMediaType.Name = "cboMediaType";
            this.cboMediaType.Size = new System.Drawing.Size(151, 20);
            this.cboMediaType.Sorted = true;
            this.cboMediaType.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.cboMediaType.TabIndex = 1;
            this.cboMediaType.WatermarkText = "Select media type";
            // 
            // labelX6
            // 
            this.labelX6.AutoSize = true;
            this.labelX6.BackColor = System.Drawing.Color.Transparent;
            this.labelX6.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelX6.Location = new System.Drawing.Point(64, 39);
            this.labelX6.Name = "labelX6";
            this.labelX6.Size = new System.Drawing.Size(85, 13);
            this.labelX6.TabIndex = 0;
            this.labelX6.Text = "Initial media type";
            this.labelX6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // wpReview
            // 
            this.wpReview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.wpReview.BackColor = System.Drawing.Color.Transparent;
            this.wpReview.CanvasColor = System.Drawing.Color.Transparent;
            this.wpReview.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.wpReview.Controls.Add(this.txtReview);
            this.wpReview.Location = new System.Drawing.Point(7, 62);
            this.wpReview.Name = "wpReview";
            this.wpReview.PageDescription = "Review";
            this.wpReview.PageTitle = "Review";
            this.wpReview.Size = new System.Drawing.Size(932, 395);
            this.wpReview.TabIndex = 7;
            this.wpReview.TabStop = true;
            this.wpReview.Text = "Review";
            this.wpReview.ThemeAware = true;
            // 
            // txtReview
            // 
            this.txtReview.BackColor = System.Drawing.SystemColors.Control;
            // 
            // 
            // 
            this.txtReview.Border.Class = "TextBoxBorder";
            this.txtReview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtReview.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtReview.Location = new System.Drawing.Point(0, 0);
            this.txtReview.Multiline = true;
            this.txtReview.Name = "txtReview";
            this.txtReview.ReadOnly = true;
            this.txtReview.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtReview.Size = new System.Drawing.Size(932, 395);
            this.txtReview.TabIndex = 4;
            // 
            // JobConfigurationControlForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(946, 509);
            this.Controls.Add(this.wizard1);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "JobConfigurationControlForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Create new job";
            this.Load += new System.EventHandler(this.JobConfigurationControlForm_Load);
            this.wizard1.ResumeLayout(false);
            this.wpJobNameAndDescription.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.wpSearchFilter.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtPickerStartSearchTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtPickerStartSearchFrom)).EndInit();
            this.wpTypeSelection.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panelSequences.ResumeLayout(false);
            this.panelSequences.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.intSegmentExport)).EndInit();
            this.panelEvidenceLock.ResumeLayout(false);
            this.panelEvidenceLock.PerformLayout();
            this.wpBookmarksEvidenceLocksSequences.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.wpExportType.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.wpTarget.ResumeLayout(false);
            this.panelTarget.ResumeLayout(false);
            this.panelTarget.PerformLayout();
            this.panelCredentials.ResumeLayout(false);
            this.panelCredentials.PerformLayout();
            this.wpArchiveSettings.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.wpReview.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Wizard wizard1;
        private DevComponents.DotNetBar.WizardPage wpJobNameAndDescription;
        private DevComponents.DotNetBar.WizardPage wpTypeSelection;
        private DevComponents.DotNetBar.WizardPage wpBookmarksEvidenceLocksSequences;
        private DevComponents.DotNetBar.WizardPage wpExportType;
        private DevComponents.DotNetBar.WizardPage wpReview;
        private DevComponents.Editors.ComboItem itemSunday;
        private DevComponents.Editors.ComboItem itemMonday;
        private DevComponents.Editors.ComboItem itemTuesday;
        private DevComponents.Editors.ComboItem itemWednesday;
        private DevComponents.Editors.ComboItem itemThursday;
        private DevComponents.Editors.ComboItem itemFriday;
        private DevComponents.Editors.ComboItem itemSaturday;
        private DevComponents.DotNetBar.Controls.TextBoxX txtReview;
        private DevComponents.DotNetBar.WizardPage wpArchiveSettings;
        private System.Windows.Forms.Panel panel1;
        private DevComponents.DotNetBar.Controls.TextBoxX txtDescription;
        private DevComponents.DotNetBar.Controls.TextBoxX txtJobName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RadioButton rdoEvidenceLocks;
        private System.Windows.Forms.RadioButton rdoBookmarks;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ListView lsvData;
        private System.Windows.Forms.ColumnHeader Header;
        private System.Windows.Forms.ColumnHeader Description;
        private System.Windows.Forms.ColumnHeader TimeBegin;
        private System.Windows.Forms.ColumnHeader TimeEnd;
        private System.Windows.Forms.ColumnHeader Cameras;
        private System.Windows.Forms.ColumnHeader Reference;
        private System.Windows.Forms.Panel panel4;
        private SegStorUserControls.ExportControl exportTypeControl;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Button btnEditCustomerInfo;
        private System.Windows.Forms.Button btnCustomerLogo;
        private System.Windows.Forms.Button btnPrintLabel;
        private System.Windows.Forms.Button btnBrowseIncludeFolder;
        private DevComponents.DotNetBar.Controls.TextBoxX txtCustomerInfo;
        private DevComponents.DotNetBar.Controls.TextBoxX txtPrintLabel;
        private DevComponents.DotNetBar.Controls.TextBoxX txtCustomerLogo;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox chkDeleteAfterArchiving;
        private DevComponents.DotNetBar.Controls.TextBoxX txtHelperApplicationPath;
        private System.Windows.Forms.Label label4;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cboUdfFormat;
        private System.Windows.Forms.Label label3;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cboMediaType;
        private System.Windows.Forms.Label labelX6;
        private DevComponents.DotNetBar.WizardPage wpSearchFilter;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private DevComponents.Editors.DateTimeAdv.DateTimeInput dtPickerStartSearchTo;
        private DevComponents.Editors.DateTimeAdv.DateTimeInput dtPickerStartSearchFrom;
        private System.Windows.Forms.ListView lsvCameras;
        private System.Windows.Forms.ColumnHeader columnHeader;
        private System.Windows.Forms.Button btnRemoveChecked;
        private System.Windows.Forms.Button btnRemoveAll;
        private System.Windows.Forms.Button btnSelectCamera;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox chkMyBookmarksOnly;
        private System.Windows.Forms.Panel panelEvidenceLock;
        private System.Windows.Forms.CheckBox chkAllELCameras;
        private System.Windows.Forms.CheckBox chkMyEvidenceLockOnly;
        private System.Windows.Forms.Label label12;
        private DevComponents.DotNetBar.Controls.TextBoxX txtSearch;
        private System.Windows.Forms.RadioButton rdoSequences;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.CheckBox chkAll;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label lblBookmarksCount;
        private System.Windows.Forms.Label lblBECount;
        private System.Windows.Forms.CheckBox chkMergerSequenceExport;
        private System.Windows.Forms.Label label16;
        private DevComponents.Editors.IntegerInput intSegmentExport;
        private System.Windows.Forms.Panel panelSequences;
        private DevComponents.DotNetBar.WizardPage wpTarget;
        private System.Windows.Forms.Button btnTest;
        private DevComponents.DotNetBar.Controls.TextBoxX txtDomain;
        private System.Windows.Forms.Label label15;
        private DevComponents.DotNetBar.Controls.TextBoxX txtPassword;
        private System.Windows.Forms.Label label14;
        private DevComponents.DotNetBar.Controls.TextBoxX txtUserName;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnBrowseExportDestination;
        private DevComponents.DotNetBar.Controls.TextBoxX txtExportDestination;
        private System.Windows.Forms.RadioButton rdoNetwork;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.RadioButton rdoLocal;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Panel panelCredentials;
        private System.Windows.Forms.Panel panelTarget;
        private System.Windows.Forms.Button btnVideoReplay;
    }
}