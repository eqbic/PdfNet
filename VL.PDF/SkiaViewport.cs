using System.Numerics;
using PdfNet.Core;
using SkiaSharp;

namespace VL.PDF
{
    public class SkiaViewport : PdfViewport<SKImage>
    {
        private SKImageInfo _info;

        public SkiaViewport(Vector2 size) : base(size)
        {
        }

        public Vector2 Size
        {
            get => base.Size;
            set => base.Size = value;
        }

        public Vector2 Position
        {
            get => base.Position;
            set => base.Position = value;
        }

        protected override void UpdateImageSize(Vector2 size)
        {
            _info = new SKImageInfo((int)size.X, (int)size.Y, SKColorType.Bgra8888);
        }

        public override SKImage Image => SKImage.FromPixels(_info, DataPointer);
    }
}