namespace ObjectTracingInVideoImage.App.Controls
{
    public class RectangleSelector
    {
        private bool _isSelecting = false;
        private Point _selectionStart;
        private bool _isActiveSelection = false;
        public Rectangle SelectionRectangle { get; private set; }

        public bool IsSelecting => _isSelecting;

        public void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _isSelecting = true;
                _selectionStart = e.Location;
                SelectionRectangle = new Rectangle(e.Location, new Size(0, 0));
                (sender as Control)?.Invalidate();
            }
        }

        public void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (_isSelecting)
            {
                var width = e.X - _selectionStart.X;
                var height = e.Y - _selectionStart.Y;
                SelectionRectangle = new Rectangle(
                    Math.Min(_selectionStart.X, e.X),
                    Math.Min(_selectionStart.Y, e.Y),
                    Math.Abs(width),
                    Math.Abs(height));
                (sender as Control)?.Invalidate();
            }
        }

        public void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (_isSelecting && e.Button == MouseButtons.Left)
            {
                _isSelecting = false;
                _isActiveSelection = true;
                (sender as Control)?.Invalidate();
            }
        }

        public void OnPaint(object sender, PaintEventArgs e)
        {
            if ((_isSelecting || _isActiveSelection) && SelectionRectangle.Width > 0 && SelectionRectangle.Height > 0)
            {
                using (var pen = new Pen(Color.Red, 2))
                {
                    e.Graphics.DrawRectangle(pen, SelectionRectangle);
                }
            }
        }

        public void Clear()
        {
            SelectionRectangle = Rectangle.Empty;
            _isActiveSelection = false;
        }

    }
}
