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

        public Vector2 InitialResolution { get; }
        private Vector2 _resolution;

        public Vector2 Resolution
        {
            get => _resolution;
            set
            {
                var width = MathF.Round(value.X);
                var height = MathF.Round(value.Y);
                _resolution = new Vector2(width, height);
                Data = new byte[(int)width * (int)height * PdfConstants.BytesPerPixel];
            }
        }

        public PdfTexture(Vector2 resolution)
        {
            InitialResolution = resolution;
            Resolution = resolution;
        }

        public void Dispose()
        {
            DataPointer = IntPtr.Zero;
            Data = null;
        }
    }
}