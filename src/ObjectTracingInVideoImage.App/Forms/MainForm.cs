using ObjectTracingInVideoImage.App.Controls;
using ObjectTracingInVideoImage.App.Extensions;
using ObjectTracingInVideoImage.Core;
using ObjectTracingInVideoImage.Core.Trackers;
using Emgu.CV;
using ObjectTracingInVideoImage.Core.Enums;
using ObjectTracingInVideoImage.Core.Factories;

namespace ObjectTracingVideoImage.App
{
    public partial class MainForm : Form
    {
        private readonly VideoManager _videoManager = new();
        private int _frameCounter = 0;
        private DateTime _lastFpsCheck = DateTime.Now;
        private readonly RectangleSelector _rectangleSelector = new();
        private IObjectTracker _tracker;
        private Mat _lastFrame;

        public MainForm()
        {
            InitializeComponent();
            pictureBoxVideo.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void BtnLoadVideoFile_Click(object sender, EventArgs e)
        {
            using OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Pliki wideo|*.mp4;*.avi; *.webm", // This string works for Polish Windows system
                Title = "Choose video file"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                if (_videoManager.LoadVideo(filePath))
                {
                    numericFpsOverride.Value = (decimal)_videoManager.Fps;
                    btnPlayVideo.Text = "▶️ Start";
                    _tracker?.Dispose();
                    var firstFrame = _videoManager.GetFirstFrame();
                    if (firstFrame != null)
                    {
                        _lastFrame = firstFrame.Clone();
                        pictureBoxVideo.Image?.Dispose();
                        pictureBoxVideo.Image = _lastFrame.ToBitmap();
                    }
                }
            }
        }

        private void NumericFpsOverride_ValueChanged(object sender, EventArgs e)
        {
            _videoManager.Fps = (double)numericFpsOverride.Value;
        }

        private async void BtnPlayVideo_Click(object sender, EventArgs e)
        {
            if (!_videoManager.IsPlaying)
            {
                btnPlayVideo.Text = "⏸️ Pause";

                await _videoManager.StartVideoAsync(ProcessFrameAsync);
                _tracker?.Dispose();

            }
            else
            {
                if (_videoManager.IsPaused)
                {
                    _videoManager.Resume();
                    btnPlayVideo.Text = "⏸️ Pause";
                }
                else
                {
                    _videoManager.Pause();
                    btnPlayVideo.Text = "▶️ Resume";
                }
            }
        }

        private async Task ProcessFrameAsync(Mat mat)
        {
            if (!IsHandleCreated) return;

            await this.InvokeAsync(() =>
            {
                pictureBoxVideo.Image?.Dispose();
                _lastFrame = mat.Clone();

                // Zawsze twórz kopię do trackera i do wyświetlania!
                using var trackerInput = mat.Clone();
                Bitmap bitmap = mat.ToBitmap();

                if (_tracker != null)
                {
                    var rect = _tracker.Track(trackerInput); // używaj klona
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
                }

                pictureBoxVideo.Image = bitmap;
                _frameCounter++;

                DisplayCurrentFps();
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

                _tracker = Enum.TryParse<TrackerType>(comboBoxTracker.SelectedItem?.ToString(), out var trackerType) ? 
                    TrackerFactory.Create(trackerType) : 
                    throw new ArgumentOutOfRangeException(nameof(trackerType), trackerType, null);
                _tracker.Initialize(_lastFrame, roi);
            }
        }
    }
}
