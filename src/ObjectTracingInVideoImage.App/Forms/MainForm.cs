using Emgu.CV;
using ObjectTracingInVideoImage.App.Controls;
using ObjectTracingInVideoImage.App.Extensions;
using ObjectTracingInVideoImage.App.Forms;
using ObjectTracingInVideoImage.App.Visualizers;
using ObjectTracingInVideoImage.App.Visuralizers;
using ObjectTracingInVideoImage.Core.Enums;
using ObjectTracingInVideoImage.Core.Factories;
using ObjectTracingInVideoImage.Core.KalmanFilter;
using ObjectTracingInVideoImage.Core.PlayerManager;
using ObjectTracingInVideoImage.Core.Testing;
using ObjectTracingInVideoImage.Core.Testing.Logging;
using ObjectTracingInVideoImage.Core.Trackers;
using ObjectTracingInVideoImage.Core.Trackers.HybridTracker;

namespace ObjectTracingVideoImage.App
{
    public partial class MainForm : Form
    {
        private IPlayerManager _playerManager;
        private int _frameCounter = 0;
        private DateTime _lastFpsCheck = DateTime.Now;
        private readonly RectangleSelector _rectangleSelector = new();
        private IObjectTracker _tracker;
        private Mat _lastFrame;
        private string _imageDirectory;
        private string _filePath;
        private TrackingLogger _trackingLogger;
        private GroundTruthData _groundTruthData;
        private TrackingEvaluator _evaluator;
        private bool _showingCorrectBox = false;
        private int _testFrameCounter = 0;
        private bool _isBenchmarkRunning = false;

        public MainForm()
        {
            InitializeComponent();
            pictureBoxVideo.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void BtnLoadVideoFile_Click(object sender, EventArgs e)
        {
            using OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Pliki wideo|*.mp4;*.avi;*.webm;*.jpg", // This string works for Polish Windows system
                Title = "Choose video file"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                _filePath = openFileDialog.FileName;
                var extension = Path.GetExtension(_filePath).ToLower();
                if (_playerManager != null)
                {
                    _playerManager.Stop();
                    pictureBoxVideo.Image?.Dispose();
                    pictureBoxVideo.Image = null;
                    btnPlayVideo.Text = "▶️";
                }
                if (extension == ".jpg")
                {
                    _playerManager = new ImageSequenceManager();
                    checkBoxTestMode.Enabled = true;
                    btnBenchmark.Enabled = true;
                }
                else if (extension == ".mp4" || extension == ".avi" || extension == ".webm")
                {
                    _playerManager = new VideoManager();
                    checkBoxTestMode.Enabled = false;
                }
                else
                {
                    MessageBox.Show("Unsupported file format. Please select a valid video file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                LoadCurrentFile();
            }
        }

        private void NumericFpsOverride_ValueChanged(object sender, EventArgs e)
        {
            _playerManager.Fps = (double)numericFpsOverride.Value;
        }

        private async void BtnPlayVideo_Click(object sender, EventArgs e)
        {
            if (!_playerManager.IsPlaying)
            {
                btnPlayVideo.Text = "⏸️";

                await _playerManager.StartVideoAsync(ProcessFrameAsync);

                _tracker?.Dispose();

            }
            else
            {
                if (_playerManager.IsPaused)
                {
                    _playerManager.Resume();
                    btnPlayVideo.Text = "⏸️";
                }
                else
                {
                    _playerManager.Pause();
                    btnPlayVideo.Text = "▶️";
                }
            }
        }

        private async Task ProcessFrameAsync(Mat mat)
        {
            if (!IsHandleCreated) return;

            Rectangle? actualRectangle = null;
            if (_tracker != null)
            {
                actualRectangle = await Task.Run(() => _tracker.Track(mat));
            }

            Rectangle? groundTruthRectangle = null;
            bool skipMetrics = false;

            if (_showingCorrectBox && _groundTruthData != null)
            {
                double? iou = null;
                groundTruthRectangle = _groundTruthData.GetBox(_testFrameCounter);
                skipMetrics = _groundTruthData.IsOccluded(_testFrameCounter) || _groundTruthData.IsOutOfView(_testFrameCounter);
                iou = _evaluator.EvaluateFrame(_testFrameCounter, actualRectangle);

                if(iou.HasValue && _trackingLogger is not null && _isBenchmarkRunning)
                {
                    LogData(actualRectangle, iou);
                }
            }

            if (!_isBenchmarkRunning || _testFrameCounter % 10 == 0)
            {
                await this.InvokeAsync(() =>
                {
                    DisplayProcessedFrame(mat, actualRectangle, groundTruthRectangle);
                });
            }
            RealTimeDataDisplayUpdate();
            _testFrameCounter++;
        }

        private void DisplayProcessedFrame(Mat mat, Rectangle? actualRectangle, Rectangle? groundTruthRectangle)
        {
            pictureBoxVideo.Image?.Dispose();
            _lastFrame = mat.Clone();

            if (_tracker is IKalmanTracker kalmanTracker && checkBoxVisualizeKalman.Checked)
            {
                KalmanVisualizer.DrawPaths(mat, kalmanTracker.GetKalmanData());
            }

            Bitmap bitmap = mat.ToBitmap();
            BoundingBoxVisualizer.DrawBoxes(bitmap, actualRectangle, groundTruthRectangle);

            if (!_rectangleSelector.IsSelecting)
            {
                _rectangleSelector.Clear();
            }

            pictureBoxVideo.Image = bitmap;
            _frameCounter++;

            mat.Dispose();
        }

        private void RealTimeDataDisplayUpdate()
        {
            DisplayCurrentFps();
            if (_showingCorrectBox && _evaluator != null)
            {
                labelIoU.Text = $"Mean IoU: {_evaluator.MeanIoU:F3}";
                labelFramesNumber.Text = $"Frames: {_evaluator.TestedFrames}";
            }
            pictureBoxVideo.Invalidate();
        }

        private void DisplayCurrentFps()
        {
            var now = DateTime.Now;
            if ((now - _lastFpsCheck).TotalSeconds >= 1)
            {
                labelFps.Text = $"Actual FPS: {_frameCounter}";
                _frameCounter = 0;
                _lastFpsCheck = now;
            }
        }

        private void PictureBoxVideo_MouseUp(object sender, MouseEventArgs e)
        {
            _rectangleSelector.OnMouseUp(sender, e);

            if (!_rectangleSelector.SelectionRectangle.IsEmpty && _lastFrame != null)
            {
                var roiControl = _rectangleSelector.SelectionRectangle;
                var roi = roiControl.ScaleRectangleToImage(pictureBoxVideo, _lastFrame.Size);

                CreateTracker();

                _tracker.Initialize(_lastFrame, roi);
            }
        }

        private void CheckBoxTestMode_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxTestMode.Checked && _imageDirectory != null)
            {
                try
                {
                    SetGroundTruthData();
                    btnInitTrackerWithGroundTruth.Enabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Cannot display test bounding box:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    checkBoxTestMode.Checked = false;
                    _showingCorrectBox = false;
                }
            }
            else
            {
                _showingCorrectBox = false;
                checkBoxTestMode.Checked = false;
                btnInitTrackerWithGroundTruth.Enabled = false;
            }
        }

