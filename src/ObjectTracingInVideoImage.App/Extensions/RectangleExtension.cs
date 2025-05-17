namespace ObjectTracingInVideoImage.App.Extensions
{
    internal static class RectangleExtension
    {
        public static Rectangle ScaleRectangleToImage(this Rectangle rectangle, PictureBox pb, Size imageSize)
        {
            float imageAspect = (float)imageSize.Width / imageSize.Height;
            float controlAspect = (float)pb.ClientSize.Width / pb.ClientSize.Height;

            float ratio;
            int offsetX = 0, offsetY = 0;

            if (imageAspect > controlAspect)
            {
                ratio = (float)pb.ClientSize.Width / imageSize.Width;
                offsetY = (int)((pb.ClientSize.Height - imageSize.Height * ratio) / 2);
            }
            else
            {
                ratio = (float)pb.ClientSize.Height / imageSize.Height;
                offsetX = (int)((pb.ClientSize.Width - imageSize.Width * ratio) / 2);
            }

            return new Rectangle(
                (int)((rectangle.X - offsetX) / ratio),
                (int)((rectangle.Y - offsetY) / ratio),
                (int)(rectangle.Width / ratio),
                (int)(rectangle.Height / ratio)
            );
        }
    }
}
