using System;
using System.Drawing;

namespace PdfNet.Core
{
    public class PdfViewport
    {
        /// <summary>
        /// Position of the center of the rectangle.
        /// </summary>
        public Vector2Int Position
        {
            get => new Vector2Int((int)(_rectangle.X + _rectangle.Width * 0.5f), (int)(_rectangle.Y + _rectangle.Height * 0.5f));
            set
            {

                _rectangle.X = (int)(value.X - 0.5f * _rectangle.Width);
                _rectangle.Y = (int)(value.Y - 0.5f * _rectangle.Height);
            }
        }
        
        /// <summary>
        /// Size of the viewport rectangle in pixels.
        /// </summary>
        public Vector2Int Size => new Vector2Int(_rectangle.Width, _rectangle.Height);

        public int Top => _rectangle.Top;
        public int Bottom => _rectangle.Bottom;

        private float _zoom = 1f;

        public float Zoom
        {
            get => _zoom;
            set
            {
                _zoom = Math.Clamp(value, 1f, float.MaxValue);
                _rectangle.Width = (int)Math.Round(_initialSize.X / _zoom);
                _rectangle.Height = (int)(_initialSize.X * _aspectRatio);
                // var posX = (int)(0.5f * _initialSize.X - 0.5f * _rectangle.Width);
                // var posY = (int)(0.5f * _initialSize.Y - 0.5f * _rectangle.Height);
                // Position = new Vector2Int(posX, posY);
            }
}

        private Rectangle _rectangle;
        private readonly Vector2Int _initialSize;
        public Rectangle Rectangle => _rectangle;
        private float _aspectRatio;
        
        public PdfViewport(Vector2Int size)
        {
            _rectangle = new Rectangle(0, 0, size.X, size.Y);
            _initialSize = size;
            _aspectRatio = (float)size.Y / size.X;
            Zoom = 1f;
        }

        public override string ToString()
        {
            return Rectangle.ToString();
        }
    }
}