        private void BtnInitTrackerWithGroundTruth_Click(object sender, EventArgs e)
        {
            if (_lastFrame == null || _groundTruthData == null) return;
            int currentFrameIdx = _testFrameCounter;
            var gtRect = _groundTruthData.GetBox(currentFrameIdx);
            if (!gtRect.HasValue) return;

            _tracker?.Dispose();
            CreateTracker();

            _tracker!.Initialize(_lastFrame, gtRect.Value);

            Bitmap bitmap = _lastFrame.ToBitmap();
            using (var g = Graphics.FromImage(bitmap))
            using (var pen = new Pen(Color.Red, 2))
                g.DrawRectangle(pen, gtRect.Value);

            pictureBoxVideo.Image?.Dispose();
            pictureBoxVideo.Image = bitmap;
        }

        private void ComboBoxTracker_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnableVisualizeKalmanCheckBox();
        }

        private void EnableVisualizeKalmanCheckBox()
        {
            if (comboBoxTracker.SelectedItem is TrackerType selectedType)
            {
                var kalmanTrackers = new List<TrackerType>
                {
                    TrackerType.Test,
                    TrackerType.Hybrid_KCF,
                    TrackerType.Hybrid_CSRT,
                    TrackerType.Hybrid_MIL,
                    TrackerType.Hybrid
                };
                checkBoxVisualizeKalman.Enabled = kalmanTrackers.Contains(selectedType);
            }
            else
            {
                checkBoxVisualizeKalman.Enabled = false;
            }
        }

        private void BtnReloadFile_Click(object sender, EventArgs e)
        {
            LoadCurrentFile();
        }

