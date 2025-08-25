namespace WhatsappAudioTranscriber
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnTranscribeAudio = new Button();
            this.textBox1 = new TextBox();
            this.SuspendLayout();
            // 
            // btnTranscribeAudio
            // 
            this.btnTranscribeAudio.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.btnTranscribeAudio.Location = new Point(14, 16);
            this.btnTranscribeAudio.Margin = new Padding(3, 4, 3, 4);
            this.btnTranscribeAudio.Name = "btnTranscribeAudio";
            this.btnTranscribeAudio.Size = new Size(255, 68);
            this.btnTranscribeAudio.TabIndex = 0;
            this.btnTranscribeAudio.Text = "Transcribe Audio";
            this.btnTranscribeAudio.UseVisualStyleBackColor = true;
            this.btnTranscribeAudio.Click += this.Button1_Click;
            // 
            // textBox1
            // 
            this.textBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.textBox1.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.textBox1.Location = new Point(14, 104);
            this.textBox1.Margin = new Padding(3, 4, 3, 4);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Size(1075, 633);
            this.textBox1.TabIndex = 1;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1103, 755);
            Controls.Add(this.textBox1);
            Controls.Add(this.btnTranscribeAudio);
            Margin = new Padding(3, 4, 3, 4);
            Name = "MainForm";
            Text = "Whatsapp Audio Transcriber";
            KeyDown += this.MainForm_KeyDown;
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private TextBox textBox1;
        private Button btnTranscribeAudio;
    }
}
