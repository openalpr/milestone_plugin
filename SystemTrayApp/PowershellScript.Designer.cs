namespace SystemTrayApp
{
    partial class PowershellScript
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PowershellScript));
            this.txtLogin = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.lblLogin = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.chkNetworkService = new System.Windows.Forms.CheckBox();
            this.lblUse = new System.Windows.Forms.Label();
            this.lblNetworkService = new System.Windows.Forms.Label();
            this.lblEventExpireAfterDays = new System.Windows.Forms.Label();
            this.nudEventExpireAfterDays = new System.Windows.Forms.NumericUpDown();
            this.nudEpochStartSecondsBefore = new System.Windows.Forms.NumericUpDown();
            this.lblEpochStartSecondsBefore = new System.Windows.Forms.Label();
            this.nudEpochEndSecondsAfter = new System.Windows.Forms.NumericUpDown();
            this.lblEpochEndSecondsAfter = new System.Windows.Forms.Label();
            this.nudServicePort = new System.Windows.Forms.NumericUpDown();
            this.lblServicePort = new System.Windows.Forms.Label();
            this.chkAddBookmarks = new System.Windows.Forms.CheckBox();
            this.chkAutoMapping = new System.Windows.Forms.CheckBox();
            this.lblServiceUrl = new System.Windows.Forms.Label();
            this.txtServiceUrl = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.nudEventExpireAfterDays)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudEpochStartSecondsBefore)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudEpochEndSecondsAfter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudServicePort)).BeginInit();
            this.SuspendLayout();
            // 
            // txtLogin
            // 
            this.txtLogin.Location = new System.Drawing.Point(12, 25);
            this.txtLogin.Name = "txtLogin";
            this.txtLogin.Size = new System.Drawing.Size(210, 20);
            this.txtLogin.TabIndex = 0;
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(12, 95);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '•';
            this.txtPassword.Size = new System.Drawing.Size(210, 20);
            this.txtPassword.TabIndex = 1;
            this.txtPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPassword_KeyDown);
            this.txtPassword.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtPassword_KeyUp);
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(12, 452);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(75, 23);
            this.btnLogin.TabIndex = 2;
            this.btnLogin.Text = "Save";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // lblLogin
            // 
            this.lblLogin.AutoSize = true;
            this.lblLogin.Location = new System.Drawing.Point(12, 7);
            this.lblLogin.Name = "lblLogin";
            this.lblLogin.Size = new System.Drawing.Size(38, 15);
            this.lblLogin.TabIndex = 3;
            this.lblLogin.Text = "Login";
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(12, 77);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(130, 15);
            this.lblPassword.TabIndex = 4;
            this.lblPassword.Text = "Current user password";
            // 
            // chkNetworkService
            // 
            this.chkNetworkService.AutoSize = true;
            this.chkNetworkService.Location = new System.Drawing.Point(12, 51);
            this.chkNetworkService.Name = "chkNetworkService";
            this.chkNetworkService.Size = new System.Drawing.Size(18, 17);
            this.chkNetworkService.TabIndex = 5;
            this.chkNetworkService.UseVisualStyleBackColor = true;
            this.chkNetworkService.CheckedChanged += new System.EventHandler(this.chkNetworkService_CheckedChanged);
            // 
            // lblUse
            // 
            this.lblUse.AutoSize = true;
            this.lblUse.Location = new System.Drawing.Point(31, 52);
            this.lblUse.Name = "lblUse";
            this.lblUse.Size = new System.Drawing.Size(29, 15);
            this.lblUse.TabIndex = 6;
            this.lblUse.Text = "Use";
            this.lblUse.Click += new System.EventHandler(this.lblUse_Click);
            // 
            // lblNetworkService
            // 
            this.lblNetworkService.AutoSize = true;
            this.lblNetworkService.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNetworkService.Location = new System.Drawing.Point(59, 52);
            this.lblNetworkService.Name = "lblNetworkService";
            this.lblNetworkService.Size = new System.Drawing.Size(125, 17);
            this.lblNetworkService.TabIndex = 7;
            this.lblNetworkService.Text = "Network Service";
            this.lblNetworkService.Click += new System.EventHandler(this.lblNetworkService_Click);
            // 
            // lblEventExpireAfterDays
            // 
            this.lblEventExpireAfterDays.AutoSize = true;
            this.lblEventExpireAfterDays.Location = new System.Drawing.Point(12, 127);
            this.lblEventExpireAfterDays.Name = "lblEventExpireAfterDays";
            this.lblEventExpireAfterDays.Size = new System.Drawing.Size(132, 15);
            this.lblEventExpireAfterDays.TabIndex = 8;
            this.lblEventExpireAfterDays.Text = "Event Expire After Days";
            // 
            // nudEventExpireAfterDays
            // 
            this.nudEventExpireAfterDays.Location = new System.Drawing.Point(12, 145);
            this.nudEventExpireAfterDays.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.nudEventExpireAfterDays.Name = "nudEventExpireAfterDays";
            this.nudEventExpireAfterDays.Size = new System.Drawing.Size(80, 20);
            this.nudEventExpireAfterDays.TabIndex = 9;
            // 
            // nudEpochStartSecondsBefore
            // 
            this.nudEpochStartSecondsBefore.InterceptArrowKeys = false;
            this.nudEpochStartSecondsBefore.Location = new System.Drawing.Point(12, 196);
            this.nudEpochStartSecondsBefore.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.nudEpochStartSecondsBefore.Name = "nudEpochStartSecondsBefore";
            this.nudEpochStartSecondsBefore.Size = new System.Drawing.Size(80, 20);
            this.nudEpochStartSecondsBefore.TabIndex = 11;
            // 
            // lblEpochStartSecondsBefore
            // 
            this.lblEpochStartSecondsBefore.AutoSize = true;
            this.lblEpochStartSecondsBefore.Location = new System.Drawing.Point(12, 178);
            this.lblEpochStartSecondsBefore.Name = "lblEpochStartSecondsBefore";
            this.lblEpochStartSecondsBefore.Size = new System.Drawing.Size(160, 15);
            this.lblEpochStartSecondsBefore.TabIndex = 10;
            this.lblEpochStartSecondsBefore.Text = "Epoch Start Seconds Before";
            // 
            // nudEpochEndSecondsAfter
            // 
            this.nudEpochEndSecondsAfter.Location = new System.Drawing.Point(12, 247);
            this.nudEpochEndSecondsAfter.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.nudEpochEndSecondsAfter.Name = "nudEpochEndSecondsAfter";
            this.nudEpochEndSecondsAfter.Size = new System.Drawing.Size(80, 20);
            this.nudEpochEndSecondsAfter.TabIndex = 13;
            // 
            // lblEpochEndSecondsAfter
            // 
            this.lblEpochEndSecondsAfter.AutoSize = true;
            this.lblEpochEndSecondsAfter.Location = new System.Drawing.Point(12, 229);
            this.lblEpochEndSecondsAfter.Name = "lblEpochEndSecondsAfter";
            this.lblEpochEndSecondsAfter.Size = new System.Drawing.Size(145, 15);
            this.lblEpochEndSecondsAfter.TabIndex = 12;
            this.lblEpochEndSecondsAfter.Text = "Epoch End Seconds After";
            // 
            // nudServicePort
            // 
            this.nudServicePort.Location = new System.Drawing.Point(12, 297);
            this.nudServicePort.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.nudServicePort.Name = "nudServicePort";
            this.nudServicePort.Size = new System.Drawing.Size(80, 20);
            this.nudServicePort.TabIndex = 15;
            // 
            // lblServicePort
            // 
            this.lblServicePort.AutoSize = true;
            this.lblServicePort.Location = new System.Drawing.Point(12, 279);
            this.lblServicePort.Name = "lblServicePort";
            this.lblServicePort.Size = new System.Drawing.Size(72, 15);
            this.lblServicePort.TabIndex = 14;
            this.lblServicePort.Text = "Service Port";
            // 
            // chkAddBookmarks
            // 
            this.chkAddBookmarks.AutoSize = true;
            this.chkAddBookmarks.Location = new System.Drawing.Point(12, 383);
            this.chkAddBookmarks.Name = "chkAddBookmarks";
            this.chkAddBookmarks.Size = new System.Drawing.Size(115, 19);
            this.chkAddBookmarks.TabIndex = 20;
            this.chkAddBookmarks.Text = "Add Bookmarks";
            this.chkAddBookmarks.UseVisualStyleBackColor = true;
            // 
            // chkAutoMapping
            // 
            this.chkAutoMapping.AutoSize = true;
            this.chkAutoMapping.Location = new System.Drawing.Point(12, 417);
            this.chkAutoMapping.Name = "chkAutoMapping";
            this.chkAutoMapping.Size = new System.Drawing.Size(105, 19);
            this.chkAutoMapping.TabIndex = 21;
            this.chkAutoMapping.Text = "Auto Mapping";
            this.chkAutoMapping.UseVisualStyleBackColor = true;
            // 
            // lblServiceUrl
            // 
            this.lblServiceUrl.AutoSize = true;
            this.lblServiceUrl.Location = new System.Drawing.Point(12, 330);
            this.lblServiceUrl.Name = "lblServiceUrl";
            this.lblServiceUrl.Size = new System.Drawing.Size(75, 15);
            this.lblServiceUrl.TabIndex = 22;
            this.lblServiceUrl.Text = "Service URL";
            // 
            // txtServiceUrl
            // 
            this.txtServiceUrl.Location = new System.Drawing.Point(12, 348);
            this.txtServiceUrl.Name = "txtServiceUrl";
            this.txtServiceUrl.Size = new System.Drawing.Size(210, 20);
            this.txtServiceUrl.TabIndex = 23;
            // 
            // PowershellScript
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(234, 491);
            this.Controls.Add(this.txtServiceUrl);
            this.Controls.Add(this.lblServiceUrl);
            this.Controls.Add(this.chkAutoMapping);
            this.Controls.Add(this.chkAddBookmarks);
            this.Controls.Add(this.nudServicePort);
            this.Controls.Add(this.lblServicePort);
            this.Controls.Add(this.nudEpochEndSecondsAfter);
            this.Controls.Add(this.lblEpochEndSecondsAfter);
            this.Controls.Add(this.nudEpochStartSecondsBefore);
            this.Controls.Add(this.lblEpochStartSecondsBefore);
            this.Controls.Add(this.nudEventExpireAfterDays);
            this.Controls.Add(this.lblEventExpireAfterDays);
            this.Controls.Add(this.lblNetworkService);
            this.Controls.Add(this.lblUse);
            this.Controls.Add(this.chkNetworkService);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.lblLogin);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtLogin);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PowershellScript";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Settings";
            this.Activated += new System.EventHandler(this.PowershellScript_Activated);
            ((System.ComponentModel.ISupportInitialize)(this.nudEventExpireAfterDays)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudEpochStartSecondsBefore)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudEpochEndSecondsAfter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudServicePort)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtLogin;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Label lblLogin;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.CheckBox chkNetworkService;
        private System.Windows.Forms.Label lblUse;
        private System.Windows.Forms.Label lblNetworkService;
        private System.Windows.Forms.Label lblEventExpireAfterDays;
        private System.Windows.Forms.NumericUpDown nudEventExpireAfterDays;
        private System.Windows.Forms.NumericUpDown nudEpochStartSecondsBefore;
        private System.Windows.Forms.Label lblEpochStartSecondsBefore;
        private System.Windows.Forms.NumericUpDown nudEpochEndSecondsAfter;
        private System.Windows.Forms.Label lblEpochEndSecondsAfter;
        private System.Windows.Forms.NumericUpDown nudServicePort;
        private System.Windows.Forms.Label lblServicePort;
        private System.Windows.Forms.CheckBox chkAddBookmarks;
        private System.Windows.Forms.CheckBox chkAutoMapping;
        private System.Windows.Forms.Label lblServiceUrl;
        private System.Windows.Forms.TextBox txtServiceUrl;
    }
}