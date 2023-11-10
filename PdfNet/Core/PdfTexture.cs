using System;
using PdfNet.Unsafe;

namespace PdfNet.Core
{
    public class PdfTexture : IDisposable
    {
        public IntPtr DataPointer { get; private set; }

        private byte[] _data;

        public byte[] Data
        {
            get => _data;
            private set
            {
                _data = value;
                DataPointer = UnsafeUtils.GetPointer(_data);
            }
        }

        private Vector2Int _resolution;

        public Vector2Int Resolution
        {
            get => _resolution;
            set
            {
                _resolution = value;
                Data = new byte[_resolution.X * _resolution.Y * PdfConstants.BytesPerPixel];
            }
        }

        public PdfTexture(Vector2Int resolution)
        {
            Resolution = resolution;
        }

        public void Dispose()
        {
            DataPointer = IntPtr.Zero;
            Data = null;
        }
    }
}