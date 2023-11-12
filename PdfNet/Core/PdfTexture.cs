using System;
using System.Numerics;
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

        private Vector2 _resolution;

        public Vector2 Resolution
        {
            get => _resolution;
            set
            {
                _resolution = value;
                Data = new byte[(int)Math.Round(_resolution.X * _resolution.Y) * PdfConstants.BytesPerPixel];
            }
        }

        public PdfTexture(Vector2 resolution)
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