using ObjectTracingInVideoImage.Core.Enums;

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
            comboBoxTracker = new ComboBox();
            checkBoxTestMode = new CheckBox();
            labelIoU = new Label();
            btnInitTrackerWithGroundTruth = new Button();
            labelFramesNumber = new Label();
            checkBoxVisualizeKalman = new CheckBox();
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
            pictureBoxVideo.MouseDown += _rectangleSelector.OnMouseDown;
            pictureBoxVideo.MouseMove += _rectangleSelector.OnMouseMove;
            pictureBoxVideo.Paint += _rectangleSelector.OnPaint;
            pictureBoxVideo.MouseUp += PictureBoxVideo_MouseUp;
            // 
            // numericFpsOverride
            // 
            numericFpsOverride.Location = new Point(12, 59);
            numericFpsOverride.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
            numericFpsOverride.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericFpsOverride.Name = "numericFpsOverride";
            numericFpsOverride.Size = new Size(120, 23);
            numericFpsOverride.TabIndex = 2;
            numericFpsOverride.Value = new decimal(new int[] { 30, 0, 0, 0 });
            numericFpsOverride.ValueChanged += NumericFpsOverride_ValueChanged;
            // 
            // btnPlayVideo
            // 
            btnPlayVideo.Enabled = false;
            btnPlayVideo.Location = new Point(12, 88);
            btnPlayVideo.Name = "btnPlayVideo";
            btnPlayVideo.Size = new Size(120, 40);
            btnPlayVideo.TabIndex = 3;
            btnPlayVideo.Text = "▶️";
            btnPlayVideo.UseVisualStyleBackColor = true;
            btnPlayVideo.Click += BtnPlayVideo_Click;
            // 
            // labelFps
            // 
            labelFps.AutoSize = true;
            labelFps.Location = new Point(16, 146);
            labelFps.Name = "labelFps";
            labelFps.Size = new Size(29, 15);
            labelFps.TabIndex = 4;
            labelFps.Text = "FPS:-";
            // 
            // comboBoxTracker
            // 
            comboBoxTracker.FormattingEnabled = true;
            comboBoxTracker.Location = new Point(11, 181);
            comboBoxTracker.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxTracker.DataSource = Enum.GetValues(typeof(TrackerType));
            comboBoxTracker.SelectedItem = "KCF";
            comboBoxTracker.Name = "comboBoxTracker";
            comboBoxTracker.Size = new Size(121, 23);
            comboBoxTracker.TabIndex = 5;
            comboBoxTracker.SelectedIndexChanged += ComboBoxTracker_SelectedIndexChanged;
            // 
            // checkBoxTestMode
            // 
            checkBoxTestMode.Appearance = Appearance.Button;
            checkBoxTestMode.AutoSize = true;
            checkBoxTestMode.Location = new Point(17, 218);
            checkBoxTestMode.Name = "checkBoxTestMode";
            checkBoxTestMode.Size = new Size(89, 25);
            checkBoxTestMode.TabIndex = 6;
            checkBoxTestMode.Text = "Testing mode";
            checkBoxTestMode.UseVisualStyleBackColor = true;
            checkBoxTestMode.Enabled = false;
            checkBoxTestMode.CheckedChanged += CheckBoxTestMode_CheckedChanged;
            // 
            // labelIoU
            // 
            labelIoU.AutoSize = true;
            labelIoU.Location = new Point(21, 316);
            labelIoU.Name = "labelIoU";
            labelIoU.Size = new Size(61, 15);
            labelIoU.TabIndex = 7;
            labelIoU.Text = "Mean IoU:-";
            // 
            // btnInitTrackerWithGroundTruth
            // 
            btnInitTrackerWithGroundTruth.Location = new Point(21, 256);
            btnInitTrackerWithGroundTruth.Name = "btnInitTrackerWithGroundTruth";
            btnInitTrackerWithGroundTruth.Size = new Size(75, 23);
            btnInitTrackerWithGroundTruth.TabIndex = 8;
            btnInitTrackerWithGroundTruth.Text = "Initialize tracking";
            btnInitTrackerWithGroundTruth.UseVisualStyleBackColor = true;
            btnInitTrackerWithGroundTruth.Enabled = false;
            btnInitTrackerWithGroundTruth.Click += BtnInitTrackerWithGroundTruth_Click;
            // 
            // labelFramesNumber
            // 
            labelFramesNumber.AutoSize = true;
            labelFramesNumber.Location = new Point(21, 341);
            labelFramesNumber.Name = "labelFramesNumber";
            labelFramesNumber.Size = new Size(48, 15);
            labelFramesNumber.TabIndex = 9;
            labelFramesNumber.Text = "Frames:-";
            // 
            // checkBoxVisualizeKalman
            // 
            checkBoxVisualizeKalman.AutoSize = true;
            checkBoxVisualizeKalman.Location = new Point(21, 377);
            checkBoxVisualizeKalman.Name = "checkBoxVisualizeKalman";
            checkBoxVisualizeKalman.Size = new Size(82, 19);
            checkBoxVisualizeKalman.TabIndex = 10;
            checkBoxVisualizeKalman.Text = "Visualize Kalman Filter";
            checkBoxVisualizeKalman.UseVisualStyleBackColor = true;
            checkBoxVisualizeKalman.Enabled = false;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(checkBoxVisualizeKalman);
            Controls.Add(labelFramesNumber);
            Controls.Add(btnInitTrackerWithGroundTruth);
            Controls.Add(labelIoU);
            Controls.Add(checkBoxTestMode);
            Controls.Add(comboBoxTracker);
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
        private ComboBox comboBoxTracker;
        private CheckBox checkBoxTestMode;
        private Label labelIoU;
        private Button btnInitTrackerWithGroundTruth;
        private Label labelFramesNumber;
        private CheckBox checkBoxVisualizeKalman;
    }
}