        private async void BtnBenchmark_Click(object sender, EventArgs e)
        {
            if (_isBenchmarkRunning)
            {
                Application.Restart();
            }

            _isBenchmarkRunning = true;
            btnBenchmark.Text = "Cancel Benchmark";

            if (!(_playerManager is ImageSequenceManager imageManager))
            {
                MessageBox.Show("Benchmarking is only available for image sequences.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            SetGroundTruthData();
            _trackingLogger = new TrackingLogger();

            var trackerTypes = new[]
            {
                TrackerType.Siammask,
                TrackerType.Hybrid,
                TrackerType.KCF,
                TrackerType.CSRT,
                TrackerType.MIL
            };

            numericFpsOverride.Value = new decimal([1000, 0, 0, 0]);

            foreach (var trackerType in trackerTypes)
            {
                LoadCurrentFile();
                ToggleUi(false);
                comboBoxTracker.SelectedItem = trackerType;
                BtnInitTrackerWithGroundTruth_Click(sender, e);
                await _playerManager.StartVideoAsync(ProcessFrameAsync);
                _tracker?.Dispose();
                _trackingLogger.SaveToCsv(_imageDirectory, comboBoxTracker.SelectedItem.ToString()!);
            }

            ToggleUi(true);

            MessageBox.Show($"Benchmark finished.", "Benchmark", MessageBoxButtons.OK, MessageBoxIcon.Information);
            _isBenchmarkRunning = false;
            btnBenchmark.Text = "Start Benchmark";
        }

        private void ToggleUi(bool enabled)
        {
            btnLoadVideo.Enabled = enabled;
            btnPlayVideo.Enabled = enabled;
            btnReloadFile.Enabled = enabled;
            numericFpsOverride.Enabled = enabled;
            comboBoxTracker.Enabled = enabled;
            checkBoxTestMode.Enabled = enabled;
            btnInitTrackerWithGroundTruth.Enabled = enabled;
            if (enabled)
            {
                EnableVisualizeKalmanCheckBox();
            }
            else
            {
                checkBoxVisualizeKalman.Enabled = false;
            }
        }


        private void BtnViewChart_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "CSV files (*.csv)|*.csv";
                ofd.Title = "Wybierz plik wyników śledzenia";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string selectedPath = ofd.FileName;
                        var chartForm = new BenchmarkForm(selectedPath);
                        chartForm.Show();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Cannot display chart", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void CreateTracker()
        {
            _tracker = Enum.TryParse<TrackerType>(comboBoxTracker.SelectedItem?.ToString(), out var trackerType)
                ? TrackerFactory.Create(trackerType)
                : throw new ArgumentOutOfRangeException(nameof(trackerType), trackerType, null);
        }

        private void LoadCurrentFile()
        {
            if (_playerManager.LoadVideo(_filePath))
            {
                numericFpsOverride.Value = (decimal)_playerManager.Fps;
                btnPlayVideo.Enabled = true;
                _tracker?.Dispose();
                var firstFrame = _playerManager.GetFirstFrame();
                if (firstFrame != null)
                {
                    _lastFrame = firstFrame.Clone();
                    pictureBoxVideo.Image?.Dispose();
                    pictureBoxVideo.Image = _lastFrame.ToBitmap();
                }
                _imageDirectory = Path.GetDirectoryName(Path.GetDirectoryName(_filePath)) ?? string.Empty;
                if(checkBoxTestMode.Checked)
                    SetGroundTruthData();
                _testFrameCounter = 0;
                _evaluator.Reset();
                btnReloadFile.Enabled = true;
            }
        }

        private void SetGroundTruthData()
        {
            var baseDir = _imageDirectory;
            var gtPath = Path.Combine(baseDir, "groundtruth.txt");
            var occPath = Path.Combine(baseDir, "full_occlusion.txt");
            var oovPath = Path.Combine(baseDir, "out_of_view.txt");

            if (!File.Exists(gtPath) || !File.Exists(occPath) || !File.Exists(oovPath))
                throw new FileNotFoundException("Culd not find necessary ground truth files in:\n" + baseDir);

            _groundTruthData = new GroundTruthData(gtPath, occPath, oovPath);
            _evaluator = new TrackingEvaluator(_groundTruthData);

            _showingCorrectBox = true;
        }

        private void LogData(Rectangle? rect, double? iou)
        {
            var wasDetected = rect.HasValue && !rect.Value.IsEmpty;
            TrackerType? usedTracker = null;
            if (_tracker is HybridObjectTrackerv2 tracker)
            {
                usedTracker = tracker.LastUsedTracker;
            }
            else
            {
                usedTracker = (TrackerType?)comboBoxTracker.SelectedItem;
            }
            _trackingLogger.Log(_testFrameCounter, wasDetected, iou, usedTracker);
        }
    }
}
