using System;
using System.Drawing;

namespace PdfNet.Core
{
    public class PdfViewport
    {
        /// <summary>
        /// Position of the top left corner of the viewport rectangle in screen space.
        /// </summary>
        public Vector2Int Position
        {
            get => new Vector2Int(_rectangle.X, _rectangle.Y);
            set
            {

                _rectangle.X = (int)Math.Clamp(value.X, -0.5f * (_rectangle.Width * _zoom - _rectangle.Width),
                    0.5f * (_rectangle.Width * _zoom - _rectangle.Width));
                _rectangle.Y = Math.Max(value.Y, 0);
            }
        }

        /// <summary>
        /// Size of the viewport rectangle in pixels.
        /// </summary>
        public Vector2Int Size => new Vector2Int(_rectangle.Width, _rectangle.Height);

        public int Top => _rectangle.Top;
        public int Bottom => _rectangle.Bottom;

        public event Action<PdfViewport> OnZoom;

        private float _zoom = 1f;

        public float Zoom
        {
            get => _zoom;
            set
            {
                _zoom = Math.Clamp(value, 1f, float.MaxValue);
                OnZoom?.Invoke(this);
            }
}

        public Vector2Int Center => new Vector2Int((int)(_rectangle.X + _rectangle.Width * 0.5f), (int)(_rectangle.Y + _rectangle.Height * 0.5f));

        private Rectangle _rectangle;
        public Rectangle Rectangle => _rectangle;

        
        public PdfViewport(Vector2Int size)
        {
            _rectangle = new Rectangle(0, 0, size.X, size.Y);
            Zoom = 1f;
        }
    }
}