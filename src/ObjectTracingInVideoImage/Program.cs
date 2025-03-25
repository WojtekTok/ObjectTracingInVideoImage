using Emgu.CV;
using Emgu.CV.Structure;
using Application = System.Windows.Forms.Application;
using ObjectTracingInVideoImage.Extensions;
using Emgu.CV.CvEnum;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

class Program
{
    static Rectangle selectedBox;
    static bool selectionMade = false;
   

    [STAThread] // Wymagane dla obsługi Windows Forms w aplikacji konsolowej
    static void Main()
    {
        // Pobieramy katalog główny projektu
        string projectRoot = Directory.GetParent(Directory.GetCurrentDirectory())!.Parent!.Parent!.Parent!.Parent!.Parent!.FullName;
        string videoPath = Path.Combine(projectRoot, "assets", "videos", "MOT17-10-SDP-raw.webm");

        if (!File.Exists(videoPath))
        {
            Console.WriteLine($"Nie znaleziono pliku: {videoPath}");
            return;
        }

        VideoCapture capture = new VideoCapture(videoPath);
        Mat frame = new Mat();
        capture.Read(frame);

        if (frame.IsEmpty)
        {
            Console.WriteLine("Błąd: Nie udało się wczytać pierwszej klatki.");
            return;
        }

        // 🔹 Tworzymy osobny wątek dla Windows Forms (nie blokuje konsoli)
        var formThread = new Thread(() =>
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            using (var form = new SelectionForm(frame))
            {
                Application.Run(form);
            }
        });

        formThread.SetApartmentState(ApartmentState.STA);
        formThread.Start();
        formThread.Join(); // Czekamy na zakończenie wyboru

        if (!selectionMade)
        {
            Console.WriteLine("Nie wybrano obiektu.");
            return;
        }

        // 🔹 Inicjalizacja trackera
        Tracker tracker = new TrackerKCF();
        tracker.Init(frame, selectedBox);

        // 🔹 Rozpoczynamy śledzenie
        while (capture.Read(frame))
        {
            tracker.Update(frame, out selectedBox);
            CvInvoke.Rectangle(frame, selectedBox, new MCvScalar(0, 255, 0), 2);
            CvInvoke.Imshow("Tracking", frame);
            if (CvInvoke.WaitKey(30) == 27) break;
        }

        capture.Dispose();
    }

    // 🔹 Klasa Windows Forms do zaznaczania obiektu myszką
    class SelectionForm : Form
    {
        public SelectionForm(Mat frame)
        {
            Text = "Kliknij na obiekt do śledzenia";
            Width = frame.Width;
            Height = frame.Height;

            PictureBox pictureBox = new PictureBox
            {
                Dock = DockStyle.Fill,
                Image = frame.MatToBitmap(),
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            Controls.Add(pictureBox);

            pictureBox.MouseClick += (s, e) =>
            {
                selectedBox = new Rectangle(e.X - 40, e.Y - 40, 80, 80);
                selectionMade = true;
                Close(); // Zamykamy okno po wyborze obiektu
            };
        }
    }
}
