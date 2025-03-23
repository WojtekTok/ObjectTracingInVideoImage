namespace ObjectTracingVideoImage.App
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            btnLoadVideo = new Button();
            pictureBoxVideo = new PictureBox();
            timerVideoPlayback = new System.Windows.Forms.Timer(components);
            ((System.ComponentModel.ISupportInitialize)pictureBoxVideo).BeginInit();
            SuspendLayout();
            // 
            // btnLoadVideo
            // 
            btnLoadVideo.Location = new Point(12, 12);
            btnLoadVideo.Name = "btnLoadVideo";
            btnLoadVideo.Size = new Size(104, 41);
            btnLoadVideo.TabIndex = 0;
            btnLoadVideo.Text = "Load video file";
            btnLoadVideo.UseVisualStyleBackColor = true;
            btnLoadVideo.Click += btnLoadVideoFile_Click;
            // 
            // pictureBoxVideo
            // 
            pictureBoxVideo.Location = new Point(178, 22);
            pictureBoxVideo.Name = "pictureBoxVideo";
            pictureBoxVideo.Size = new Size(602, 406);
            pictureBoxVideo.TabIndex = 1;
            pictureBoxVideo.TabStop = false;
            pictureBoxVideo.BackColor = Color.Black;
            // 
            // timerVideoPlayback
            // 
            timerVideoPlayback.Tick += timerVideoPlayback_Tick;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(pictureBoxVideo);
            Controls.Add(btnLoadVideo);
            Name = "MainForm";
            Text = "Video object tracer";
            ((System.ComponentModel.ISupportInitialize)pictureBoxVideo).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Button btnLoadVideo;
        private PictureBox pictureBoxVideo;
        private System.Windows.Forms.Timer timerVideoPlayback;
    }
}
