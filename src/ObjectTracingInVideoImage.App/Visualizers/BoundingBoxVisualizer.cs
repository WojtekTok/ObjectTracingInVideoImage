namespace ObjectTracingInVideoImage.App.Visualizers
{
    public static class BoundingBoxVisualizer
    {
        public static void DrawBoxes(Bitmap bitmap, Rectangle? boundingBox, Rectangle? grundTruthBox)
        {
            DrawBoxIfNotNull(bitmap, grundTruthBox, Color.LimeGreen);
            DrawBoxIfNotNull(bitmap, boundingBox, Color.Red);
        }

        private static void DrawBoxIfNotNull(Bitmap bitmap, Rectangle? rectangle, Color color)
        {
            if (rectangle.HasValue)
            {
                using var g = Graphics.FromImage(bitmap);
                using var pen = new Pen(color, 2);
                g.DrawRectangle(pen, rectangle.Value);
            }
        }
    }
}
