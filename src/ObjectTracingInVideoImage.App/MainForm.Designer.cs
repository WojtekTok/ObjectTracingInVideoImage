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
            btnLoadVideo = new Button();
            pictureBoxVideo = new PictureBox();
            numericFpsOverride = new NumericUpDown();
            btnPlayVideo = new Button();
            labelFps = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBoxVideo).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericFpsOverride).BeginInit();
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
            btnLoadVideo.Click += BtnLoadVideoFile_Click;
            // 
            // pictureBoxVideo
            // 
            pictureBoxVideo.BackColor = Color.Black;
            pictureBoxVideo.Location = new Point(178, 22);
            pictureBoxVideo.Name = "pictureBoxVideo";
            pictureBoxVideo.Size = new Size(602, 406);
            pictureBoxVideo.TabIndex = 1;
            pictureBoxVideo.TabStop = false;
            // 
            // numericFpsOverride
            // 
            numericFpsOverride.Location = new Point(12, 59);
            numericFpsOverride.Maximum = new decimal(new int[] { 240, 0, 0, 0 });
            numericFpsOverride.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericFpsOverride.Name = "numericFpsOverride";
            numericFpsOverride.Size = new Size(120, 23);
            numericFpsOverride.TabIndex = 2;
            numericFpsOverride.Value = new decimal(new int[] { 30, 0, 0, 0 });
            numericFpsOverride.ValueChanged += numericFpsOverride_ValueChanged;
            // 
            // btnPlayVideo
            // 
            btnPlayVideo.Location = new Point(12, 88);
            btnPlayVideo.Name = "btnPlayVideo";
            btnPlayVideo.Size = new Size(120, 40);
            btnPlayVideo.TabIndex = 3;
            btnPlayVideo.Text = "▶️ Start";
            btnPlayVideo.UseVisualStyleBackColor = true;
            btnPlayVideo.Click += BtnPlayVideo_Click;
            // 
            // labelFps
            // 
            labelFps.AutoSize = true;
            labelFps.Location = new Point(16, 146);
            labelFps.Name = "labelFps";
            labelFps.Size = new Size(38, 15);
            labelFps.TabIndex = 4;
            labelFps.Text = "FPS:";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(labelFps);
            Controls.Add(btnPlayVideo);
            Controls.Add(numericFpsOverride);
            Controls.Add(pictureBoxVideo);
            Controls.Add(btnLoadVideo);
            Name = "MainForm";
            Text = "Video object tracer";
            ((System.ComponentModel.ISupportInitialize)pictureBoxVideo).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericFpsOverride).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnLoadVideo;
        private PictureBox pictureBoxVideo;
        private NumericUpDown numericFpsOverride;
        private Button btnPlayVideo;
        private Label labelFps;
    }
}
