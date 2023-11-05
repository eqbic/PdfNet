using System;
using System.Drawing;
using System.Numerics;

namespace PdfNet.Core
{
    public abstract class PdfViewport<T>
    {
        protected Vector2 _size;
        protected Vector2 _position;
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

        public Vector2 Position
        {
            get => _position;
            set
            {
                _position = value;
                _rectangle.X = _position.X;
                _rectangle.Y = _position.Y;
            }
        }
    
        public Vector2 Size
        {
            get => _size;
            set
            {
                _size = value;
                UpdateImageSize(_size);
                Data = new byte[(int)_size.X * (int)_size.Y * PdfConstants.BytesPerPixel];
                _rectangle.Width = _size.X;
                _rectangle.Height = _size.Y;
            }
        }

        protected abstract void UpdateImageSize(Vector2 size);
        
        public abstract T Image { get; }
    
        private RectangleF _rectangle;
        public RectangleF Rectangle => _rectangle;
    
        public PdfViewport(Vector2 size)
        {
            _rectangle = new RectangleF(0, 0, size.X, size.Y);
            Size = size;
        
        }
    }
}