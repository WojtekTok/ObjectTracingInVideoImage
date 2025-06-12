using ObjectTracingInVideoImage.App.Controls;
using ObjectTracingInVideoImage.App.Extensions;
using ObjectTracingInVideoImage.Core.Trackers;
using Emgu.CV;
using ObjectTracingInVideoImage.Core.Enums;
using ObjectTracingInVideoImage.Core.Factories;
using ObjectTracingInVideoImage.Core.PlayerManager;
using ObjectTracingInVideoImage.Core.Testing;

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

        private GroundTruthData _groundTruthData;
        private TrackingEvaluator _evaluator;
        private bool _showingCorrectBox = false;
        private int _testFrameCounter = 0;

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
                var filePath = openFileDialog.FileName;
                var extension = Path.GetExtension(filePath).ToLower();
                if (_playerManager != null)
                {
                    _playerManager.Stop();
                    pictureBoxVideo.Image?.Dispose();
                    pictureBoxVideo.Image = null;
                    btnPlayVideo.Text = "▶️ Start";
                }
                if (extension == ".jpg")
                {
                    _playerManager = new ImageSequenceManager();
                }
                else if (extension == ".mp4" || extension == ".avi" || extension == ".webm")
                {
                    _playerManager = new VideoManager();
                }
                else
                {
                    MessageBox.Show("Unsupported file format. Please select a valid video file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (_playerManager.LoadVideo(filePath))
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
                    _imageDirectory = Path.GetDirectoryName(Path.GetDirectoryName(filePath)) ?? string.Empty;
                    _evaluator = new TrackingEvaluator(_groundTruthData);
                    _testFrameCounter = 0;
                }
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
                btnPlayVideo.Text = "⏸️ Pause";

                await _playerManager.StartVideoAsync(ProcessFrameAsync);
                _tracker?.Dispose();

            }
            else
            {
                if (_playerManager.IsPaused)
                {
                    _playerManager.Resume();
                    btnPlayVideo.Text = "⏸️ Pause";
                }
                else
                {
                    _playerManager.Pause();
                    btnPlayVideo.Text = "▶️ Resume";
                }
            }
        }

        private async Task ProcessFrameAsync(Mat mat)
        {
            if (!IsHandleCreated) return;

            Rectangle? rect = null;
            if (_tracker != null)
            {
                rect = await Task.Run(() => _tracker.Track(mat));
            }

            Rectangle? groundTruthRect = null;
            bool skipMetrics = false;

            if (_showingCorrectBox && _groundTruthData != null)
            {
                groundTruthRect = _groundTruthData.GetBox(_testFrameCounter);
                skipMetrics = _groundTruthData.IsOccluded(_testFrameCounter) || _groundTruthData.IsOutOfView(_testFrameCounter);
                if (!skipMetrics && groundTruthRect.HasValue)
                {
                    _evaluator.EvaluateFrame(_testFrameCounter, rect);
                }
            }

            await this.InvokeAsync(() =>
            {
                pictureBoxVideo.Image?.Dispose();
                _lastFrame = mat.Clone();

                Bitmap bitmap = mat.ToBitmap();

                if (groundTruthRect.HasValue)
                {
                    using var g = Graphics.FromImage(bitmap);
                    using var pen = new Pen(Color.LimeGreen, 2);
                    g.DrawRectangle(pen, groundTruthRect.Value);
                }

                if (rect.HasValue)
                {
                    using var g = Graphics.FromImage(bitmap);
                    using var pen = new Pen(Color.Red, 2);
                    g.DrawRectangle(pen, rect.Value);
                }
                if (!_rectangleSelector.IsSelecting)
                {
                    _rectangleSelector.Clear();
                }

                pictureBoxVideo.Image = bitmap;
                _frameCounter++;
                _testFrameCounter++;

                DisplayCurrentFps();
                if (_showingCorrectBox && _evaluator != null)
                {
                    labelIoU.Text = $"Mean IoU: {_evaluator.MeanIoU:F3}";
                    labelFramesNumber.Text = $"Frames: {_evaluator.TestedFrames}";
                } // Refactor this method as it is too long and has too many responsibilities
                pictureBoxVideo.Invalidate();

                mat.Dispose();
            });
        }


        private void DisplayCurrentFps()
        {
            var now = DateTime.Now;
            if ((now - _lastFpsCheck).TotalSeconds >= 1)
            {
                labelFps.Text = $"FPS: {_frameCounter}";
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

                GetTrackerAndInitialize(roi);
            }
        }

        private void CheckBoxTestMode_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxTestMode.Checked)
            {
                SetGroundTruthData();
            }
            else
            {
                _showingCorrectBox = false;
            }
        }

        private void BtnInitTrackerWithGroundTruth_Click(object sender, EventArgs e)
        {
            if (_lastFrame == null || _groundTruthData == null) return;
            int currentFrameIdx = _testFrameCounter;
            var gtRect = _groundTruthData.GetBox(currentFrameIdx);
            if (!gtRect.HasValue) return;

            _tracker?.Dispose();
            _tracker = Enum.TryParse<TrackerType>(comboBoxTracker.SelectedItem?.ToString(), out var trackerType)
                ? TrackerFactory.Create(trackerType)
                : throw new ArgumentOutOfRangeException(nameof(trackerType), trackerType, null);

            _tracker.Initialize(_lastFrame, gtRect.Value);

            Bitmap bitmap = _lastFrame.ToBitmap();
            using (var g = Graphics.FromImage(bitmap))
            using (var pen = new Pen(Color.Red, 2))
                g.DrawRectangle(pen, gtRect.Value);

            pictureBoxVideo.Image?.Dispose();
            pictureBoxVideo.Image = bitmap;
        }

        private void SetGroundTruthData()
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show("Cannot display test bounding box:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                checkBoxTestMode.Checked = false;
                _showingCorrectBox = false;
            }
        }

        private void GetTrackerAndInitialize(Rectangle roi)
        {
            _tracker?.Dispose();
            _tracker = Enum.TryParse<TrackerType>(comboBoxTracker.SelectedItem?.ToString(), out var trackerType)
                ? TrackerFactory.Create(trackerType)
                : throw new ArgumentOutOfRangeException(nameof(trackerType), trackerType, null);

            _tracker.Initialize(_lastFrame, roi);
        }
    }
}
