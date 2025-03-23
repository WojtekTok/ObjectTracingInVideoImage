using ObjectTracingInVideoImage.Core;
using ObjectTracingVideoImage.App.Extensions;

namespace ObjectTracingVideoImage.App
{
    public partial class MainForm : Form
    {
        private VideoManager _videoManager = new VideoManager();

        public MainForm()
        {
            InitializeComponent();
            timerVideoPlayback.Tick += timerVideoPlayback_Tick!;
            pictureBoxVideo.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void btnLoadVideoFile_Click(object sender, EventArgs e)
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
                    timerVideoPlayback.Interval = (int)(1000.0 / _videoManager.Fps);
                    _videoManager.Play();
                    timerVideoPlayback.Start();
                }
            }
        }

        private void timerVideoPlayback_Tick(object sender, EventArgs e)
        {
            var frame = _videoManager.GetNextFrame();

            if (frame != null)
            {
                pictureBoxVideo.Image?.Dispose();
                pictureBoxVideo.Image = frame.ToBitmap();
            }
            else
            {
                timerVideoPlayback.Stop();
            }
        }
    }
}
