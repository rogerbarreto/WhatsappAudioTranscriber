namespace WhatsappAudioTranscriber
{
    partial class ProgressDialog
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
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
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
            this.progressBar = new ProgressBar();
            this.lblStatus = new Label();
            this.lblTitle = new Label();
            this.SuspendLayout();
            // 
            // progressBar
            // 
            this.progressBar.Location = new Point(20, 60);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new Size(360, 23);
            this.progressBar.TabIndex = 0;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new Point(20, 95);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new Size(87, 15);
            this.lblStatus.TabIndex = 1;
            this.lblStatus.Text = "Initializing...";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.lblTitle.Location = new Point(20, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new Size(178, 21);
            this.lblTitle.TabIndex = 2;
            this.lblTitle.Text = "Loading Whisper Model";
            // 
            // ProgressDialog
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(400, 140);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.progressBar);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProgressDialog";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Loading Model";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private ProgressBar progressBar;
        private Label lblStatus;
        private Label lblTitle;
    }
}
