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
            this.TxtALPRCameraName = new System.Windows.Forms.TextBox();
            this.TxtALPRCameraId = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // TxtMilestoneCameraName
            // 
            this.TxtMilestoneCameraName.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.TxtMilestoneCameraName.Location = new System.Drawing.Point(3, 5);
            this.TxtMilestoneCameraName.Name = "TxtMilestoneCameraName";
            this.TxtMilestoneCameraName.ReadOnly = true;
            this.TxtMilestoneCameraName.Size = new System.Drawing.Size(340, 20);
            this.TxtMilestoneCameraName.TabIndex = 10;
            this.TxtMilestoneCameraName.TabStop = false;
            this.TxtMilestoneCameraName.WordWrap = false;
            // 
            // TxtALPRCameraName
            // 
            this.TxtALPRCameraName.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.TxtALPRCameraName.Location = new System.Drawing.Point(347, 5);
            this.TxtALPRCameraName.Name = "TxtALPRCameraName";
            this.TxtALPRCameraName.Size = new System.Drawing.Size(197, 20);
            this.TxtALPRCameraName.TabIndex = 9;
            this.TxtALPRCameraName.TabStop = false;
            this.TxtALPRCameraName.WordWrap = false;
            // 
            // TxtALPRCameraId
            // 
            this.TxtALPRCameraId.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.TxtALPRCameraId.Location = new System.Drawing.Point(548, 5);
            this.TxtALPRCameraId.Name = "TxtALPRCameraId";
            this.TxtALPRCameraId.Size = new System.Drawing.Size(180, 20);
            this.TxtALPRCameraId.TabIndex = 11;
            this.TxtALPRCameraId.TabStop = false;
            this.TxtALPRCameraId.WordWrap = false;
            // 
            // CameraPairControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.TxtALPRCameraId);
            this.Controls.Add(this.TxtMilestoneCameraName);
            this.Controls.Add(this.TxtALPRCameraName);
            this.Name = "CameraPairControl";
            this.Size = new System.Drawing.Size(736, 31);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.TextBox TxtMilestoneCameraName;
        internal System.Windows.Forms.TextBox TxtALPRCameraName;
        internal System.Windows.Forms.TextBox TxtALPRCameraId;
    }
}
