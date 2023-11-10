using System;
using System.Drawing;
using System.Numerics;

namespace PdfNet.Core
{
    public class PdfViewport
    {
        /// <summary>
        /// Position of the center of the rectangle.
        /// </summary>
        public Vector2 Position
        {
            get => new Vector2(_rectangle.X + _rectangle.Width * 0.5f, _rectangle.Y + _rectangle.Height * 0.5f);
            set
            {
                                
                _rectangle.X = value.X - 0.5f * _rectangle.Width;
                _rectangle.Y = value.Y - 0.5f * _rectangle.Height;
            }
        }

        public void Translate(Vector2 deltaPosition)
        {
            var x = Math.Clamp(Position.X + deltaPosition.X, 0.5f * _rectangle.Width, _initialSize.X - 0.5f * _rectangle.Width);
            var y = Math.Max(Position.Y + deltaPosition.Y, 0.5f * _rectangle.Height);
            Position = new Vector2(x, y);
        }
        
        /// <summary>
        /// Size of the viewport rectangle in pixels.
        /// </summary>
        public Vector2 Size => new Vector2(_rectangle.Width, _rectangle.Height);

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

        private RectangleF _rectangle;
        private readonly Vector2 _initialSize;
        public RectangleF Rectangle => _rectangle;
        private float _aspectRatio;
        
        public PdfViewport(Vector2 size)
        {
            _rectangle = new RectangleF(0, 0, size.X, size.Y);
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