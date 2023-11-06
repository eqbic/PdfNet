using System;
using System.Drawing;

namespace PdfNet.Core
{
    public abstract class PdfViewport
    {
        protected IntPtr DataPointer;

        private byte[] _data;

        public byte[] Data
        {
            get => _data;
            private set
            {
                _data = value;
                try
                {
                    unsafe
                    {
                        fixed (byte* pointer = _data)
                        {
                            DataPointer = (IntPtr)pointer;
                        }
                    }
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
        }

        /// <summary>
        /// Position of the top left corner of the viewport rectangle in screen space.
        /// </summary>
        public Vector2Int Position
        {
            get => new Vector2Int(_rectangle.X, _rectangle.Y);
            set
            {
                _rectangle.X = value.X;
                _rectangle.Y = value.Y;
            }
        }
    
        /// <summary>
        /// Size of the viewport rectangle in pixels.
        /// </summary>
        public Vector2Int Size
        {
            get => new Vector2Int(_rectangle.Width, _rectangle.Height);
            set
            {
                _rectangle.Width = value.X;
                _rectangle.Height = value.Y;
                UpdateImageSize();
                Data = new byte[_rectangle.Width * _rectangle.Height * PdfConstants.BytesPerPixel];
            }
        }

        protected abstract void UpdateImageSize();
        
        private Rectangle _rectangle;
        public Rectangle Rectangle => _rectangle;
        
        public PdfViewport(Vector2Int size)
        {
            _rectangle = new Rectangle(0, 0, size.X, size.Y);
        }
    }
}