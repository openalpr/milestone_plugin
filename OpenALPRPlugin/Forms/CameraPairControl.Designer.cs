namespace OpenALPRPlugin.Forms
{
    partial class CameraPairControl
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
            this.TxtMilestoneCameraName = new System.Windows.Forms.TextBox();
            this.cboName = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // TxtMilestoneCameraName
            // 
            this.TxtMilestoneCameraName.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.TxtMilestoneCameraName.Location = new System.Drawing.Point(3, 3);
            this.TxtMilestoneCameraName.Name = "TxtMilestoneCameraName";
            this.TxtMilestoneCameraName.ReadOnly = true;
            this.TxtMilestoneCameraName.Size = new System.Drawing.Size(340, 20);
            this.TxtMilestoneCameraName.TabIndex = 10;
            this.TxtMilestoneCameraName.TabStop = false;
            this.TxtMilestoneCameraName.WordWrap = false;
            // 
            // cboName
            // 
            this.cboName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboName.FormattingEnabled = true;
            this.cboName.Location = new System.Drawing.Point(350, 3);
            this.cboName.Name = "cboName";
            this.cboName.Size = new System.Drawing.Size(197, 21);
            this.cboName.TabIndex = 11;
            // 
            // CameraPairControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cboName);
            this.Controls.Add(this.TxtMilestoneCameraName);
            this.Name = "CameraPairControl";
            this.Size = new System.Drawing.Size(553, 27);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.TextBox TxtMilestoneCameraName;
        internal System.Windows.Forms.ComboBox cboName;
    }
}
