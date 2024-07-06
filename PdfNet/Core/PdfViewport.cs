using System;
using System.Drawing;
using System.Numerics;

namespace PdfNet.Core
{
    public class PdfViewport
    {
        public Vector2 Center { get; private set; }
        private Vector2 _scaledOffset;
        private RectangleF _rectangle;
        private readonly Vector2 _initialSize;
        public float DocumentHeight { get; set; }

        public void Translate(Vector2 deltaPosition)
        {
            var bounds = 0.5f * (_initialSize - _rectangle.Size());
            float offsetY = DocumentHeight - _rectangle.Size().Y;
            Center = Vector2.Clamp(Center + deltaPosition, -bounds, new Vector2(bounds.X, offsetY));
            SetClampedPosition(_scaledOffset + Center);
        }

        private void SetClampedPosition(Vector2 position)
        {
            _rectangle.X = Math.Clamp(position.X, 0f, _initialSize.X - _rectangle.Width);
            _rectangle.Y = Math.Max(0f, position.Y);
        }

        public void Scale(float scaleFactor)
        {
            _rectangle.Width = _initialSize.X * scaleFactor;
            _rectangle.Height = _initialSize.Y * scaleFactor;

            _scaledOffset = 0.5f * new Vector2(_initialSize.X - _rectangle.Width, _initialSize.Y - _rectangle.Height);
            SetClampedPosition(_scaledOffset + Center);
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
                _zoom = Math.Max(value, 1f);
                Scale(1f / _zoom);
            }
        }

        public RectangleF Rectangle => _rectangle;
        
        public PdfViewport(Vector2 size)
        {
            _rectangle = new RectangleF(0, 0, size.X, size.Y);
            _initialSize = size;
            Zoom = 1f;
        }

        public override string ToString()
        {
            return Rectangle.ToString();
        }
    }
}