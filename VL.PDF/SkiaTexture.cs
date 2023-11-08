using PdfNet.Core;
using SkiaSharp;
using Stride.Core.Mathematics;

namespace VL.PDF;

public class SkiaTexture : PdfTexture
{
    private SKImageInfo _info;

    public Int2 Resolution
    {
        get => new Int2(base.Resolution.X, base.Resolution.Y);
        set
        {
            base.Resolution = new Vector2Int(value.X, value.Y);
            _info.Width = Resolution.X;
            _info.Height = Resolution.Y;
        }
    }
    
    public SKImage Image => SKImage.FromPixels(_info, DataPointer);
    
    public SkiaTexture(Int2 resolution) : base(new Vector2Int(resolution.X, resolution.Y))
    {
        _info = new SKImageInfo(resolution.X, resolution.Y, SKColorType.Bgra8888);
    }
}