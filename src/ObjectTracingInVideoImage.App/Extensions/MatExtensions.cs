using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Emgu.CV;

namespace ObjectTracingInVideoImage.App.Extensions
{
    static class MatExtensions
    {
        public static Bitmap ToBitmap(this Mat mat)
        {
            Bitmap bitmap = new Bitmap(mat.Width, mat.Height, PixelFormat.Format24bppRgb);
            Rectangle rect = new Rectangle(0, 0, mat.Width, mat.Height);
            BitmapData bitmapData = bitmap.LockBits(rect, ImageLockMode.WriteOnly, bitmap.PixelFormat);

            byte[] matData = new byte[mat.Width * mat.Height * mat.NumberOfChannels];
            Marshal.Copy(mat.DataPointer, matData, 0, matData.Length);
            Marshal.Copy(matData, 0, bitmapData.Scan0, matData.Length);

            bitmap.UnlockBits(bitmapData);
            return bitmap;
        }
    }
}
