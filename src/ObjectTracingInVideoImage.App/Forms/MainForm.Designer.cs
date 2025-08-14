using System.Drawing.Drawing2D;
using ObjectTracingInVideoImage.Core.Enums;
using ObjectTracingInVideoImage.App.UIHelpers;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
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
            labelChooseTracker = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBoxVideo).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericFpsOverride).BeginInit();
            SuspendLayout();
            // 
            // btnLoadVideo
            // 
            btnLoadVideo.BackColor = SystemColors.Control;
            btnLoadVideo.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 238);
            btnLoadVideo.Location = new Point(221, 446);
            btnLoadVideo.Name = "btnLoadVideo";
            btnLoadVideo.Size = new Size(116, 60);
            btnLoadVideo.TabIndex = 0;
            btnLoadVideo.Text = "Load video file";
            btnLoadVideo.UseVisualStyleBackColor = false;
            btnLoadVideo.Click += BtnLoadVideoFile_Click;
            // 
            // pictureBoxVideo
            // 
            pictureBoxVideo.BackColor = Color.Black;
            pictureBoxVideo.Location = new Point(221, 21);
            pictureBoxVideo.Name = "pictureBoxVideo";
            pictureBoxVideo.Size = new Size(602, 406);
            pictureBoxVideo.TabIndex = 1;
            pictureBoxVideo.TabStop = false;
            pictureBoxVideo.MouseDown += _rectangleSelector.OnMouseDown;
            pictureBoxVideo.MouseMove += _rectangleSelector.OnMouseMove;
            pictureBoxVideo.Paint += _rectangleSelector.OnPaint;
            pictureBoxVideo.MouseUp += PictureBoxVideo_MouseUp;
            ControlStyler.RoundControlCorners(pictureBoxVideo, 20);
            // 
            // numericFpsOverride
            // 
            numericFpsOverride.BackColor = SystemColors.Control;
            numericFpsOverride.Font = new Font("Segoe UI", 12F);
            numericFpsOverride.Location = new Point(767, 448);
            numericFpsOverride.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            numericFpsOverride.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericFpsOverride.Name = "numericFpsOverride";
            numericFpsOverride.Size = new Size(58, 29);
            numericFpsOverride.TabIndex = 2;
            numericFpsOverride.Value = new decimal(new int[] { 30, 0, 0, 0 });
            numericFpsOverride.ValueChanged += NumericFpsOverride_ValueChanged;
            ControlStyler.RoundControlCorners(numericFpsOverride, 15);
            // 
            // btnPlayVideo
            // 
            btnPlayVideo.BackColor = SystemColors.Control;
            btnPlayVideo.Enabled = false;
            btnPlayVideo.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 238);
            btnPlayVideo.Location = new Point(486, 448);
            btnPlayVideo.Name = "btnPlayVideo";
            btnPlayVideo.Size = new Size(62, 60);
            btnPlayVideo.TabIndex = 3;
            btnPlayVideo.Text = "▶️";
            btnPlayVideo.UseVisualStyleBackColor = false;
            btnPlayVideo.Click += BtnPlayVideo_Click;
            // 
            // labelFps
            // 
            labelFps.AutoSize = true;
            labelFps.BackColor = Color.Transparent;
            labelFps.Font = new Font("Segoe UI", 12F);
            labelFps.Location = new Point(683, 487);
            labelFps.Name = "labelFps";
            labelFps.Size = new Size(92, 21);
            labelFps.TabIndex = 4;
            labelFps.Text = "Actual FPS:-";
            // 
            // comboBoxTracker
            // 
            comboBoxTracker.BackColor = SystemColors.Window;
            comboBoxTracker.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxTracker.DataSource = Enum.GetValues(typeof(TrackerType));
            comboBoxTracker.SelectedItem = "KCF";
            comboBoxTracker.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 238);
            comboBoxTracker.FormattingEnabled = true;
            comboBoxTracker.Location = new Point(26, 68);
            comboBoxTracker.Name = "comboBoxTracker";
            comboBoxTracker.Size = new Size(148, 29);
            comboBoxTracker.TabIndex = 5;
            comboBoxTracker.SelectedIndexChanged += ComboBoxTracker_SelectedIndexChanged;
            ControlStyler.RoundControlCorners(comboBoxTracker, 15);
            // 
            // checkBoxTestMode
            // 
            checkBoxTestMode.AutoSize = true;
            checkBoxTestMode.BackColor = Color.Transparent;
            checkBoxTestMode.Enabled = false;
            checkBoxTestMode.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 238);
            checkBoxTestMode.Location = new Point(26, 115);
            checkBoxTestMode.Name = "checkBoxTestMode";
            checkBoxTestMode.Size = new Size(165, 25);
            checkBoxTestMode.TabIndex = 6;
            checkBoxTestMode.Text = "Show Ground Truth\r\n";
            checkBoxTestMode.UseVisualStyleBackColor = false;
            checkBoxTestMode.CheckedChanged += CheckBoxTestMode_CheckedChanged;
            // 
            // labelIoU
            // 
            labelIoU.AutoSize = true;
            labelIoU.BackColor = Color.Transparent;
            labelIoU.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 238);
            labelIoU.Location = new Point(26, 267);
            labelIoU.Name = "labelIoU";
            labelIoU.Size = new Size(86, 21);
            labelIoU.TabIndex = 7;
            labelIoU.Text = "Mean IoU:-";
            // 
            // btnInitTrackerWithGroundTruth
            // 
            btnInitTrackerWithGroundTruth.BackColor = SystemColors.Control;
            btnInitTrackerWithGroundTruth.Enabled = false;
            btnInitTrackerWithGroundTruth.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 238);
            btnInitTrackerWithGroundTruth.Location = new Point(26, 188);
            btnInitTrackerWithGroundTruth.Name = "btnInitTrackerWithGroundTruth";
            btnInitTrackerWithGroundTruth.Size = new Size(148, 50);
            btnInitTrackerWithGroundTruth.TabIndex = 8;
            btnInitTrackerWithGroundTruth.Text = "Initialize Tracker";
            btnInitTrackerWithGroundTruth.UseVisualStyleBackColor = false;
            btnInitTrackerWithGroundTruth.Click += BtnInitTrackerWithGroundTruth_Click;
            // 
            // labelFramesNumber
            // 
            labelFramesNumber.AutoSize = true;
            labelFramesNumber.BackColor = Color.Transparent;
            labelFramesNumber.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 238);
            labelFramesNumber.Location = new Point(26, 300);
            labelFramesNumber.Name = "labelFramesNumber";
            labelFramesNumber.Size = new Size(70, 21);
            labelFramesNumber.TabIndex = 9;
            labelFramesNumber.Text = "Frames:-";
            // 
            // checkBoxVisualizeKalman
            // 
            checkBoxVisualizeKalman.AutoSize = true;
            checkBoxVisualizeKalman.BackColor = Color.Transparent;
            checkBoxVisualizeKalman.Enabled = false;
            checkBoxVisualizeKalman.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 238);
            checkBoxVisualizeKalman.Location = new Point(26, 146);
            checkBoxVisualizeKalman.Name = "checkBoxVisualizeKalman";
            checkBoxVisualizeKalman.Size = new Size(171, 25);
            checkBoxVisualizeKalman.TabIndex = 10;
            checkBoxVisualizeKalman.Text = "Show Kalman Tracks";
            checkBoxVisualizeKalman.UseVisualStyleBackColor = false;
            // 
            // btnReloadFile
            // 
            btnReloadFile.BackColor = SystemColors.Control;
            btnReloadFile.Enabled = false;
            btnReloadFile.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 238);
            btnReloadFile.Location = new Point(572, 459);
            btnReloadFile.Name = "btnReloadFile";
            btnReloadFile.Size = new Size(42, 37);
            btnReloadFile.TabIndex = 11;
            btnReloadFile.Text = "⟲";
            btnReloadFile.UseVisualStyleBackColor = false;
            btnReloadFile.Click += BtnReloadFile_Click;
            // 
            // btnBenchmark
            // 
            btnBenchmark.BackColor = SystemColors.Control;
            btnBenchmark.Enabled = false;
            btnBenchmark.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 238);
            btnBenchmark.ForeColor = SystemColors.ControlText;
            btnBenchmark.Location = new Point(26, 373);
            btnBenchmark.Name = "btnBenchmark";
            btnBenchmark.Size = new Size(157, 64);
            btnBenchmark.TabIndex = 12;
            btnBenchmark.Text = "Benchmark trackers";
            btnBenchmark.UseVisualStyleBackColor = false;
            btnBenchmark.Click += BtnBenchmark_Click;
            // 
            // btnViewChart
            // 
            btnViewChart.BackColor = SystemColors.Control;
            btnViewChart.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 238);
            btnViewChart.Location = new Point(26, 452);
            btnViewChart.Name = "btnViewChart";
            btnViewChart.Size = new Size(116, 52);
            btnViewChart.TabIndex = 13;
            btnViewChart.Text = "View Chart";
            btnViewChart.UseVisualStyleBackColor = false;
            btnViewChart.Click += BtnViewChart_Click;
            // 
            // labelChooseFps
            // 
            labelChooseFps.AutoSize = true;
            labelChooseFps.BackColor = Color.Transparent;
            labelChooseFps.Font = new Font("Segoe UI", 12F);
            labelChooseFps.Location = new Point(683, 450);
            labelChooseFps.Name = "labelChooseFps";
            labelChooseFps.Size = new Size(72, 21);
            labelChooseFps.TabIndex = 14;
            labelChooseFps.Text = "Max FPS:";
            // 
            // labelChooseTracker
            // 
            labelChooseTracker.AutoSize = true;
            labelChooseTracker.BackColor = Color.Transparent;
            labelChooseTracker.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 238);
            labelChooseTracker.Location = new Point(26, 44);
            labelChooseTracker.Name = "labelChooseTracker";
            labelChooseTracker.Size = new Size(116, 21);
            labelChooseTracker.TabIndex = 15;
            labelChooseTracker.Text = "Chosen Tracker";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(845, 530);
            Controls.Add(labelChooseTracker);
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
            Text = "Video Object Tracker";
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
        private Label labelChooseTracker;
    }
}
