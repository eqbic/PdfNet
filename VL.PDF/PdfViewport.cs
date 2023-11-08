using PdfNet.Core;
using Stride.Core.Mathematics;

namespace VL.PDF;

public class PdfViewport : PdfNet.Core.PdfViewport
{
    public PdfViewport(Int2 size) : base(new Vector2Int(size.X, size.Y))
    {
    }

    public Int2 Position
    {
        get => new Int2(base.Position.X, base.Position.Y);
        set => base.Position = new Vector2Int(value.X, value.Y);
    }

    public float Zoom
    {
        get => base.Zoom;
        set => base.Zoom = value;
    }
}