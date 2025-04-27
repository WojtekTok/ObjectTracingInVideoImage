using ObjectTracingInVideoImage.App.Controls;
using ObjectTracingInVideoImage.App.Extensions;
using ObjectTracingInVideoImage.Core;

namespace ObjectTracingVideoImage.App
{
    public partial class MainForm : Form
    {
        private VideoManager _videoManager = new VideoManager();
        private int _frameCounter = 0;
        private DateTime _lastFpsCheck = DateTime.Now;
        private RectangleSelector _rectangleSelector = new RectangleSelector();


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
                }
            }
        }

        private void numericFpsOverride_ValueChanged(object sender, EventArgs e)
        {
            _videoManager.Fps = (double)numericFpsOverride.Value;
        }

        private async void BtnPlayVideo_Click(object sender, EventArgs e)
        {
            if (!_videoManager.IsPlaying)
            {
                btnPlayVideo.Text = "⏸️ Pause";

                await _videoManager.StartVideoAsync(async mat =>
                {
                    if (IsHandleCreated)
                    {
                        await this.InvokeAsync(() =>
                        {
                            pictureBoxVideo.Image?.Dispose();
                            pictureBoxVideo.Image = mat.ToBitmap();
                            mat.Dispose();
                            _frameCounter++;

                            var now = DateTime.Now;
                            if ((now - _lastFpsCheck).TotalSeconds >= 1)
                            {
                                labelFps.Text = $"FPS: {_frameCounter}";
                                _frameCounter = 0;
                                _lastFpsCheck = now;
                            }
                            pictureBoxVideo.Invalidate();
                        });
                    }
                });

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
    }
}
