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
            btnReloadFile = new Button();
            btnBenchmark = new Button();
            btnViewChart = new Button();
            labelChooseFps = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBoxVideo).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericFpsOverride).BeginInit();
            SuspendLayout();
            // 
            // btnLoadVideo
            // 
            btnLoadVideo.BackColor = Color.Orange;
            btnLoadVideo.Location = new Point(12, 12);
            btnLoadVideo.Name = "btnLoadVideo";
            btnLoadVideo.Size = new Size(104, 41);
            btnLoadVideo.TabIndex = 0;
            btnLoadVideo.Text = "Load video file";
            btnLoadVideo.UseVisualStyleBackColor = false;
            btnLoadVideo.Click += BtnLoadVideoFile_Click;
            // 
            // pictureBoxVideo
            // 
            pictureBoxVideo.BackColor = Color.Black;
            pictureBoxVideo.Location = new Point(231, 22);
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
            numericFpsOverride.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
            numericFpsOverride.BackColor = Color.Orange;
            numericFpsOverride.Location = new Point(67, 59);
            numericFpsOverride.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericFpsOverride.Name = "numericFpsOverride";
            numericFpsOverride.Size = new Size(49, 23);
            numericFpsOverride.TabIndex = 2;
            numericFpsOverride.Value = new decimal(new int[] { 30, 0, 0, 0 });
            numericFpsOverride.ValueChanged += NumericFpsOverride_ValueChanged;
            // 
            // btnPlayVideo
            // 
            btnPlayVideo.BackColor = Color.Orange;
            btnPlayVideo.Enabled = false;
            btnPlayVideo.Location = new Point(12, 88);
            btnPlayVideo.Name = "btnPlayVideo";
            btnPlayVideo.Size = new Size(75, 40);
            btnPlayVideo.TabIndex = 3;
            btnPlayVideo.Text = "▶️";
            btnPlayVideo.UseVisualStyleBackColor = false;
            btnPlayVideo.Click += BtnPlayVideo_Click;
            // 
            // labelFps
            // 
            labelFps.AutoSize = true;
            labelFps.Location = new Point(16, 146);
            labelFps.Name = "labelFps";
            labelFps.Size = new Size(71, 15);
            labelFps.TabIndex = 4;
            labelFps.Text = "Actual FPS:-";
            // 
            // comboBoxTracker
            // 
            comboBoxTracker.BackColor = SystemColors.Window;
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
            checkBoxTestMode.BackColor = Color.Orange;
            checkBoxTestMode.Enabled = false;
            checkBoxTestMode.Location = new Point(11, 210);
            checkBoxTestMode.Name = "checkBoxTestMode";
            checkBoxTestMode.Size = new Size(120, 25);
            checkBoxTestMode.TabIndex = 6;
            checkBoxTestMode.Text = "Show Ground Truth\r\n";
            checkBoxTestMode.UseVisualStyleBackColor = false;
            checkBoxTestMode.CheckedChanged += CheckBoxTestMode_CheckedChanged;
            // 
            // labelIoU
            // 
            labelIoU.AutoSize = true;
            labelIoU.Location = new Point(20, 277);
            labelIoU.Name = "labelIoU";
            labelIoU.Size = new Size(66, 15);
            labelIoU.TabIndex = 7;
            labelIoU.Text = "Mean IoU:-";
            // 
            // btnInitTrackerWithGroundTruth
            // 
            btnInitTrackerWithGroundTruth.BackColor = Color.Orange;
            btnInitTrackerWithGroundTruth.Enabled = false;
            btnInitTrackerWithGroundTruth.Location = new Point(11, 241);
            btnInitTrackerWithGroundTruth.Name = "btnInitTrackerWithGroundTruth";
            btnInitTrackerWithGroundTruth.Size = new Size(75, 23);
            btnInitTrackerWithGroundTruth.TabIndex = 8;
            btnInitTrackerWithGroundTruth.Text = "Initialize tracking";
            btnInitTrackerWithGroundTruth.UseVisualStyleBackColor = false;
            btnInitTrackerWithGroundTruth.Click += BtnInitTrackerWithGroundTruth_Click;
            // 
            // labelFramesNumber
            // 
            labelFramesNumber.AutoSize = true;
            labelFramesNumber.Location = new Point(21, 302);
            labelFramesNumber.Name = "labelFramesNumber";
            labelFramesNumber.Size = new Size(53, 15);
            labelFramesNumber.TabIndex = 9;
            labelFramesNumber.Text = "Frames:-";
            // 
            // checkBoxVisualizeKalman
            // 
            checkBoxVisualizeKalman.AutoSize = true;
            checkBoxVisualizeKalman.BackColor = SystemColors.ControlDarkDark;
            checkBoxVisualizeKalman.Enabled = false;
            checkBoxVisualizeKalman.Location = new Point(21, 344);
            checkBoxVisualizeKalman.Name = "checkBoxVisualizeKalman";
            checkBoxVisualizeKalman.Size = new Size(143, 19);
            checkBoxVisualizeKalman.TabIndex = 10;
            checkBoxVisualizeKalman.Text = "Visualize Kalman Filter";
            checkBoxVisualizeKalman.UseVisualStyleBackColor = false;
            // 
            // btnReloadFile
            // 
            btnReloadFile.BackColor = Color.Orange;
            btnReloadFile.Enabled = false;
            btnReloadFile.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 238);
            btnReloadFile.Location = new Point(93, 100);
            btnReloadFile.Name = "btnReloadFile";
            btnReloadFile.Size = new Size(31, 28);
            btnReloadFile.TabIndex = 11;
            btnReloadFile.Text = "⟲";
            btnReloadFile.UseVisualStyleBackColor = false;
            btnReloadFile.Click += BtnReloadFile_Click;
            // 
            // btnBenchmark
            // 
            btnBenchmark.BackColor = Color.Orange;
            btnBenchmark.Enabled = false;
            btnBenchmark.ForeColor = SystemColors.ControlText;
            btnBenchmark.Location = new Point(17, 405);
            btnBenchmark.Name = "btnBenchmark";
            btnBenchmark.Size = new Size(141, 32);
            btnBenchmark.TabIndex = 12;
            btnBenchmark.Text = "Benchmark trackers";
            btnBenchmark.UseVisualStyleBackColor = false;
            btnBenchmark.Click += BtnBenchmark_Click;
            // 
            // btnViewChart
            // 
            btnViewChart.BackColor = Color.Orange;
            btnViewChart.Location = new Point(26, 375);
            btnViewChart.Name = "btnViewChart";
            btnViewChart.Size = new Size(112, 25);
            btnViewChart.TabIndex = 13;
            btnViewChart.Text = "View Chart";
            btnViewChart.UseVisualStyleBackColor = false;
            btnViewChart.Click += BtnViewChart_Click;
            // 
            // labelChooseFps
            // 
            labelChooseFps.AutoSize = true;
            labelChooseFps.Location = new Point(13, 61);
            labelChooseFps.Name = "labelChooseFps";
            labelChooseFps.Size = new Size(54, 15);
            labelChooseFps.TabIndex = 14;
            labelChooseFps.Text = "Max FPS:";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlDarkDark;
            ClientSize = new Size(845, 450);
            Controls.Add(labelChooseFps);
            Controls.Add(btnViewChart);
            Controls.Add(btnBenchmark);
            Controls.Add(btnReloadFile);
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
        private Button btnReloadFile;
        private Button btnBenchmark;
        private Button btnViewChart;
        private Label labelChooseFps;
    }
}
